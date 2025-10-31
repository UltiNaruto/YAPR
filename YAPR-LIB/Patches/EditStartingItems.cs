using UndertaleModLib;
using UndertaleModLib.Decompiler;
using YAPR_LIB.Utils;

namespace YAPR_LIB.Patches
{
    public static class EditStartingItems
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext, Room room, Dictionary<string, int>? startingItems, Text? startingMemo)
        {
            var newCode = new List<String>();

            var world_load_code = gmData.Code.ByName("gml_Script_World_Load");
            var world_load = Decompiler.Decompile(world_load_code, decompileContext);

            var hasMissiles = startingItems?.ContainsKey("Missile Launcher") ?? false;
            var hasSuperMissiles = room == Room.rm_Novus && (startingItems?.ContainsKey("Super Missile Launcher") ?? false);
            var hasWaveBeam = startingItems?.ContainsKey("Wave Beam") ?? false;

            var stringPrecedingInsert = StringUtils.MakeUnixString(
                """
                if (room == rm_Novus)
                                Current_Area = (1 << 0)
                """
            );
            var insertIndex = world_load.IndexOf(stringPrecedingInsert) + stringPrecedingInsert.Length + 1;

            if (hasMissiles)
                newCode.Add("            obj_Samus.Upgrade[16] = 1");

            if (hasSuperMissiles)
                newCode.Add("            obj_Samus.Upgrade[17] = 1");

            if (startingItems is not null && startingItems.Count > 0)
            {
                foreach (var (itemName, count) in startingItems)
                {
                    // should not happen
                    if (count <= 0)
                        continue;

                    if (itemName == "Long Beam")
                    {
                        newCode.Add("            obj_Samus.Upgrade[1] = 1");
                    }

                    if (itemName == "Ice Beam")
                    {
                        if (hasWaveBeam && room == Room.rm_Zebeth)
                            newCode.Add("            obj_Samus.Upgrade[3] = 0.5");
                        else
                            newCode.Add("            obj_Samus.Upgrade[3] = 1");
                    }

                    if (itemName == "Wave Beam")
                    {
                        newCode.Add("            obj_Samus.Upgrade[4] = 1");
                    }

                    if (itemName == "Spazer Beam" && room == Room.rm_Novus)
                    {
                        newCode.Add("            obj_Samus.Upgrade[5] = 1");
                    }

                    if (itemName == "Energy Tank")
                    {
                        newCode.Add($"            obj_Samus.Upgrade[7] = {Math.Clamp(count, 0, 8)}");
                        newCode.Add($"            obj_Samus.Energy_Max = {99 + 100 * Math.Clamp(count, 0, 8)}");
                        newCode.Add($"            obj_Samus.Energy = {99 + 100 * Math.Clamp(count, 0, 8)}");
                    }

                    if (itemName == "Varia Suit")
                    {
                        newCode.Add("            obj_Samus.Upgrade[8] = 1");
                        newCode.Add("            obj_Samus.Current_Suit = 2");
                    }

                    if (itemName == "Morph Ball")
                    {
                        newCode.Add("            obj_Samus.Upgrade[10] = 1");
                    }

                    if (itemName == "Spring Ball" && room == Room.rm_Novus)
                    {
                        newCode.Add("            obj_Samus.Upgrade[11] = 1");
                    }

                    if (itemName == "Bombs")
                    {
                        newCode.Add("            obj_Samus.Upgrade[14] = 1");
                    }

                    if (itemName == "Missiles")
                    {
                        newCode.Add($"            obj_Samus.Ammo_Cap[1] = {Math.Clamp(count, 0, 999)}");
                        newCode.Add($"            obj_Samus.Ammo[1] = {Math.Clamp(count, 0, 999)}");
                    }

                    if (itemName == "Super Missiles" && room == Room.rm_Novus)
                    {
                        newCode.Add($"            obj_Samus.Ammo_Cap[2] = {Math.Clamp(count, 0, 999)}");
                        newCode.Add($"            obj_Samus.Ammo[2] = {Math.Clamp(count, 0, 999)}");
                    }

                    if (itemName == "Hi-Jump Boots")
                    {
                        newCode.Add("            obj_Samus.Upgrade[18] = 1");
                    }

                    if (itemName == "Speed Booster" && room == Room.rm_Novus)
                    {
                        newCode.Add("            obj_Samus.Upgrade[20] = 1");
                    }

                    if (itemName == "Screw Attack")
                    {
                        newCode.Add("            obj_Samus.Upgrade[21] = 1");
                    }

                    if (itemName == "Sensor Visor" && room == Room.rm_Novus)
                    {
                        newCode.Add("            obj_Samus.Upgrade[22] = 1");
                    }

                    if (itemName == "Tourian Key")
                    {
                        newCode.Add($"            obj_Samus.Upgrade[30] = {count}");
                    }
                }
            }

            if (newCode.Count > 0)
            {
                // prepends a newline
                world_load = world_load.Insert(insertIndex, String.Join("\n", newCode));

                newCode.Clear();
            }


            stringPrecedingInsert = StringUtils.MakeUnixString(
                """
                        if (obj_NETWORK.Connection_Pos > -1)
                            NET_Apply_Shared_Data()
                """
            );
            insertIndex = world_load.IndexOf(stringPrecedingInsert) + stringPrecedingInsert.Length + 1;

            if (startingMemo is not null)
            {
                newCode.AddRange(
                  $$"""
                            if (global.GAME_TIME < 1)
                            {
                                obj_MAIN.Current_Event = (1 << 0)
                                obj_MAIN.Current_Event_Timer = 120
                                obj_MAIN.Item_Message = 1
                                obj_MAIN.Item_Message_Header = "{{startingMemo.Header}}"
                                obj_MAIN.Item_Message_Description = "{{String.Join("\n", startingMemo.Description ?? new List<string>())}}"
                                obj_MAIN.Item_Event_Type = 0
                            }
                    """.Split("\r\n")
                );
            }

            if (newCode.Count > 0)
            {
                world_load = world_load.Insert(insertIndex, String.Join("\n", newCode));
            }

            world_load_code.ReplaceGML(world_load, gmData);
        }
    }
}
