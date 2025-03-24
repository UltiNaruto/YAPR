using UndertaleModLib;
using UndertaleModLib.Decompiler;
using UndertaleModLib.Models;
using YAPR_LIB.Utils;

namespace YAPR_LIB.Patches
{
    public static class MoveSavesToRandovaniaFolder
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext, Room room, string wordHash)
        {
            var code = default(UndertaleCode);
            var code_str = string.Empty;
            var SCRIPTS = new string[] {
                "gml_Script_scr_Save_Data_Init",
                "gml_Script_scr_Clear_Save_Data",
                "gml_Script_int_Saves",
                "gml_Script_scr_Save_Completed",
                "gml_Script_World_Load"
            };

            var newZebethFilePath = $"Randovania/{wordHash}/Zebeth-";
            var newNovusFilePath = $"Randovania/{wordHash}/Novus-";

            foreach(var script in SCRIPTS)
            {
                code = gmData.Code.ByName(script);
                code_str = Decompiler.Decompile(code, decompileContext);
                // prevent using vanilla saves
                if (room == Room.rm_Zebeth)
                {
                    code_str = code_str.Replace("world[1] = \"Zebeth-\"", $"world[1] = \"{newZebethFilePath}\"");
                    code_str = code_str.Replace("world[(1 << 0)] = \"Zebeth-\"", $"world[1] = \"{newZebethFilePath}\"");
                }
                if (room == Room.rm_Novus)
                {
                    code_str = code_str.Replace("world[2] = \"Novus-\"", $"world[2] = \"{newNovusFilePath}\"");
                    code_str = code_str.Replace("world[(2 << 0)] = \"Novus-\"", $"world[2] = \"{newNovusFilePath}\"");
                }
                code.ReplaceGML(code_str, gmData);
            }

            // Init keys for new save data
            var scr_Save_Data_Init_code = gmData.Code.ByName("gml_Script_scr_Save_Data_Init");
            var scr_Save_Data_Init = Decompiler.Decompile(scr_Save_Data_Init_code, decompileContext);
            scr_Save_Data_Init = scr_Save_Data_Init.Replace(
                "var game = ds_map_create()\n",
                """
                var game = ds_map_create()
                ds_map_add(game, "Tourian Keys", obj_Samus.Upgrade[30])

                """.ReplaceLineEndings("\n")
            );

            scr_Save_Data_Init_code.ReplaceGML(scr_Save_Data_Init, gmData);

            // Save keys to save data
            var scr_Save_Data_Full_code = gmData.Code.ByName("gml_Script_scr_Save_Data_Full");
            var scr_Save_Data_Full = Decompiler.Decompile(scr_Save_Data_Full_code, decompileContext);
            scr_Save_Data_Full = scr_Save_Data_Full.Replace(
                "var game = ds_map_find_value(save_map, \"GAME DATA\")\n",
                """
                var game = ds_map_find_value(save_map, "GAME DATA")
                ds_map_set(game, "Tourian Keys", obj_Samus.Upgrade[30])

                """.ReplaceLineEndings("\n")
            );
            scr_Save_Data_Full = scr_Save_Data_Full.Replace(
                "repeat (30)",
                "repeat (50)"
            );
            scr_Save_Data_Full = scr_Save_Data_Full.Replace(
                "if (index == (7 << 0))",
                "if (index == (7 << 0) || index == (30 << 0))"
            );
            scr_Save_Data_Full_code.ReplaceGML(scr_Save_Data_Full, gmData);

            // Load keys from save data
            var World_Load_code = gmData.Code.ByName("gml_Script_World_Load");
            var World_Load = Decompiler.Decompile(World_Load_code, decompileContext);

            World_Load = World_Load.Replace(
                "scr_Load_Save_Data(global.Save_Data[global.GAME_WORLD, global.GAME_SAVE])",
                "scr_Load_Rando_Save(global.Save_Data[global.GAME_WORLD, global.GAME_SAVE])"
            );
            World_Load_code.ReplaceGML(World_Load, gmData);

            // Post-hook load save to load our own stuff
            CodeUtils.CreateFunction(gmData, "scr_Load_Rando_Save", """
                                                                    var save_map = argument0;
                                                                    if (ds_exists(save_map, 1) == 0)
                                                                        return;
                                                                    var game = ds_map_find_value(save_map, "GAME DATA")
                                                                    obj_Samus.Upgrade[30] = ds_map_find_value(game, "Tourian Keys")
                                                                    scr_Load_Save_Data(save_map)
                                                                    """.ReplaceLineEndings("\n"));
        }
    }
}
