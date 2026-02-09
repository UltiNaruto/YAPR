using System.Reflection;
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

    public class PickupType
    {
        public static readonly PickupType Long_Beam = new PickupType(1, "Long Beam", "obj_Item_Long_Beam", "spr_ITEM_Long_Beam", "bgm_Item_Get");
        public static readonly PickupType Ice_Beam = new PickupType(3, "Ice Beam", "obj_Item_Ice_Beam", "spr_ITEM_Ice_Beam", "bgm_Item_Get");
        public static readonly PickupType Wave_Beam = new PickupType(4, "Wave Beam", "obj_Item_Wave_Beam", "spr_ITEM_Wave_Beam", "bgm_Item_Get");
        public static readonly PickupType Spazer_Beam = new PickupType(5, "Spazer Beam", "obj_Item_Spazer_Beam", "spr_ITEM_Spazer_Beam", "bgm_Item_Get", false);
        public static readonly PickupType Energy_Tank = new PickupType(7, "Energy Tank", "obj_Item_Energy_Tank", "spr_ITEM_Energy_Tank", "bgm_Item_Get");
        public static readonly PickupType Varia_Suit = new PickupType(8, "Varia Suit", "obj_Item_Varia", "spr_ITEM_Varia", "bgm_Item_Get");
        public static readonly PickupType Morph_Ball = new PickupType(10, "Morph Ball", "obj_Item_Morph", "spr_ITEM_Morph", "bgm_Item_Get");
        public static readonly PickupType Spring_Ball = new PickupType(11, "Spring Ball", "obj_Item_Spring_Ball", "spr_ITEM_Spring_Ball", "bgm_Item_Get", false);
        public static readonly PickupType Bombs = new PickupType(14, "Bombs", "obj_Item_Bomb", "spr_ITEM_Bomb", "bgm_Item_Get");
        public static readonly PickupType Missile_Launcher = new PickupType(16, "Missile Launcher", "obj_Item_Missles", "spr_ITEM_Missile", "bgm_Item_Get");
        public static readonly PickupType Missile_Tank = new PickupType(16, "Missile Tank", "obj_Item_Missles", "spr_ITEM_Missile", "bgm_Minor_Item_Get");
        public static readonly PickupType Big_Missile_Tank = new PickupType(16, "Big Missile Tank", "obj_Item_Missles", "spr_ITEM_Big_Missile", "bgm_Minor_Item_Get");
        public static readonly PickupType Super_Missile_Launcher = new PickupType(17, "Super Missile Launcher", "obj_Item_Super_Missles", "spr_ITEM_Super_Missile", "bgm_Item_Get", false, false);
        public static readonly PickupType Super_Missile_Tank = new PickupType(17, "Super Missile Tank", "obj_Item_Super_Missles", "spr_ITEM_Super_Missile", "bgm_Minor_Item_Get", false, false);
        public static readonly PickupType Hi_Jump_Boots = new PickupType(18, "Hi-Jump Boots", "obj_Item_Jump_Boots", "spr_ITEM_Jump_Boots", "bgm_Item_Get");
        public static readonly PickupType Speed_Booster = new PickupType(20, "Speed Booster", "obj_Item_Speed_Booster", "spr_ITEM_Speed_Booster", "bgm_Item_Get", false, false);
        public static readonly PickupType Screw_Attack = new PickupType(21, "Screw Attack", "obj_Item_Screw_Attack", "spr_ITEM_Screw_Attack", "bgm_Item_Get");
        public static readonly PickupType Sensor_Visor = new PickupType(22, "Sensor Visor", "obj_Item_Sensor_Visor", "spr_ITEM_Sensor_Visor", "bgm_Item_Get", false);
        public static readonly PickupType Tourian_Key = new PickupType(30, "Tourian Key", "obj_Item_Tourian_Key", "spr_ITEM_Tourian_Key", "sfx_Notify_Boss_Defeated");

        public static readonly PickupType Nothing = new PickupType(20, "Nothing", "obj_Item_Nothing", "spr_ITEM_Nothing", "sfx_Notify_Item_Collect", false);

        public static bool AllowNonNestroidItems = false;

        public readonly int Type;
        public readonly String DisplayName;
        public readonly String ObjectName;
        public readonly String SpriteName;
        public readonly String AcquiredSoundName;
        public readonly bool IsNestroidItem;
        public readonly bool IsRandomizerItem;
        public readonly bool IsImplemented;

        public PickupType(int type, String displayName, String objectName, String spriteName, String acquiredSoundName, bool isNestroidItem=true, bool isRandomizerItem=false, bool isImplemented=true)
        {
            this.Type = type;
            this.DisplayName = displayName;
            this.ObjectName = objectName;
            this.SpriteName = spriteName;
            this.AcquiredSoundName = acquiredSoundName;
            this.IsNestroidItem = isNestroidItem;
            this.IsImplemented = isImplemented;
        }

        public static Dictionary<String, PickupType> Values => typeof(PickupType).GetFields(BindingFlags.Static | BindingFlags.Public)
                                                                                 .Select(m => m.GetValue(null) ?? throw new ArgumentNullException())
                                                                                 .Where(m => m is not null)
                                                                                 .Select(m => (PickupType)m)
                                                                                 .ToDictionary(k => k.DisplayName, k => k);

        private static void CheckIfItemIsValid(String name)
        {
            if (!Values.ContainsKey(name))
                throw new Exception("It's not a valid item.");

            if (!Values[name].IsImplemented)
                throw new Exception($"The item is not implemented yet.");

            if (!Values[name].IsNestroidItem && !AllowNonNestroidItems)
                throw new Exception($"The item is not allowed in Nestroid.");
        }

        public static int GetItemTypeFromName(String? name)
        {
            if (name == null)
                return Values["Nothing"].Type;

            try {
                CheckIfItemIsValid(name);
            } catch (Exception ex) {
                throw new Exception($"Cannot get item type from {name}. {ex.Message}");
            }

            return Values[name].Type;
        }

        public static String GetObjectFromName(String? name)
        {
            if (name == null)
                return Values["Nothing"].ObjectName;

            try {
                CheckIfItemIsValid(name);
            } catch (Exception ex) {
                throw new Exception($"Cannot get object from {name}. {ex.Message}");
            }

            return Values[name].ObjectName;
        }

        public static String GetAcquiredSfxFromName(String? name)
        {
            if (name == null)
                return Values["Nothing"].AcquiredSoundName;

            try {
                CheckIfItemIsValid(name);
            } catch (Exception ex) {
                throw new Exception($"Cannot get acquired sfx from {name}. {ex.Message}");
            }

            return Values[name].AcquiredSoundName;
        }
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
