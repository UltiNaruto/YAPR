using UndertaleModLib;
using UndertaleModLib.Decompiler;
using YAPR_LIB.Utils;

namespace YAPR_LIB.Patches
{
    public static class CustomElevatorHandling
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext, Dictionary<int, Elevator> Elevators)
        {
            if (Elevators.Count == 0)
                return;

            var global_ELEVATORS = new Dictionary<int, Dictionary<string, int>>();
            foreach((int idx, Elevator elv) in Elevators)
            {
                global_ELEVATORS.Add(idx, elv.ToDictionary());
            }

            CodeUtils.AddGlobalVariables(gmData, decompileContext, new Dictionary<String, Object>()
            {
                {
                    "ELEVATORS", global_ELEVATORS
                }
            });

            // scr_Warp_From(elevator_id)
            CodeUtils.CreateFunction(gmData, "scr_Warp_From",
                """
                var elevator_id = string(argument0)
                var elevator = ds_map_find_value(global.ELEVATORS, elevator_id)
                if (is_undefined(elevator))
                    return 0;
                obj_MAIN.Screen_Cover_Alpha = 1
                obj_MAIN.Screen_Cover_Dir = -1
                if (ds_map_find_value(elevator, "end_game") > 0)
                    scr_Go_to_Ending()
                else
                {
                    obj_Samus.Riding = 0
                    obj_Samus.x = ds_map_find_value(elevator, "destination_x")
                    obj_Samus.y = ds_map_find_value(elevator, "destination_y")
                    screen_x = floor((obj_Samus.x / global.GAME_SCREEN_W))
                    screen_y = floor((obj_Samus.y / global.GAME_SCREEN_H))
                    var xx = ((screen_x * global.GAME_SCREEN_W) - ((camera_get_view_width(view_camera[0]) - global.GAME_SCREEN_W) / 2))
                    var yy = (screen_y * global.GAME_SCREEN_H)
                    camera_set_view_pos(view_camera[0], xx, yy)
                }
                return 1;
                """.ReplaceLineEndings("\n"));

            // obj_Elevator_Step_0
            var obj_Elevator_Step_0_code = gmData.Code.ByName("gml_Object_obj_Elevator_Step_0");
            var obj_Elevator_Step_0 = Decompiler.Decompile(obj_Elevator_Step_0_code, decompileContext);

            var insertIndex = obj_Elevator_Step_0.IndexOf("\n        if (stop == 1)");

            var customCode = """

                                     else
                                     {
                                         var starting_screen_x = floor((Starting_X / global.GAME_SCREEN_W))
                                         var starting_screen_y = floor((Starting_Y / global.GAME_SCREEN_H))
                                         screen_x = floor((x / global.GAME_SCREEN_W))
                                         screen_y = floor((y / global.GAME_SCREEN_H))
                                         if (starting_screen_x != screen_x || starting_screen_y != screen_y)
                                         {
                                             if (scr_Warp_From(id) == 1)
                                                 stop = 1
                                         }
                                     }
                             """.ReplaceLineEndings("\n");

            obj_Elevator_Step_0 = obj_Elevator_Step_0.Insert(insertIndex, customCode);

            obj_Elevator_Step_0_code.ReplaceGML(obj_Elevator_Step_0, gmData);
        }
    }
}
