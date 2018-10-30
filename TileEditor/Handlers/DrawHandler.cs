using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TileEditor.Handlers
{
    public class DrawHandler
    {
        private Canvas _canvas;
        private GridHandler _gridHandler;
        private CameraHandler _cameraHandler;

        public int GridThickness { get; set; }

        public DrawHandler(Canvas canvas, GridHandler gridHandler, CameraHandler cameraHandler)
        {
            _canvas = canvas;
            _gridHandler = gridHandler;
            _cameraHandler = cameraHandler;

            GridThickness = 1;
        }

        /// <summary>
        /// Redraws all the elements to the canvas
        /// </summary>
        public void Update()
        {
            Clear();
            DrawGrid();
            DrawHoverSquare();
        }

        /// <summary>
        /// Draws the square around the highlighted square
        /// </summary>
        private void DrawHoverSquare()
        {
            if(_gridHandler.HoverTile == new Point(-1, -1)) { return; };

            CreateSquare(_gridHandler.TileSize, _gridHandler.GetCoordsFromPoint(_gridHandler.HoverTile));
        }

        /// <summary>
        /// Draws a grid based on the defined size
        /// </summary>
        public void DrawGrid()
        {
            double width = _gridHandler.TileSize * _gridHandler.MAP_SIZE_WIDTH;
            double height = _gridHandler.TileSize * _gridHandler.MAP_SIZE_HEIGHT;

            // Columns
            for (int i = 0; i <= _gridHandler.MAP_SIZE_WIDTH; i++)
            {
                int x = i * (int)_gridHandler.TileSize;
                CreateLine(x, 0, x, height);
            }

            // Rows
            for (int i = 0; i <= _gridHandler.MAP_SIZE_HEIGHT; i++)
            {
                int y = i * (int)_gridHandler.TileSize;
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

            line.X1 = (x1 + _cameraHandler.Position.X);
            line.Y1 = (y1 + _cameraHandler.Position.Y);

            line.X2 = (x2 + _cameraHandler.Position.X);
            line.Y2 = (y2 + _cameraHandler.Position.Y);

            _canvas.Children.Add(line);
        }

        /// <summary>
        /// Creates a hollow square
        /// </summary>
        /// <param name="size"></param>
        private void CreateSquare(float size, Point position)
        {
            Rectangle rect = new Rectangle();
            rect.Stroke = new SolidColorBrush(Colors.IndianRed);
            rect.StrokeThickness = 3;
            rect.Width = size;
            rect.Height = size;
            Canvas.SetLeft(rect, position.X);
            Canvas.SetTop(rect, position.Y);

            _canvas.Children.Add(rect);
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
