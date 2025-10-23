using UndertaleModLib;
using YAPR_LIB.Utils;

namespace YAPR_LIB.Patches
{
    public static class RemoveChozoStatueSpheres
    {
        public static void Apply(UndertaleData gmData, Room room)
        {
            var tiles_to_patch = default((int, int)[]);

            switch (room)
            {
                case Room.rm_Zebeth:
                    tiles_to_patch = new (int, int)[]
                    {
                        // Brinstar - Long Beam room
                        (103, 63),
                        // Brinstar - Varia Suit room
                        (231, 18),
                        // Brinstar - Ice Beam room
                        (295, 123),
                        // Brinstar - Bomb room
                        (391, 63),
                        // Norfair - Ice Beam room
                        (407, 168),
                        // Norfair - Hi-Jump room
                        (423, 243),
                        // Norfair - Screw Attack room
                        (231, 228),
                        // Norfair - Wave Beam room
                        (279, 303),
                    };
                    break;
                case Room.rm_Novus:
                    throw new NotImplementedException("Cannot remove Chozo Statue spheres for Novus yet!");
            }

            if (tiles_to_patch is not null)
                foreach (var (x, y) in tiles_to_patch)
                    gmData.Rooms[(int)room]
                          .Layers[(int)Layer.Tiles_Solid]
                          .TilesData
                          .TileData[y][x] = 0;
        }
    }
}
