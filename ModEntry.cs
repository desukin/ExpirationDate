using BaseLib.Config;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;

namespace ExpirationDate;

[ModInitializer("Init")]
public class ModEntry
{
    public static Harmony Harmony { get; private set; } = null!;

    public static void Init()
    {
        try
        {
            ModConfigRegistry.Register("ExpirationDate", new ExpirationDateConfig());

            Log.Info($"[ExpirationDate] Mod loading, expiration={ExpirationDateConfig.ExpirationEnabled}, randomEnchant={ExpirationDateConfig.RandomEnchantEnabled}");

            Harmony = new Harmony("com.minimento.expirationdate");
            Harmony.PatchAll();
            Log.Info("[ExpirationDate] Harmony patches applied successfully");
        }
        catch (Exception ex)
        {
            Log.Error($"[ExpirationDate] Init failed: {ex}");
        }
    }
}
