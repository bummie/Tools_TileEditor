using System;
using System.Collections.Generic;
using System.Windows;
using TileEditor.Model;

namespace TileEditor.Handlers
{
    public class TileHandler
    {
        private GridHandler _gridHandler;

        public Dictionary<int, TileProperty> TilePropertyDictionary { get; set; }
        public Dictionary<Point, Tile> TileDictionary { get; set; }
        public int SelectedTileTextureId { get; set; }

        public TileHandler(GridHandler gridHandler)
        {
            TilePropertyDictionary = new Dictionary<int, TileProperty>();
            TileDictionary = new Dictionary<Point, Tile>();

            SelectedTileTextureId = 0;

            _gridHandler = gridHandler;
        }

        /// <summary>
        /// Adds a new tile at the given position with given texture
        /// </summary>
        /// <param name="position"></param>
        /// <param name="textureId"></param>
        public void AddTile(Point position)
        {
            if(position == new Point(-1, -1)) { return; }


            if (!TilePropertyDictionary.ContainsKey(SelectedTileTextureId)) { CreateTileProperty(SelectedTileTextureId); }

            if (TileDictionary.ContainsKey(position))
            {
                TileDictionary[position].TextureId = SelectedTileTextureId;
                return;
            }

            TileDictionary[position] = new Tile(position, SelectedTileTextureId);
        }

        /// <summary>
        /// Removes tile at given position
        /// </summary>
        /// <param name="position"></param>
        public void RemoveTile(Point position)
        {
            if (position == new Point(-1, -1)) { return; }

            if (TileDictionary.ContainsKey(position))
            {
                TileDictionary.Remove(position);
                return;
            }
        }

        /// <summary>
        /// Add tile with textureid
        /// </summary>
        /// <param name="position"></param>
        /// <param name="textureId"></param>
        public void AddTile(Point position, int textureId)
        {
            if (position == new Point(-1, -1)) { return; }
            int tilePropertyId = (textureId == -1) ? SelectedTileTextureId : textureId;

            if (!TilePropertyDictionary.ContainsKey(tilePropertyId)) { CreateTileProperty(tilePropertyId); }

            if (TileDictionary.ContainsKey(position))
            {
                TileDictionary[position].TextureId = textureId;
                return;
            }

            TileDictionary[position] = new Tile(position, textureId);
        }

        /// <summary>
        /// Fills tiles with given textureid
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="targetTexture"></param>
        /// <param name="replacementTexture"></param>
        public void FillTiles(Point tile, int targetTexture)
        {
            if (!_gridHandler.IsTileInsideGrid(tile)) { return; }
            if (!TileDictionary.ContainsKey(tile)) { AddTile(tile, -1); }
            if (targetTexture == SelectedTileTextureId) { RemoveNegativeTileTextureId(tile); return; }
            if (TileDictionary[tile].TextureId != targetTexture) { RemoveNegativeTileTextureId(tile); return; }

            TileDictionary[tile].TextureId = SelectedTileTextureId;

            FillTiles(new Point(tile.X + 1, tile.Y), targetTexture);
            FillTiles(new Point(tile.X, tile.Y + 1), targetTexture);
            FillTiles(new Point(tile.X - 1, tile.Y), targetTexture);
            FillTiles(new Point(tile.X, tile.Y - 1), targetTexture);
        }

        private void RemoveNegativeTileTextureId(Point tile)
        {
            if (TileDictionary[tile].TextureId == -1)
            {
                TileDictionary.Remove(tile);
            }
        }

        /// <summary>
        /// Creates a new tileproperty
        /// </summary>
        /// <param name="textureId"></param>
        private void CreateTileProperty(int textureId)
        {
            if (TilePropertyDictionary.ContainsKey(textureId)) { return; }
            TilePropertyDictionary.Add(textureId, new TileProperty(textureId));            
        }

        /// <summary>
        /// Adds given tileproperty to the dictionary
        /// </summary>
        /// <param name="tileProperty"></param>
        public void AddTileProperty(TileProperty tileProperty)
        {
            if (TilePropertyDictionary.ContainsKey(tileProperty.TextureId)) { return; }
            TilePropertyDictionary.Add(tileProperty.TextureId, tileProperty);
        }

        /// <summary>
        /// Resets the stored tiles and properties
        /// </summary>
        public void Reset()
        {
            TilePropertyDictionary.Clear();
            TileDictionary.Clear();
        }

        /// <summary>
        /// Returns the propterty with given id
        /// </summary>
        /// <param name="textureId"></param>
        /// <returns></returns>
        public TileProperty GetTileProperty(int textureId)
        {
            if (!TilePropertyDictionary.ContainsKey(textureId)) { return null; }

            return TilePropertyDictionary[textureId];
        }

        /// <summary>
        /// Returns the tile at given point
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        public Tile GetTile(Point position)
        {
            if (!TileDictionary.ContainsKey(position)) { return null; }

            return TileDictionary[position];
        }
    }
}
