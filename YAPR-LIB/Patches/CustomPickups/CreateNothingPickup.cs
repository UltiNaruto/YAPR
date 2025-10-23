using UndertaleModLib;
using UndertaleModLib.Decompiler;
using UndertaleModLib.Models;

namespace YAPR_LIB.Patches.CustomPickups
{
    public static class CreateNothingPickup
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext)
        {
            var obj_Item_Nothing_PreCreate_0_str = new UndertaleString("gml_Object_obj_Item_Nothing_PreCreate_0");
            gmData.Strings.Add(obj_Item_Nothing_PreCreate_0_str);

            var obj_Item_Nothing_PreCreate_0_code = new UndertaleCode();
            obj_Item_Nothing_PreCreate_0_code.Name = obj_Item_Nothing_PreCreate_0_str;
            obj_Item_Nothing_PreCreate_0_code.ReplaceGML("event_inherited()\n", gmData);

            gmData.Code.Add(obj_Item_Nothing_PreCreate_0_code);

            var obj_Item_Nothing_str = new UndertaleString("obj_Item_Nothing");
            gmData.Strings.Add(obj_Item_Nothing_str);

            var obj_Item_Nothing = new UndertaleGameObject();
            obj_Item_Nothing.Name = obj_Item_Nothing_str;
            obj_Item_Nothing.Sprite = gmData.Sprites.FirstOrDefault(spr => spr.Name.Content == "spr_ITEM_Nothing");
            obj_Item_Nothing.Visible = true;
            obj_Item_Nothing.Solid = false;
            obj_Item_Nothing.Persistent = false;
            obj_Item_Nothing.ParentId = gmData.GameObjects.FirstOrDefault(obj => obj.Name.Content == "par_Item");
            obj_Item_Nothing.UsesPhysics = false;
            obj_Item_Nothing.IsSensor = false;
            obj_Item_Nothing.CollisionShape = CollisionShapeFlags.Box;
            obj_Item_Nothing.Density = .5f;
            obj_Item_Nothing.Restitution = .1f;
            obj_Item_Nothing.Group = 0;
            obj_Item_Nothing.LinearDamping = .1f;
            obj_Item_Nothing.AngularDamping = .1f;
            obj_Item_Nothing.Friction = .2f;
            obj_Item_Nothing.Awake = true;
            obj_Item_Nothing.Kinematic = false;

            var obj_Item_Nothing_PreCreate_0 = new UndertaleGameObject.Event();
            obj_Item_Nothing_PreCreate_0.Actions.Add(new UndertaleGameObject.EventAction()
            {
                CodeId = obj_Item_Nothing_PreCreate_0_code
            });
            obj_Item_Nothing.Events[(int)EventType.PreCreate].Add(obj_Item_Nothing_PreCreate_0);
            gmData.GameObjects.Add(obj_Item_Nothing);
        }
    }
}
