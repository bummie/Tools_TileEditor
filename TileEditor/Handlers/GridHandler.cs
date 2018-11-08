using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TileEditor.Handlers
{
    public class GridHandler
    {
        public int GridWidth { get; set; }
        public int GridHeight { get; set; }

        private float _tileSize;
        private readonly CameraHandler _cameraHandler;

        public float TileSize
        {
            get { return (float) Math.Ceiling(_tileSize * _cameraHandler.Zoom); }
            set { _tileSize = value; }
        }

        public Point SelectedTilePoint { get; set; }
        public Point HoverTile { get; set; }

        public GridHandler(CameraHandler cameraHandler)
        {
            GridWidth = 16;
            GridHeight = 16;
            TileSize = 16;

            _cameraHandler = cameraHandler;
        }

        public GridHandler(int width, int height, int tileSize, CameraHandler cameraHandler)
        {
            GridWidth = width;
            GridHeight = height;
            _cameraHandler = cameraHandler;

            TileSize = tileSize;
        }

        /// <summary>
        /// Returns a point for given tile based on coords
        /// </summary>
        /// <param name="coordX"></param>
        /// <param name="coordY"></param>
        /// <returns></returns>
        public Point GetPointFromCoords(Point coord)
        {
            Point oldPoint = coord;

            coord.X -= _cameraHandler.Position.X;
            coord.Y -= _cameraHandler.Position.Y;

            if (oldPoint.X < _cameraHandler.Position.X || coord.X > (GridWidth * TileSize)) { return new Point(-1, -1); }
            if (oldPoint.Y < _cameraHandler.Position.Y || coord.Y > (GridHeight * TileSize)) { return new Point(-1, -1); }

            int x = (int)(coord.X / TileSize);
            int y = (int)(coord.Y / TileSize);

            return new Point(x, y);    
        }

        /// <summary>
        /// Returns correct coord based on tile point
        /// </summary>
        /// <param name="tilePoint"></param>
        /// <returns></returns>
        public Point GetCoordsFromPoint(Point tilePoint)
        {
            return new Point((tilePoint.X * TileSize) + _cameraHandler.Position.X, (tilePoint.Y * TileSize) + _cameraHandler.Position.Y);
        }

        /// <summary>
        /// Returns whether a given tile is inside the bounds of the grid
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        public bool IsTileInsideGrid(Point tile)
        {
            if(tile.X < 0 || tile.X >= GridWidth) { return false; }
            if (tile.Y < 0 || tile.Y >= GridHeight) { return false; }

            return true;
        }
    }
}
