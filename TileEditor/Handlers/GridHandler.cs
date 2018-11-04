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
        public readonly int MAP_SIZE_WIDTH;
        public readonly int MAP_SIZE_HEIGHT;

        private float _tileSize;
        private readonly CameraHandler _cameraHandler;

        public float TileSize
        {
            get { return (float) Math.Ceiling(_tileSize * _cameraHandler.Zoom); }
            set { _tileSize = value; }
        }

        public Point SelectedTilePoint { get; set; }
        public Point HoverTile { get; set; }

        public GridHandler(int width, int height, int tileSize, CameraHandler cameraHandler)
        {
            MAP_SIZE_WIDTH = width;
            MAP_SIZE_HEIGHT = height;
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

            if (oldPoint.X < _cameraHandler.Position.X || coord.X > (MAP_SIZE_WIDTH * TileSize)) { return new Point(-1, -1); }
            if (oldPoint.Y < _cameraHandler.Position.Y || coord.Y > (MAP_SIZE_HEIGHT * TileSize)) { return new Point(-1, -1); }

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
    }
}
