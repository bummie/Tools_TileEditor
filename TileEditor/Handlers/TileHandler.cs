using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using TileEditor.Model;

namespace TileEditor.Handlers
{
    public class TileHandler
    {
        // Tileprop array
        // Tiles array
        public Dictionary<int, TileProperty> TilePropertyDictionary { get; set; }
        public Dictionary<Point, Tile> TileDictionary { get; set; }

        public TileHandler()
        {
            TilePropertyDictionary = new Dictionary<int, TileProperty>();
            TileDictionary = new Dictionary<Point, Tile>();
        }

        /// <summary>
        /// Adds a new tile at the given position with given texture
        /// </summary>
        /// <param name="position"></param>
        /// <param name="textureId"></param>
        public void AddTile(Point position, int textureId)
        {
            if(position == new Point(-1, -1)) { return; }

            if (!TilePropertyDictionary.ContainsKey(textureId)) { CreateTileProperty(textureId); }

            if (TileDictionary.ContainsKey(position))
            {
                TileDictionary[position].TextureId = textureId;
                return;
            }

            TileDictionary[position] = new Tile(position, textureId);
        }
        

        /// <summary>
        /// Creates a new tileproperty
        /// </summary>
        /// <param name="textureId"></param>
        private void CreateTileProperty(int textureId)
        {
            TilePropertyDictionary.Add(textureId, new TileProperty(textureId));            
        }

        /* EXPORT */

        /* IMPORT */
    }
}
