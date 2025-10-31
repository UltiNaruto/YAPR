using UndertaleModLib.Decompiler;
using UndertaleModLib;
using static UndertaleModLib.Models.UndertaleRoom;
using UndertaleModLib.Models;
using YAPR_LIB.Extensions;

namespace YAPR_LIB.Patches.Fixes
{
    public static class SeparateRidleyPickupsFromDeath
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext)
        {
            // Activate items on boss death
            var scr_Boss_Remove_code = gmData.Code.ByName("gml_Script_scr_Boss_Remove");
            var scr_Boss_Remove = Decompiler.Decompile(scr_Boss_Remove_code, decompileContext);
            scr_Boss_Remove = scr_Boss_Remove.UnixReplace(
                """
                            missile = 75

                """,
                """
                            with(par_Item)
                            {
                                if (id == 100085)
                                {
                                    Active = 1
                                }
                                if (id == 100086)
                                {
                                   Active = 1
                                }
                           }
                           return;

                """
            );

            scr_Boss_Remove_code.ReplaceGML(scr_Boss_Remove, gmData);

            // Reactivate items if save was reloaded
            var scr_Handle_Door_Transition_code = gmData.Code.ByName("gml_Script_scr_Handle_Door_Transition");
            var scr_Handle_Door_Transition = Decompiler.Decompile(scr_Handle_Door_Transition_code, decompileContext);
            scr_Handle_Door_Transition = scr_Handle_Door_Transition.UnixReplace(
                """
                        Room_Transition_Timer -= 1
                        if (Room_Transition_Timer <= 0)
                        {

                """,
                """
                        Room_Transition_Timer -= 1
                        if (Room_Transition_Timer <= 0)
                        {
                            if (obj_MAIN.Screen_X == 16 && obj_MAIN.Screen_Y == 28 && !instance_exists(obj_Boss_Ridley))
                            {
                                with(par_Item)
                                {
                                    if (global.SAVE_BACKUP > -1)
                                    {
                                        if (id == 100085 && ds_map_find_value(ds_list_find_value(ds_map_find_value(global.SAVE_BACKUP, "ITEMS"), 40), "collected") == 0)
                                            Active = 1
                                        if (id == 100086 && ds_map_find_value(ds_list_find_value(ds_map_find_value(global.SAVE_BACKUP, "ITEMS"), 41), "collected") == 0)
                                            Active = 1
                                    }
                                }
                            }

                """
            );

            scr_Handle_Door_Transition_code.ReplaceGML(scr_Handle_Door_Transition, gmData);

            AddTourianKeyToRoom(gmData);
            AddBigMissileTankToRoom(gmData);
        }

        static void AddTourianKeyToRoom(UndertaleData gmData)
        {
            var obj_Item_Tourian_Key = gmData.GameObjects
                                             .FirstOrDefault(obj => obj.Name.Content == "obj_Item_Tourian_Key");

            if (obj_Item_Tourian_Key is null)
                throw new Exception("Shouldn't happen!");

            obj_Item_Tourian_Key = obj_Item_Tourian_Key.Clone();

            // Events
            var obj_Item_Ridley_Key_Create_0_code_str = new UndertaleString("gml_Object_obj_Item_Ridley_Key_Create_0");
            gmData.Strings.Add(obj_Item_Ridley_Key_Create_0_code_str);

            var obj_Item_Ridley_Key_Create_0_code = new UndertaleCode();
            obj_Item_Ridley_Key_Create_0_code.Name = obj_Item_Ridley_Key_Create_0_code_str;
            var code = StringUtils.MakeUnixString(
                """
                event_inherited()
                Active = 0
                Item_Name = "Tourian Key"
                Item_ID = 40
                Item_Type = 30
                Item_Quantity = 1
                Item_Acquired_Sound = sfx_Notify_Boss_Defeated
                Item_Text_Header = "TOURIAN KEY ACQUIRED"
                Item_Text_Description = "Opens Tourian access when all of the keys are retrieved"
                sprite_index = spr_ITEM_Tourian_Key

                """
            );
            obj_Item_Ridley_Key_Create_0_code.ReplaceGML(code, gmData);
            gmData.Code.Add(obj_Item_Ridley_Key_Create_0_code);

            var obj_Item_Ridley_Key_PreCreate_0_code_str = new UndertaleString("gml_Object_obj_Item_Ridley_Key_PreCreate_0");
            gmData.Strings.Add(obj_Item_Ridley_Key_PreCreate_0_code_str);

            var obj_Item_Ridley_Key_PreCreate_0_code = new UndertaleCode();
            obj_Item_Ridley_Key_PreCreate_0_code.Name = obj_Item_Ridley_Key_PreCreate_0_code_str;
            obj_Item_Ridley_Key_PreCreate_0_code.ReplaceGML("event_inherited()\n", gmData);

            gmData.Code.Add(obj_Item_Ridley_Key_PreCreate_0_code);

            var obj_Item_Ridley_Key_Step_0_code_str = new UndertaleString("gml_Object_obj_Item_Ridley_Key_Step_0");
            gmData.Strings.Add(obj_Item_Ridley_Key_Step_0_code_str);

            var obj_Item_Ridley_Key_Step_0_code = new UndertaleCode();
            obj_Item_Ridley_Key_Step_0_code.Name = obj_Item_Ridley_Key_Step_0_code_str;
            obj_Item_Ridley_Key_Step_0_code.ReplaceGML(
                StringUtils.MakeUnixString(
                    """
                    Item_ID = 40
                    event_inherited()
                
                    """
                ),
                gmData
            );
            gmData.Code.Add(obj_Item_Ridley_Key_Step_0_code);

            // Object creation
            var obj_Item_Ridley_Key_str = new UndertaleString("obj_Item_Ridley_Key");
            gmData.Strings.Add(obj_Item_Ridley_Key_str);

            var obj_Item_Ridley_Key_Create_0 = new UndertaleGameObject.Event();
            obj_Item_Ridley_Key_Create_0.Actions.Add(new UndertaleGameObject.EventAction()
            {
                CodeId = obj_Item_Ridley_Key_Create_0_code
            });

            var obj_Item_Ridley_Key_PreCreate_0 = new UndertaleGameObject.Event();
            obj_Item_Ridley_Key_PreCreate_0.Actions.Add(new UndertaleGameObject.EventAction()
            {
                CodeId = obj_Item_Ridley_Key_PreCreate_0_code
            });

            var obj_Item_Ridley_Key_Step_0 = new UndertaleGameObject.Event();
            obj_Item_Ridley_Key_Step_0.Actions.Add(new UndertaleGameObject.EventAction()
            {
                CodeId = obj_Item_Ridley_Key_Step_0_code
            });

            var obj_Item_Ridley_Key = new GameObject();
            obj_Item_Ridley_Key.X = 4232;
            obj_Item_Ridley_Key.Y = 6856;
            obj_Item_Ridley_Key.ObjectDefinition = obj_Item_Tourian_Key;
            obj_Item_Ridley_Key.ObjectDefinition.Name = obj_Item_Ridley_Key_str;
            obj_Item_Ridley_Key.ObjectDefinition.Events[(int)EventType.Create].Add(obj_Item_Ridley_Key_Create_0);
            obj_Item_Ridley_Key.ObjectDefinition.Events[(int)EventType.PreCreate].Add(obj_Item_Ridley_Key_PreCreate_0);
            obj_Item_Ridley_Key.ObjectDefinition.Events[(int)EventType.Step].Add(obj_Item_Ridley_Key_Step_0);
            obj_Item_Ridley_Key.ObjectDefinition.Sprite = gmData.Sprites.FirstOrDefault(spr => spr.Name.Content == "spr_ITEM_Tourian_Key");
            obj_Item_Ridley_Key.InstanceID = 100085;
            obj_Item_Ridley_Key.ScaleX = 1;
            obj_Item_Ridley_Key.ScaleY = 1;
            obj_Item_Ridley_Key.Color = 0xFFFFFFFF;
            obj_Item_Ridley_Key.Rotation = 0;

            gmData.GameObjects.Add(obj_Item_Ridley_Key.ObjectDefinition);
            gmData.Rooms[5].GameObjects.Add(obj_Item_Ridley_Key);
            gmData.Rooms[5].Layers[4].InstancesData.Instances.Add(obj_Item_Ridley_Key);
        }

        static void AddBigMissileTankToRoom(UndertaleData gmData)
        {
            var obj_Item_Big_Missile_def = gmData.GameObjects
                                                 .FirstOrDefault(obj => obj.Name.Content == "obj_Item_Big_Missile");

            if (obj_Item_Big_Missile_def is null)
                throw new Exception("Shouldn't happen!");

            obj_Item_Big_Missile_def = obj_Item_Big_Missile_def.Clone();

            // Events
            var obj_Item_Big_Missile_Create_0_code_str = new UndertaleString("gml_Object_obj_Item_Big_Missile_Create_0");
            gmData.Strings.Add(obj_Item_Big_Missile_Create_0_code_str);

            var obj_Item_Big_Missile_Create_0_code = new UndertaleCode();
            obj_Item_Big_Missile_Create_0_code.Name = obj_Item_Big_Missile_Create_0_code_str;
            var code = StringUtils.MakeUnixString(
                """
                event_inherited()
                Active = 0
                Item_Name = "Tourian Key"
                Item_ID = 41
                Item_Type = 16
                Item_Quantity = 75
                Item_Acquired_Sound = bgm_Minor_Item_Get
                Item_Text_Header = "BIG MISSILE TANK ACQUIRED"
                Item_Text_Description = "Missile capacity increased by 75"
                sprite_index = spr_ITEM_Big_Missile

                """
            );
            obj_Item_Big_Missile_Create_0_code.ReplaceGML(code, gmData);
            gmData.Code.Add(obj_Item_Big_Missile_Create_0_code);

            var obj_Item_Big_Missile_PreCreate_0_code_str = new UndertaleString("gml_Object_obj_Item_Big_Missile_PreCreate_0");
            gmData.Strings.Add(obj_Item_Big_Missile_PreCreate_0_code_str);

            var obj_Item_Big_Missile_PreCreate_0_code = new UndertaleCode();
            obj_Item_Big_Missile_PreCreate_0_code.Name = obj_Item_Big_Missile_PreCreate_0_code_str;
            obj_Item_Big_Missile_PreCreate_0_code.ReplaceGML("event_inherited()\n", gmData);

            gmData.Code.Add(obj_Item_Big_Missile_PreCreate_0_code);

            var obj_Item_Big_Missile_Step_0_code_str = new UndertaleString("gml_Object_obj_Item_Big_Missile_Step_0");
            gmData.Strings.Add(obj_Item_Big_Missile_Step_0_code_str);

            var obj_Item_Big_Missile_Step_0_code = new UndertaleCode();
            obj_Item_Big_Missile_Step_0_code.Name = obj_Item_Big_Missile_Step_0_code_str;
            obj_Item_Big_Missile_Step_0_code.ReplaceGML(
                StringUtils.MakeUnixString(
                    """
                    Item_ID = 41
                    event_inherited()
                
                    """
                ),
                gmData
            );
            gmData.Code.Add(obj_Item_Big_Missile_Step_0_code);

            // Object creation
            var obj_Item_Big_Missile_str = new UndertaleString("obj_Item_Ridley_Big_Missile");
            gmData.Strings.Add(obj_Item_Big_Missile_str);

            var obj_Item_Big_Missile_Create_0 = new UndertaleGameObject.Event();
            obj_Item_Big_Missile_Create_0.Actions.Add(new UndertaleGameObject.EventAction()
            {
                CodeId = obj_Item_Big_Missile_Create_0_code
            });

            var obj_Item_Big_Missile_PreCreate_0 = new UndertaleGameObject.Event();
            obj_Item_Big_Missile_PreCreate_0.Actions.Add(new UndertaleGameObject.EventAction()
            {
                CodeId = obj_Item_Big_Missile_PreCreate_0_code
            });

            var obj_Item_Big_Missile_Step_0 = new UndertaleGameObject.Event();
            obj_Item_Big_Missile_Step_0.Actions.Add(new UndertaleGameObject.EventAction()
            {
                CodeId = obj_Item_Big_Missile_Step_0_code
            });

            var obj_Item_Big_Missile = new GameObject();
            obj_Item_Big_Missile.X = 4328;
            obj_Item_Big_Missile.Y = 6872;
            obj_Item_Big_Missile.ObjectDefinition = obj_Item_Big_Missile_def;
            obj_Item_Big_Missile.ObjectDefinition.Name = obj_Item_Big_Missile_str;
            obj_Item_Big_Missile.ObjectDefinition.Events[(int)EventType.Create].Add(obj_Item_Big_Missile_Create_0);
            obj_Item_Big_Missile.ObjectDefinition.Events[(int)EventType.PreCreate].Add(obj_Item_Big_Missile_PreCreate_0);
            obj_Item_Big_Missile.ObjectDefinition.Events[(int)EventType.Step].Add(obj_Item_Big_Missile_Step_0);
            obj_Item_Big_Missile.ObjectDefinition.Sprite = gmData.Sprites.FirstOrDefault(spr => spr.Name.Content == "spr_ITEM_Big_Missile");
            obj_Item_Big_Missile.InstanceID = 100086;
            obj_Item_Big_Missile.ScaleX = 1;
            obj_Item_Big_Missile.ScaleY = 1;
            obj_Item_Big_Missile.Color = 0xFFFFFFFF;
            obj_Item_Big_Missile.Rotation = 0;

            gmData.Rooms[5].GameObjects.Add(obj_Item_Big_Missile);
            gmData.Rooms[5].Layers[4].InstancesData.Instances.Add(obj_Item_Big_Missile);
        }
    }
}
