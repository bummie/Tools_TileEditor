using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Windows;
using TileEditor.Handlers;
using TileEditor.Model;

namespace TileEditor.Loaders
{
    public class MapLoader
    {
        private readonly string MAPS_PATH = System.IO.Path.Combine(System.IO.Path.GetFullPath(@"..\..\"), @"Resources\Maps\");
        private readonly string MAPS_FILETYPE = ".tm";

        private readonly MapData _mapData;    

        public readonly TileHandler _tileHandler;
        public readonly GridHandler _gridHandler;
        public readonly TilesetLoader _tilesetLoader;

        public MapLoader(TileHandler tileHandler, GridHandler gridHandler, TilesetLoader tilesetLoader, MapData mapData)
        {
            _tileHandler = tileHandler;
            _gridHandler = gridHandler;
            _tilesetLoader = tilesetLoader;
            _mapData = mapData;
            Reset();
        }

        private void Reset()
        {
            _mapData.MapName = "Unnamed";
            _mapData.Date = "";
            _mapData.TileSet = "set.gif";
            _mapData.TileSize = 16;
            _mapData.GridWidth = 16;
            _mapData.GridHeight = 16;
        }

        /// <summary>
        /// Saves the map to the maps folder in resources
        /// </summary>
        public void SaveMap()
        {
            Task.Factory.StartNew(() =>
            {
                string path = CreateMapPath(_mapData.MapName);
                IOHandler.WriteToFile(path, CreateMapToJSON());
                MessageBox.Show("The map has been saved!");
            });
        }
        
        /// <summary>
        /// Creates a path to the given mapname
        /// </summary>
        /// <param name="mapName"></param>
        /// <returns></returns>
        private string CreateMapPath(string mapName)
        {
            return MAPS_PATH + _mapData.MapName.ToLower() + MAPS_FILETYPE;
        }

        /// <summary>
        /// Loads given map
        /// </summary>
        public void LoadMap()
        {
            string data = IOHandler.ReadFromFile(GetMapPath());

            if(data == null) { MessageBox.Show("Could not load map"); return; }

            Task.Factory.StartNew(() =>
            {
                _tileHandler.Reset();

                JSONToMap(JObject.Parse(data));
                ResetEditor();
            });
        }

        /// <summary>
        /// Resets the tilehandler, gridhandler, and tilesethandler
        /// </summary>
        private void ResetEditor()
        {
            _gridHandler.GridWidth = _mapData.GridWidth;
            _gridHandler.GridHeight = _mapData.GridHeight;
            _gridHandler.TileSize = _mapData.TileSize;

            _tilesetLoader.LoadTileset(_mapData.TileSet, _mapData.TileSize);
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
                { "Name", _mapData.MapName },
                { "Created", System.DateTime.Today.ToString() },
                { "Width", _mapData.GridWidth },
                { "Height", _mapData.GridHeight },
                { "Tileset", _mapData.TileSet },
                { "TileSize", _mapData.TileSize }
            };

            mapObject.Add("TileProperties", CreateTilePropertyArray());
            mapObject.Add("Tiles", CreateTileArray());

            return mapObject.ToString(Formatting.Indented);
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

        /// <summary>
        /// Turns a JSONObjectfile into the map?
        /// </summary>
        /// <param name="mapObject"></param>
        private void JSONToMap(JObject mapObject)
        {
            if( mapObject == null) { return; }

            _mapData.MapName = (string)mapObject["Name"];
            _mapData.Date = (string)mapObject["Created"];
            _mapData.GridWidth = (int)mapObject["Width"];
            _mapData.GridHeight = (int)mapObject["Width"];
            _mapData.TileSet = (string)mapObject["Tileset"];
            _mapData.TileSize = (int)mapObject["TileSize"];

            LoadTilePropertiesFromJSON((JArray)mapObject["TileProperties"]);
            LoadTilesFromJSON((JArray)mapObject["Tiles"]);
        }

        /// <summary>
        /// Loads tileproperties from a json array into the tilehandlers array
        /// </summary>
        /// <param name="mapProperties"></param>
        private void LoadTilePropertiesFromJSON(JArray mapProperties)
        {
            if(mapProperties == null) { return; }

            foreach(var jsonProperty in mapProperties)
            {
                TileProperty tileProperty = new TileProperty((int)jsonProperty["Id"]);
                tileProperty.SpeedMultiplier = (float)jsonProperty["SpeedMultiplier"];
                tileProperty.Damage = (float)jsonProperty["Damage"];
                tileProperty.DamageInterval = (float)jsonProperty["DamageInterval"];
                tileProperty.Walkable = (bool)jsonProperty["Walkable"];
                tileProperty.Water = (bool)jsonProperty["Water"];
                
                _tileHandler.AddTileProperty(tileProperty);
            }
        }

        /// <summary>
        /// Loads tiles from a json array into the tilehandlers array
        /// </summary>
        /// <param name="mapProperties"></param>
        private void LoadTilesFromJSON(JArray mapTiles)
        {
            if (mapTiles == null) { return; }

            foreach (var jsonProperty in mapTiles)
            {
                var jsonPosition = jsonProperty["Position"];
                Point position = new Point((int)jsonPosition["X"], (int)jsonPosition["Y"]);
                
                _tileHandler.AddTile(position, (int)jsonProperty["Id"]);
            }
        }

        /// <summary>
        /// Gives the path to a file selected by the user
        /// </summary>
        /// <returns></returns>
        private string GetMapPath()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "Select a TilEditor map";
            fileDialog.Filter = "TileEditor map files|*.tm";

            if (fileDialog.ShowDialog() == true)
            {
                return fileDialog.FileName;
            }

            return "";
        }
    }
}
