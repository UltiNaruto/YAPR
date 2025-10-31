using UndertaleModLib;
using UndertaleModLib.Decompiler;

namespace YAPR_LIB.Patches.Fixes
{
    public static class FixRipperDamageVulnerabilities
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext)
        {
            var scr_Enemy_Build_code = gmData.Code.ByName("gml_Script_scr_Enemy_Build");
            var scr_Enemy_Build = Decompiler.Decompile(scr_Enemy_Build_code, decompileContext);

            scr_Enemy_Build = scr_Enemy_Build.Replace(
                """
                    if (enemy_type == (12 << 0))
                    {
                        sprite_index = spr_Enemy_Ripper_Fire
                        AI_Script = 129
                        if (level == 0)
                        {
                            Energy = 4
                            Damage = 10
                        }
                        Speed = 3
                        Vulnerable[(0 << 0)] = (-1 << 0)
                        Vulnerable[(1 << 0)] = (-1 << 0)
                        Vulnerable[(4 << 0)] = (-1 << 0)
                        Vulnerable[(2 << 0)] = (-1 << 0)
                        Vulnerable[(6 << 0)] = (-1 << 0)
                    }
                """,
                """
                    if (enemy_type == (12 << 0))
                    {
                        sprite_index = spr_Enemy_Ripper_Fire
                        AI_Script = 129
                        if (level == 0)
                        {
                            Energy = 4
                            Damage = 10
                            Vulnerable[(2 << 0)] = (0 << 0)
                            Vulnerable[(6 << 0)] = (0 << 0)
                        }
                        Speed = 3
                        Vulnerable[(0 << 0)] = (-1 << 0)
                        Vulnerable[(1 << 0)] = (-1 << 0)
                        Vulnerable[(4 << 0)] = (-1 << 0)
                        Vulnerable[(2 << 0)] = (-1 << 0)
                        Vulnerable[(6 << 0)] = (-1 << 0)
                    }
                """
            );

            scr_Enemy_Build = scr_Enemy_Build.Replace(
                """
                    if (enemy_type == (18 << 0))
                    {
                        sprite_index = spr_Enemy_Ripper
                        AI_Script = 129
                        Vulnerable[(0 << 0)] = (-1 << 0)
                        Vulnerable[(1 << 0)] = (-1 << 0)
                        Vulnerable[(4 << 0)] = (-1 << 0)
                        Vulnerable[(2 << 0)] = (-1 << 0)
                        Vulnerable[(6 << 0)] = (-1 << 0)
                        if (level == 0)
                        {
                            Energy = 4
                            Damage = 8
                        }
                        if (level == 1)
                        {
                            Energy = 4
                            Damage = 8
                            Vulnerable[(2 << 0)] = (0 << 0)
                            Vulnerable[(6 << 0)] = (0 << 0)
                        }
                        Speed = 1.5
                    }
                """,
                """
                    if (enemy_type == (18 << 0))
                    {
                        sprite_index = spr_Enemy_Ripper
                        AI_Script = 129
                        Vulnerable[(0 << 0)] = (-1 << 0)
                        Vulnerable[(1 << 0)] = (-1 << 0)
                        Vulnerable[(4 << 0)] = (-1 << 0)
                        Vulnerable[(2 << 0)] = (-1 << 0)
                        Vulnerable[(6 << 0)] = (-1 << 0)
                        if (level == 0)
                        {
                            Energy = 4
                            Damage = 8
                            Vulnerable[(2 << 0)] = (0 << 0)
                            Vulnerable[(6 << 0)] = (0 << 0)
                        }
                        if (level == 1)
                        {
                            Energy = 4
                            Damage = 8
                            Vulnerable[(2 << 0)] = (0 << 0)
                            Vulnerable[(6 << 0)] = (0 << 0)
                        }
                        Speed = 1.5
                    }
                """
            );

            scr_Enemy_Build_code.ReplaceGML(scr_Enemy_Build, gmData);
        }
    }
}
