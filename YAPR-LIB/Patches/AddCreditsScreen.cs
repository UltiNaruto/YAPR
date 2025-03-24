using UndertaleModLib;
using UndertaleModLib.Decompiler;
using YAPR_LIB.Utils;

namespace YAPR_LIB.Patches
{
    public static class AddCreditsScreen
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext, Room room, List<Text>? creditsString)
        {
            if (room == Room.rm_Novus)
                throw new NotImplementedException();

            if (creditsString is null)
                return;

            CodeUtils.CreateFunction(gmData, "scr_calculate_credits_length", """
                                                                             var credits = argument0;
                                                                             var len = 50
                                                                             var sub_texts = []
                                                                             for (var i = 0; i < array_length_2d(credits, 0); i++)
                                                                             {
                                                                                 len += 50
                                                                                 sub_texts = credits[0, i]
                                                                                 for (var j = 0; j < array_length_1d(sub_texts); j++)
                                                                                     len += 20
                                                                             }
                                                                             if (len == 50)
                                                                                return 0;
                                                                             len += 20
                                                                             return len
                                                                             """.ReplaceLineEndings("\n"));

            var obj_End_Screen_Create_0_code = gmData.Code.ByName("gml_Object_obj_End_Screen_Create_0");
            var obj_End_Screen_Create_0 = Decompiler.Decompile(obj_End_Screen_Create_0_code, decompileContext);
            var init_credits = "Credits = [\n";
            foreach (var entry in creditsString)
            {
                init_credits += $$"""
                                    [
                                        "{{entry.Header}}"
                                """;
                if (entry.Description?.Count > 0)
                {
                    init_credits += """
                                ,
                                        [

                                """;
                    foreach (var desc in entry.Description)
                    {
                        init_credits += $$"""
                                                    "{{desc}}",
                                          
                                        """;
                    }
                    init_credits = init_credits.Substring(0, init_credits.LastIndexOf(',')) + "\n";
                    init_credits += """
                                        ]

                                """;
                }

                init_credits += """
                                ],
                                
                            """;
            }
            init_credits = init_credits.Substring(0, init_credits.LastIndexOf(',')) + "\n";
            init_credits += """
                            ]
                                
                            """;
            init_credits += """
                            Credits_Scroll = 0
                            Credits_Scroll_Start_Time = 20
                            Credits_Displayed_Time = scr_calculate_credits_length(Credits)
                            Credits_Scroll_End_Time = (Credits_Displayed_Time - 50)
                            """;

            // patching the menu so you get only the planet you're playing            
            obj_End_Screen_Create_0 = obj_End_Screen_Create_0.Replace(
                """
                Go_To_Room = 1
                """.ReplaceLineEndings("\n"),
              $$"""
                Go_To_Room = 1
                {{init_credits}}
                """.ReplaceLineEndings("\n")
            );
            obj_End_Screen_Create_0_code.ReplaceGML(obj_End_Screen_Create_0, gmData);

            var obj_End_Screen_Step_0_code = gmData.Code.ByName("gml_Object_obj_End_Screen_Step_0");
            var obj_End_Screen_Step_0 = Decompiler.Decompile(obj_End_Screen_Step_0_code, decompileContext);

            if (room == Room.rm_Zebeth)
            {
                obj_End_Screen_Step_0 = obj_End_Screen_Step_0.Replace(
                    """
                        var lay = layer_get_id("Tiles_Ground")
                        layer_set_visible(lay, 1)
                    """.ReplaceLineEndings("\n"),
                    """
                        var lay = layer_get_id("Tiles_Ground")
                        layer_set_visible(lay, 1)
                        if (Timer < Credits_Scroll_End_Time)
                        {
                            if (Timer > Credits_Scroll_Start_Time)
                                Credits_Scroll++
                            obj_Samus_Ending.Timer = 0
                        }
                        obj_Samus_Ending.Active = Timer > Credits_Scroll_End_Time
                        obj_Samus_Ending.visible = Timer > Credits_Scroll_End_Time
                    """.ReplaceLineEndings("\n")
                );

                obj_End_Screen_Step_0 = obj_End_Screen_Step_0.Replace("if (input == 1 && Timer > 600)", "if (input == 1 && Timer > (Credits_Displayed_Time + 300))");
            }

            obj_End_Screen_Step_0_code.ReplaceGML(obj_End_Screen_Step_0, gmData);

            var obj_End_Screen_Draw_0_code = gmData.Code.ByName("gml_Object_obj_End_Screen_Draw_0");
            var obj_End_Screen_Draw_0 = Decompiler.Decompile(obj_End_Screen_Draw_0_code, decompileContext);

            if (room == Room.rm_Zebeth)
            {
                obj_End_Screen_Draw_0 = obj_End_Screen_Draw_0.Replace(
                    """
                        draw_set_color(c_white)
                        draw_set_font(font_Metroid)
                        draw_text(146, 15, "GREAT !!")
                        draw_text(64, 40, "YOU FULFILED YOUR MISSION.")
                        draw_text(64, 55, "IT WILL REVIVE PEACE IN")
                        draw_text(56, 70, "SPACE.")
                        draw_text(64, 85, "BUT,IT MAY BE INVADED BY")
                        draw_text(56, 100, "THE OTHER METROID.")
                        draw_text(64, 115, "PRAY FOR A TRUE PEACE IN")
                        draw_text(56, 130, "SPACE!")
                    """.ReplaceLineEndings("\n"),
                    """
                        if (Timer > Credits_Displayed_Time)
                        {
                            draw_set_color(c_white)
                            draw_set_font(font_Metroid)
                            draw_text(146, 15, "GREAT !!")
                            draw_text(64, 40, "YOU FULFILED YOUR MISSION.")
                            draw_text(64, 55, "IT WILL REVIVE PEACE IN")
                            draw_text(56, 70, "SPACE.")
                            draw_text(64, 85, "BUT,IT MAY BE INVADED BY")
                            draw_text(56, 100, "THE OTHER METROID.")
                            draw_text(64, 115, "PRAY FOR A TRUE PEACE IN")
                            draw_text(56, 130, "SPACE!")
                        }
                        else
                        {
                            if (Timer >= Credits_Scroll_End_Time && Timer <= Credits_Displayed_Time)
                                draw_set_alpha((1 - ((Timer - Credits_Scroll_End_Time) / (Credits_Displayed_Time - Credits_Scroll_End_Time))))
                            if (Timer <= Credits_Displayed_Time)
                            {
                                var c = c_black
                                draw_rectangle_color(0, 0, global.CAMERA_W, global.CAMERA_H, c, c, c, c, 0)
                            }
                            if (Timer > Credits_Scroll_Start_Time && array_length_2d(Credits, 0) > 0)
                            {
                                var xx = (global.CAMERA_W - global.GAME_SCREEN_W)
                                var yy = (global.CAMERA_H - Credits_Scroll)
                                var pickup = []
                                draw_set_color(c_white)
                                draw_set_font(font_Mago2)
                                draw_set_halign(fa_center)
                                draw_set_color(c_green)
                                draw_text_transformed(xx, yy, "MAJOR ITEM LOCATIONS", 3, 3, 0)
                                yy += 40
                                for (var i = 0; i < array_length_2d(Credits, 0); i++)
                                {
                                    pickup = Credits[0, i]
                                    if (yy >= 0 && yy < global.CAMERA_H)
                                    {
                                        draw_set_color(c_green)
                                        draw_text_transformed(xx, yy, pickup[0], 2, 2, 0)
                                    }
                                    if (array_length_1d(pickup) < 2)
                                        continue;
                                    yy += 10
                                    var pickup_locations = pickup[1]
                                    for (var j = 0; j < array_length_1d(pickup_locations); j++)
                                    {
                                        yy += 20
                                        if (yy >= 0 && yy < global.CAMERA_H)
                                        {
                                            draw_set_color(c_white)
                                            draw_text_transformed(xx, yy, pickup_locations[j], 1.5, 1.5, 0)
                                        }
                                    }
                                    yy += 30
                                }
                                draw_set_halign(fa_left)
                                draw_set_color(c_white)
                            }
                        }
                    """.ReplaceLineEndings("\n")
                );

                obj_End_Screen_Draw_0 = obj_End_Screen_Draw_0.Replace("var n = 300", "var n = Credits_Displayed_Time + 200");
            }

            obj_End_Screen_Draw_0_code.ReplaceGML(obj_End_Screen_Draw_0, gmData);
        }
    }
}