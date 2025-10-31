using UndertaleModLib;
using UndertaleModLib.Models;
using YAPR_LIB.Extensions;
using YAPR_LIB.Utils;

namespace YAPR_LIB.Patches
{
    public static class EditPickup
    {
        static int GetIndexFromObjectIndex(Room room, int obj_id)
        {
            if (room != Room.rm_Zebeth)
                throw new NotImplementedException();

            if (obj_id == 100018)
                return 0;
            else if (obj_id >= 100027 && obj_id <= 100063) // 1 to 37
                return obj_id - 100026;
            else if (obj_id >= 100083 && obj_id <= 100086) // 38 to 41
                return 38 + (obj_id - 100083);

            throw new NotImplementedException();
        }

        public static void Apply(UndertaleData gmData, Room room, int obj_id, Pickup pickup)
        {
            var idx = pickup.Index is not null ? pickup.Index : GetIndexFromObjectIndex(room, obj_id);
            var pickup_obj = gmData.Rooms[(int)room]
                                   .GameObjects
                                   .Select(obj => obj)
                                   .FirstOrDefault(obj => obj.InstanceID == obj_id);
            if (pickup_obj is null) return;

            var new_pickup_obj_type = gmData.GameObjects
                                            .Select(obj => obj)
                                            .FirstOrDefault(obj => obj.Name.Content == PickupUtils.GetObjectFromName(pickup.Type))
                                            .Clone();

            if (new_pickup_obj_type is null) return;

            var new_pickup_obj_sprite = gmData.Sprites
                                              .Select(spr => spr)
                                              .FirstOrDefault(spr => spr.Name.Content == pickup.Model);

            if (new_pickup_obj_sprite is null) return;

            // Events

            var obj_randomizerItem__Create_0_code_str = new UndertaleString($"gml_Object_obj_RandomizerItem_{idx}_Create_0");
            gmData.Strings.Add(obj_randomizerItem__Create_0_code_str);

            var obj_randomizerItem__Create_0_code = new UndertaleCode();
            obj_randomizerItem__Create_0_code.Name = obj_randomizerItem__Create_0_code_str;
            var code = StringUtils.MakeUnixString(
              $$"""
                event_inherited()
                Item_Name = "{{pickup.Type}}"
                Item_ID = {{idx}}
                Item_Type = {{PickupUtils.GetItemTypeFromName(pickup.Type)}}
                Item_Quantity = {{pickup.Quantity}}
                Item_Acquired_Sound = {{PickupUtils.GetAcquiredSfxFromName(pickup.Type)}}
                Item_Text_Header = "{{(pickup.Text?.Header ?? string.Empty).ToUpper()}}"
                Item_Text_Description = "{{String.Join("\n", pickup.Text?.Description ?? new List<string>())}}"
                Item_Text_Locked_Header = "{{(pickup.LockedText?.Header ?? string.Empty).ToUpper()}}"
                Item_Text_Locked_Description = "{{String.Join("\n", pickup.LockedText?.Description ?? new List<string>())}}"
                Item_Is_Launcher = {{(pickup.IsLauncher ? 1 : 0)}};
                sprite_index = {{pickup.Model}}

                """
            );
            if (obj_id >= 100083 && obj_id <= 100086)
                code += "Active = 0";
            code += "\n";

            obj_randomizerItem__Create_0_code.ReplaceGML(code, gmData);
            gmData.Code.Add(obj_randomizerItem__Create_0_code);

            var obj_randomizerItem__PreCreate_0_code_str = new UndertaleString($"gml_Object_obj_RandomizerItem_{idx}_PreCreate_0");
            gmData.Strings.Add(obj_randomizerItem__PreCreate_0_code_str);

            var obj_randomizerItem__PreCreate_0_code = new UndertaleCode();
            obj_randomizerItem__PreCreate_0_code.Name = obj_randomizerItem__PreCreate_0_code_str;
            obj_randomizerItem__PreCreate_0_code.ReplaceGML("event_inherited()\n", gmData);

            gmData.Code.Add(obj_randomizerItem__PreCreate_0_code);

            var obj_randomizerItem__Step_0_code_str = new UndertaleString($"gml_Object_obj_RandomizerItem_{idx}_Step_0");
            gmData.Strings.Add(obj_randomizerItem__Step_0_code_str);

            var obj_randomizerItem__Step_0_code = new UndertaleCode();
            obj_randomizerItem__Step_0_code.Name = obj_randomizerItem__Step_0_code_str;
            obj_randomizerItem__Step_0_code.ReplaceGML(
                StringUtils.MakeUnixString(
                  $$"""
                    Item_ID = {{idx}}
                    sprite_index = {{pickup.Model}}
                    event_inherited()

                    """
                ),
                gmData
            );

            gmData.Code.Add(obj_randomizerItem__Step_0_code);

            // Game object creation

            var obj_randomizerItem__str = new UndertaleString($"obj_RandomizerItem_{idx}");
            gmData.Strings.Add(obj_randomizerItem__str);

            var pickup_obj_event_create_0 = new UndertaleGameObject.Event();
            pickup_obj_event_create_0.Actions.Add(new UndertaleGameObject.EventAction()
            {
                CodeId = obj_randomizerItem__Create_0_code
            });

            var pickup_obj_event_precreate_0 = new UndertaleGameObject.Event();
            pickup_obj_event_precreate_0.Actions.Add(new UndertaleGameObject.EventAction()
            {
                CodeId = obj_randomizerItem__PreCreate_0_code
            });

            var pickup_obj_event_step_0 = new UndertaleGameObject.Event();
            pickup_obj_event_step_0.Actions.Add(new UndertaleGameObject.EventAction()
            {
                CodeId = obj_randomizerItem__Step_0_code
            });

            pickup_obj.ObjectDefinition = new_pickup_obj_type;
            pickup_obj.ObjectDefinition.Events[(int)EventType.Create].Add(pickup_obj_event_create_0);
            pickup_obj.ObjectDefinition.Events[(int)EventType.PreCreate].Add(pickup_obj_event_precreate_0);
            pickup_obj.ObjectDefinition.Events[(int)EventType.Step].Add(pickup_obj_event_step_0);
            pickup_obj.ObjectDefinition.Name = obj_randomizerItem__str;

            gmData.GameObjects.Add(new_pickup_obj_type);
        }
    }
}