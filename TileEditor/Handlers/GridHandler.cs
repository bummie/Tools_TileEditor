using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEditor.Handlers
{
    public class GridHandler
    {
        public readonly int MAP_SIZE_WIDTH;
        public readonly int MAP_SIZE_HEIGHT;
        public readonly int TILE_SIZE;

        public GridHandler(int width, int height, int tileSize)
        {
            MAP_SIZE_WIDTH = width;
            MAP_SIZE_HEIGHT = height;
            TILE_SIZE = tileSize;
        }
    }
}
