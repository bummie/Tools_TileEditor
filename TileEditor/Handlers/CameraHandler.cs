using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEditor.Handlers
{
    public class CameraHandler
    {
        private readonly float MIN_ZOOM = 0.1f;
        private readonly float MAX_ZOOM = 10f;
        public static readonly float ZOOM_LEVEL = 0.1f;

        #region Properties
        public int X { get; set; }
        public int Y { get; set; }

        private float _zoom;
        
        public float Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = Clamp(value, MIN_ZOOM, MAX_ZOOM);
            }
        }


        #endregion

        public CameraHandler()
        {
            X = 0;
            Y = 0;
            Zoom = 1;
        }

        /// <summary>
        /// Clamps the value based on min and max value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private float Clamp(float value, float min, float max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }
    }
}
