using System;
using System.Collections;
using System.Drawing;

namespace TileEditor.Handlers
{
    public class TilesetHandler
    {
        private int _tileSize;
        private Bitmap _tileSet = null;
        public ArrayList TileBitmaps { get; set; }


        public TilesetHandler(string tilesetName, int tileSize)
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
            if (_tileSet == null) { return; }

            int horizontalTiles = _tileSet.Size.Width / _tileSize;
            int verticalTiles = _tileSet.Size.Height / _tileSize;

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

            _tileSet.Dispose();
            Console.WriteLine($"{horizontalTiles*verticalTiles} tiles were loaded!");
        }

        /// <summary>
        /// Loads the tileset into memory
        /// </summary>
        /// <param name="tilesetName"></param>
        private void LoadTileset(string tilesetName)
        {
            string resourcesPath = System.IO.Path.Combine(System.IO.Path.GetFullPath(@"..\..\"), @"Resources\Tilesets\" + tilesetName);
            Console.WriteLine(resourcesPath);

            try
            {
                _tileSet = new Bitmap(resourcesPath);
            }
            catch (System.IO.FileNotFoundException exception)
            {
                _tileSet = null;
                Console.WriteLine(exception.Message + ": " + resourcesPath);
            }
            catch (System.ArgumentException exception)
            {
                _tileSet = null;
                Console.WriteLine(exception.Message + ": " + resourcesPath);
            }
        }

        /// <summary>
        /// Clones given area and adds it to the list of bitmaptiles
        /// </summary>
        /// <param name="area"></param>
        private void AddTileBitmap(RectangleF area )
        {
            if(_tileSet == null) { return; }

            //Console.WriteLine("Adding bitmap from area: " + area.ToString());
            TileBitmaps.Add(_tileSet.Clone(area, _tileSet.PixelFormat));
        }
    }
}
