using UndertaleModLib.Models;

namespace YAPR_LIB.Extensions
{
    public static class GameObjectExtensions
    {
        public static UndertaleGameObject CopyTo(this UndertaleGameObject src, ref UndertaleGameObject dst)
        {
            dst.AngularDamping = src.AngularDamping;
            dst.Awake = src.Awake;
            dst.CollisionShape = src.CollisionShape;
            dst.Density = src.Density;
            dst.Depth = src.Depth;
            dst.Friction = src.Friction;
            dst.Group = src.Group;
            dst.IsSensor = src.IsSensor;
            dst.Kinematic = src.Kinematic;
            dst.LinearDamping = src.LinearDamping;
            dst.Managed = src.Managed;
            dst.Name = src.Name;
            dst.ParentId = src.ParentId;
            dst.Persistent = src.Persistent;
            dst.PhysicsVertices.AddRange(src.PhysicsVertices);
            dst.Restitution = src.Restitution;
            dst.Solid = src.Solid;
            dst.Sprite = src.Sprite;
            dst.TextureMaskId = src.TextureMaskId;
            dst.UsesPhysics = src.UsesPhysics;
            dst.Visible = src.Visible;
            return dst;
        }

        public static UndertaleGameObject Clone(this UndertaleGameObject obj)
        {
            var newObj = new UndertaleGameObject();
            obj.CopyTo(ref newObj);
            return newObj;
        }
    }
}
