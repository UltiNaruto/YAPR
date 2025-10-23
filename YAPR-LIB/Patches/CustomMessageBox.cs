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
                "Item_Message = 0\n",
                """
                Item_Message = 0
                Item_Message_Header = ""
                Item_Message_Description = ""
                """
            );

            obj_MAIN_Create_0_code.ReplaceGML(obj_MAIN_Create_0, gmData);

            // gml_Script_scr_Draw_Item_Message
            var scr_draw_item_message_code = gmData.Code.ByName("gml_Script_scr_Draw_Item_Message");
            var scr_draw_item_message = Decompiler.Decompile(scr_draw_item_message_code, decompileContext);

            var newCode = """
                          text1 = obj_MAIN.Item_Message_Header
                          text2 = obj_MAIN.Item_Message_Description
                          
                          """;

            var insertIndex = scr_draw_item_message.IndexOf("if (Item_Event_Type == 0)");

            scr_draw_item_message = scr_draw_item_message.Insert(insertIndex, newCode.ReplaceLineEndings("\n"));
            scr_draw_item_message_code.ReplaceGML(scr_draw_item_message, gmData);
        }
    }
}
