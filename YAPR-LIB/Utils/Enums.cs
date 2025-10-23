using System.Text.Json.Serialization;

namespace YAPR_LIB.Utils
{
    #region Game enums
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Room
    {
        rm_Zebeth = 5,
        rm_Novus
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Layer
    {
        Tiles_Objects,
        Tiles_Foreground,
        Tiles_Solid,
        Tiles_Liquids,
        Instances,
        Tiles_Background,
        World_Builder
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
    #endregion

    #region Config enums
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RoomGuiType
    {
        NONE,
        WITH_FADE,
        ALWAYS
    }
    #endregion
}
