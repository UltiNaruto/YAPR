using UndertaleModLib.Decompiler;
using UndertaleModLib;

namespace YAPR_LIB.Patches.Fixes
{
    public static class FixZebethMinimap
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext)
        {
            var scr_Draw_Map_Screen_code = gmData.Code.ByName("gml_Script_scr_Draw_Map_Screen");
            var scr_Draw_Map_Screen = Decompiler.Decompile(scr_Draw_Map_Screen_code, decompileContext);

            scr_Draw_Map_Screen = scr_Draw_Map_Screen.Replace(
                """

                var draw_base = 1
                """,
                """
                if (room == rm_Zebeth && screen_x == 23 && screen_y == 26)
                    draw_sprite_ext(spr_Map_Tile_Side, 0, draw_x, draw_y, 1, 1, 0, c_white, 1)
                var draw_base = 1
                """
            );

            scr_Draw_Map_Screen_code.ReplaceGML(scr_Draw_Map_Screen, gmData);
        }
    }
}
