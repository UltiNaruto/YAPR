using UndertaleModLib;
using UndertaleModLib.Decompiler;
using YAPR_LIB.Utils;

namespace YAPR_LIB.Patches.QoL
{
    public static class ShowUnexploredMap
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext, bool defaultValue)
        {
            CodeUtils.AddGlobalVariables(gmData, decompileContext, new Dictionary<string, object>()
            {
                { "SHOW_UNEXPLORED_MAP", defaultValue }
            });

            var ts_setup_menu_code = gmData.Code.ByName("gml_Script_scr_TS_Setup_Menu");
            var ts_setup_menu = Decompiler.Decompile(ts_setup_menu_code, decompileContext);

            ts_setup_menu = ts_setup_menu.UnixReplace(
                "scr_TS_Build_Option(9, (xx + 80), (yy + sep * 7), 70, 14, \"SM BOSS THEME\", \"OFF-ON-\", 8, 4, -1, 7, 8, -1, -1)",
                "scr_TS_Build_Option(9, (xx + 80), (yy + sep * 7), 70, 14, \"SM BOSS THEME\", \"OFF-ON-\", 8, 4, -1, 7, 8, 11, -1)"
            );

            ts_setup_menu = ts_setup_menu.UnixReplace(
                """
                    scr_TS_Build_Option(10, (xx - 80), (yy + sep * 8.5), 70, 14, "ALT ESCAPE THEME", "OFF-ON-", 9, 4, -1, 8, -1, -1, -1)
                """,
                """
                    scr_TS_Build_Option(10, (xx - 80), (yy + sep * 8.5), 70, 14, "ALT ESCAPE THEME", "OFF-ON-", 9, 4, 11, 8, -1, -1, -1)
                    scr_TS_Build_Option(11, (xx + 80), (yy + sep * 8.5), 70, 14, "SHOW UNEXPLORED MAP", "OFF-ON-", 10, 4, -1, 9, 10, -1, -1)
                """
            );

            ts_setup_menu_code.ReplaceGML(ts_setup_menu, gmData);

            var ts_option_actions_code = gmData.Code.ByName("gml_Script_scr_TS_Option_Actions");
            var ts_option_actions = Decompiler.Decompile(ts_option_actions_code, decompileContext);

            ts_option_actions = ts_option_actions.UnixReplace(
                """
                    if (action == 9)
                        global.ALTERNATIVE_ESCAPE_THEME = wrap_val((global.ALTERNATIVE_ESCAPE_THEME + 1), 0, 2)
                """,
                """
                    if (action == 9)
                        global.ALTERNATIVE_ESCAPE_THEME = wrap_val((global.ALTERNATIVE_ESCAPE_THEME + 1), 0, 2)
                    if (action == 10)
                        global.SHOW_UNEXPLORED_MAP = wrap_val((global.SHOW_UNEXPLORED_MAP + 1), 0, 2)
                """
            );

            ts_option_actions_code.ReplaceGML(ts_option_actions, gmData);

            var ts_update_option_code = gmData.Code.ByName("gml_Script_scr_TS_Update_Option");
            var ts_update_option = Decompiler.Decompile(ts_update_option_code, decompileContext);

            ts_update_option = ts_update_option.UnixReplace(
                """
                }
                if (menu == 5)
                {
                """,
                """
                    if (name == "SHOW UNEXPLORED MAP")
                    {
                        if (global.SHOW_UNEXPLORED_MAP == 0)
                            val = 0
                        if (global.SHOW_UNEXPLORED_MAP == 1)
                            val = 1
                    }
                }
                if (menu == 5)
                {
                """
            );

            ts_update_option_code.ReplaceGML(ts_update_option, gmData);

            var scr_Draw_Map_Screen_code = gmData.Code.ByName("gml_Script_scr_Draw_Map_Screen");
            var scr_Draw_Map_Screen = Decompiler.Decompile(scr_Draw_Map_Screen_code, decompileContext);

            scr_Draw_Map_Screen = scr_Draw_Map_Screen.UnixReplace(
                "if (global.Map_Data[screen_n, (8 << 0)] == 0 && global.DEBUG_RANDOMIZER == 0)",
                "if (global.SHOW_UNEXPLORED_MAP == 0 && global.Map_Data[screen_n, (8 << 0)] == 0 && global.DEBUG_RANDOMIZER == 0)"
            );

            scr_Draw_Map_Screen_code.ReplaceGML(scr_Draw_Map_Screen, gmData);

            var scr_Save_Options_code = gmData.Code.ByName("gml_Script_scr_Save_Options");
            var scr_Save_Options = Decompiler.Decompile(scr_Save_Options_code, decompileContext);

            scr_Save_Options = scr_Save_Options.UnixReplace(
                """
                ini_write_real("GAME OPTIONS", "Alternative Escape Theme", global.ALTERNATIVE_ESCAPE_THEME)
                """,
                """
                ini_write_real("GAME OPTIONS", "Alternative Escape Theme", global.ALTERNATIVE_ESCAPE_THEME)
                ini_write_real("GAME OPTIONS", "Show Unexplored Map", global.SHOW_UNEXPLORED_MAP)
                """
            );

            scr_Save_Options_code.ReplaceGML(scr_Save_Options, gmData);

            var scr_Load_Options_code = gmData.Code.ByName("gml_Script_scr_Load_Options");
            var scr_Load_Options = Decompiler.Decompile(scr_Load_Options_code, decompileContext);

            scr_Load_Options = scr_Load_Options.UnixReplace(
                """
                global.ALTERNATIVE_ESCAPE_THEME = clamp(round(global.ALTERNATIVE_ESCAPE_THEME), 0, 1)
                """,
                """
                global.ALTERNATIVE_ESCAPE_THEME = clamp(round(global.ALTERNATIVE_ESCAPE_THEME), 0, 1)
                global.SHOW_UNEXPLORED_MAP = ini_read_real("GAME OPTIONS", "Show Unexplored Map", 0)
                global.SHOW_UNEXPLORED_MAP = clamp(round(global.SHOW_UNEXPLORED_MAP), 0, 1)
                """
            );

            scr_Load_Options_code.ReplaceGML(scr_Load_Options, gmData);
        }
    }
}
