using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TileEditor.Model
{
    public class Tile
    {
        public int TextureId { get; set; }
        public Point Position { get; set; }

        public Tile(Point position, int textureId)
        {
            Position = position;
            TextureId = textureId;
        }

        private void Reset()
        {
            TextureId = 0;
            Position = new Point(0, 0);
        }
    }
}
