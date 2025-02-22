using System.Globalization;
using System.Reflection;
using System.Text.Json;
using UndertaleModLib;
using UndertaleModLib.Decompiler;

namespace YAPR_LIB;

public class Patcher
{
    private static string? _ExecutableDir;
    public static string? ExecutableDir => _ExecutableDir = _ExecutableDir ?? Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

    private static string CreateVersionString()
    {
        Version? assembly = Assembly.GetExecutingAssembly().GetName().Version;
        if (assembly is null) return "";

        return $"{assembly.Major}.{assembly.Minor}.{assembly.Build}";
    }

    public static string Version = CreateVersionString();

    public static void Main(string inputPath, string outputPath, string json)
    {
        int pickup_idx = 0;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        var trimmed_json = json.Trim('\r', '\n', ' ');

        // if json is passed as file path rather than json
        if (trimmed_json.First() != '{' && trimmed_json.First() != '[' &&
            trimmed_json.Last() != '}' && trimmed_json.Last() != ']')
            json = File.ReadAllText(json);

        var inputDir = Directory.GetParent(inputPath);
        if (inputDir == null)
            throw new DirectoryNotFoundException("Couldn't find the parent folder of input path!");
        var outputDir = Directory.GetParent(outputPath);
        if (outputDir == null)
            throw new DirectoryNotFoundException("Couldn't find the parent folder of output path!");
        RandomizerConfig? randomizerConfig = JsonSerializer.Deserialize<RandomizerConfig>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        );

        var gmData = new UndertaleData();

        using (FileStream fs = new FileInfo(inputPath).OpenRead())
        {
            gmData = UndertaleIO.Read(fs);
        }

        // Check version
        if (gmData.GeneralInfo.DisplayName.Content != "Metroid Planets v1.27g")
            throw new Exception("The selected game is not Metroid Planets 1.27g!");

        var decompileContext = new GlobalDecompileContext(gmData, false);

        var planetName = randomizerConfig.LevelData.Room == "rm_Zebeth" ? "zebeth" : "novus";
        var room_id = planetName == "zebeth" ? 5 : 6;

        // Import custom sprites into the game
        Patches.CustomSprites.Apply(gmData);

        // Globals
        Patches.CustomPickups.Globals.AddCustomItemsToGlobals.Apply(gmData, decompileContext);

        // Create non vanilla pickups (multiworld or randomizer items)
        Patches.CustomPickups.CreateBigMissileTankPickup.Apply(gmData, decompileContext);
        Patches.CustomPickups.CreateNothingPickup.Apply(gmData, decompileContext);
        Patches.CustomPickups.CreateTourianKeyPickup.Apply(gmData, decompileContext);

        // Do patches
        Patches.RemoveChozoStatueSpheres.Apply(gmData, room_id);
        Patches.DisplaySeedHash.Apply(gmData, decompileContext, randomizerConfig);
        Patches.EditMenu.Apply(gmData, decompileContext, planetName);
        Patches.MoveSavesToRandovaniaFolder.Apply(gmData, decompileContext, randomizerConfig);
        Patches.CustomMessageBox.Apply(gmData, decompileContext);
        Patches.CustomPickupHandling.Apply(gmData, decompileContext);
        Patches.Fixes.AddTourianKeysToHUD.Apply(gmData, decompileContext);
        Patches.Fixes.AddTourianKeysToSaveSlot.Apply(gmData, decompileContext);
        Patches.CustomPickups.Replay.CustomItemsReplaySupport.Apply(gmData, decompileContext, planetName);
        Patches.Fixes.FixRipperDamageVulnerabilities.Apply(gmData, decompileContext);
        Patches.AddCreditsScreen.Apply(gmData, decompileContext, planetName, randomizerConfig.GameConfig.CreditsString);
        Patches.Fixes.RequiredLauncherChange.Apply(gmData, decompileContext);
#if DEBUG
        Patches.Debug.AddSavingGameSaveAsJSON.Apply(gmData, decompileContext);
        Patches.Debug.DrawScreenCoords.Apply(gmData, decompileContext);
#endif

