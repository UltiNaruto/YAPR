using UndertaleModLib.Models;
using UndertaleModLib;
using UndertaleModLib.Decompiler;

namespace YAPR_LIB.Patches.CustomPickups
{
    public static class CreateTourianKeyPickup
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext)
        {
            // Object creation
            var obj_Item_Tourian_Key_PreCreate_0_str = new UndertaleString("gml_Object_obj_Item_Tourian_Key_PreCreate_0");
            gmData.Strings.Add(obj_Item_Tourian_Key_PreCreate_0_str);

            var obj_Item_Tourian_Key_PreCreate_0_code = new UndertaleCode();
            obj_Item_Tourian_Key_PreCreate_0_code.Name = obj_Item_Tourian_Key_PreCreate_0_str;
            obj_Item_Tourian_Key_PreCreate_0_code.ReplaceGML("event_inherited()\n", gmData);

            gmData.Code.Add(obj_Item_Tourian_Key_PreCreate_0_code);

            var obj_Item_Tourian_Key_str = new UndertaleString("obj_Item_Tourian_Key");
            gmData.Strings.Add(obj_Item_Tourian_Key_str);

            var obj_Item_Tourian_Key = new UndertaleGameObject();
            obj_Item_Tourian_Key.Name = obj_Item_Tourian_Key_str;
            obj_Item_Tourian_Key.Sprite = gmData.Sprites.FirstOrDefault(spr => spr.Name.Content == "spr_ITEM_Tourian_Key");
            obj_Item_Tourian_Key.Visible = true;
            obj_Item_Tourian_Key.Solid = false;
            obj_Item_Tourian_Key.Persistent = false;
            obj_Item_Tourian_Key.ParentId = gmData.GameObjects.FirstOrDefault(obj => obj.Name.Content == "par_Item");
            obj_Item_Tourian_Key.UsesPhysics = false;
            obj_Item_Tourian_Key.IsSensor = false;
            obj_Item_Tourian_Key.CollisionShape = CollisionShapeFlags.Box;
            obj_Item_Tourian_Key.Density = .5f;
            obj_Item_Tourian_Key.Restitution = .1f;
            obj_Item_Tourian_Key.Group = 0;
            obj_Item_Tourian_Key.LinearDamping = .1f;
            obj_Item_Tourian_Key.AngularDamping = .1f;
            obj_Item_Tourian_Key.Friction = .2f;
            obj_Item_Tourian_Key.Awake = true;
            obj_Item_Tourian_Key.Kinematic = false;

            var obj_Item_Tourian_Key_PreCreate_0 = new UndertaleGameObject.Event();
            obj_Item_Tourian_Key_PreCreate_0.Actions.Add(new UndertaleGameObject.EventAction()
            {
                CodeId = obj_Item_Tourian_Key_PreCreate_0_code
            });
            obj_Item_Tourian_Key.Events[(int)EventType.PreCreate].Add(obj_Item_Tourian_Key_PreCreate_0);
            gmData.GameObjects.Add(obj_Item_Tourian_Key);
        }
    }
}
