using UndertaleModLib.Decompiler;
using UndertaleModLib;

namespace YAPR_LIB.Patches.Fixes
{
    public static class FixBadRNG
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext)
        {
            var obj_Enemy_Spawner_Step_0_code = gmData.Code.ByName("gml_Object_obj_Enemy_Spawner_Step_0");
            var obj_Enemy_Spawner_Step_0 = Decompiler.Decompile(obj_Enemy_Spawner_Step_0_code, decompileContext);

            obj_Enemy_Spawner_Step_0 = obj_Enemy_Spawner_Step_0.Replace(
                """
                        Spawn = scr_Enemy_Build(x, y, Spawn_Type, Spawn_Level, ID_Slot, Data_Map)
                """,
                """
                        var screen_x = floor((x / global.GAME_SCREEN_W));
                        var screen_y = floor((y / global.GAME_SCREEN_H));     
                        if ((screen_x == 7 || screen_x == 8) && screen_y == 13)
                        {
                            if (Spawn_Type == 0)
                                return;
                        }
                        else if (screen_x == 29)
                        {
                            if (screen_y == 2 || screen_y == 3 || screen_y == 4)
                            {
                                if (screen_y == 4)
                                    return;
                                if (Spawn_Type != 12 && Spawn_Type != 18)
                                    return;
                            }
                            if (screen_y == 10 && y == 2568)
                                Spawn_Type = 12
                            if (screen_y == 10 && y != 2808)
                                Spawn_Type = 12
                            if (screen_y == 11 && y != 2920)
                                Spawn_Type = 12
                        }
                        Spawn = scr_Enemy_Build(x, y, Spawn_Type, Spawn_Level, ID_Slot, Data_Map)
                """
            );

            obj_Enemy_Spawner_Step_0_code.ReplaceGML(obj_Enemy_Spawner_Step_0, gmData);
        }
    }
}
