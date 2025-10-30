using UndertaleModLib.Decompiler;
using UndertaleModLib;

namespace YAPR_LIB.Patches.QoL
{
    public static class WarpToStart
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext)
        {
            var scr_Draw_Pause_Screen_code = gmData.Code.ByName("gml_Script_scr_Draw_Pause_Screen");
            var scr_Draw_Pause_Screen = Decompiler.Decompile(scr_Draw_Pause_Screen_code, decompileContext);

            scr_Draw_Pause_Screen = scr_Draw_Pause_Screen.Replace(
                """
                xx = center_x - global.CAMERA_W - Pause_Menu_X
                yy = center_y + 25
                c = c_black
                if (Pause_Cursor == 1 && Pause_Menu_Select == 1)
                    c = make_color_rgb(25, 75, 150)
                scr_Draw_Text_Outline(xx, yy, "TITLE SCREEN", c_white, c, font_Metroid)
                draw_set_halign(fa_left)
                draw_set_alpha(1)
                """.ReplaceLineEndings("\n"),
                """
                xx = ((center_x - global.CAMERA_W) - Pause_Menu_X)
                yy = center_y
                c = c_black
                if (Pause_Cursor == 1 && Pause_Menu_Select == 1)
                    c = make_color_rgb(25, 75, 150)
                scr_Draw_Text_Outline(xx, yy, "WARP TO START", c_main, c, font_Metroid)
                xx = ((center_x - global.CAMERA_W) - Pause_Menu_X)
                yy = (center_y + 25)
                c = c_black
                if (Pause_Cursor == 2 && Pause_Menu_Select == 1)
                    c = make_color_rgb(25, 75, 150)
                scr_Draw_Text_Outline(xx, yy, "TITLE SCREEN", c_white, c, font_Metroid)
                draw_set_halign(fa_left)
                draw_set_alpha(1)
                """.ReplaceLineEndings("\n")
            );

            scr_Draw_Pause_Screen_code.ReplaceGML(scr_Draw_Pause_Screen, gmData);

            var scr_Handle_Pause_Menu_code = gmData.Code.ByName("gml_Script_scr_Handle_Pause_Menu");
            var scr_Handle_Pause_Menu = Decompiler.Decompile(scr_Handle_Pause_Menu_code, decompileContext);

            scr_Handle_Pause_Menu = scr_Handle_Pause_Menu.Replace(
                "Pause_Cursor = clamp(Pause_Cursor, 0, 1)",
                "Pause_Cursor = clamp(Pause_Cursor, 0, 2)"
            );

            scr_Handle_Pause_Menu = scr_Handle_Pause_Menu.Replace(
                """
                            if (Pause_Cursor == 1)
                                scr_Return_To_Title()
                """.ReplaceLineEndings("\n"),
                """
                            if (Pause_Cursor == 1 && Escaping == 0)
                            {
                                global.RUN_DATA[(2 << 0)] += 1
                                scr_Replay_Add_Stamp(2, 7)
                                if (audio_system_is_available() == 1)
                                    audio_stop_all()
                                Pause_Trans = -1
                                Screen_Cover_Alpha = 1
                                Screen_Cover_Dir = -1
                                obj_Samus.Riding = 0
                                if (audio_is_playing(sfx_Elevator_Hum) == 1)
                                    stop_sfx(0)
                                obj_Samus.x = global.START_LOCATION_X
                                obj_Samus.y = global.START_LOCATION_Y
                                obj_Samus.Energy = (99 + (100 * obj_Samus.Upgrade[7]))
                                obj_Samus.Ammo[1] = obj_Samus.Ammo_Cap[1]
                                obj_Samus.Ammo[2] = obj_Samus.Ammo_Cap[2]
                                obj_Samus.Ammo[3] = obj_Samus.Ammo_Cap[3]
                                var screen_x = floor((obj_Samus.x / global.GAME_SCREEN_W))
                                var screen_y = floor((obj_Samus.y / global.GAME_SCREEN_H))
                                var xx = ((screen_x * global.GAME_SCREEN_W) - ((camera_get_view_width(view_camera[0]) - global.GAME_SCREEN_W) / 2))
                                var yy = (screen_y * global.GAME_SCREEN_H)
                                camera_set_view_pos(view_camera[0], xx, yy)
                                Current_Event = (2 << 0)
                                Current_Event_Timer = 190
                                Spawn_Location = 0
                                play_bgm(bgm_Samus_Entrance, 0)
                            }
                            if (Pause_Cursor == 2)
                                scr_Return_To_Title()
                """.ReplaceLineEndings("\n")
            );

            scr_Handle_Pause_Menu_code.ReplaceGML(scr_Handle_Pause_Menu, gmData);

            var Replay_Add_Event_code = gmData.Code.ByName("gml_Script_Replay_Add_Event");
            var Replay_Add_Event = Decompiler.Decompile(Replay_Add_Event_code, decompileContext);

            Replay_Add_Event = Replay_Add_Event.Replace(
                """
                    if (num == 6)
                    {
                        event = "Saved"
                        Replay_Save_State()
                    }
                """.ReplaceLineEndings("\n"),
                """
                    if (num == 6)
                    {
                        event = "Saved"
                        Replay_Save_State()
                    }
                    if (num == 7)
                        event = "Warped To Start"
                """.ReplaceLineEndings("\n")
            );

            Replay_Add_Event_code.ReplaceGML(Replay_Add_Event, gmData);
        }
    }
}