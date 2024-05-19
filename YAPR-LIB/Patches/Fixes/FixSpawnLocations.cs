using UndertaleModLib;
using UndertaleModLib.Decompiler;

namespace YAPR_LIB.Patches.Fixes
{
    public static class FixSpawnLocations
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext)
        {
            var scr_Spawn_Setup_code = gmData.Code.ByName("gml_Script_scr_Spawn_Setup");
            var scr_Spawn_Setup = Decompiler.Decompile(scr_Spawn_Setup_code, decompileContext);

            scr_Spawn_Setup = scr_Spawn_Setup.Replace(
                """
                    if (room == rm_Zebeth)
                    {
                        Current_Area = (2 << 0)
                        Area_Start_X[0] = xx
                        Area_Start_Y[0] = yy
                        Area_Start_X[1] = 5504
                        Area_Start_Y[1] = 2992
                        Area_Start_X[2] = 1664
                        Area_Start_Y[2] = 4672
                        Area_Start_X[3] = 6272
                        Area_Start_Y[3] = 5632
                        Area_Start_X[4] = 640
                        Area_Start_Y[4] = 832
                    }
                """.ReplaceLineEndings("\n"),
                """
                    if (room == rm_Zebeth)
                    {
                        Area_Start_X[1] = 1664
                        Area_Start_Y[1] = 4192
                        Area_Start_X[2] = 640
                        Area_Start_Y[2] = 352
                        Area_Start_X[3] = 5504
                        Area_Start_Y[3] = 2512
                        Area_Start_X[4] = 5504
                        Area_Start_Y[4] = 2992
                        Area_Start_X[5] = 6272
                        Area_Start_Y[5] = 5152
                        Area_Start_X[6] = 1664
                        Area_Start_Y[6] = 4672
                        Area_Start_X[7] = 6272
                        Area_Start_Y[7] = 5632
                        Area_Start_X[8] = 640
                        Area_Start_Y[8] = 832
                    }
                """.ReplaceLineEndings("\n")
            );

            scr_Spawn_Setup_code.ReplaceGML(scr_Spawn_Setup, gmData);

            var scr_Spawn_Location_Update_code = gmData.Code.ByName("gml_Script_scr_Spawn_Location_Update");
            var scr_Spawn_Location_Update = Decompiler.Decompile(scr_Spawn_Location_Update_code, decompileContext);

            scr_Spawn_Location_Update = scr_Spawn_Location_Update.Replace(
                """
                    if (room == rm_Zebeth)
                    {
                        if (screen_x == 6 && screen_y == 17)
                            return 0;
                        if (screen_x == 2 && screen_y == 1)
                            return 0;
                        if (screen_x == 21 && screen_y == 10)
                            return 0;
                        if (screen_x == 21 && screen_y == 12)
                            return 1;
                        if (screen_x == 24 && screen_y == 21)
                            return 1;
                        if (screen_x == 6 && screen_y == 19)
                            return 2;
                        if (screen_x == 24 && screen_y == 23)
                            return 3;
                        if (screen_x == 2 && screen_y == 3)
                            return 4;
                    }
                """.ReplaceLineEndings("\n"),
                // Brinstar - Kraid Elevator (6, 17)
                // Brinstar - Transport to Tourian (2, 1)
                // Brinstar - Transport to Norfair (21, 10)
                // Norfair - Transport to Brinstar (21, 12)
                // Kraid - Entrance Shaft (6, 19)
                // Norfair - Ridley Elevator (24, 21)
                // Ridley - Eerie Cave (24, 23)
                // Tourian - Entrance Shaft (2, 3)
                """
                    if (room == rm_Zebeth)
                    {
                        if (screen_x == 6 && screen_y == 17)
                            return 1;
                        if (screen_x == 2 && screen_y == 1)
                            return 2;
                        if (screen_x == 21 && screen_y == 10)
                            return 3;
                        if (screen_x == 21 && screen_y == 12)
                            return 4;
                        if (screen_x == 24 && screen_y == 21)
                            return 5;
                        if (screen_x == 6 && screen_y == 19)
                            return 6;
                        if (screen_x == 24 && screen_y == 23)
                            return 7;
                        if (screen_x == 2 && screen_y == 3)
                            return 8;
                    }
                """.ReplaceLineEndings("\n")
            );

            scr_Spawn_Location_Update_code.ReplaceGML(scr_Spawn_Location_Update, gmData);

            // fixes area names in save slot
            var scr_TS_Area_Name_code = gmData.Code.ByName("gml_Script_scr_TS_Area_Name");
            var scr_TS_Area_Name = Decompiler.Decompile(scr_TS_Area_Name_code, decompileContext);

            scr_TS_Area_Name = scr_TS_Area_Name.Replace(
                """
                if (world == (1 << 0))
                {
                    if (area == 0)
                        name = "BRINSTAR"
                    if (area == 1)
                        name = "NORFAIR"
                    if (area == 2)
                        name = "KRAID"
                    if (area == 3)
                        name = "RIDLEY"
                    if (area == 4)
                        name = "TOURIAN"
                }
                """.ReplaceLineEndings("\n"),
                // Brinstar - Kraid Elevator (6, 17)
                // Brinstar - Transport to Tourian (2, 1)
                // Brinstar - Transport to Norfair (21, 10)
                // Norfair - Transport to Brinstar (21, 12)
                // Kraid - Entrance Shaft (6, 19)
                // Norfair - Ridley Elevator (24, 21)
                // Ridley - Eerie Cave (24, 23)
                // Tourian - Entrance Shaft (2, 3)
                """
                if (world == (1 << 0))
                {
                    if (area == 0)
                        name = "START"
                    if (area == 1 || area == 2 || area == 3)
                        name = "BRINSTAR"
                    if (area == 4 || area == 6)
                        name = "NORFAIR"
                    if (area == 5)
                        name = "KRAID"
                    if (area == 7)
                        name = "RIDLEY"
                    if (area == 8)
                        name = "TOURIAN"
                }
                """.ReplaceLineEndings("\n")
            );

            scr_TS_Area_Name_code.ReplaceGML(scr_TS_Area_Name, gmData);

            // Fix crash related to audio manager
            var scr_Audio_Handle_code = gmData.Code.ByName("gml_Script_scr_Audio_Handle");
            var scr_Audio_Handle = Decompiler.Decompile(scr_Audio_Handle_code, decompileContext);

            scr_Audio_Handle = scr_Audio_Handle.Replace(
                "    var room_bgm = BGM_Grid[xx, yy]",
                """
                    if (xx < 0 || yy < 0)
                        return;
                    var room_bgm = BGM_Grid[xx, yy]
                """.ReplaceLineEndings("\n")
            );

            scr_Audio_Handle_code.ReplaceGML(scr_Audio_Handle, gmData);

            // Fix crash related to current area update
            var scr_Current_Area_Update_code = gmData.Code.ByName("gml_Script_scr_Current_Area_Update");
            var scr_Current_Area_Update = Decompiler.Decompile(scr_Current_Area_Update_code, decompileContext);

            scr_Current_Area_Update = """
                               if (obj_MAIN.Screen_X < 0 || obj_MAIN.Screen_Y < 0)
                                   return -1;
                               """.ReplaceLineEndings("\n") + scr_Current_Area_Update;

            scr_Current_Area_Update_code.ReplaceGML(scr_Current_Area_Update, gmData);
        }
    }
}
