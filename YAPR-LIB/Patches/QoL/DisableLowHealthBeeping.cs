using UndertaleModLib;
using UndertaleModLib.Decompiler;
using YAPR_LIB.Utils;

namespace YAPR_LIB.Patches.QoL
{
    public static class DisableLowHealthBeeping
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext, bool defaultValue)
        {
            CodeUtils.AddGlobalVariables(gmData, decompileContext, new Dictionary<string, object>()
            {
                { "LOW_HP_BEEP", defaultValue }
            });

            var ts_setup_menu_code = gmData.Code.ByName("gml_Script_scr_TS_Setup_Menu");
            var ts_setup_menu = Decompiler.Decompile(ts_setup_menu_code, decompileContext);

            ts_setup_menu = ts_setup_menu.Replace("scr_TS_Build_Option(6, (xx - 80), (yy + (sep * 5.5)), 70, 14, \"BGM VOLUME\", \"\", 5, 7, 7, 3, -1, -1, -1)", "scr_TS_Build_Option(6, (xx - 80), (yy + (sep * 5.5)), 70, 14, \"BGM VOLUME\", \"\", 5, 7, 7, 3, -1, 8, -1)");

            ts_setup_menu = ts_setup_menu.Replace(
                """
                        scr_TS_Build_Option(7, (xx + 80), (yy + (sep * 5.5)), 70, 14, "SFX VOLUME", "", 6, 7, -1, 4, 6, -1, -1)
                """.ReplaceLineEndings("\n"),
                """
                        scr_TS_Build_Option(7, (xx + 80), (yy + (sep * 5.5)), 70, 14, "SFX VOLUME", "", 6, 7, -1, 4, 6, -1, -1)
                        scr_TS_Build_Option(8, (xx - 80), (yy + (sep * 7)), 70, 14, "LOW HP BEEP", "OFF-ON-", 7, 4, -1, 6, -1, -1, -1)
                """.ReplaceLineEndings("\n")
            );

            ts_setup_menu_code.ReplaceGML(ts_setup_menu, gmData);

            var ts_option_actions_code = gmData.Code.ByName("gml_Script_scr_TS_Option_Actions");
            var ts_option_actions = Decompiler.Decompile(ts_option_actions_code, decompileContext);

            ts_option_actions = ts_option_actions.Replace(
                """
                    if (action == 6)
                        Range_Edit = 1
                """.ReplaceLineEndings("\n"),
                """
                    if (action == 6)
                        Range_Edit = 1
                    if (action == 7)
                        global.LOW_HP_BEEP = wrap_val((global.LOW_HP_BEEP + 1), 0, 2)
                """.ReplaceLineEndings("\n")
            );

            ts_option_actions_code.ReplaceGML(ts_option_actions, gmData);

            var ts_update_option_code = gmData.Code.ByName("gml_Script_scr_TS_Update_Option");
            var ts_update_option = Decompiler.Decompile(ts_update_option_code, decompileContext);

            ts_update_option = ts_update_option.Replace(
                """
                }
                if (menu == 5)
                {
                """.ReplaceLineEndings("\n"),
                """
                    if (name == "LOW HP BEEP")
                    {
                        if (global.LOW_HP_BEEP == 0)
                            val = 0
                        if (global.LOW_HP_BEEP == 1)
                            val = 1
                    }
                }
                if (menu == 5)
                {
                """.ReplaceLineEndings("\n")
            );

            ts_update_option_code.ReplaceGML(ts_update_option, gmData);

            var obj_Samus_Step_0_code = gmData.Code.ByName("gml_Object_obj_Samus_Step_0");
            var obj_Samus_Step_0 = Decompiler.Decompile(obj_Samus_Step_0_code, decompileContext);

            obj_Samus_Step_0 = obj_Samus_Step_0.Replace(
                "if (sfx_active(22) == 0)",
                "if (sfx_active(22) == 0 && global.LOW_HP_BEEP == 1)"
            );

            obj_Samus_Step_0_code.ReplaceGML(obj_Samus_Step_0, gmData);

            var scr_Save_Options_code = gmData.Code.ByName("gml_Script_scr_Save_Options");
            var scr_Save_Options = Decompiler.Decompile(scr_Save_Options_code, decompileContext);

            scr_Save_Options = scr_Save_Options.Replace(
                """
                ini_write_real("GAME OPTIONS", "Aspect Ratio", global.ASPECT_RATIO)
                """.ReplaceLineEndings("\n"),
                """
                ini_write_real("GAME OPTIONS", "Aspect Ratio", global.ASPECT_RATIO)
                ini_write_real("GAME OPTIONS", "Low HP Beep", global.LOW_HP_BEEP)
                """.ReplaceLineEndings("\n")
            );

            scr_Save_Options_code.ReplaceGML(scr_Save_Options, gmData);

            var scr_Load_Options_code = gmData.Code.ByName("gml_Script_scr_Load_Options");
            var scr_Load_Options = Decompiler.Decompile(scr_Load_Options_code, decompileContext);

            scr_Load_Options = scr_Load_Options.Replace(
                """
                    global.ASPECT_RATIO = 3
                """.ReplaceLineEndings("\n"),
                """
                    global.ASPECT_RATIO = 3
                    global.LOW_HP_BEEP = ini_read_real("GAME OPTIONS", "Low HP Beep", 1)
                    global.LOW_HP_BEEP = clamp(round(global.LOW_HP_BEEP), 0, 1)
                """.ReplaceLineEndings("\n")
            );

            scr_Load_Options_code.ReplaceGML(scr_Load_Options, gmData);
        }
    }
}
