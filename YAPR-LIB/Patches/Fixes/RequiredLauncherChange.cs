using UndertaleModLib;
using UndertaleModLib.Decompiler;

namespace YAPR_LIB.Patches.Fixes
{
    internal class RequiredLauncherChange
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext)
        {
            var scr_Draw_HUD_code = gmData.Code.ByName("gml_Script_scr_Draw_HUD");
            var scr_Draw_HUD = Decompiler.Decompile(scr_Draw_HUD_code, decompileContext);

            // requires now to have missile launcher
            scr_Draw_HUD = scr_Draw_HUD.Replace(
                "        draw_sprite_ext(spr_Missile, index, xx, yy, 1, 1, 90, c_white, 1)",
                """
                        if (i.Upgrade[16] == 0)
                        {
                            c = c_red
                            c2 = c_maroon
                            index = 1
                            text = "0"
                        }
                        draw_sprite_ext(spr_Missile, index, xx, yy, 1, 1, 90, c_white, 1)
                """
            );
            scr_Draw_HUD = scr_Draw_HUD.Replace(
                "if (i.Ammo[(1 << 0)] == i.Ammo_Cap[(1 << 0)])",
                "if (i.Upgrade[16] == 1 && i.Ammo[1] == i.Ammo_Cap[1])"
            );

            // requires now to have super missile launcher
            scr_Draw_HUD = scr_Draw_HUD.Replace(
                "        draw_sprite_ext(spr_Super_Missile, index, xx, yy, 1, 1, 90, c_white, 1)",
                """
                        if (i.Upgrade[17] == 0)
                        {
                            c = c_red
                            c2 = c_maroon
                            index = 1
                            text = "0"
                        }
                        draw_sprite_ext(spr_Super_Missile, index, xx, yy, 1, 1, 90, c_white, 1)
                """
            );
            scr_Draw_HUD = scr_Draw_HUD.Replace(
                "if (i.Ammo[(2 << 0)] == i.Ammo_Cap[(2 << 0)])",
                "if (i.Upgrade[17] == 1 && i.Ammo[2] == i.Ammo_Cap[2])"
            );

            scr_Draw_HUD_code.ReplaceGML(scr_Draw_HUD, gmData);

            var obj_Samus_Step_0_code = gmData.Code.ByName("gml_Object_obj_Samus_Step_0");
            var obj_Samus_Step_0 = Decompiler.Decompile(obj_Samus_Step_0_code, decompileContext);

            // requires now to have missile launcher or super missile launcher
            // to use their respective ammo
            obj_Samus_Step_0 = obj_Samus_Step_0.Replace(
                "    Spinning += (1 / global.FPS)\n",
                """
                    Spinning += (1 / global.FPS)
                Ammo_Disabled[1] = (Upgrade[16] + 1) % 2
                Ammo_Disabled[2] = (Upgrade[17] + 1) % 2

                """.ReplaceLineEndings("\n")
            );

            obj_Samus_Step_0_code.ReplaceGML(obj_Samus_Step_0, gmData);
        }
    }
}
