using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TileEditor.Handlers
{
    public class CameraHandler
    {
        private readonly float MIN_ZOOM = 0.5f;
        private readonly float MAX_ZOOM = 2.0f;
        public static readonly float ZOOM_LEVEL = 0.1f;
        public readonly float MOVE_AMOUNT = 3f;

        #region Properties
        public Point Position { get; set; }

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
            Position = new Point(100, 100);
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

        /// <summary>
        /// Updates the position and zoom of the camera based on keyboard input
        /// </summary>
        /// <param name="key"></param>
        public void UpdateMovement(Key key)
        {
            switch (key)
            {
                // Movement
                case Key.W:
                    Position = new Point(Position.X, Position.Y + MOVE_AMOUNT);
                    break;
                case Key.S:
                    Position = new Point(Position.X, Position.Y - MOVE_AMOUNT);
                    break;
                case Key.A:
                    Position = new Point(Position.X + MOVE_AMOUNT, Position.Y);
                    break;
                case Key.D:
                    Position = new Point(Position.X - MOVE_AMOUNT, Position.Y);
                    break;

                // Zoom
                case Key.Q:
                    Zoom += ZOOM_LEVEL;
                    break;
                case Key.E:
                    Zoom -= ZOOM_LEVEL;
                    break;
            }
        }
    }
}
