using SixLabors.ImageSharp;
using UndertaleModLib;
using YAPR_LIB.Utils;

namespace YAPR_LIB.Patches
{
    public static class CustomSprites
    {
        public static void Apply(UndertaleData gmData)
        {
            // First available slot in the texture page
            var x = 42;
            var y = 1092;

            // Planets Big Missile Tank
            gmData.Sprites.Add(TextureUtils.CreateSprite(gmData, "spr_ITEM_Big_Missile", ref x, ref y, 16, 16,
                Image.Load(Path.Combine(Patcher.ExecutableDir, "Sprites", "Planets", "spr_ITEM_Big_Missile", "spr_ITEM_Big_Missile_0.png")),
                Image.Load(Path.Combine(Patcher.ExecutableDir, "Sprites", "Planets", "spr_ITEM_Big_Missile", "spr_ITEM_Big_Missile_1.png")),
                Image.Load(Path.Combine(Patcher.ExecutableDir, "Sprites", "Planets", "spr_ITEM_Big_Missile", "spr_ITEM_Big_Missile_2.png")),
                Image.Load(Path.Combine(Patcher.ExecutableDir, "Sprites", "Planets", "spr_ITEM_Big_Missile", "spr_ITEM_Big_Missile_3.png"))
            ));
            // Planets Boss Key
            gmData.Sprites.Add(TextureUtils.CreateSprite(gmData, "spr_ITEM_Tourian_Key", ref x, ref y, 16, 16,
                Image.Load(Path.Combine(Patcher.ExecutableDir, "Sprites", "Planets", "spr_ITEM_Tourian_Key", "spr_ITEM_Tourian_Key_0.png")),
                Image.Load(Path.Combine(Patcher.ExecutableDir, "Sprites", "Planets", "spr_ITEM_Tourian_Key", "spr_ITEM_Tourian_Key_1.png"))
            ));
            // Planets Nothing
            gmData.Sprites.Add(TextureUtils.GetSprite(gmData, "spr_ITEM_Nothing", 16, 16, new Rectangle(211, 110, 15, 15)));
            // Planets Offworld Item
            gmData.Sprites.Add(TextureUtils.GetSprite(gmData, "spr_ITEM_Multiworld_Item", 16, 16, new Rectangle(111, 50, 15, 15)));
            // TODO: Add offworld models
            /*
            // Prime 1 varia suit
            gmData.Sprites.Add(TextureUtils.CreateSprite(gmData, "spr_ITEM_Prime_Varia_Suit", ref x, ref y, 16, 16, Properties.Resources.spr_ITEM_Prime_Varia_Suit_0));
            // Prime 1 gravity suit
            gmData.Sprites.Add(TextureUtils.CreateSprite(gmData, "spr_ITEM_Prime_Gravity_Suit", ref x, ref y, 16, 16, Properties.Resources.spr_ITEM_Prime_Gravity_Suit_0));
            // Prime 1 phazon suit
            gmData.Sprites.Add(TextureUtils.CreateSprite(gmData, "spr_ITEM_Prime_Phazon_Suit", ref x, ref y, 16, 16, Properties.Resources.spr_ITEM_Prime_Phazon_Suit_0));
            // Prime 2 light suit
            gmData.Sprites.Add(TextureUtils.CreateSprite(gmData, "spr_ITEM_Echoes_Light_Suit", ref x, ref y, 16, 16, Properties.Resources.spr_ITEM_Echoes_Light_Suit_0));
            */
        }
    }
}