        Patches.EditStartingArea.Apply(gmData, decompileContext, planetName, randomizerConfig.GameConfig.StartingRoom);
        Patches.EditStartingItems.Apply(gmData, decompileContext, planetName, randomizerConfig.GameConfig.StartingItems, randomizerConfig.GameConfig.StartingMemo);

        if (randomizerConfig.LevelData != null)
        {
            if (room_id == 5)
            {
                // Fixes minor inconsistencies in the minimap
                Patches.Fixes.FixZebethMinimap.Apply(gmData, decompileContext);

                // Fixes the spawnpoint system
                Patches.Fixes.FixSpawnLocations.Apply(gmData, decompileContext);

                // Fixes the bad RNG
                Patches.Fixes.FixBadRNG.Apply(gmData, decompileContext);

                var keyCount = randomizerConfig.LevelData.Pickups != null ? (randomizerConfig.LevelData.Pickups.Count > 40 ? randomizerConfig.LevelData.Pickups.Count(kvp => kvp.Value.Type == "Tourian Key") : 2) : 2;

                // Switch to key locked bridge
                Patches.Fixes.CheckForZebethTourianKeys.Apply(gmData, decompileContext, keyCount);

                // Make the boss death items appear instead of giving them directly
                // Separate the pickups from Kraid's death
                Patches.Fixes.SeparateKraidPickupsFromDeath.Apply(gmData, decompileContext);

                // Separate the pickups from Ridley's death
                Patches.Fixes.SeparateRidleyPickupsFromDeath.Apply(gmData, decompileContext);
            }

            // Patching pickups
            if (randomizerConfig.LevelData.Pickups != null)
            {
                var pickups = randomizerConfig.LevelData.Pickups.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                foreach ((int pickup_obj_id, Pickup pickup) in pickups)
                {
                    if (gmData.Rooms[room_id].GameObjects.Any(obj => obj.InstanceID == pickup_obj_id))
                    {
                        Patches.EditPickup.Apply(gmData,
                                                 room_id,
                                                 pickup_obj_id,
                                                 pickup);
                    }
                }
            }
        }

        // QoL patches
        if (randomizerConfig.GameConfig != null)
        {
            if (randomizerConfig.GameConfig.WarpToStart)
                Patches.QoL.WarpToStart.Apply(gmData, decompileContext);

            if (randomizerConfig.GameConfig.OpenMissileDoorsWithOneMissile)
                Patches.QoL.OpenMissileDoorsWithOneMissile.Apply(gmData, decompileContext);

            /*if (randomizerConfig.GameConfig.AllowDownwardShots)
                Patches.QoL.AllowDownwardShots.Apply(gmData, decompileContext);

            if (randomizerConfig.GameConfig.AllowWallJump)
                Patches.QoL.AllowWallJump.Apply(gmData, decompileContext);*/
        }

        Patches.QoL.DisableLowHealthBeeping.Apply(gmData, decompileContext, !randomizerConfig.Preferences.DisableLowHealthBeeping);
        Patches.QoL.UseSMBossTheme.Apply(gmData, decompileContext, randomizerConfig.Preferences.UseSMBossTheme);
        Patches.QoL.UseAlternativeEscapeTheme.Apply(gmData, decompileContext, randomizerConfig.Preferences.UseAlternativeMusicTheme);
        Patches.QoL.ShowUnexploredMap.Apply(gmData, decompileContext, randomizerConfig.Preferences.ShowUnexploredMap);
        Patches.QoL.RoomNameOnHUD.Apply(gmData, decompileContext, randomizerConfig.Preferences.RoomNameOnHUD);

        if (!Directory.Exists(outputDir.FullName))
            Directory.CreateDirectory(outputDir.FullName);

        if (File.Exists(outputPath))
            File.Delete(outputPath);

        using (FileStream fs = new FileInfo(outputPath).OpenWrite())
        {
            UndertaleIO.Write(fs, gmData, Console.WriteLine);
        }
    }
}