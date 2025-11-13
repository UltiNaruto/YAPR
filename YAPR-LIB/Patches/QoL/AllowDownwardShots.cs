using UndertaleModLib;
using UndertaleModLib.Decompiler;

namespace YAPR_LIB.Patches.QoL
{
    public static class AllowDownwardShots
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext)
        {
            var obj_Samus_Create_0_code = gmData.Code.ByName("gml_Object_obj_Samus_Create_0");
            var obj_Samus_Create_0 = Decompiler.Decompile(obj_Samus_Create_0_code, decompileContext);

            obj_Samus_Create_0 = obj_Samus_Create_0.Replace(
                """
                if (room == rm_Zebeth || room == rm_Novus)
                    Downwards_Aiming = 0
                """,
                "");

            obj_Samus_Create_0_code.ReplaceGML(obj_Samus_Create_0, gmData);
        }
    }
}
