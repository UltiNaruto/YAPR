using System.Globalization;
using System.Reflection;
using System.Text.Json;
using UndertaleModLib;
using UndertaleModLib.Decompiler;
using YAPR_LIB.Utils;

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

    public static void Main(string inputPath, string outputPath, string jsonPath)
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        var inputDir = Directory.GetParent(inputPath);
        if (inputDir is null)
            throw new DirectoryNotFoundException("Couldn't find the parent folder of input path!");
        var outputDir = Directory.GetParent(outputPath);
        if (outputDir is null)
            throw new DirectoryNotFoundException("Couldn't find the parent folder of output path!");
        RandomizerConfig? randomizerConfig = JsonSerializer.Deserialize<RandomizerConfig>(
            File.ReadAllText(jsonPath),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        );

        if (randomizerConfig is null)
            throw new Exception("Invalid config detected!");
        if (randomizerConfig.LevelData is null)
            throw new Exception("Invalid config detected!");
        if (randomizerConfig.GameConfig is null)
            throw new Exception("Invalid config detected!");

        var gmData = new UndertaleData();

        using (FileStream fs = new FileInfo(inputPath).OpenRead())
        {
            gmData = UndertaleIO.Read(fs);
        }

        // Check version
        if (gmData.GeneralInfo.DisplayName.Content != "Metroid Planets v1.27g")
            throw new Exception("The selected game is not Metroid Planets 1.27g!");

        var decompileContext = new GlobalDecompileContext(gmData, false);

        // Import custom sprites into the game
        Patches.CustomSprites.Apply(gmData);

        // Globals
        Patches.CustomPickups.Globals.AddCustomItemsToGlobals.Apply(gmData, decompileContext);

        // Create non vanilla pickups (multiworld or randomizer items)
        Patches.CustomPickups.CreateBigMissileTankPickup.Apply(gmData, decompileContext);
        Patches.CustomPickups.CreateNothingPickup.Apply(gmData, decompileContext);
        Patches.CustomPickups.CreateTourianKeyPickup.Apply(gmData, decompileContext);

        // Do patches
        Patches.RemoveChozoStatueSpheres.Apply(gmData, randomizerConfig.LevelData.Room);
        Patches.DisplaySeedHash.Apply(gmData, decompileContext, randomizerConfig.LevelData.Room, randomizerConfig.GameConfig?.SeedIdentifier?.WordHash ?? string.Empty);
        Patches.EditMenu.Apply(gmData, decompileContext, randomizerConfig.LevelData.Room);
        Patches.MoveSavesToRandovaniaFolder.Apply(gmData, decompileContext, randomizerConfig.LevelData.Room, randomizerConfig.GameConfig?.SeedIdentifier?.WordHash ?? string.Empty);
        Patches.CustomMessageBox.Apply(gmData, decompileContext);
        Patches.CustomPickupHandling.Apply(gmData, decompileContext);
        Patches.Fixes.AddTourianKeysToHUD.Apply(gmData, decompileContext);
        Patches.Fixes.AddTourianKeysToSaveSlot.Apply(gmData, decompileContext);
        Patches.CustomPickups.Replay.CustomItemsReplaySupport.Apply(gmData, decompileContext, randomizerConfig.LevelData.Room);
        Patches.Fixes.FixRipperDamageVulnerabilities.Apply(gmData, decompileContext);
        Patches.AddCreditsScreen.Apply(gmData, decompileContext, randomizerConfig.LevelData.Room, randomizerConfig.GameConfig?.CreditsString);
        Patches.Fixes.RequiredLauncherChange.Apply(gmData, decompileContext);
#if DEBUG
        Patches.Debug.AddSavingGameSaveAsJSON.Apply(gmData, decompileContext);
        Patches.Debug.DrawScreenCoords.Apply(gmData, decompileContext);
#endif

        if (randomizerConfig.LevelData.Room == Room.rm_Zebeth)
        {
            // Fixes minor inconsistencies in the minimap
            Patches.Fixes.FixZebethMinimap.Apply(gmData, decompileContext);

            // Fixes the spawnpoint system
            Patches.Fixes.FixSpawnLocations.Apply(gmData, decompileContext);

            // Fixes the bad RNG
            Patches.Fixes.FixBadRNG.Apply(gmData, decompileContext);

            var keyCount = randomizerConfig.LevelData.Pickups is not null ? (randomizerConfig.LevelData.Pickups.Count > 40 ? randomizerConfig.LevelData.Pickups.Count(kvp => kvp.Value.Type == "Tourian Key") : 2) : 2;

            // Switch to key locked bridge
            Patches.Fixes.CheckForZebethTourianKeys.Apply(gmData, decompileContext, keyCount);

            // Make the boss death items appear instead of giving them directly
            // Separate the pickups from Kraid's death
            Patches.Fixes.SeparateKraidPickupsFromDeath.Apply(gmData, decompileContext);

            // Separate the pickups from Ridley's death
            Patches.Fixes.SeparateRidleyPickupsFromDeath.Apply(gmData, decompileContext);
        }

        // Patching pickups
        if (randomizerConfig.LevelData.Pickups is not null)
        {
            var pickups = randomizerConfig.LevelData.Pickups.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            foreach ((int pickup_obj_id, Pickup pickup) in pickups)
            {
                if (gmData.Rooms[(int)randomizerConfig.LevelData.Room].GameObjects.Any(obj => obj.InstanceID == pickup_obj_id))
                {
                    Patches.EditPickup.Apply(gmData,
                                                randomizerConfig.LevelData.Room,
                                                pickup_obj_id,
                                                pickup);
                }
            }
        }

        // QoL patches
        if (randomizerConfig.GameConfig?.WarpToStart ?? false)
            Patches.QoL.WarpToStart.Apply(gmData, decompileContext);

        if (randomizerConfig.GameConfig?.OpenMissileDoorsWithOneMissile ?? false)
            Patches.QoL.OpenMissileDoorsWithOneMissile.Apply(gmData, decompileContext);

        if (randomizerConfig.GameConfig?.AllowDownwardShots ?? false)
            Patches.QoL.AllowDownwardShots.Apply(gmData, decompileContext);

        /*if (randomizerConfig.GameConfig.AllowWallJump)
            Patches.QoL.AllowWallJump.Apply(gmData, decompileContext);*/

        Patches.QoL.DisableLowHealthBeeping.Apply(gmData, decompileContext, !randomizerConfig.Preferences?.DisableLowHealthBeeping ?? false);
        Patches.QoL.UseSMBossTheme.Apply(gmData, decompileContext, randomizerConfig.Preferences?.UseSMBossTheme ?? false);
        Patches.QoL.UseAlternativeEscapeTheme.Apply(gmData, decompileContext, randomizerConfig.Preferences?.UseAlternativeMusicTheme ?? false);
        Patches.QoL.ShowUnexploredMap.Apply(gmData, decompileContext, randomizerConfig.Preferences?.ShowUnexploredMap ?? false);
        Patches.QoL.RoomNameOnHUD.Apply(gmData, decompileContext, randomizerConfig.Preferences?.RoomNameOnHUD ?? RoomGuiType.NONE);

        // Starting condition patches
        Patches.EditStartingArea.Apply(gmData, decompileContext, randomizerConfig.LevelData.Room, randomizerConfig.GameConfig?.StartingRoom ?? new StartingLocation() { X = 648, Y = 3296 });
        Patches.EditStartingItems.Apply(gmData, decompileContext, randomizerConfig.LevelData.Room, randomizerConfig.GameConfig?.StartingItems, randomizerConfig.GameConfig?.StartingMemo);

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
