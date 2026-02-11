using System.Text.Json.Serialization;
using YAPR_LIB.Utils;

namespace YAPR_LIB
{
    #region Structs
    public class RandomizerConfig
    {
        [JsonPropertyName("seed")]
        public int Seed { get; set; } = 0;

        [JsonPropertyName("game_config")]
        public GameConfig GameConfig { get; set; } = new();

        [JsonPropertyName("preferences")]
        public Preferences Preferences { get; set; } = new();

        [JsonPropertyName("level_data")]
        public LevelData LevelData { get; set; } = new();
    }

    public class GameConfig
    {
        [JsonPropertyName("starting_room")]
        public StartingLocation StartingRoom { get; set; } = new();

        [JsonPropertyName("seed_identifier")]
        public SeedIdentifier SeedIdentifier { get; set; } = new();

        [JsonPropertyName("required_messages")]
        public Dictionary<String, Text> RequiredMessages { get; set; } = new();

        [JsonPropertyName("starting_items")]
        public Dictionary<string, int> StartingItems { get; set; } = new();

        [JsonPropertyName("starting_memo")]
        public Text? StartingMemo { get; set; } = null;

        [JsonPropertyName("warp_to_start")]
        public bool WarpToStart { get; set; } = false;

        [JsonPropertyName("open_missile_doors_with_one_missile")]
        public bool OpenMissileDoorsWithOneMissile { get; set; } = false;

        [JsonPropertyName("allow_downward_shots")]
        public bool AllowDownwardShots { get; set; } = false;

        [JsonPropertyName("credits_string")]
        public List<Text> CreditsString { get; set; } = new();
    }

    public class StartingLocation
    {
        [JsonPropertyName("x")]
        public int X { get; set; } = 648;

        [JsonPropertyName("y")]
        public int Y { get; set; } = 3296;
    }

    public class SeedIdentifier
    {
        [JsonPropertyName("word_hash")]
        public string WordHash { get; set; } = string.Empty;

        [JsonPropertyName("hash")]
        public string Hash { get; set; } = string.Empty;

        [JsonPropertyName("session_uuid")]
        public string SessionUUID { get; set; } = string.Empty;
    }

    public class Preferences
    {
        [JsonPropertyName("show_unexplored_map")]
        public bool ShowUnexploredMap { get; set; } = false;

        [JsonPropertyName("room_names_on_hud")]
        public RoomGuiType RoomNameOnHUD { get; set; } = RoomGuiType.NONE;

        [JsonPropertyName("disable_low_health_beeping")]
        public bool DisableLowHealthBeeping { get; set; } = false;

        [JsonPropertyName("use_sm_boss_theme")]
        public bool UseSMBossTheme { get; set; } = false;

        [JsonPropertyName("use_alternative_escape_theme")]
        public bool UseAlternativeMusicTheme { get; set; } = false;
    }

    public class LevelData
    {
        [JsonPropertyName("room")]
        public Room Room { get; set; } = Room.rm_Invalid;

        [JsonPropertyName("pickups")]
        public Dictionary<int, Pickup> Pickups { get; set; } = new();

        [JsonPropertyName("doors")]
        public Dictionary<string, DoorType> Doors { get; set; } = new();

        [JsonPropertyName("elevators")]
        public Dictionary<int, Elevator> Elevators { get; set; } = new();
    }

    public class Pickup
    {
        [JsonPropertyName("index")]
        public int? Index { get; set; } = null;

        [JsonPropertyName("model")]
        [JsonConverter(typeof(PickupType.Converter))]
        public PickupType Model { get; set; } = PickupType.Nothing;

        [JsonPropertyName("type")]
        [JsonConverter(typeof(PickupType.Converter))]
        public PickupType Type { get; set; } = PickupType.Nothing;

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; } = 0;

        [JsonPropertyName("text")]
        public Text Text { get; set; } = new();
    }

    public class Elevator
    {
        [JsonPropertyName("destination_x")]
        public int DestinationX { get; set; } = 0;

        [JsonPropertyName("destination_y")]
        public int DestinationY { get; set; } = 0;

        [JsonPropertyName("end_game")]
        public bool EndGame { get; set; } = false;
    }

    public class Text
    {
        [JsonPropertyName("header")]
        public string Header { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public List<string> Description { get; set; } = new();
    }
    #endregion
}
