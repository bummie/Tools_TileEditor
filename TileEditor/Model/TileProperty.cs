using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEditor.Model
{
    public class TileProperty
    {
        #region Properties

        public int TextureId { get; set; }
        public float SpeedMultiplier { get; set; }
        public float Damage { get; set; }
        public float DamageInterval { get; set; }
        public bool Walkable { get; set; }
        public bool Water { get; set; }

        #endregion

        public TileProperty(int textureId)
        {
            Reset();
            TextureId = textureId;
        }

        /// <summary>
        /// Resets to default values
        /// </summary>
        public void Reset()
        {
            TextureId = 0;
            SpeedMultiplier = 1;
            Damage = 0;
            DamageInterval = 1;
            Walkable = true;
            Water = false;
        }
    }
}
