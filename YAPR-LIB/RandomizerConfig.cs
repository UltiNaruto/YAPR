using Newtonsoft.Json;

namespace YAPR_LIB
{
    [Serializable]
    public class RandomizerConfig
    {
        [JsonProperty("seed")]
        public int Seed;

        [JsonProperty("game_config")]
        public GameConfig? GameConfig;

        [JsonProperty("preferences")]
        public Preferences? Preferences;

        [JsonProperty("level_data")]
        public LevelData? LevelData;
    }

    [Serializable]
    public class GameConfig
    {
        [JsonProperty("starting_room")]
        public StartingLocation? StartingRoom;

        [JsonProperty("seed_identifier")]
        public SeedIdentifier? SeedIdentifier;

        [JsonProperty("starting_items")]
        public Dictionary<string, int>? StartingItems;

        [JsonProperty("starting_memo")]
        public string? StartingMemo;

        [JsonProperty("warp_to_start")]
        public bool WarpToStart;

        [JsonProperty("open_missile_doors_with_one_missile")]
        public bool OpenMissileDoorsWithOneMissile;

        [JsonProperty("credits_string")]
        public List<CreditEntry>? CreditsString;

        // TODO: implement logic for those
        /*[JsonProperty("allow_downward_shots")]
        public bool AllowDownwardShots;

        [JsonProperty("allow_wall_jump")]
        public bool AllowWallJump;*/
    }

    [Serializable]
    public class StartingLocation
    {
        [JsonProperty("x")]
        public int X;

        [JsonProperty("y")]
        public int Y;
    }

    [Serializable]
    public class SeedIdentifier
    {
        [JsonProperty("word_hash")]
        public string? WordHash;

        [JsonProperty("hash")]
        public string? Hash;

        [JsonProperty("session_uuid")]
        public string? SessionUUID;
    }

    [Serializable]
    public class CreditEntry
    {
        [JsonProperty("text")]
        public string? Text;

        [JsonProperty("sub_texts")]
        public List<string>? SubTexts;

        [JsonProperty("spacer")]
        public bool Spacer;
    }

    [Serializable]
    public class Preferences
    {
        [JsonProperty("show_unexplored_map")]
        public bool ShowUnexploredMap;

        [JsonProperty("room_name_on_hud")]
        public RoomGuiType RoomNameOnHUD;

        [JsonProperty("disable_low_health_beeping")]
        public bool DisableLowHealthBeeping;

        [JsonProperty("use_sm_boss_theme")]
        public bool UseSMBossTheme;

        [JsonProperty("use_alternative_escape_theme")]
        public bool UseAlternativeMusicTheme;
    }

    public enum RoomGuiType
    {
        NONE,
        WITH_FADE,
        ALWAYS
    }

    [Serializable]
    public class LevelData
    {
        [JsonProperty("room")]
        public string? Room;

        [JsonProperty("pickups")]
        public Dictionary<int, Pickup>? Pickups;

        [JsonProperty("doors")]
        public Dictionary<string, DoorType>? Doors;

        [JsonProperty("elevators")]
        public Dictionary<int, Elevator>? Elevators;
    }

    [Serializable]
    public class Pickup
    {
        [JsonProperty("index")]
        public int? Index;

        [JsonProperty("model")]
        public string? Model;

        [JsonProperty("type")]
        public string? Type;

        [JsonProperty("quantity")]
        public int? Quantity;

        [JsonProperty("text")]
        public PickupText? Text;
    }

    [Serializable]
    public class PickupText
    {
        [JsonProperty("header")]
        public string? Header;

        [JsonProperty("description")]
        public string? Description;
    }

    public enum DoorType
    {
        NONE,
        BLUE_WEST = 21,
        BLUE_EAST,
        MISSILE_WEST,
        MISSILE_EAST,
        TEN_MISSILES_WEST,
        TEN_MISSILES_EAST,
        LOCKED_DOOR_WEST,
        LOCKED_DOOR_EAST,
        SUPER_MISSILE_WEST,
        SUPER_MISSILE_EAST,
        BOMB_WEST,
        BOMB_EAST,
        SCREW_ATTACK_WEST,
        SCREW_ATTACK_EAST,
        ICE_WEST,
        ICE_EAST,
        WAVE_WEST,
        WAVE_EAST,
        LONG_WEST,
        LONG_EAST
    }

    [Serializable]
    public class Elevator
    {
        [JsonProperty("destination_x")]
        public int? DestinationX;

        [JsonProperty("destination_y")]
        public int? DestinationY;

        [JsonProperty("end_game")]
        public bool? EndGame;
    }
}
