using Newtonsoft.Json.Linq;
using TileEditor.Handlers;

namespace TileEditor.Loaders
{
    public class MapLoader
    {
        private readonly string MAPS_PATH = System.IO.Path.Combine(System.IO.Path.GetFullPath(@"..\..\"), @"Resources\Maps\");
        private readonly string MAPS_FILETYPE = ".json";

        public string MapName { get; set; }
        public string Date { get; set; }
        public string Tileset { get; set; }
        public int TileSize { get; set; }
        public int GridWidth { get; set; }
        public int GridHeight { get; set; }

        public TileHandler TileHandler;

        public MapLoader()
        {
            
            Reset();
        }

        private void Reset()
        {
            MapName = "Unnamed";
            Date = "";
            Tileset = "set.gif";
            TileSize = 16;
            GridWidth = 16;
            GridHeight = 16;
        }

        /// <summary>
        /// Creates a jsonobject containing the map data
        /// </summary>
        private void CreateMapToJSON()
        {
            JObject MapObject = new JObject
            {
                { "Name", MapName },
                { "Created", System.DateTime.Today.ToString() },
                { "Width", GridWidth },
                { "Height", GridHeight },
                { "Tileset", Tileset },
                { "TileSize", TileSize },
                { "TileProperties", new JArray() },
                { "Tiles", new JArray() }
            };


        }



    }
}
