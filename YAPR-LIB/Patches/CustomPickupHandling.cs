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

            scr_Collect_Item = scr_Collect_Item.UnixReplace(
                "var quantity = 1\n",
                ""
            );

            scr_Collect_Item = scr_Collect_Item.UnixReplace(
                "if (player.Upgrade[item] > 0)",
                """
                var quantity = i.Item_Quantity
                if (player.Upgrade[item] > 0)
                """
            );

            scr_Collect_Item = scr_Collect_Item.UnixReplace(
                "if (scene_type == 1)",
                "if (i.Item_Acquired_Sound != bgm_Item_Get)"
            );

            scr_Collect_Item = scr_Collect_Item.UnixReplace(
                "        play_bgm(bgm_Minor_Item_Get, 0)\n",
                ""
            );

            scr_Collect_Item = scr_Collect_Item.UnixReplace(
                """
                    else
                        play_bgm(bgm_Item_Get, 0)
                """,
                """
                    play_bgm(i.Item_Acquired_Sound)
                    play_sfx(i.Item_Acquired_Sound)
                """
            );

            // checks if item has a launcher
            // then check if pick up was not launcher
            // then check if we ever picked up launcher
            // and decide if we display locked text or normal text
            scr_Collect_Item = scr_Collect_Item.UnixReplace(
                """
                    obj_MAIN.Item_Message = item

                """,
                """
                    obj_MAIN.Item_Message = item
                    if ((item == 16 && !i.Item_Is_Launcher && player.Upgrade[16] == 0) ||
                        (item == 17 && !i.Item_Is_Launcher && player.Upgrade[17] == 0))
                    {
                        obj_MAIN.Item_Message_Header = i.Item_Text_Locked_Header
                        obj_MAIN.Item_Message_Description = i.Item_Text_Locked_Description
                    }
                    else
                    {
                        obj_MAIN.Item_Message_Header = i.Item_Text_Header
                        obj_MAIN.Item_Message_Description = i.Item_Text_Description
                    }

                """
            );

            scr_Collect_Item = scr_Collect_Item.UnixReplace(
                "    obj_MAIN.Current_Event_Timer = 180",
                "    obj_MAIN.Current_Event_Timer = 120"
            );

            scr_Collect_Item = scr_Collect_Item.UnixReplace(
                """
                        obj_MAIN.Item_Event_Type = 1

                """,
                ""
            );

            scr_Collect_Item = scr_Collect_Item.UnixReplace(
                "player.Upgrade[item] += 1",
                """
                if (item == 7 || item == 30)
                    player.Upgrade[item] += 1
                else if (item == 16 || item == 17)
                {
                    if (i.Item_Is_Launcher == 1)
                        player.Upgrade[item] = 1
                }
                else
                    player.Upgrade[item] = 1
                if (item == 30 && player.Upgrade[item] > 9)
                    player.Upgrade[item] = 9
                """
            );

            scr_Collect_Item_code.ReplaceGML(scr_Collect_Item, gmData);
        }
    }
}
