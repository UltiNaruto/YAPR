using UndertaleModLib;
using UndertaleModLib.Decompiler;

namespace YAPR_LIB.Patches
{
    public static class EditMenu
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext, Room room)
        {
            var ts_setup_menu_code = gmData.Code.ByName("gml_Script_scr_TS_Setup_Menu");
            var ts_setup_menu = Decompiler.Decompile(ts_setup_menu_code, decompileContext);

            // patching the menu so you get only the planet you're playing
            if (room == Room.rm_Zebeth)
                ts_setup_menu = ts_setup_menu.Replace("        scr_TS_Build_Option(0, xx, (yy + sep * 1), 65, 9, \"PLANET ZEBETH\", \"\", 0, 1, -1, -1, -1, 1, -1)\n", "        scr_TS_Build_Option(0, xx, (yy + sep * 1), 65, 9, \"RANDOVANIA\", \"\", 0, 1, -1, -1, -1, 4, -1)\n");
            else
                ts_setup_menu = ts_setup_menu.Replace("        scr_TS_Build_Option(0, xx, (yy + sep * 1), 65, 9, \"PLANET ZEBETH\", \"\", 0, 1, -1, -1, -1, 1, -1)\n", "");
            if (room == Room.rm_Novus)
                ts_setup_menu = ts_setup_menu.Replace("        scr_TS_Build_Option(1, xx, (yy + sep * 2), 65, 9, \"PLANET NOVUS\", \"\", 1, 1, -1, 0, -1, 2, -1)\n", "        scr_TS_Build_Option(0, xx, (yy + sep * 1), 65, 9, \"RANDOVANIA\", \"\", 1, 1, -1, -1, -1, 4, -1)\n");
            else
                ts_setup_menu = ts_setup_menu.Replace("        scr_TS_Build_Option(1, xx, (yy + sep * 2), 65, 9, \"PLANET NOVUS\", \"\", 1, 1, -1, 0, -1, 2, -1)\n", "");
            // if mobile == 0
            ts_setup_menu = ts_setup_menu.Replace("        scr_TS_Build_Option(2, xx, (yy + sep * 3), 65, 9, \"PLANET ENIGMA\", \"\", 2, 1, -1, 1, -1, 3, -1)\n", "");
            ts_setup_menu = ts_setup_menu.Replace("        scr_TS_Build_Option(3, xx, (yy + sep * 4), 65, 9, \"CONNECT\", \"\", 3, 1, -1, 2, -1, 4, -1)\n", "");
            ts_setup_menu = ts_setup_menu.Replace("        scr_TS_Build_Option(4, xx, (yy + sep * 5), 65, 9, \"OPTIONS\", \"\", 4, 1, -1, 3, -1, 6, -1)\n", "        scr_TS_Build_Option(4, xx, (yy + sep * 2), 65, 9, \"OPTIONS\", \"\", 4, 1, -1, 0, -1, 7, -1)\n");
            ts_setup_menu = ts_setup_menu.Replace("        scr_TS_Build_Option(6, xx, (yy + sep * 6), 65, 9, \"CREDITS\", \"\", 5, 1, -1, 4, -1, 7, -1)\n", "");
            ts_setup_menu = ts_setup_menu.Replace("        scr_TS_Build_Option(7, xx, (yy + sep * 7), 65, 9, \"QUIT\", \"\", 6, 1, -1, 6, -1, -1, -1)\n", "        scr_TS_Build_Option(7, xx, (yy + sep * 3), 65, 9, \"QUIT\", \"\", 6, 1, -1, 4, -1, -1, -1)\n");
            // if mobile == 1
            ts_setup_menu = ts_setup_menu.Replace("        scr_TS_Build_Option(2, xx, (yy + sep * 3), 65, 9, \"CONNECT\", \"\", 3, 1, -1, 1, -1, 3, -1)\n", "");
            ts_setup_menu = ts_setup_menu.Replace("        scr_TS_Build_Option(3, xx, (yy + sep * 4), 65, 9, \"OPTIONS\", \"\", 4, 1, -1, 3, -1, 4, -1)\n", "        scr_TS_Build_Option(4, xx, (yy + sep * 2), 65, 9, \"OPTIONS\", \"\", 4, 1, -1, 0, -1, -1, -1)\n");
            ts_setup_menu = ts_setup_menu.Replace("        scr_TS_Build_Option(4, xx, (yy + sep * 6), 65, 9, \"CREDITS\", \"\", 5, 1, -1, 3, -1, -1, -1)\n", "");

            // remove all of the options when starting game
            ts_setup_menu = ts_setup_menu.Replace("    scr_TS_Build_Option(1, xx, (yy + 50), 110, 15, \"DIFFICULTY\", \"NORMAL-HARD-\", 2, 4, -1, 0, -1, 2, global.GAME_DIFFICULTY)\n", "");
            ts_setup_menu = ts_setup_menu.Replace("    scr_TS_Build_Option(2, xx, (yy + 90), 110, 15, \"RANDOMIZED MODE\", \"OFF-ON-\", 3, 4, -1, 1, -1, 3, global.GAME_MODE)\n", "");
            ts_setup_menu = ts_setup_menu.Replace("    scr_TS_Build_Option(3, xx, (yy + 118), 110, 9, \"MODIFY SEED\", \"\", 4, 3, -1, 2, -1, 4, Random_Seed)\n", "");
            ts_setup_menu = ts_setup_menu.Replace("    scr_TS_Build_Option(4, xx, (yy + 152), 110, 15, \"STRIKE MODE\", \"OFF-ON-\", 5, 4, -1, 3, -1, -1, global.GAME_MODE)\n", "");

            // reorganize options menu
            ts_setup_menu = ts_setup_menu.Replace(
                "    yy = 50",
                """
                    yy = 20
                    sep = 20
                """.ReplaceLineEndings("\n")
            );
            ts_setup_menu = ts_setup_menu.Replace("scr_TS_Build_Option(3, (xx - 80), (yy + sep * 4), 70, 14, \"WINDOW SCALE\", \"x2-x3-MAX-\", 2, 4, 6, 2, -1, 4, -1)", "scr_TS_Build_Option(3, (xx - 80), (yy + sep * 4), 70, 14, \"WINDOW SCALE\", \"x2-x3-MAX-\", 2, 4, 4, 2, -1, 6, -1)");
            ts_setup_menu = ts_setup_menu.Replace("scr_TS_Build_Option(4, (xx - 80), (yy + sep * 5.5), 70, 14, \"ASPECT RATIO\", \"16:9-4:3-\", 3, 4, 7, 3, -1, -1, -1)", "scr_TS_Build_Option(4, (xx + 80), (yy + sep * 4), 70, 14, \"ASPECT RATIO\", \"16:9-4:3-\", 3, 4, -1, 5, 3, 7, -1)");
            ts_setup_menu = ts_setup_menu.Replace("scr_TS_Build_Option(5, (xx + 80), (yy + sep * 2.5), 70, 14, \"PARTICLES\", \"OFF-ON-\", 4, 4, -1, 0, 2, 6, -1)", "scr_TS_Build_Option(5, (xx + 80), (yy + sep * 2.5), 70, 14, \"PARTICLES\", \"OFF-ON-\", 4, 4, -1, 0, 2, 4, -1)");
            ts_setup_menu = ts_setup_menu.Replace("scr_TS_Build_Option(6, (xx + 80), (yy + sep * 4), 70, 14, \"BGM VOLUME\", \"\", 5, 7, -1, 5, 3, 7, -1)", "scr_TS_Build_Option(6, (xx - 80), (yy + sep * 5.5), 70, 14, \"BGM VOLUME\", \"\", 5, 7, 7, 3, -1, -1, -1)");
            ts_setup_menu = ts_setup_menu.Replace("scr_TS_Build_Option(7, (xx + 80), (yy + sep * 5.5), 70, 14, \"SFX VOLUME\", \"\", 6, 7, -1, 6, 4, -1, -1)", "scr_TS_Build_Option(7, (xx + 80), (yy + sep * 5.5), 70, 14, \"SFX VOLUME\", \"\", 6, 7, -1, 4, 6, -1, -1)");

            ts_setup_menu = ts_setup_menu.Replace(
                """
                }
                if (floor(menu) == 5)
                {
                """.ReplaceLineEndings("\n"),
                """
                    sep = 25
                }
                if (floor(menu) == 5)
                {
                """.ReplaceLineEndings("\n")
            );
            ts_setup_menu_code.ReplaceGML(ts_setup_menu, gmData);

            var menu_info_panel_code = gmData.Code.ByName("gml_Script_scr_TS_Draw_Info_Panel");
            var menu_info_panel = Decompiler.Decompile(menu_info_panel_code, decompileContext);

            // tells which planet we're playing
            if (room == Room.rm_Zebeth)
                menu_info_panel = menu_info_panel.Replace(
                    """
                    if (index == 0)
                        text = "The original Metroid,\njust better."
                    """.ReplaceLineEndings("\n"),
                    """
                    if (index == 0)
                        text = "Planet Zebeth"
                    """.ReplaceLineEndings("\n")
                );
            if (room == Room.rm_Novus)
                menu_info_panel = menu_info_panel.Replace(
                    """
                    if (index == 0)
                        text = "The original Metroid,\njust better."
                    """.ReplaceLineEndings("\n"),
                    """
                    if (index == 0)
                        text = "Planet Novus"
                    """.ReplaceLineEndings("\n")
                );

            menu_info_panel_code.ReplaceGML(menu_info_panel, gmData);

            // remove update notification
            var NETWORK_Other_62_code = gmData.Code.ByName("gml_Object_obj_NETWORK_Other_62");
            var NETWORK_Other_62 = Decompiler.Decompile(NETWORK_Other_62_code, decompileContext);

            NETWORK_Other_62 = NETWORK_Other_62.Replace("if (_status == 0)", "if (false)");

            NETWORK_Other_62_code.ReplaceGML(NETWORK_Other_62, gmData);
        }
    }
}