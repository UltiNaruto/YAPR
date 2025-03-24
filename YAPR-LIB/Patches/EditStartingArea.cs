using UndertaleModLib;
using UndertaleModLib.Decompiler;
using YAPR_LIB.Utils;

namespace YAPR_LIB.Patches
{
    public static class EditStartingArea
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext, Room room, StartingLocation location)
        {
            CodeUtils.AddGlobalVariables(gmData, decompileContext, new Dictionary<string, object>()
            {
                { "START_LOCATION_X", location.X },
                { "START_LOCATION_Y", location.Y }
            });

            var instance_id = (Room room) =>
            {
                switch (room)
                {
                    case Room.rm_Zebeth:
                        return 100076;
                    case Room.rm_Novus:
                        return 100084;
                    default:
                        throw new Exception("Cannot randomize this planet!");
                }
            };

            var samus = gmData.Rooms[(int)room].GameObjects.Select(obj => obj).Where(obj => obj.InstanceID == instance_id(room)).FirstOrDefault();

            if (samus == null)
                throw new Exception("Cannot randomize this planet!");

            samus.X = location.X;
            samus.Y = location.Y;
        }
    }
}
