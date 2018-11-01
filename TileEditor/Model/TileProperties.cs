using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEditor.Model
{
    public class TileProperties
    {
        #region Properties

        public int TextureId { get; set; }
        public float SpeedMultiplier { get; set; }
        public Point Position { get; set; }
        public float Damage { get; set; }
        public float DamageInterval { get; set; }
        public bool Walkable { get; set; }
        public bool Water { get; set; }

        #endregion

        public TileProperties()
        {
            Reset();
        }

        /// <summary>
        /// Resets to default values
        /// </summary>
        public void Reset()
        {
            TextureId = 0;
            SpeedMultiplier = 1;
            Position = new Point(0, 0);
            Damage = 0;
            DamageInterval = 1;
            Walkable = true;
            Water = false;
        }
    }
}
