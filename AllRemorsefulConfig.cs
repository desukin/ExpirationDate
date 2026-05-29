using BaseLib.Config;
using BaseLib.Config.UI;
using Godot;

namespace ExpirationDate;

public class ExpirationDateConfig : SimpleModConfig
{
    public static bool ExpirationEnabled { get; set; } = false;
    public static bool RandomEnchantEnabled { get; set; } = false;
    public static bool ScalingHpEnabled { get; set; } = false;
    public static bool HideEnemyHpEnabled { get; set; } = false;

    public override void SetupConfigUI(Control optionContainer)
    {
        AddToggle(optionContainer, nameof(ExpirationEnabled), "保质期：卡牌5场战斗后删除");
        AddToggle(optionContainer, nameof(RandomEnchantEnabled), "随机附魔：卡牌获得随机原版附魔");
        AddToggle(optionContainer, nameof(ScalingHpEnabled), "敌人成长：每分钟增加1%血量");
        AddToggle(optionContainer, nameof(HideEnemyHpEnabled), "隐藏血量：不显示敌人生命值");

        AddRestoreDefaultsButton(optionContainer);
        SetupFocusNeighbors(optionContainer);
    }

    private void AddToggle(Control container, string propName, string labelText)
    {
        var toggle = CreateRawTickboxControl(typeof(ExpirationDateConfig).GetProperty(propName)!);
        var row = new NConfigOptionRow(ModPrefix, propName,
            CreateRawLabelControl(labelText, 28), toggle);
        container.AddChild(row);
    }
}
