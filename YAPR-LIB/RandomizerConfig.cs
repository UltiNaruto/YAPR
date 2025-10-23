using System.Text.Json.Serialization;
using YAPR_LIB.Utils;

namespace YAPR_LIB
{
    #region Structs
    public class RandomizerConfig : IJsonOnDeserialized
    {
        [JsonInclude]
        [JsonPropertyName("seed")]
        public int Seed;

        [JsonInclude]
        [JsonPropertyName("game_config")]
        public GameConfig? GameConfig;

        [JsonInclude]
        [JsonPropertyName("preferences")]
        public Preferences? Preferences;

        [JsonInclude]
        [JsonPropertyName("level_data")]
        public LevelData? LevelData;

        public void OnDeserialized()
        {
            if (GameConfig is null)
                throw new Exception("game_config is missing!");
            if (LevelData is null)
                throw new Exception("level_data is missing!");
        }
    }

    public class GameConfig
    {
        [JsonInclude]
        [JsonPropertyName("starting_room")]
        public StartingLocation? StartingRoom;

        [JsonInclude]
        [JsonPropertyName("seed_identifier")]
        public SeedIdentifier? SeedIdentifier;

        [JsonInclude]
        [JsonPropertyName("starting_items")]
        public Dictionary<string, int>? StartingItems;

        [JsonInclude]
        [JsonPropertyName("starting_memo")]
        public Text? StartingMemo;

        [JsonInclude]
        [JsonPropertyName("warp_to_start")]
        public bool WarpToStart;

        [JsonInclude]
        [JsonPropertyName("open_missile_doors_with_one_missile")]
        public bool OpenMissileDoorsWithOneMissile;

        [JsonInclude]
        [JsonPropertyName("credits_string")]
        public List<Text>? CreditsString;
    }

    public class StartingLocation
    {
        [JsonInclude]
        [JsonPropertyName("x")]
        public int X;

        [JsonInclude]
        [JsonPropertyName("y")]
        public int Y;
    }

    public class SeedIdentifier
    {
        [JsonInclude]
        [JsonPropertyName("word_hash")]
        public string? WordHash;

        [JsonInclude]
        [JsonPropertyName("hash")]
        public string? Hash;

        [JsonInclude]
        [JsonPropertyName("session_uuid")]
        public string? SessionUUID;
    }

    public class Preferences
    {
        [JsonInclude]
        [JsonPropertyName("show_unexplored_map")]
        public bool ShowUnexploredMap;

        [JsonInclude]
        [JsonPropertyName("room_names_on_hud")]
        public RoomGuiType RoomNameOnHUD;

        [JsonInclude]
        [JsonPropertyName("disable_low_health_beeping")]
        public bool DisableLowHealthBeeping;

        [JsonInclude]
        [JsonPropertyName("use_sm_boss_theme")]
        public bool UseSMBossTheme;

        [JsonInclude]
        [JsonPropertyName("use_alternative_escape_theme")]
        public bool UseAlternativeMusicTheme;
    }

    public class LevelData
    {
        [JsonInclude]
        [JsonPropertyName("room")]
        public Room Room;

        [JsonInclude]
        [JsonPropertyName("pickups")]
        public Dictionary<int, Pickup>? Pickups;

        [JsonInclude]
        [JsonPropertyName("doors")]
        public Dictionary<string, DoorType>? Doors;

        [JsonInclude]
        [JsonPropertyName("elevators")]
        public Dictionary<int, Elevator>? Elevators;
    }

    public class Pickup
    {
        [JsonInclude]
        [JsonPropertyName("index")]
        public int? Index;

        [JsonInclude]
        [JsonPropertyName("model")]
        public string? Model;

        [JsonInclude]
        [JsonPropertyName("type")]
        public string? Type;

        [JsonInclude]
        [JsonPropertyName("quantity")]
        public int Quantity;

        [JsonInclude]
        [JsonPropertyName("text")]
        public Text? Text;

        [JsonInclude]
        [JsonPropertyName("locked_text")]
        public Text? LockedText;

        // only used for Missile/Super Missile Launcher
        [JsonInclude]
        [JsonPropertyName("is_launcher")]
        public bool IsLauncher;
    }

    public class Elevator
    {
        [JsonInclude]
        [JsonPropertyName("destination_x")]
        public int DestinationX;

        [JsonInclude]
        [JsonPropertyName("destination_y")]
        public int DestinationY;

        [JsonInclude]
        [JsonPropertyName("end_game")]
        public bool EndGame;
    }

    public class Text
    {
        [JsonInclude]
        [JsonPropertyName("header")]
        public string? Header;

        [JsonInclude]
        [JsonPropertyName("description")]
        public List<string>? Description;
    }
    #endregion
}
