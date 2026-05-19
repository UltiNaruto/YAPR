using UndertaleModLib;
using UndertaleModLib.Decompiler;
using UndertaleModLib.Models;
using YAPR_LIB.Utils;

namespace YAPR_LIB.Patches.QoL
{
    internal class AllowScrewAttackToBreakBlocks
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext)
        {
            var obj_Samus_Pre_Step_0 = """
                                       var xx = x;
                                       var yy = y;
                                       if (Spinning >= 2 && Upgrade[21] == 1)
                                       {
                                           with (obj_Block)
                                           {
                                               if (Type == (0 << 0) || Type == (1 << 0))
                                               {
                                                   if (State == (0 << 0))
                                                   {
                                                       if (distance_to_object(obj_Samus) < 8)
                                                           State = (1 << 0)
                                                   }
                                               }
                                           }
                                       }
                                       """;

            CodeUtils.CreateFunction(gmData, "obj_Samus_Pre_Step_0", obj_Samus_Pre_Step_0);

            gmData.GameObjects.ByName("obj_Samus").Events[(int)EventType.Step].Add(new UndertaleGameObject.Event()
            {
                EventSubtype = (uint)EventSubtypeStep.BeginStep,
                Actions = new UndertalePointerList<UndertaleGameObject.EventAction>()
                {
                    new UndertaleGameObject.EventAction()
                    {
                        CodeId = gmData.Code.ByName("gml_Script_obj_Samus_Pre_Step_0")
                    }
                }
            });
        }
    }
}
