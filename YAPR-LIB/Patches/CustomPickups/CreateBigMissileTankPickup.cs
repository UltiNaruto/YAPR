using UndertaleModLib.Decompiler;
using UndertaleModLib.Models;
using UndertaleModLib;

namespace YAPR_LIB.Patches.CustomPickups
{
    public static class CreateBigMissileTankPickup
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext)
        {
            var obj_Item_Big_Missile_PreCreate_0_str = new UndertaleString("gml_Object_obj_Item_Big_Missile_PreCreate_0");
            gmData.Strings.Add(obj_Item_Big_Missile_PreCreate_0_str);

            var obj_Item_Big_Missile_PreCreate_0_code = new UndertaleCode();
            obj_Item_Big_Missile_PreCreate_0_code.Name = obj_Item_Big_Missile_PreCreate_0_str;
            obj_Item_Big_Missile_PreCreate_0_code.ReplaceGML("event_inherited()\n", gmData);

            gmData.Code.Add(obj_Item_Big_Missile_PreCreate_0_code);

            var obj_Item_Big_Missile_str = new UndertaleString("obj_Item_Big_Missile");
            gmData.Strings.Add(obj_Item_Big_Missile_str);

            var obj_Item_Big_Missile = new UndertaleGameObject();
            obj_Item_Big_Missile.Name = obj_Item_Big_Missile_str;
            obj_Item_Big_Missile.Sprite = gmData.Sprites.FirstOrDefault(spr => spr.Name.Content == "spr_ITEM_Big_Missile");
            obj_Item_Big_Missile.Visible = true;
            obj_Item_Big_Missile.Solid = false;
            obj_Item_Big_Missile.Persistent = false;
            obj_Item_Big_Missile.ParentId = gmData.GameObjects.FirstOrDefault(obj => obj.Name.Content == "par_Item");
            obj_Item_Big_Missile.UsesPhysics = false;
            obj_Item_Big_Missile.IsSensor = false;
            obj_Item_Big_Missile.CollisionShape = CollisionShapeFlags.Box;
            obj_Item_Big_Missile.Density = .5f;
            obj_Item_Big_Missile.Restitution = .1f;
            obj_Item_Big_Missile.Group = 0;
            obj_Item_Big_Missile.LinearDamping = .1f;
            obj_Item_Big_Missile.AngularDamping = .1f;
            obj_Item_Big_Missile.Friction = .2f;
            obj_Item_Big_Missile.Awake = true;
            obj_Item_Big_Missile.Kinematic = false;

            var obj_Item_Big_Missile_PreCreate_0 = new UndertaleGameObject.Event();
            obj_Item_Big_Missile_PreCreate_0.Actions.Add(new UndertaleGameObject.EventAction()
            {
                CodeId = obj_Item_Big_Missile_PreCreate_0_code
            });
            obj_Item_Big_Missile.Events[(int)EventType.PreCreate].Add(obj_Item_Big_Missile_PreCreate_0);
            gmData.GameObjects.Add(obj_Item_Big_Missile);
        }
    }
}
