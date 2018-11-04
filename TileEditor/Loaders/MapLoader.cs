using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
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

        public readonly TileHandler _tileHandler;
        public readonly GridHandler _gridHandler;
        public readonly TilesetLoader _tilesetLoader;

        public MapLoader(TileHandler tileHandler, GridHandler gridHandler, TilesetLoader tilesetLoader)
        {
            _tileHandler = tileHandler;
            _gridHandler = gridHandler;
            _tilesetLoader = tilesetLoader;

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
        /// Saves the map to the maps folder in resources
        /// </summary>
        public void SaveMap()
        {
            Task.Factory.StartNew(() =>
            {
                string path = MAPS_PATH + MapName + MAPS_FILETYPE;
                IOHandler.WriteToFile(path, CreateMapToJSON());
            });
        }
        
        /// <summary>
        /// Loads given map
        /// </summary>
        public void LoadMap(string mapName)
        {

            ResetEditor();
        }

        /// <summary>
        /// Resets the tilehandler, gridhandler, and tilesethandler
        /// </summary>
        private void ResetEditor()
        {
            _tileHandler.Reset();

            _gridHandler.GridWidth = GridWidth;
            _gridHandler.GridHeight = GridHeight;
            _gridHandler.TileSize = TileSize;

            _tilesetLoader.LoadTileset(Tileset, TileSize);
        }

        /// <summary>
        /// Creates a jsonobject containing the map data
        /// TODO:: Cleaner more dynamic solution
        /// </summary>
        private string CreateMapToJSON()
        {
            if(_tileHandler == null) { return null; }

            JObject mapObject = new JObject
            {
                { "Name", MapName },
                { "Created", System.DateTime.Today.ToString() },
                { "Width", GridWidth },
                { "Height", GridHeight },
                { "Tileset", Tileset },
                { "TileSize", TileSize }
            };

            mapObject.Add("TileProperties", CreateTilePropertyArray());
            mapObject.Add("Tiles", CreateTileArray());

            return mapObject.ToString(Formatting.None);
        }

        /// <summary>
        /// Creates an JArray filled with JSONObjects based on the tileproperty dara
        /// </summary>
        /// <returns></returns>
        private JArray CreateTilePropertyArray()
        {
            JArray tilePropertiesArray = new JArray();
            foreach (var tileProperty in _tileHandler.TilePropertyDictionary.Values)
            {
                JObject tilePropertyObject = new JObject()
                {
                    {"Id", tileProperty.TextureId },
                    {"SpeedMultiplier", tileProperty.SpeedMultiplier},
                    {"Damage", tileProperty.Damage },
                    {"DamageInterval", tileProperty.DamageInterval },
                    {"Walkable", tileProperty.Walkable },
                    {"Water", tileProperty.Water }
                };
                tilePropertiesArray.Add(tilePropertyObject);
            }

            return tilePropertiesArray;
        }

        /// <summary>
        /// Creats an JSON array from the data in TileHandler
        /// </summary>
        /// <returns>Json array</returns>
        private JArray CreateTileArray()
        {
            JArray tilesArray = new JArray();
            foreach (var tile in _tileHandler.TileDictionary.Values)
            {
                JObject tileObject = new JObject()
                {
                    {"Id", tile.TextureId },
                    {"Position", new JObject()
                        {
                            {"X", tile.Position.X},
                            {"Y", tile.Position.Y}
                        }
                    }
                };
                tilesArray.Add(tileObject);
            }

            return tilesArray;
        }
    }
}
