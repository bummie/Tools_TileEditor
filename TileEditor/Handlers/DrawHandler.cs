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
        private Canvas _canvas;
        private GridHandler _gridHandler;

        public int GridThickness { get; set; }

        public DrawHandler(Canvas canvas, GridHandler gridHandler)
        {
            _canvas = canvas;
            _gridHandler = gridHandler;

            GridThickness = 1;
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
            double width = _gridHandler.TILE_SIZE * _gridHandler.MAP_SIZE_WIDTH;
            double height = _gridHandler.TILE_SIZE * _gridHandler.MAP_SIZE_HEIGHT;

            for (int i = 0; i <= _gridHandler.MAP_SIZE_WIDTH; i++)
            {
                int x = i * _gridHandler.TILE_SIZE;
                CreateLine(x, 0, x, height);
            }

            for (int i = 0; i <= _gridHandler.MAP_SIZE_HEIGHT; i++)
            {
                int y = i * _gridHandler.TILE_SIZE;
                CreateLine(0, y, width, y);
            }
        }

        /// <summary>
        /// Creates a line an adds it to the canvas
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        private void CreateLine(double x1, double y1, double x2, double y2)
        {
            Line line = new Line();
            line.Stroke = Brushes.Black;
            line.StrokeThickness = GridThickness;

            line.X1 = x1;
            line.Y1 = y1;

            line.X2 = x2;
            line.Y2 = y2;

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
