using UndertaleModLib;
using UndertaleModLib.Decompiler;
using YAPR_LIB.Utils;
using static UndertaleModLib.Models.UndertaleRoom;

namespace YAPR_LIB.Patches
{
    public static class EditStartingArea
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext, string room, StartingLocation location)
        {
            CodeUtils.AddGlobalVariables(gmData, decompileContext, new Dictionary<string, object>()
            {
                { "START_LOCATION_X", location.X },
                { "START_LOCATION_Y", location.Y }
            });

            var samus = default(GameObject);
            if (room == "zebeth")
                samus = gmData.Rooms[5].GameObjects.Select(obj => obj).Where(obj => obj.InstanceID == 100076).FirstOrDefault();
            if (room == "novus")
                samus = gmData.Rooms[6].GameObjects.Select(obj => obj).Where(obj => obj.InstanceID == 100084).FirstOrDefault();

            if (samus == null)
                throw new Exception("Cannot randomize this planet!");

            samus.X = location.X;
            samus.Y = location.Y;
        }
    }
}
