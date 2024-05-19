using UndertaleModLib;
using UndertaleModLib.Decompiler;

namespace YAPR_LIB.Patches
{
    public class DisplaySeedHash
    {
        public static void Apply(UndertaleData gmData, GlobalDecompileContext decompileContext, RandomizerConfig randomizerConfig)
        {
            var menu_headers_code = gmData.Code.ByName("gml_Script_scr_TS_Menu_Header");
            var menu_headers = Decompiler.Decompile(menu_headers_code, decompileContext);

            // tells which seed we're playing
            if (randomizerConfig.LevelData.Room == "rm_Zebeth")
                menu_headers = menu_headers.Replace("\"PLANET ZEBETH\"", $"\"PLANET ZEBETH - {randomizerConfig.GameConfig.SeedIdentifier.WordHash.ToUpper()}\"");
            if (randomizerConfig.LevelData.Room == "rm_Novus")
                menu_headers = menu_headers.Replace("\"PLANET NOVUS\"", $"\"PLANET NOVUS - ({randomizerConfig.GameConfig.SeedIdentifier.WordHash.ToUpper()})\"");

            menu_headers_code.ReplaceGML(menu_headers, gmData);
        }
    }
}