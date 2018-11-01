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

        public Tile() { }
    }
}
