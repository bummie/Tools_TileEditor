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

        public List<Point> BorderControlled = new List<Point>();

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
                StartAddBorders(position, TilePropertyDictionary[SelectedTileTextureId].GroupId);
                return;
            }

            TileDictionary[position] = new Tile(position, SelectedTileTextureId);
            StartAddBorders(position, TilePropertyDictionary[SelectedTileTextureId].GroupId);
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
                StartAddBorders(position, TilePropertyDictionary[tilePropertyId].GroupId);
                return;
            }

            TileDictionary[position] = new Tile(position, textureId);
            StartAddBorders(position, TilePropertyDictionary[tilePropertyId].GroupId);
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
            Clear();
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

        /// <summary>
        /// Clears the tiles
        /// </summary>
        public void Clear()
        {
            TileDictionary.Clear();
        }

        /// <summary>
        /// Starts the the recursion proccess of adding borders, and then cleans up afterwards.
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="groupId"></param>
        private void StartAddBorders(Point tile, int groupId)
        {
            AddBorders(tile, groupId);
            BorderControlled.Clear();
        }

        /// <summary>
        /// Add borders to neighbouring tiles if in the same group
        /// </summary>
        /// <param name="tile"></param>
        private void AddBorders(Point tile, int groupId)
        {
            if (BorderControlled.Contains(tile)) { return; }
            if (!_gridHandler.IsTileInsideGrid(tile)) { return; }
            if (!TileDictionary.ContainsKey(tile)) { return; }
            if (!TilePropertyDictionary.ContainsKey(GetTile(tile).TextureId)) { return; }
            if (GetTileProperty(GetTile(tile).TextureId).GroupId != groupId) { return; }
            if (GetTileProperty(GetTile(tile).TextureId).GroupId == -1 || groupId == -1) { return; }

            Point TileTop = new Point(tile.X, tile.Y - 1);
            Point TileBottom = new Point(tile.X, tile.Y + 1);
            Point TileLeft = new Point(tile.X - 1, tile.Y);
            Point TileRight = new Point(tile.X + 1, tile.Y);

            int groupPosition = CalculateGroupPosition(
                GetTileGroup(TileTop) == groupId ? true : false,
                GetTileGroup(TileBottom) == groupId ? true : false,
                GetTileGroup(TileLeft) == groupId ? true : false,
                GetTileGroup(TileRight) == groupId ? true : false
                );

            if (groupPosition == -1) { return; }
            int newTextureId = FindTilePropertyWithGroupPosition(groupId, groupPosition);

            if (newTextureId == -1) { return; }
            TileDictionary[tile].TextureId = newTextureId;

            BorderControlled.Add(tile);

            // Recursion ?!
            AddBorders(TileTop, groupId);
            AddBorders(TileBottom, groupId);
            AddBorders(TileLeft, groupId);
            AddBorders(TileRight, groupId);
        }

        /// <summary>
        /// Discovers the texturetid for given tile with correct groupu and position
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="groupPosition"></param>
        /// <returns></returns>
        private int FindTilePropertyWithGroupPosition(int groupId, int groupPosition)
        {
            foreach(TileProperty tileProperty in TilePropertyDictionary.Values)
            {
                if(tileProperty.GroupId == groupId && tileProperty.GroupPosition == groupPosition)
                {
                    return tileProperty.TextureId;
                }
            }

            return -1;
        }

        /// <summary>
        /// Calculates the position in the group based on if the neighbours are in the same group
        /// </summary>
        /// <param name="top"></param>
        /// <param name="bottom"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private int CalculateGroupPosition(bool top, bool bottom, bool left, bool right)
        {
            // Top left
            if (!top && bottom && !left && right) { return 1; }

            // Top
            if (!top && bottom && !left && !right) { return 2; }
            if (!top && bottom && left && right) { return 2; }

            // Top Right
            if (!top && bottom && left && !right) { return 3; }

            // Center Left
            if (top && bottom && !left && right) { return 4; }
            if (!top && !bottom && !left && right) { return 4; }

            // Center
            if (top && bottom && left && right) { return 5; }

            // Center Right
            if (top && bottom && left && !right) { return 6; }
            if (!top && !bottom && left && !right) { return 6; }

            // Bottom Left
            if (top && !bottom && !left && right) { return 7; }

            // Bottom
            if (top && !bottom && !left && !right) { return 8; }
            if (top && !bottom && left && right) { return 8; }

            // Bottom Right
            if (top && !bottom && left && !right) { return 9; }

            return -1;
        }

        /// <summary>
        /// Returns the group of the tile
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        private int GetTileGroup(Point tile)
        {
            if (GetTile(tile) == null) { return -1; }
            if (GetTileProperty(GetTile(tile).TextureId) == null) { return -1; }

            return GetTileProperty(GetTile(tile).TextureId).GroupId;
        }
    }
}
