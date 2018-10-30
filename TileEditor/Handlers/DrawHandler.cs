using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TileEditor.Handlers
{
    public class DrawHandler
    {
        private const int MAP_SIZE_WIDTH = 48;
        private const int MAP_SIZE_HEIGHT = 64;
        private const int TILE_SIZE = 64;

        private Canvas _canvas;

        public DrawHandler(Canvas canvas)
        {
            _canvas = canvas;
        }

        /// <summary>
        /// Redraws all the elements to the canvas
        /// </summary>
        public void Update()
        {
            Clear();
            DrawGrid();
        }

        /// <summary>
        /// Draws a grid based on the defined size
        /// </summary>
        public void DrawGrid()
        {
            Line line = new Line();
            line.Stroke = Brushes.LightSteelBlue;

            line.X1 = 1;
            line.X2 = 50;
            line.Y1 = 1;
            line.Y2 = 50;

            line.StrokeThickness = 2;
            _canvas.Children.Add(line);
        }

        /// <summary>
        /// Clears the canvas
        /// </summary>
        public void Clear()
        {
            _canvas.Children.Clear();
        }

    }

}
