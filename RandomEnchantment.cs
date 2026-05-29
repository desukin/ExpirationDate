using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Enchantments;

namespace ExpirationDate;

public static class RandomEnchantment
{
    private static readonly Random _rng = new();

    public static void Apply(CardModel card)
    {
        if (card.Enchantment != null)
        {
            Log.Info($"[ExpirationDate] {card.Id.Entry} already enchanted, skipping random");
            return;
        }

        var compatible = new List<EnchantmentModel>();

        TryAdd<Adroit>(compatible, card);
        TryAdd<Corrupted>(compatible, card);
        TryAdd<Glam>(compatible, card);
        TryAdd<Goopy>(compatible, card);
        TryAdd<Imbued>(compatible, card);
        TryAdd<Inky>(compatible, card);
        TryAdd<Instinct>(compatible, card);
        TryAdd<Momentum>(compatible, card);
        TryAdd<Nimble>(compatible, card);
        TryAdd<PerfectFit>(compatible, card);
        TryAdd<RoyallyApproved>(compatible, card);
        TryAdd<Sharp>(compatible, card);
        TryAdd<Slither>(compatible, card);
        TryAdd<SlumberingEssence>(compatible, card);
        TryAdd<SoulsPower>(compatible, card);
        TryAdd<Sown>(compatible, card);
        TryAdd<Spiral>(compatible, card);
        TryAdd<Steady>(compatible, card);
        TryAdd<Swift>(compatible, card);
        TryAdd<TezcatarasEmber>(compatible, card);
        TryAdd<Vigorous>(compatible, card);

        if (compatible.Count == 0)
        {
            Log.Info($"[ExpirationDate] {card.Id.Entry}: no compatible enchantment");
            return;
        }

        var picked = compatible[_rng.Next(compatible.Count)];
        CardCmd.Enchant(picked, card, 1);
        Log.Info($"[ExpirationDate] {card.Id.Entry} ← {picked.GetType().Name} ({compatible.Count} compatible)");
    }

    private static void TryAdd<T>(List<EnchantmentModel> list, CardModel card) where T : EnchantmentModel
    {
        try
        {
            var enchantment = ModelDb.Enchantment<T>().ToMutable();
            if (enchantment.CanEnchant(card))
                list.Add(enchantment);
        }
        catch { }
    }
}
