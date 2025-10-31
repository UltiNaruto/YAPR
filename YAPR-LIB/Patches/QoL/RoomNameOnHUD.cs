using UndertaleModLib;
using UndertaleModLib.Decompiler;
using YAPR_LIB.Utils;

namespace YAPR_LIB.Patches.QoL
{
    public static class RoomNameOnHUD
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext, RoomGuiType defaultValue)
        {
            CodeUtils.AddGlobalVariables(gmData, decompileContext, new Dictionary<string, object>()
            {
                { "ROOM_NAME_ON_HUD", (int)defaultValue },
                { "CURRENT_ROOM", "" },
                { "ROOM_NAME_REMAINING_DISPLAY_TIME", 0 },
                { "ROOM_NAME_TIME_TO_DISPLAY", 125 }
            });

            CodeUtils.CreateFunction(gmData, "scr_Current_Room_Name", """
                                                                      var xx = argument0
                                                                      var yy = argument1

                                                                      if (room != rm_Zebeth)
                                                                          return ""

                                                                      // Brinstar
                                                                      if (xx >= 1 && xx <= 4 && yy == 13)
                                                                          return "Maru Mari Hall"
                                                                      if (xx == 5 && yy >= 13 && yy <= 17)
                                                                          return "Early Construction"
                                                                      if (xx == 6 && yy == 17)
                                                                          return "Kraid Prelude"
                                                                      if (xx >= 6 && xx <= 9 && yy == 13)
                                                                          return "The Overhang"
                                                                      if (xx == 10 && yy >= 1 && yy <= 13)
                                                                          return "Blue Tower"
                                                                      if (xx >= 7 && xx <= 9 && yy == 4)
                                                                          return "Item Corridor (Long Beam)"
                                                                      if (xx == 6 && yy == 4)
                                                                          return "Item Room (Long Beam)"
                                                                      if (xx == 11 && yy == 6)
                                                                          return "Tower Bridge"
                                                                      if (xx == 12 && yy >= 1 && yy <= 10)
                                                                          return "Orange Tower"
                                                                      if (xx >= 13 && xx <= 20 && yy == 10)
                                                                          return "Orange Corridor"
                                                                      if (xx == 21 && yy == 10)
                                                                          return "Norfair Prelude"
                                                                      if (xx >= 13 && xx <= 16 && yy == 6)
                                                                          return "Blue Rocks West"
                                                                      if (xx == 17 && yy >= 6 && yy <= 8)
                                                                          return "Blue Rock Connector West"
                                                                      if (xx >= 18 && xx <= 20 && yy == 6)
                                                                          return "Blue Rocks Central"
                                                                      if (xx == 21 && yy >= 6 && yy <= 8)
                                                                          return "Blue Rock Connector East"
                                                                      if (xx >= 19 && xx <= 20 && yy == 8)
                                                                          return "Item Corridor (Ice Beam)"
                                                                      if (xx == 18 && yy == 8)
                                                                          return "Item Room (Ice Beam)"
                                                                      if (xx >= 22 && xx <= 28 && yy == 6)
                                                                          return "Blue Rocks East"
                                                                      if (xx == 29 && yy >= 2 && yy <= 6)
                                                                          return "Orange Tower 2"
                                                                      if (xx >= 25 && xx <= 28 && yy == 4)
                                                                          return "Item Corridor (Bombs)"
                                                                      if (xx == 24 && yy == 4)
                                                                          return "Item Room (Bombs)"
                                                                      if (xx >= 20 && xx <= 28 && yy == 2)
                                                                          return "Green Sanctuary E"
                                                                      if (xx == 19 && yy >= 1 && yy <= 2)
                                                                          return "Green Hall"
                                                                      if (xx >= 15 && xx <= 18 && yy == 1)
                                                                          return "Item Corridor (Varia)"
                                                                      if (xx == 14 && yy == 1)
                                                                          return "Item Room (Varia)"
                                                                      if (xx >= 13 && xx <= 18 && yy == 2)
                                                                          return "Green Sanctuary W"
                                                                      if (xx >= 4 && xx <= 9 && yy == 1)
                                                                          return "Final Highway"
                                                                      if (xx == 3 && yy == 1)
                                                                          return "Statue Chamber"
                                                                      if (xx == 2 && yy == 1)
                                                                          return "Tourian Prelude"

                                                                      // Kraid
                                                                      if (xx == 6 && yy >= 19 && yy <= 23)
                                                                          return "Kraid Entrance"
                                                                      if (xx >= 7 && xx <= 9 && yy == 20)
                                                                          return "Upper Hall"
                                                                      if (xx == 10 && yy >= 16 && yy <= 20)
                                                                          return "Apex Tower"
                                                                      if (xx >= 8 && xx <= 9 && yy == 16)
                                                                          return "Upper Attic"
                                                                      if (xx >= 8 && xx <= 9 && yy == 17)
                                                                          return "Central Attic"
                                                                      if (xx >= 8 && xx <= 9 && yy == 18)
                                                                          return "Lower Attic"
                                                                      if (xx == 7 && yy >= 16 && yy <= 18)
                                                                          return "Attic Access"
                                                                      if (xx >= 1 && xx <= 5 && yy == 20)
                                                                          return "Arrival Chamber"
                                                                      if (xx == 0 && yy >= 20 && yy <= 26)
                                                                          return "Bottomless Pond"
                                                                      if (xx >= 1 && xx <= 5 && yy == 21)
                                                                          return "Pond Access"
                                                                      if (xx >= 7 && xx <= 9 && yy == 21)
                                                                          return "Blue Closet"
                                                                      if (xx >= 4 && xx <= 5 && yy == 25)
                                                                          return "Hall of Rejection"
                                                                      if (xx == 3 && yy >= 23 && yy <= 25)
                                                                          return "Stairway of Hope"
                                                                      if (xx >= 4 && xx <= 5 && yy == 23)
                                                                          return "Hall of Returns"
                                                                      if (xx >= 7 && xx <= 10 && yy == 23)
                                                                          return "Acid Gauntlet"
                                                                      if (xx == 11 && yy >= 23 && yy <= 28)
                                                                          return "Tower of Despair"
                                                                      if (xx >= 9 && xx <= 10 && yy == 24)
                                                                          return "Acid Pier"
                                                                      if (xx >= 9 && xx <= 10 && yy == 28)
                                                                          return "Waiting Room"
                                                                      if (xx == 8 && yy == 28)
                                                                          return "Kraid Arena Access"
                                                                      if (xx == 7 && yy == 28)
                                                                          return "Kraid Arena"
                                                                      if (xx == 8 && yy == 27)
                                                                          return "Green Parlor"
                                                                      if (xx >= 3 && xx <= 7 && yy == 27)
                                                                          return "Hall of Illusion"
                                                                      if (xx == 2 && yy >= 26 && yy <= 27)
                                                                          return "Drain"
                                                                      if (xx == 1 && yy == 26)
                                                                          return "Antechamber"
                                                                      if (xx >= 3 && xx <= 7 && yy == 26)
                                                                          return "Back Storage"
                                                                      if (xx == 8 && yy >= 25 && yy <= 26)
                                                                          return "Return Stairwell"
                                                                      if (xx == 7 && yy == 25)
                                                                          return "Back Entrance"
                                                                      if (xx == 9 && yy == 26)
                                                                          return "Upper Side Room Access"
                                                                      if (xx == 10 && yy >= 26 && yy <= 27)
                                                                          return "East Stairwell"
                                                                      if (xx == 9 && yy == 27)
                                                                          return "Lower Side Room Access"

                                                                      // Norfair
                                                                      if (xx == 21 && yy >= 12 && yy <= 13)
                                                                          return "Norfair Entrance"
                                                                      if (xx >= 3 && xx <= 20 && yy == 13)
                                                                          return "Bubble Wall"
                                                                      if (xx == 12 && yy >= 13 && yy <= 14)
                                                                          return "Questionable Stairs"
                                                                      if (xx >= 13 && xx <= 16 && yy == 14)
                                                                          return "Basalt Cache"
                                                                      if (xx >= 22 && xx <= 28 && yy == 13)
                                                                          return "Ember Hall"
                                                                      if (xx == 29 && yy >= 9 && yy <= 20)
                                                                          return "Bubble Tower"
                                                                      if (xx >= 24 && xx <= 28 && yy == 9)
                                                                          return "Hidden Cache"
                                                                      if (xx >= 25 && xx <= 28 && yy == 10)
                                                                          return "Weapons Cache"
                                                                      if (xx >= 26 && xx <= 28 && yy == 11)
                                                                          return "Item Trial (Ice Beam)"
                                                                      if (xx >= 24 && xx <= 25 && yy == 11)
                                                                          return "Item Room (Ice Beam)"
                                                                      if (xx == 23 && yy >= 9 && yy <= 11)
                                                                          return "Cache Access"
                                                                      if (xx >= 18 && xx <= 28 && yy == 14)
                                                                          return "Gauntlet"
                                                                      if (xx >= 27 && xx <= 28 && yy == 16)
                                                                          return "Item Trial (High Jump)"
                                                                      if (xx >= 22 && xx <= 26 && yy == 16)
                                                                          return "Item Room (High Jump)"
                                                                      if (xx == 21 && yy == 16)
                                                                          return "Runway"
                                                                      if (xx >= 18 && xx <= 20 && yy == 16)
                                                                          return "Lava Entryway"
                                                                      if (xx == 17 && yy >= 15 && yy <= 17)
                                                                          return "Bubble Atrium"
                                                                      if (xx >= 15 && xx <= 16 && yy == 15)
                                                                          return "Item Trial (Screw Attack)"
                                                                      if (xx == 14 && yy == 15)
                                                                          return "Item Room (Screw Attack)"
                                                                      if (xx >= 18 && xx <= 27 && yy == 17)
                                                                          return "Gumdrop Hall"
                                                                      if (xx == 28 && yy >= 17 && yy <= 18)
                                                                          return "Lava Pier"
                                                                      if (xx >= 25 && xx <= 27 && yy == 18)
                                                                          return "Fire Sea"
                                                                      if (xx >= 13 && xx <= 16 && yy == 16)
                                                                          return "Upper Lava Highway"
                                                                      if (xx >= 13 && xx <= 16 && yy == 17)
                                                                          return "Lower Lava Highway"
                                                                      if (xx == 12 && yy >= 16 && yy <= 19)
                                                                          return "Highway Access"
                                                                      if (xx >= 13 && xx <= 15 && yy == 19)
                                                                          return "Destroyed Highway"
                                                                      if (xx == 16 && yy >= 19 && yy <= 21)
                                                                          return "Bubble Corridor West"
                                                                      if (xx >= 17 && xx <= 19 && yy == 21)
                                                                          return "Bubble Cache"
                                                                      if (xx >= 17 && xx <= 19 && yy == 19)
                                                                          return "Bubble Pillars"
                                                                      if (xx == 20 && yy >= 19 && yy <= 20)
                                                                          return "Bubble Corridor East"
                                                                      if (xx >= 21 && xx <= 27 && yy == 19)
                                                                          return "Path of Destruction"
                                                                      if (xx >= 18 && xx <= 19 && yy == 20)
                                                                          return "Item Trial (Wave Beam)"
                                                                      if (xx == 17 && yy == 20)
                                                                          return "Item Room (Wave Beam)"
                                                                      if (xx >= 24 && xx <= 28 && yy == 20)
                                                                          return "Bubble Hall"
                                                                      if (xx == 23 && yy >= 20 && yy <= 21)
                                                                          return "Purple Stairwell"
                                                                      if (xx == 24 && yy == 21)
                                                                          return "Ridley Prelude"

                                                                      // Ridley
                                                                      if (xx == 24 && yy >= 23 && yy <= 24)
                                                                          return "Ridley Entrance"
                                                                      if (xx == 23 && yy == 24)
                                                                          return "Block Room E"
                                                                      if (xx == 22 && yy >= 23 && yy <= 24)
                                                                          return "Block Room W"
                                                                      if (xx >= 17 && xx <= 21 && yy == 23)
                                                                          return "Purple Gauntlet"
                                                                      if (xx >= 17 && xx <= 21 && yy == 24)
                                                                          return "Hall of Pillars"
                                                                      if (xx == 16 && yy >= 24 && yy <= 26)
                                                                          return "Artificial Passage"
                                                                      if (xx >= 14 && xx <= 15 && yy == 24)
                                                                          return "Dessgeega Attic"
                                                                      if (xx >= 14 && xx <= 15 && yy == 26)
                                                                          return "Path of Reflection"
                                                                      if (xx == 13 && yy >= 24 && yy <= 29)
                                                                          return "Tower of Doom"
                                                                      if (xx >= 17 && xx <= 20 && yy == 26)
                                                                          return "Green Bridge"
                                                                      if (xx == 21 && yy >= 26 && yy <= 28)
                                                                          return "Gambit Tower"
                                                                      if (xx >= 22 && xx <= 23 && yy == 26)
                                                                          return "Central Cache"
                                                                      if (xx >= 17 && xx <= 20 && yy == 28)
                                                                          return "Ominous Hall"
                                                                      if (xx == 16 && yy == 28)
                                                                          return "Ridley Arena"
                                                                      if (xx >= 14 && xx <= 15 && yy == 28)
                                                                          return "Lava Reliquary"
                                                                      if (xx >= 22 && xx <= 28 && yy == 28)
                                                                          return "Green Gauntlet"
                                                                      if (xx == 29 && yy >= 24 && yy <= 29)
                                                                          return "Purple Tower"
                                                                      if (xx >= 25 && xx <= 28 && yy == 24)
                                                                          return "Lava Foyer"
                                                                      if (xx >= 24 && xx <= 28 && yy == 26)
                                                                          return "The Wall"
                                                                      if (xx >= 14 && xx <= 28 && yy == 29)
                                                                          return "Infinite Gauntlet"

                                                                      // Tourian
                                                                      if (xx == 2 && yy >= 3 && yy <= 6)
                                                                          return "Tourian Entrance"
                                                                      if (xx >= 3 && xx <= 8 && yy == 6)
                                                                          return "Testing Chamber"
                                                                      if (xx == 9 && yy >= 6 && yy <= 10)
                                                                          return "Rinka Tower"
                                                                      if (xx >= 4 && xx <= 8 && yy == 10)
                                                                          return "Pipe Hall"
                                                                      if (xx >= 1 && xx <= 3 && yy == 10)
                                                                          return "Command Center"
                                                                      if (xx == 0 && yy >= 2 && yy <= 10)
                                                                          return "Escape Route"

                                                                      return "???"
                                                                      """);

            var ts_setup_menu_code = gmData.Code.ByName("gml_Script_scr_TS_Setup_Menu");
            var ts_setup_menu = Decompiler.Decompile(ts_setup_menu_code, decompileContext);

            ts_setup_menu = ts_setup_menu.Replace(
                "scr_TS_Build_Option(10, (xx - 80), (yy + (sep * 8.5)), 70, 14, \"ALT ESCAPE THEME\", \"OFF-ON-\", 9, 4, 11, 8, -1, -1, -1)",
                "scr_TS_Build_Option(10, (xx - 80), (yy + (sep * 8.5)), 70, 14, \"ALT ESCAPE THEME\", \"OFF-ON-\", 9, 4, 11, 8, -1, 12, -1)"
            );

            ts_setup_menu = ts_setup_menu.Replace(
                """
                    scr_TS_Build_Option(11, (xx + 80), (yy + (sep * 8.5)), 70, 14, "SHOW UNEXPLORED MAP", "OFF-ON-", 10, 4, -1, 9, 10, -1, -1)
                """,
                """
                    scr_TS_Build_Option(11, (xx + 80), (yy + (sep * 8.5)), 70, 14, "SHOW UNEXPLORED MAP", "OFF-ON-", 10, 4, -1, 9, 10, 12, -1)
                    scr_TS_Build_Option(12, xx, (yy + (sep * 10)), 110, 14, "ROOM NAME ON HUD", "NONE-WITH FADE-ALWAYS-", 11, 4, 11, -1, 10, -1, -1)
                """
            );

            ts_setup_menu_code.ReplaceGML(ts_setup_menu, gmData);

            var ts_option_actions_code = gmData.Code.ByName("gml_Script_scr_TS_Option_Actions");
            var ts_option_actions = Decompiler.Decompile(ts_option_actions_code, decompileContext);

            ts_option_actions = ts_option_actions.Replace(
                """
                    if (action == 10)
                        global.SHOW_UNEXPLORED_MAP = wrap_val((global.SHOW_UNEXPLORED_MAP + 1), 0, 2)
                """,
                """
                    if (action == 10)
                        global.SHOW_UNEXPLORED_MAP = wrap_val((global.SHOW_UNEXPLORED_MAP + 1), 0, 2)
                    if (action == 11)
                        global.ROOM_NAME_ON_HUD = wrap_val((global.ROOM_NAME_ON_HUD + 1), 0, 3)
                """
            );

            ts_option_actions_code.ReplaceGML(ts_option_actions, gmData);

            var ts_update_option_code = gmData.Code.ByName("gml_Script_scr_TS_Update_Option");
            var ts_update_option = Decompiler.Decompile(ts_update_option_code, decompileContext);

            ts_update_option = ts_update_option.Replace(
                """
                }
                if (menu == 5)
                {
                """,
                """
                    if (name == "ROOM NAME ON HUD")
                    {
                        if (global.ROOM_NAME_ON_HUD == 0)
                            val = 0
                        if (global.ROOM_NAME_ON_HUD == 1)
                            val = 1
                        if (global.ROOM_NAME_ON_HUD == 2)
                            val = 2
                    }
                }
                if (menu == 5)
                {
                """
            );

            ts_update_option_code.ReplaceGML(ts_update_option, gmData);

            // display room name when transitioning room through a door
            var scr_Handle_Door_Transition_code = gmData.Code.ByName("gml_Script_scr_Handle_Door_Transition");
            var scr_Handle_Door_Transition = Decompiler.Decompile(scr_Handle_Door_Transition_code, decompileContext);

            scr_Handle_Door_Transition = scr_Handle_Door_Transition.Replace(
                """
                    if (Door_Transition == 3)
                    {
                        var time = 40

                """,
                """
                    // Don't show the current room if transitioning
                    if (Door_Transition == 3)
                    {
                        global.CURRENT_ROOM = ""
                        var time = 40

                """
            );

            scr_Handle_Door_Transition = scr_Handle_Door_Transition.Replace(
                """
                        Room_Transition_Timer -= 1
                        if (Room_Transition_Timer <= 0)
                        {

                """,
                """
                        Room_Transition_Timer -= 1
                        if (Room_Transition_Timer <= 0)
                        {
                            global.CURRENT_ROOM = scr_Current_Room_Name(Screen_X, Screen_Y)
                            global.ROOM_NAME_REMAINING_DISPLAY_TIME = global.ROOM_NAME_TIME_TO_DISPLAY

                """
            );

            scr_Handle_Door_Transition_code.ReplaceGML(scr_Handle_Door_Transition, gmData);

            // display room name when world is loaded
            var world_load_code = gmData.Code.ByName("gml_Script_World_Load");
            var world_load = Decompiler.Decompile(world_load_code, decompileContext);

            var stringPrecedingInsert = """
                                                if (obj_NETWORK.Connection_Pos > -1)
                                                    NET_Apply_Shared_Data()
                                        """;
            var insertIndex = world_load.IndexOf(stringPrecedingInsert) + stringPrecedingInsert.Length + 1;
            

            var newCode = $$"""
                            global.CURRENT_ROOM = scr_Current_Room_Name(Screen_X, Screen_Y)
                            global.ROOM_NAME_REMAINING_DISPLAY_TIME = global.ROOM_NAME_TIME_TO_DISPLAY

                            """;

            world_load = world_load.Insert(insertIndex, newCode);

            world_load_code.ReplaceGML(world_load, gmData);

            // display room name when using elevator
            var obj_Elevator_Step_0_code = gmData.Code.ByName("gml_Object_obj_Elevator_Step_0");
            var obj_Elevator_Step_0 = Decompiler.Decompile(obj_Elevator_Step_0_code, decompileContext);

            insertIndex = obj_Elevator_Step_0.IndexOf("    if (obj_Samus.Dead > 0)\n");

            newCode = $$"""
                        global.CURRENT_ROOM = ""

                        """;

            obj_Elevator_Step_0 = obj_Elevator_Step_0.Insert(insertIndex, newCode);

            obj_Elevator_Step_0 = obj_Elevator_Step_0.Replace(
                """
                            stop = 1

                """,
              $$"""
                        {
                            stop = 1
                            var screen_x = floor((x / global.GAME_SCREEN_W))
                            var screen_y = floor((y / global.GAME_SCREEN_H))
                            global.CURRENT_ROOM = scr_Current_Room_Name(screen_x, screen_y)
                            global.ROOM_NAME_REMAINING_DISPLAY_TIME = global.ROOM_NAME_TIME_TO_DISPLAY
                        }

                """
            );

            obj_Elevator_Step_0_code.ReplaceGML(obj_Elevator_Step_0, gmData);

            // display room name on warp to start
            var scr_Handle_Pause_Menu_code = gmData.Code.ByName("gml_Script_scr_Handle_Pause_Menu");
            var scr_Handle_Pause_Menu = Decompiler.Decompile(scr_Handle_Pause_Menu_code, decompileContext);

            scr_Handle_Pause_Menu = scr_Handle_Pause_Menu.Replace(
                """
                                play_bgm(bgm_Samus_Entrance, 0)

                """,
                """
                                play_bgm(bgm_Samus_Entrance, 0)
                                var screen_x = floor((global.START_LOCATION_X / global.GAME_SCREEN_W))
                                var screen_y = floor((global.START_LOCATION_Y / global.GAME_SCREEN_H))
                                global.CURRENT_ROOM = scr_Current_Room_Name(screen_x, screen_y)
                                global.ROOM_NAME_REMAINING_DISPLAY_TIME = global.ROOM_NAME_TIME_TO_DISPLAY

                """
            );

            scr_Handle_Pause_Menu_code.ReplaceGML(scr_Handle_Pause_Menu, gmData);

            var scr_Draw_HUD_code = gmData.Code.ByName("gml_Script_scr_Draw_HUD");
            var scr_Draw_HUD = Decompiler.Decompile(scr_Draw_HUD_code, decompileContext);

            scr_Draw_HUD = scr_Draw_HUD.Replace(
                "    scr_Draw_Text_Outline(xx, yy, text, c_lime, c_black, font_Mago2)",
                """
                    scr_Draw_Text_Outline(xx, yy, text, c_lime, c_black, font_Mago2)
                    if (global.ROOM_NAME_ON_HUD >= 1 && global.ROOM_NAME_ON_HUD <= 2)
                    {
                        if (global.ROOM_NAME_ON_HUD == 1 && global.ROOM_NAME_REMAINING_DISPLAY_TIME > 0)
                        {
                            //var fade_in = (global.ROOM_NAME_TIME_TO_DISPLAY * 0.9)
                            var fade_out = (global.ROOM_NAME_TIME_TO_DISPLAY * 0.1)
                            //if (global.ROOM_NAME_REMAINING_DISPLAY_TIME >= fade_in && global.ROOM_NAME_REMAINING_DISPLAY_TIME <= global.ROOM_NAME_TIME_TO_DISPLAY)
                            //    draw_set_alpha((global.ROOM_NAME_TIME_TO_DISPLAY - global.ROOM_NAME_REMAINING_DISPLAY_TIME) / fade_in)
                            //else
                            if (global.ROOM_NAME_REMAINING_DISPLAY_TIME >= 0 && global.ROOM_NAME_REMAINING_DISPLAY_TIME <= fade_out)
                                draw_set_alpha(global.ROOM_NAME_REMAINING_DISPLAY_TIME / fade_out)
                            else
                                draw_set_alpha(1)
                            global.ROOM_NAME_REMAINING_DISPLAY_TIME--
                        }
                        if (global.ROOM_NAME_REMAINING_DISPLAY_TIME == 0)
                            global.CURRENT_ROOM = ""
                        var orig_valign = draw_get_valign()
                        draw_set_valign(fa_bottom)
                        draw_set_color(c_white)
                        draw_text_ext((cam_x + (border_w / 2)), ((cam_y + cam_h) - 5), global.CURRENT_ROOM, 10, (border_w - 20))
                        draw_set_color(c_lime)
                        draw_set_valign(orig_valign)
                        draw_set_alpha(1)
                    }
                """
            );

            scr_Draw_HUD_code.ReplaceGML(scr_Draw_HUD, gmData);

            var scr_Save_Options_code = gmData.Code.ByName("gml_Script_scr_Save_Options");
            var scr_Save_Options = Decompiler.Decompile(scr_Save_Options_code, decompileContext);

            scr_Save_Options = scr_Save_Options.Replace(
                """
                ini_write_real("GAME OPTIONS", "Show Unexplored Map", global.SHOW_UNEXPLORED_MAP)
                """,
                """
                ini_write_real("GAME OPTIONS", "Show Unexplored Map", global.SHOW_UNEXPLORED_MAP)
                ini_write_real("GAME OPTIONS", "Room Name on HUD", global.ROOM_NAME_ON_HUD)
                """
            );

            scr_Save_Options_code.ReplaceGML(scr_Save_Options, gmData);

            var scr_Load_Options_code = gmData.Code.ByName("gml_Script_scr_Load_Options");
            var scr_Load_Options = Decompiler.Decompile(scr_Load_Options_code, decompileContext);

            scr_Load_Options = scr_Load_Options.Replace(
                """
                global.SHOW_UNEXPLORED_MAP = ini_read_real("GAME OPTIONS", "Show Unexplored Map", 0)
                """,
                """
                global.SHOW_UNEXPLORED_MAP = ini_read_real("GAME OPTIONS", "Show Unexplored Map", 0)
                global.ROOM_NAME_ON_HUD = ini_read_real("GAME OPTIONS", "Room Name on HUD", 0)
                global.ROOM_NAME_ON_HUD = clamp(round(global.ROOM_NAME_ON_HUD), 0, 2)
                """
            );

            scr_Load_Options_code.ReplaceGML(scr_Load_Options, gmData);
        }
    }
}
