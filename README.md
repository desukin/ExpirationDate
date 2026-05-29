# Expiration Date（保质期）

《杀戮尖塔2》Mod —— 卡牌有了保质期？战斗后可能消失！可自定义的挑战性 modifiers，支持游戏内 Mod 配置菜单开关。

- **作者**: mius
- **版本**: v1.2.0
- **游戏版本**: v0.103.2
- **前置 Mod**: [BaseLib](https://github.com/minimento/BaseLib)（v3.1.3+）

---

## 功能介绍

所有功能均可在游戏内 Mod 配置菜单中**独立开关**。

| 开关 | 效果 |
|------|------|
| 🕒 **保质期** | 获得的卡牌在 **5 场战斗后** 自动从牌组中移除。谨慎规划你的卡组——没有什么能永恒。 |
| ✨ **随机附魔** | 每张进入牌组的卡牌都会获得一个**随机原版附魔**。让每场游戏都充满变数。 |
| ❓ **全是事件** | 地图上的**小怪房间全部变为未知(?)房间**。适合速通或只想打精英/Boss。 |
| 📈 **敌人成长** | 怪物血量会随**实时游戏时长**增长：每分钟 +1% 最大生命值。打得越久，战斗越难。 |
| ❓ **隐藏血量** | 敌人的生命值和格挡值被隐藏（显示为"?"）。血条本身仍然可见，只能粗略判断。 |

---

## 安装方法

1. 先安装前置 Mod：[BaseLib](https://github.com/minimento/BaseLib)（v3.1.3+）
2. 下载本 Mod 的最新版本
3. 解压到 `Slay the Spire 2/mods/ExpirationDate/` 目录下
4. 在游戏内 Mod 管理器中启用

### 从源码编译

```bash
cd ExpirationDate_backup
dotnet build CustomModifiers.csproj -c Release
```

编译后 `.dll` 和配置 `json` 会自动复制到游戏 mods 目录。

---

## 项目结构

```
ExpirationDate/
├── ExpirationDate.json          # Mod 清单文件
├── CustomModifiers.csproj       # .NET 9.0 项目文件
├── ModEntry.cs                  # 入口文件 & Harmony 补丁
├── ExpirationDateConfig.cs      # 配置 UI
├── AllRemorsefulMod.cs          # 核心逻辑（5 个 Harmony Patch）
├── RandomEnchantment.cs         # 随机附魔逻辑
├── project.godot                # Godot 项目文件（用于 PCK 导出）
├── README.md                    # 本文件
└── 开发报告.md                   # 中文技术开发文档
```

---

## 兼容性

- **游戏版本**: v0.103.2+
- **BaseLib**: v3.1.3+
- **多人兼容**: `affects_gameplay: true` — 多人联机时所有玩家均需安装本 Mod
