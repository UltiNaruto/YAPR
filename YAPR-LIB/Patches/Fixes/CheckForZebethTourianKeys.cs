using UndertaleModLib.Decompiler;
using UndertaleModLib;

namespace YAPR_LIB.Patches.Fixes
{
    public static class CheckForZebethTourianKeys
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext, int keyCount = 2)
        {
            var scr_Handle_Door_Transition_code = gmData.Code.ByName("gml_Script_scr_Handle_Door_Transition");
            var scr_Handle_Door_Transition = Decompiler.Decompile(scr_Handle_Door_Transition_code, decompileContext);
            scr_Handle_Door_Transition = scr_Handle_Door_Transition.Replace(
                """
                        Room_Transition_Timer -= 1
                        if (Room_Transition_Timer <= 0)
                        {

                """.ReplaceLineEndings("\n"),
                """
                        Room_Transition_Timer -= 1
                        if (Room_Transition_Timer <= 0)
                        {
                            if (obj_MAIN.Screen_X == 3 && obj_MAIN.Screen_Y == 1)
                            {
                                var layer = layer_tilemap_get_id("Tiles_Solid")
                                for (var bridgeX = 52; bridgeX < 52 + obj_Samus.Upgrade[30]; bridgeX += 1)
                                    tilemap_set(layer, 0, bridgeX, 22)
                                if (obj_Samus.Upgrade[30])
                                    play_sfx(sfx_Notify_Boss_Defeated)
                            }

                """.ReplaceLineEndings("\n")
            );

            scr_Handle_Door_Transition_code.ReplaceGML(scr_Handle_Door_Transition, gmData);

            // Use blocks instead of pit
            for (var i = 0; i < keyCount; i++)
            {
                gmData.Rooms[5].Layers[2].TilesData.TileData[22][52 + (8 - i)] = 81;
            }

            // Remove pit
            for (var i = 0; i < 8; i++)
            {
                gmData.Rooms[5].Layers[2].TilesData.TileData[23][53 + i] = 86;
            }

            // Remove bridge since we don't use it anymore
            var bridge_parts = gmData.Rooms[5].GameObjects.Select(obj => obj)
                                                          .Where(obj => obj.ObjectDefinition.Name.Content == "obj_Bridge");
            foreach (var bridge_part in bridge_parts)
                bridge_part.ScaleX = bridge_part.ScaleY = 0;

            var obj_bridge = gmData.GameObjects.FirstOrDefault(obj => obj.Name.Content == "obj_Bridge");
            obj_bridge.Visible = false;
            foreach (var ev in obj_bridge.Events)
                ev.Clear();

            // Remove Kraid statue since we don't use it anymore
            var kraid_statue = gmData.Rooms[5].GameObjects.FirstOrDefault(obj => obj.ObjectDefinition.Name.Content == "obj_Statue_Kraid");
            kraid_statue.ScaleX = kraid_statue.ScaleY = 0;
            kraid_statue.ObjectDefinition.Visible = false;
            gmData.Rooms[5].Layers[2].TilesData.TileData[21][56] = 86;

            // Remove Ridley statue since we don't use it anymore
            var ridley_statue = gmData.Rooms[5].GameObjects.FirstOrDefault(obj => obj.ObjectDefinition.Name.Content == "obj_Statue_Ridley");
            ridley_statue.ScaleX = ridley_statue.ScaleY = 0;
            ridley_statue.ObjectDefinition.Visible = false;
            gmData.Rooms[5].Layers[2].TilesData.TileData[19][54] = 86;

            // Another pillar?
            gmData.Rooms[5].Layers[2].TilesData.TileData[19][52] = 86;
        }
    }
}
