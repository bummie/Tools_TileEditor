using System;
using System.Collections;
using System.Drawing;

namespace TileEditor.Loaders
{
    public class TilesetLoader
    {
        private int _tileSize;
        public Bitmap Tileset { get; set; }
        public ArrayList TileBitmaps { get; set; }

        /// <summary>
        /// Constructor for tilesethander
        /// Loads the given tileset into memory
        /// //TODO:: Thread the loading operation
        /// </summary>
        /// <param name="tilesetName"></param>
        /// <param name="tileSize"></param>
        public TilesetLoader(string tilesetName, int tileSize)
        {
            TileBitmaps = new ArrayList();
            _tileSize = tileSize;

            LoadTileset(tilesetName);
            LoadAllTiles();
        }

        /// <summary>
        /// Loads all tiles into the array
        /// </summary>
        private void LoadAllTiles()
        {
            if (Tileset == null) { return; }

            int horizontalTiles = Tileset.Size.Width / _tileSize;
            int verticalTiles = Tileset.Size.Height / _tileSize;

            Rectangle area = new Rectangle();
            area.Height = _tileSize;
            area.Width = _tileSize;
            for (int x = 0; x < horizontalTiles; x++)
            {
                for (int y = 0; y < verticalTiles; y++)
                {
                    area.X = x * area.Width;
                    area.Y = y * area.Height;
                    AddTileBitmap(area);
                }
            }

            //_tileSet.Dispose();
            Console.WriteLine($"{horizontalTiles*verticalTiles} tiles were loaded!");
        }

        /// <summary>
        /// Loads the tileset into memory
        /// </summary>
        /// <param name="tilesetName"></param>
        private void LoadTileset(string tilesetName)
        {
            string resourcesPath = System.IO.Path.Combine(System.IO.Path.GetFullPath(@"..\..\"), @"Resources\Tilesets\" + tilesetName);

            try
            {
                Tileset = new Bitmap(resourcesPath);
            }
            catch (System.IO.FileNotFoundException exception)
            {
                Tileset = null;
                Console.WriteLine(exception.Message + ": " + resourcesPath);
            }
            catch (ArgumentException exception)
            {
                Tileset = null;
                Console.WriteLine(exception.Message + ": " + resourcesPath);
            }
        }

        /// <summary>
        /// Receives a rectangle and ass it to the Tile Arraylist
        /// </summary>
        /// <param name="area"></param>
        private void AddTileBitmap(Rectangle area )
        {
            if(Tileset == null) { return; }

            TileBitmaps.Add(area);
            //TileBitmaps.Add(_tileSet.Clone(area, _tileSet.PixelFormat));
        }
    }
}
