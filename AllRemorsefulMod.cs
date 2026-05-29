using System.Reflection;
using BaseLib.Utils;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Map;

namespace ExpirationDate;

public static class AllRemorsefulMod
{
    public const int MaxCombats = 5;
    public static readonly SavedSpireField<CardModel, int> RemainingCombats = new(() => MaxCombats, "expiration_date_remaining");

    [HarmonyPatch]
    public static class CardAddPatch
    {
        static MethodBase TargetMethod() => AccessTools.Method(typeof(CardPileCmd), "Add",
            [typeof(CardModel), typeof(PileType), typeof(CardPilePosition), typeof(AbstractModel), typeof(bool)]);
        static void Prefix(CardModel card, PileType newPileType)
        {
            if (newPileType != PileType.Deck) return;
            if (card.Type == CardType.Curse || card.Type == CardType.Status) return;
            try
            {
                if (ExpirationDateConfig.ExpirationEnabled) RemainingCombats[card] = MaxCombats;
                if (ExpirationDateConfig.RandomEnchantEnabled) RandomEnchantment.Apply(card);
            }
            catch (Exception ex) { Log.Error($"[ExpirationDate] CardAdd: {ex.Message}"); }
        }
    }

    [HarmonyPatch]
    public static class ScalingHpPatch
    {
        static MethodBase TargetMethod() => typeof(Hook).GetMethod("BeforeCombatStart", BindingFlags.Public | BindingFlags.Static)!;
        static void Postfix()
        {
            if (!ExpirationDateConfig.ScalingHpEnabled) return;
            try
            {
                double multiplier = 1.0 + (RunManager.Instance.RunTime / 60.0) / 100.0;
                var cm = CombatManager.Instance;
                var sf = cm.GetType().GetField("_state", BindingFlags.NonPublic | BindingFlags.Instance);
                if (sf?.GetValue(cm) is not CombatState state) return;
                var setMax = typeof(Creature).GetMethod("SetMaxHpInternal", BindingFlags.Public | BindingFlags.Instance);
                var setCur = typeof(Creature).GetMethod("SetCurrentHpInternal", BindingFlags.Public | BindingFlags.Instance);
                foreach (var e in state.Enemies)
                {
                    int nm = (int)(e.MaxHp * multiplier);
                    if (nm <= e.MaxHp) continue;
                    int g = nm - e.MaxHp;
                    setMax?.Invoke(e, [(decimal)nm]);
                    setCur?.Invoke(e, [(decimal)(e.CurrentHp + g)]);
                }
            }
            catch (Exception ex) { Log.Error($"[ExpirationDate] ScalingHp: {ex.Message}"); }
        }
    }

    [HarmonyPatch]
    public static class AfterCombatPatch
    {
        static MethodBase TargetMethod() => AccessTools.Method(typeof(Hook), nameof(Hook.AfterCombatEnd));
        static void Postfix(IRunState runState)
        {
            if (!ExpirationDateConfig.ExpirationEnabled) return;
            try
            {
                foreach (var p in runState.Players)
                    foreach (var c in p.Deck.Cards.ToList())
                    {
                        if (c.Type == CardType.Curse || c.Type == CardType.Status) continue;
                        int r = RemainingCombats[c] - 1;
                        if (r <= 0) { RemainingCombats[c] = 0; _ = CardPileCmd.RemoveFromDeck(c, false); }
                        else RemainingCombats[c] = r;
                    }
            }
            catch (Exception ex) { Log.Error($"[ExpirationDate] AfterCombat: {ex.Message}"); }
        }
    }

    [HarmonyPatch]
    public static class HideHpPatch
    {
        static void Postfix(NHealthBar __instance)
        {
        static void Postfix(NHealthBar __instance)
        {
            if (!ExpirationDateConfig.HideEnemyHpEnabled) return;

            var cf = typeof(NHealthBar).GetField("_creature", BindingFlags.NonPublic | BindingFlags.Instance);
            if (cf?.GetValue(__instance) is Creature creature && creature.IsPlayer) return;

            // Hide the HP bar container (background + foreground)
            var fgc = typeof(NHealthBar).GetField("_hpForegroundContainer", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fgc?.GetValue(__instance) is Control ctrl) ctrl.Visible = false;

            // Text already shows ? from previous logic, but belt-and-suspenders:
            var hf = typeof(NHealthBar).GetField("_hpLabel", BindingFlags.NonPublic | BindingFlags.Instance);
            var bf = typeof(NHealthBar).GetField("_blockLabel", BindingFlags.NonPublic | BindingFlags.Instance);
            if (hf?.GetValue(__instance) is Label hl) hl.Text = "?";
        }
    }
    }

    // ── 5. All Events: Monster rooms → Unknown ──

    [HarmonyPatch]
    public static class AllEventsPatch
    {
        static MethodBase TargetMethod() => typeof(Hook).GetMethod("ModifyGeneratedMapLate",
            BindingFlags.Public | BindingFlags.Static)!;

        static void Postfix(ActMap __result)
        {
            if (!ExpirationDateConfig.AllEventsEnabled) return;
            try
            {
                foreach (var point in __result.GetAllMapPoints())
                {
                    if (point.PointType == MapPointType.Monster)
                        point.PointType = MapPointType.Unknown;
                }
            }
            catch (Exception ex) { Log.Error($"[ExpirationDate] AllEvents: {ex.Message}"); }
        }
    }
}
