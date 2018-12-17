using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEditor.Handlers
{
    public class ModeHandler
    {
        public enum MODE { SELECT, DRAW, FILL, ERASE };
        public MODE CurrentMode { get; set; }

        public ModeHandler()
        {
            CurrentMode = MODE.SELECT;
        }
    }
}
