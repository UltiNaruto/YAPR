using UndertaleModLib;
using UndertaleModLib.Decompiler;

namespace YAPR_LIB.Patches.CustomPickups.Globals
{
    public static class AddCustomItemsToGlobals
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext)
        {
            var int_Globals_References_code = gmData.Code.ByName("gml_Script_int_Globals_References");
            var int_Globals_References = Decompiler.Decompile(int_Globals_References_code, decompileContext);

            int_Globals_References = int_Globals_References.Replace(
                "global.Item_Sprite[(100 << 0)] = 139",
              $$"""
                global.Item_Sprite[(27 << 0)] = {{gmData.Sprites.IndexOf(gmData.Sprites.Select(c => c).Where(c => c.Name.Content == "spr_ITEM_Nothing").First())}}
                global.Item_Sprite[(30 << 0)] = {{gmData.Sprites.IndexOf(gmData.Sprites.Select(c => c).Where(c => c.Name.Content == "spr_ITEM_Tourian_Key").First())}}
                global.Item_Sprite[(31 << 0)] = {{gmData.Sprites.IndexOf(gmData.Sprites.Select(c => c).Where(c => c.Name.Content == "spr_ITEM_Big_Missile").First())}}
                global.Item_Sprite[(100 << 0)] = 139
                """
            );

            int_Globals_References = int_Globals_References.Replace(
                "global.Item_Name[100] = \"IMPASSABLE\"",
                """
                global.Item_Name[(27 << 0)] = "NOTHING"
                global.Item_Name[(30 << 0)] = "TOURIAN KEY"
                global.Item_Name[(31 << 0)] = "BIG MISSILE TANK"
                global.Item_Name[100] = "IMPASSABLE"
                """
            );

            int_Globals_References_code.ReplaceGML(int_Globals_References, gmData);
        }
    }
}
