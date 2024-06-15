using UndertaleModLib.Decompiler;
using UndertaleModLib;

namespace YAPR_LIB.Patches.CustomPickups.Replay
{
    public static class CustomItemsReplaySupport
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext, string planetName)
        {
            if (planetName == "novus")
                throw new NotImplementedException();

            // Fix replay icon
            var par_Item_Step_0_code = gmData.Code.ByName("gml_Object_par_Item_Step_0");
            var par_Item_Step_0 = Decompiler.Decompile(par_Item_Step_0_code, decompileContext);

            par_Item_Step_0 = par_Item_Step_0.Replace(
                "        scr_Replay_Add_Stamp(1, Item_Type)",
                """
                        if (Item_Name == "Nothing")
                            scr_Replay_Add_Stamp(1, 27)
                        else if (Item_Name == "Big Missile Tank")
                            scr_Replay_Add_Stamp(1, 31)
                        else
                            scr_Replay_Add_Stamp(1, Item_Type)
                """.ReplaceLineEndings("\n")
            );

            par_Item_Step_0_code.ReplaceGML(par_Item_Step_0, gmData);

            var Replay_Draw_Items_code = gmData.Code.ByName("gml_Script_Replay_Draw_Items");
            var Replay_Draw_Items = Decompiler.Decompile(Replay_Draw_Items_code, decompileContext);

            Replay_Draw_Items = Replay_Draw_Items.Replace(
                "repeat (30)",
                "repeat (31)"
            );

            Replay_Draw_Items = Replay_Draw_Items.Replace(
                "draw_set_halign(fa_center)",
                """
                if (Replay_Items[(30 << 0)] > 0)
                    base_y2 += draw_h
                if (Replay_Items[(31 << 0)] > 0)
                    base_y2 += draw_h
                draw_set_halign(fa_center)
                """.ReplaceLineEndings("\n")
            );

            Replay_Draw_Items = Replay_Draw_Items.Replace(
                "\n        sprite = spr_Energy",
                """

                        sprite = spr_Energy
                        if (index == 30)
                            sprite = spr_ITEM_Tourian_Key
                        if (index == 31)
                            sprite = spr_ITEM_Big_Missile
                """.ReplaceLineEndings("\n")
            );

            Replay_Draw_Items = Replay_Draw_Items.Replace(
                """
                        if (index == (16 << 0) || index == (7 << 0))
                """.ReplaceLineEndings("\n"),
                """
                        if (index == (30 << 0))
                            text = string(Replay_Items[index])
                        if (index == (31 << 0))
                            text = string(Replay_Items[index])
                        if (index == (16 << 0) || index == (7 << 0) || index == (30 << 0) || index == (31 << 0))
                """.ReplaceLineEndings("\n")
            );

            Replay_Draw_Items = Replay_Draw_Items.Replace(
                "if (index != (16 << 0) && index != (7 << 0))",
                "if (index != (16 << 0) && index != (7 << 0) && index != (30 << 0) && index != (31 << 0))"
            );

            Replay_Draw_Items_code.ReplaceGML(Replay_Draw_Items, gmData);

            var Replay_Add_Event_code = gmData.Code.ByName("gml_Script_Replay_Add_Event");
            var Replay_Add_Event = Decompiler.Decompile(Replay_Add_Event_code, decompileContext);

            Replay_Add_Event = Replay_Add_Event.Replace(
                """
                    if (obj_NETWORK.Connection_Pos == -1 || player == obj_NETWORK.Connection_Pos || obj_NETWORK.Share_Items == 1)
                """,
                """
                    if (num == (27 << 0))
                        event = "Nothing"
                    if (num == (30 << 0))
                        event = "Tourian Key"
                    if (num == (31 << 0))
                        event = "Big Missile Tank"
                    if (obj_NETWORK.Connection_Pos == -1 || player == obj_NETWORK.Connection_Pos || obj_NETWORK.Share_Items == 1)
                """.ReplaceLineEndings("\n")
            );
            Replay_Add_Event_code.ReplaceGML(Replay_Add_Event, gmData);

            var Replay_Load_State_code = gmData.Code.ByName("gml_Script_Replay_Load_State");
            var Replay_Load_State = Decompiler.Decompile(Replay_Load_State_code, decompileContext);

            Replay_Load_State = Replay_Load_State.Replace(
                "repeat (30)",
                "repeat (31)"
            );

            Replay_Load_State_code.ReplaceGML(Replay_Load_State, gmData);

            var Replay_Save_State_code = gmData.Code.ByName("gml_Script_Replay_Save_State");
            var Replay_Save_State = Decompiler.Decompile(Replay_Save_State_code, decompileContext);

            Replay_Save_State = Replay_Save_State.Replace(
                "repeat (30)",
                "repeat (31)"
            );

            Replay_Save_State_code.ReplaceGML(Replay_Save_State, gmData);
        }
    }
}
