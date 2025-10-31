using UndertaleModLib;
using UndertaleModLib.Decompiler;

namespace YAPR_LIB.Patches.Fixes
{
    public static class AddTourianKeysToSaveSlot
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext)
        {
            var scr_TS_Draw_Save_code = gmData.Code.ByName("gml_Script_scr_TS_Draw_Save");
            var scr_TS_Draw_Save = Decompiler.Decompile(scr_TS_Draw_Save_code, decompileContext);
            scr_TS_Draw_Save = scr_TS_Draw_Save.UnixReplace(
                """
                draw_sprite(spr_Samus_Crouch, 0, (xx - 116), (yy + 6))

                """,
                """
                xx -= 16
                draw_sprite_ext(spr_ITEM_Tourian_Key, 1, (xx - 116), (yy + 6), 1, 1, 0, c_white, 1)
                xx += 8
                yy += 4
                var tourian_keys = ds_map_find_value(game_data, "Tourian Keys")
                scr_Draw_Text_Outline((xx - 116), (yy + 6), string(tourian_keys)+"/9", c_white, c_black, font_Metroid)
                xx += 8
                yy -= 4

                """
            );

            scr_TS_Draw_Save_code.ReplaceGML(scr_TS_Draw_Save, gmData);
        }
    }
}
