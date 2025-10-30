using UndertaleModLib;
using UndertaleModLib.Decompiler;

namespace YAPR_LIB.Patches
{
    public static class CustomMessageBox
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext)
        {
            // gml_Object_obj_MAIN_Create_0
            var obj_MAIN_Create_0_code = gmData.Code.ByName("gml_Object_obj_MAIN_Create_0");
            var obj_MAIN_Create_0 = Decompiler.Decompile(obj_MAIN_Create_0_code, decompileContext);

            obj_MAIN_Create_0 = obj_MAIN_Create_0.Replace(
                """
                Item_Message = 0

                """.ReplaceLineEndings("\n"),
                """
                Item_Message = 0
                Item_Message_Header = ""
                Item_Message_Description = ""

                """.ReplaceLineEndings("\n")
            );

            obj_MAIN_Create_0_code.ReplaceGML(obj_MAIN_Create_0, gmData);

            // gml_Script_scr_Draw_Item_Message
            var scr_draw_item_message_code = gmData.Code.ByName("gml_Script_scr_Draw_Item_Message");
            var scr_draw_item_message = Decompiler.Decompile(scr_draw_item_message_code, decompileContext);

            scr_draw_item_message = scr_draw_item_message.Replace(
                """
                if (Item_Event_Type == 0)
                {
                """.ReplaceLineEndings("\n"),
                """
                text1 = obj_MAIN.Item_Message_Header
                text2 = obj_MAIN.Item_Message_Description
                if (Item_Event_Type == 0)
                {
                    var h = string_count("\n", obj_MAIN.Item_Message_Description) * 14
                """.ReplaceLineEndings("\n")
            );

            scr_draw_item_message = scr_draw_item_message.Replace(
                """
                    draw_rectangle_color((xx - w), (yy - dis), (xx + w), (yy + dis), c, c, c, c, 0)
                    c = c_white
                    draw_set_alpha(1)
                    draw_line_color((xx - w - 1), (yy - dis), (xx + w), (yy - dis), c, c)
                    draw_line_color((xx - w - 1), (yy + dis), (xx + w), (yy + dis), c, c)
                """.ReplaceLineEndings("\n"),
                """
                    draw_rectangle_color((xx - w), (yy - dis - h / 2), (xx + w), (yy + dis + h / 2), c, c, c, c, 0)
                    c = c_white
                    draw_set_alpha(1)
                    draw_line_color((xx - w - 1), (yy - dis - h / 2), (xx + w), (yy - dis - h / 2), c, c)
                    draw_line_color((xx - w - 1), (yy + dis + h / 2), (xx + w), (yy + dis + h / 2), c, c)
                """.ReplaceLineEndings("\n")
            );

            scr_draw_item_message = scr_draw_item_message.Replace(
                """
                        draw_text_color(xx, (yy - 10), text1, c, c, c, c, 1)
                        c = c_white
                        draw_set_font(font_Mago2)
                        draw_text_color(xx, (yy + 1), text2, c, c, c, c, 1)
                """.ReplaceLineEndings("\n"),
                """
                        draw_text_color(xx, (yy - 10 - h / 2), text1, c, c, c, c, 1)
                        c = c_white
                        draw_set_font(font_Mago2)
                        draw_text_color(xx, (yy + 1 - h / 2), text2, c, c, c, c, 1)
                """.ReplaceLineEndings("\n")
            );

            scr_draw_item_message_code.ReplaceGML(scr_draw_item_message, gmData);
        }
    }
}
