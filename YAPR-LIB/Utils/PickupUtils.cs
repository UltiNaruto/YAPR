namespace YAPR_LIB.Utils
{
    public static class PickupUtils
    {
        public static string GetObjectFromName(string name)
        {
            switch (name)
            {
                case "Morph Ball":
                    return "obj_Item_Morph";
                case "Varia Suit":
                    return "obj_Item_Varia";
                case "Long Beam":
                    return "obj_Item_Long_Beam";
                case "Wave Beam":
                    return "obj_Item_Wave_Beam";
                case "Spazer Beam":
                    return "obj_Item_Spazer_Beam";
                case "Ice Beam":
                    return "obj_Item_Ice_Beam";
                case "Screw Attack":
                    return "obj_Item_Screw_Attack";
                case "Hi-Jump Boots":
                    return "obj_Item_Jump_Boots";
                case "Missile Launcher":
                case "Missile Tank":
                case "Big Missile Tank":
                    return "obj_Item_Missles";
                case "Super Missile Launcher":
                case "Super Missile Tank":
                    return "obj_Item_Super_Missles";
                case "Bombs":
                    return "obj_Item_Bomb";
                case "Energy Tank":
                    return "obj_Item_Energy_Tank";
                case "Sensor Visor":
                    return "obj_Item_Sensor_Visor";
                case "Spring Ball":
                    return "obj_Item_Spring_Ball";
                case "Speed Booster":
                    return "obj_Item_Speed_Booster";
                case "Tourian Key":
                    return "obj_Item_Tourian_Key";
                default:
                    return "obj_Item_Nothing";
            }
        }

        public static int GetItemTypeFromName(string name)
        {
            switch (name)
            {
                case "Long Beam":
                    return 1;
                case "Ice Beam":
                    return 3;
                case "Wave Beam":
                    return 4;
                case "Spazer Beam":
                    return 5;
                case "Energy Tank":
                    return 7;
                case "Varia Suit":
                    return 8;
                case "Morph Ball":
                    return 10;
                case "Spring Ball":
                    return 11;
                case "Bombs":
                    return 14;
                case "Missile Launcher":
                case "Missile Tank":
                case "Big Missile Tank":
                    return 16;
                case "Super Missile Launcher":
                case "Super Missile Tank":
                    return 17;
                case "Hi-Jump Boots":
                    return 18;
                case "Speed Booster":
                    return 20;
                case "Screw Attack":
                    return 21;
                case "Sensor Visor":
                    return 22;
                case "Tourian Key":
                    return 30;
                default:
                    return 20;
            }
        }

        public static string GetAcquiredSfxFromName(string name)
        {
            switch (name)
            {
                case "Long Beam":
                    return "bgm_Item_Get";
                case "Ice Beam":
                    return "bgm_Item_Get";
                case "Wave Beam":
                    return "bgm_Item_Get";
                case "Spazer Beam":
                    return "bgm_Item_Get";
                case "Energy Tank":
                    return "bgm_Item_Get";
                case "Varia Suit":
                    return "bgm_Item_Get";
                case "Morph Ball":
                    return "bgm_Item_Get";
                case "Spring Ball":
                    return "bgm_Item_Get";
                case "Bombs":
                    return "bgm_Item_Get";
                case "Missile Launcher":
                    return "bgm_Item_Get";
                case "Missile Tank":
                    return "bgm_Minor_Item_Get";
                case "Big Missile Tank":
                    return "bgm_Minor_Item_Get";
                case "Super Missile Launcher":
                    return "bgm_Item_Get";
                case "Super Missile Tank":
                    return "bgm_Minor_Item_Get";
                case "Hi-Jump Boots":
                    return "bgm_Item_Get";
                case "Speed Booster":
                    return "bgm_Item_Get";
                case "Screw Attack":
                    return "bgm_Item_Get";
                case "Sensor Visor":
                    return "bgm_Item_Get";
                case "Tourian Key":
                    return "sfx_Notify_Boss_Defeated";
                default:
                    return "sfx_Notify_Item_Collect";
            }
        }
    }
}
