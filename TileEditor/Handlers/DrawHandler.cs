using System;
using System.Diagnostics;
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

        private WriteableBitmap _writeableBitmap;
        private Image _canvasRender = null;
        private System.Drawing.Bitmap _bitmapRender;

        private System.Drawing.Pen _gridPen;
        private System.Drawing.Pen _hoverPen;
        private System.Drawing.Pen _selectedPen;
        private System.Drawing.Font _fpsFont;


        private Stopwatch _stopWatch;
        private int _frames = 0;
        private float _fpsCounter = 0;
        
        public int GridThickness { get; set; }

        public DrawHandler(Canvas canvas, GridHandler gridHandler, CameraHandler cameraHandler, TilesetHandler tilesetHandler)
        {
            _canvas = canvas;
            _gridHandler = gridHandler;
            _cameraHandler = cameraHandler;
            _tilesetHandler = tilesetHandler;
            _stopWatch = new Stopwatch();

            GridThickness = 1;

            _gridPen = new System.Drawing.Pen(System.Drawing.Color.NavajoWhite, GridThickness);
            _hoverPen = new System.Drawing.Pen(System.Drawing.Color.LightGray, 3);
            _selectedPen = new System.Drawing.Pen(System.Drawing.Color.GhostWhite, 3);

            _fpsFont = new System.Drawing.Font("Monospace", 12);
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
            DrawFPS();

            UpdateCanvasImage();
            CalculateFPS();
        }

        /// <summary>
        /// Calculates FPS
        /// </summary>
        private void CalculateFPS()
        {
            if(!_stopWatch.IsRunning) { _stopWatch.Start(); }

            if(_stopWatch.ElapsedMilliseconds >= 500)
            {
                _fpsCounter = ((float)_frames*1000 / (_stopWatch.ElapsedMilliseconds));
                //Console.WriteLine("FPS: " + _fpsCounter);
                _frames = 0;
                _stopWatch.Restart();
            }

            _frames++;
        }

        private void CreateCanvasImage(WriteableBitmap bitmap)
        {
            if(_canvasRender == null) { _canvasRender = new Image(); }
            
            _canvasRender.Width = _canvas.ActualWidth;
            _canvasRender.Height = _canvas.ActualHeight;
            _canvasRender.Source = bitmap;
            _canvas.Children.Add(_canvasRender);

            Canvas.SetLeft(_canvasRender, 0);
            Canvas.SetTop(_canvasRender, 0);
        }

        /// <summary>
        /// Updates the image on the canvas with the new bitmap
        /// </summary>
        private void UpdateCanvasImage()
        {
            if(_bitmapRender == null) { return; }

            var source = BitmapToSource(_bitmapRender); //System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            _writeableBitmap = new WriteableBitmap(source);
            CreateCanvasImage(_writeableBitmap);
        }

        /// <summary>
        /// Creates a bitmapsource from a bitmap
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public BitmapSource BitmapToSource(System.Drawing.Bitmap bitmap)
        {
            var bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var bitmapSource = BitmapSource.Create(
                bitmapData.Width, bitmapData.Height,
                bitmap.HorizontalResolution, bitmap.VerticalResolution,
                PixelFormats.Bgr32, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);
            return bitmapSource;
        }

        /// <summary>
        /// Draws the first loaded tile
        /// </summary>
        private void DrawFirstTiles()
        {
            if (_tilesetHandler.TileBitmaps.Count <= 0 || _bitmapRender == null) { return; }

            for(int i = 0; i < 32; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    DrawTile(_gridHandler.GetCoordsFromPoint(new Point(i, j)), (int)_gridHandler.TileSize, (System.Drawing.Bitmap)_tilesetHandler.TileBitmaps[i+j]);
                }
            }
        }

        /// <summary>
        /// Draws the square around the highlighted square
        /// </summary>
        private void DrawHoverSquare()
        {
            if(_gridHandler.HoverTile == new Point(-1, -1) || _bitmapRender == null) { return; };

            DrawHollowSquare(_gridHandler.GetCoordsFromPoint(_gridHandler.HoverTile), (int)_gridHandler.TileSize, _hoverPen);
        }

        /// <summary>
        /// Draws a string with the fps count
        /// </summary>
        private void DrawFPS()
        {
            if (_bitmapRender == null) { return; }

            DrawText(new Point(0, 0), 250, 80, _fpsFont, $"FPS: {_fpsCounter}");
        }

        /// <summary>
        /// Draws the square around the highlighted square
        /// </summary>
        private void DrawSelectedSquare()
        {
            if (_gridHandler.SelectedTilePoint == new Point(-1, -1) || _bitmapRender == null) { return; };

            DrawHollowSquare(_gridHandler.GetCoordsFromPoint(_gridHandler.SelectedTilePoint), (int)_gridHandler.TileSize, _selectedPen);
        }

        /// <summary>
        /// Draws a grid based on the defined size
        /// </summary>
        public void DrawGrid()
        {
            if (_bitmapRender == null) { return; }

            double width = _gridHandler.TileSize * _gridHandler.MAP_SIZE_WIDTH;
            double height = _gridHandler.TileSize * _gridHandler.MAP_SIZE_HEIGHT;

            // Columns
            for (int i = 0; i <= _gridHandler.MAP_SIZE_WIDTH; i++)
            {
                int x = (i * (int)_gridHandler.TileSize) + (int)_cameraHandler.Position.X;
                int y = (int)_cameraHandler.Position.Y;
                DrawLine(new Point(x, y), new Point(x, height+y));
            }

            // Rows
            for (int i = 0; i <= _gridHandler.MAP_SIZE_HEIGHT; i++)
            {
                int x = (int)_cameraHandler.Position.X;
                int y = (i * (int)_gridHandler.TileSize) + (int)_cameraHandler.Position.Y;
                DrawLine(new Point(x, y), new Point(width+x, y));
            }
        }

        /// <summary>
        /// Draws a line on top of the bitmap
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void DrawLine(Point start, Point end)
        {
            using (var graphics = System.Drawing.Graphics.FromImage(_bitmapRender))
            {
                graphics.DrawLine(_gridPen, (float)start.X, (float)start.Y, (float)end.X, (float)end.Y);
            }
        }

        /// <summary>
        /// Draws a hollow square with size and position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="pen"></param>
        private void DrawHollowSquare(Point position, int size, System.Drawing.Pen pen)
        {
            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(_bitmapRender))
            {
                graphics.DrawRectangle(pen, new System.Drawing.Rectangle((int)position.X, (int)position.Y, size, size));
            }
        }

        /// <summary>
        /// Draws given bitmap tile to the canvasRenderBitmap
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="bitmap"></param>
        private void DrawTile(Point position, int size, System.Drawing.Bitmap bitmap)
        {
            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(_bitmapRender))
            {
                graphics.DrawImage(bitmap, new System.Drawing.Rectangle((int)position.X, (int)position.Y, size, size));
            }
        }

        /// <summary>
        /// Draws a string onto the bitmap
        /// </summary>
        /// <param name="position"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="font"></param>
        /// <param name="text"></param>
        private void DrawText(Point position, int width, int height, System.Drawing.Font font, string text)
        {
            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(_bitmapRender))
            {
                graphics.DrawString(text, font, System.Drawing.Brushes.Black, new System.Drawing.Rectangle((int)position.X, (int)position.Y, width, height));
            }
        }

        /// <summary>
        /// Clears the canvas
        /// </summary>
        public void Clear()
        {
            _bitmapRender = CreateEmptyBitmap();
            //ImagePool.ReleaseAll();
            _canvas.Children.Clear();
        }
        
        /// <summary>
        /// Creates the empty bitmap to render the content onto
        /// </summary>
        /// <returns></returns>
        private System.Drawing.Bitmap CreateEmptyBitmap()
        { 
            int width = (int)_canvas.ActualWidth;
            int height = (int)_canvas.ActualHeight;

            if(width == 0) { return null; }

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(width, height);
            using (System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(bmp))
            {
                System.Drawing.Rectangle ImageSize = new System.Drawing.Rectangle(0, 0, width, height);
                graph.FillRectangle(System.Drawing.Brushes.RoyalBlue, ImageSize);
            }
            return bmp;
        }

    }

}
