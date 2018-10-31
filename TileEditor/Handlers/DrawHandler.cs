using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TileEditor.Handlers
{
    public class DrawHandler
    {
        private Canvas _canvas;
        private GridHandler _gridHandler;
        private CameraHandler _cameraHandler;
        private TilesetHandler _tilesetHandler;

        public int GridThickness { get; set; }

        public DrawHandler(Canvas canvas, GridHandler gridHandler, CameraHandler cameraHandler, TilesetHandler tilesetHandler)
        {
            _canvas = canvas;
            _gridHandler = gridHandler;
            _cameraHandler = cameraHandler;
            _tilesetHandler = tilesetHandler;

            GridThickness = 1;
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// Redraws all the elements to the canvas
        /// </summary>
        public void Update()
        {
            Clear();
            DrawGrid();

            DrawFirstTiles();

            DrawHoverSquare();
            DrawSelectedSquare();
        }

        /// <summary>
        /// Draws the first loaded tile
        /// </summary>
        private void DrawFirstTiles()
        {
            if (_tilesetHandler.TileBitmaps.Count <= 0) { return; }

            for(int i = 0; i < 3; i++)
            {
                CreateBitmap(_gridHandler.GetCoordsFromPoint(new Point(i, 0)), _gridHandler.TileSize, (System.Drawing.Bitmap)_tilesetHandler.TileBitmaps[i]);
            }
        }

        /// <summary>
        /// Draws the square around the highlighted square
        /// </summary>
        private void DrawHoverSquare()
        {
            if(_gridHandler.HoverTile == new Point(-1, -1)) { return; };

            CreateSquare(_gridHandler.GetCoordsFromPoint(_gridHandler.HoverTile), _gridHandler.TileSize, Colors.DarkGray);
        }


        /// <summary>
        /// Draws the square around the highlighted square
        /// </summary>
        private void DrawSelectedSquare()
        {
            if (_gridHandler.SelectedTilePoint == new Point(-1, -1)) { return; };

            CreateSquare(_gridHandler.GetCoordsFromPoint(_gridHandler.SelectedTilePoint), _gridHandler.TileSize, Colors.White);
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
            line.Stroke = Brushes.NavajoWhite;
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
        private void CreateSquare(Point position, float size, Color color)
        {
            Rectangle rect = new Rectangle();
            rect.Stroke = new SolidColorBrush(color);
            rect.StrokeThickness = 3;
            rect.Width = size;
            rect.Height = size;
            Canvas.SetLeft(rect, position.X);
            Canvas.SetTop(rect, position.Y);

            _canvas.Children.Add(rect);
        }

        /// <summary>
        /// Adds the given bitmap to the canvas
        /// </summary>
        /// <param name="position"></param>
        /// <param name="bitmap"></param>
        private void CreateBitmap(Point position, float size, System.Drawing.Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();

            try
            {
                var source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                Image ImageIcon = ImagePool.GetObject();
                ImageIcon.Source = new WriteableBitmap(source);
                ImageIcon.Width = size;
                ImageIcon.Height = size;
                _canvas.Children.Add(ImageIcon);

                Canvas.SetLeft(ImageIcon, position.X);
                Canvas.SetTop(ImageIcon, position.Y);
            }
            finally
            {
                DeleteObject(hBitmap);
            }
        }


        /// <summary>
        /// Clears the canvas
        /// </summary>
        public void Clear()
        {
            ImagePool.ReleaseAll();
            _canvas.Children.Clear();
        }

    }

}
