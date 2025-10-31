using UndertaleModLib;
using UndertaleModLib.Decompiler;

namespace YAPR_LIB.Patches.Fixes
{
    public static class AddTourianKeysToHUD
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext)
        {
            var scr_Draw_HUD_code = gmData.Code.ByName("gml_Script_scr_Draw_HUD");
            var scr_Draw_HUD = Decompiler.Decompile(scr_Draw_HUD_code, decompileContext);
            scr_Draw_HUD = scr_Draw_HUD.Replace(
                """
                    start_x = min((cam_x + cam_w - 37), (cam_x + cam_w - border_w / 2 - floor(15)))

                """,
                """
                    n = string_length("9")
                    text = digit_buff(i.Upgrade[30], n)
                    xx = ((cam_x + (border_w / 2)) - 0)
                    yy = (cam_y + 175)
                    c = c_white
                    c2 = c_gray
                    index = 1
                    draw_sprite_ext(spr_ITEM_Tourian_Key, index, xx, yy, 1, 1, 90, c_white, 1)
                    yy += 8
                    draw_set_font(font_Mago2)
                    if (border_w > 30)
                    {
                        draw_set_font(font_Metroid)
                        yy += 5
                    }
                    draw_set_halign(fa_center)
                    draw_text_color((xx + 1), (yy - 5), text, c, c, c, c, 1)
                    if (border_w > 30)
                        yy -= 4
                    draw_line_color((xx - 8), (yy + 8), (xx + 7), (yy + 8), c2, c2)
                    draw_set_font(font_Mago2)
                    draw_text_color((xx + 1), (yy + 5), "9", c2, c2, c2, c2, 1)
                    draw_set_halign(fa_left)
                    start_x = min(((cam_x + cam_w) - 37), (((cam_x + cam_w) - (border_w / 2)) - floor(15)))

                """
            );

            scr_Draw_HUD_code.ReplaceGML(scr_Draw_HUD, gmData);
        }
    }
}
