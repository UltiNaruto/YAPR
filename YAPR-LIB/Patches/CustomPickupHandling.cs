using UndertaleModLib;
using UndertaleModLib.Decompiler;

namespace YAPR_LIB.Patches
{
    public static class CustomPickupHandling
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext)
        {
            // scr_Collect_Item
            var scr_Collect_Item_code = gmData.Code.ByName("gml_Script_scr_Collect_Item");
            var scr_Collect_Item = Decompiler.Decompile(scr_Collect_Item_code, decompileContext);

            scr_Collect_Item = scr_Collect_Item.Replace("var quantity = 1\n", "");
            scr_Collect_Item = scr_Collect_Item.Replace("\nif (player.Upgrade[item] > 0)", "\nvar quantity = i.Item_Quantity\nif (player.Upgrade[item] > 0)");
            scr_Collect_Item = scr_Collect_Item.Replace("if (scene_type == 1)", "if (i.Item_Acquired_Sound != bgm_Item_Get)");
            scr_Collect_Item = scr_Collect_Item.Replace("        play_bgm(bgm_Minor_Item_Get, 0)\n", "");
            scr_Collect_Item = scr_Collect_Item.Replace("    else\n        play_bgm(bgm_Item_Get, 0)\n", "    play_bgm(i.Item_Acquired_Sound)\n    play_sfx(i.Item_Acquired_Sound)");
            scr_Collect_Item = scr_Collect_Item.Replace("    obj_MAIN.Item_Message = item\n", "    obj_MAIN.Item_Message = item\n    obj_MAIN.Item_Message_Header = i.Item_Text_Header\n    obj_MAIN.Item_Message_Description = i.Item_Text_Description\n");
            scr_Collect_Item = scr_Collect_Item.Replace("    obj_MAIN.Current_Event_Timer = 180", "    obj_MAIN.Current_Event_Timer = 120");
            scr_Collect_Item = scr_Collect_Item.Replace("        obj_MAIN.Item_Event_Type = 1\n", "");
            scr_Collect_Item = scr_Collect_Item.Replace("player.Upgrade[item] += 1", "if (item == 7 || item == 16 || item == 17 || item == 30)\n    player.Upgrade[item] += 1\nelse\n    player.Upgrade[item] = 1\nif (item == 30 && player.Upgrade[item] > 9)\n    player.Upgrade[item] = 9");

            scr_Collect_Item_code.ReplaceGML(scr_Collect_Item, gmData);
        }
    }
}
