using UndertaleModLib;

namespace YAPR_LIB.Patches
{
    public static class RemoveChozoStatueSpheres
    {
        public static void Apply(UndertaleData gmData, int room_id)
        {
            // Patching Zebeth planet
            if(room_id == 5)
            {
                // Brinstar - Long Beam room
                gmData.Rooms[5].Layers[2].TilesData.TileData[63][103] = 0;
                // Brinstar - Varia Suit room
                gmData.Rooms[5].Layers[2].TilesData.TileData[18][231] = 0;
                // Brinstar - Ice Beam room
                gmData.Rooms[5].Layers[2].TilesData.TileData[123][295] = 0;
                // Brinstar - Bomb room
                gmData.Rooms[5].Layers[2].TilesData.TileData[63][391] = 0;
                // Norfair - Ice Beam room
                gmData.Rooms[5].Layers[2].TilesData.TileData[168][407] = 0;
                // Norfair - Hi-Jump room
                gmData.Rooms[5].Layers[2].TilesData.TileData[243][423] = 0;
                // Norfair - Screw Attack room
                gmData.Rooms[5].Layers[2].TilesData.TileData[228][231] = 0;
                // Norfair - Wave Beam room
                gmData.Rooms[5].Layers[2].TilesData.TileData[303][279] = 0;
            }
        }
    }
}
