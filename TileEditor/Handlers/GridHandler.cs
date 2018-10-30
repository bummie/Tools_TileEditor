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
        public readonly int TILE_SIZE;

        private CameraHandler _cameraHandler;

        public Point SelectedTilePoint { get; set; }

        public GridHandler(int width, int height, int tileSize, CameraHandler cameraHandler)
        {
            MAP_SIZE_WIDTH = width;
            MAP_SIZE_HEIGHT = height;
            TILE_SIZE = tileSize;

            _cameraHandler = cameraHandler;
        }

        /// <summary>
        /// Returns a point for given tile based on coords
        /// </summary>
        /// <param name="coordX"></param>
        /// <param name="coordY"></param>
        /// <returns></returns>
        public Point GetPointFromCoords(Point coord)
        {
            float scaledTileSize = TILE_SIZE * _cameraHandler.Zoom;
            Point oldPoint = coord;

            coord.X -= _cameraHandler.Position.X;
            coord.Y -= _cameraHandler.Position.Y;

            if (oldPoint.X < _cameraHandler.Position.X || coord.X > (MAP_SIZE_WIDTH * scaledTileSize)) { return new Point(-1, -1); }
            if (oldPoint.Y < _cameraHandler.Position.Y || coord.Y > (MAP_SIZE_HEIGHT * scaledTileSize)) { return new Point(-1, -1); }

            int x = (int)(coord.X / scaledTileSize); // + (int)_cameraHandler.Position.X;
            int y = (int)(coord.Y / scaledTileSize); //+ (int)_cameraHandler.Position.Y;

            return new Point(x, y);    
        }
    }
}
