using UndertaleModLib;
using UndertaleModLib.Decompiler;

namespace YAPR_LIB.Patches.QoL
{
    public static class OpenMissileDoorsWithOneMissile
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext)
        {
            var obj_Door_Create_0_code = gmData.Code.ByName("gml_Object_obj_Door_Create_0");
            var obj_Door_Create_0 = Decompiler.Decompile(obj_Door_Create_0_code, decompileContext);

            obj_Door_Create_0 = obj_Door_Create_0.UnixReplace(
                "HP[(2 << 0)] = 5",
                "HP[(2 << 0)] = 1"
            );

            obj_Door_Create_0_code.ReplaceGML(obj_Door_Create_0, gmData);
        }
    }
}
