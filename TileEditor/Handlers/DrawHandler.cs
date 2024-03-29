﻿using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TileEditor.Loaders;

namespace TileEditor.Handlers
{
    public class DrawHandler : IDisposable
    {
        private readonly Canvas _canvas;
        private readonly GridHandler _gridHandler;
        private readonly CameraHandler _cameraHandler;
        private readonly TilesetLoader _tilesetHandler;
        private readonly TileHandler _tileHandler;
        private readonly ModeHandler _modeHandler;
        public int SelectedTileTextureId { get; set; }

        private WriteableBitmap _writeableBitmap;
        private Image _canvasRender = null;
        private System.Drawing.Bitmap _bitmapRender;

        private readonly System.Drawing.Pen _gridPen;
        private readonly System.Drawing.Pen _hoverPen;
        private readonly System.Drawing.Pen _selectedPen;
        private readonly System.Drawing.Font _fpsFont;

        private Stopwatch _stopWatch;
        private int _frames = 0;
        private float _fpsCounter = 0;
        
        public int GridThickness { get; set; }

        public DrawHandler(Canvas canvas, GridHandler gridHandler, CameraHandler cameraHandler, TilesetLoader tilesetHandler, TileHandler tileHandler, ModeHandler modeHandler)
        {
            _canvas = canvas;
            _gridHandler = gridHandler;
            _cameraHandler = cameraHandler;
            _tilesetHandler = tilesetHandler;
            _tileHandler = tileHandler;
            _modeHandler = modeHandler;

            _stopWatch = new Stopwatch();
            SelectedTileTextureId = 0;

            GridThickness = 1;

            _gridPen = new System.Drawing.Pen(System.Drawing.Color.WhiteSmoke, GridThickness);
            _hoverPen = new System.Drawing.Pen(System.Drawing.Color.LightGray, 3);
            _selectedPen = new System.Drawing.Pen(System.Drawing.Color.GhostWhite, 3);

            _fpsFont = new System.Drawing.Font("Monospace", 12);
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// Redraws all the elements to the canvas
        /// </summary>
        public void Update()
        {
            Clear();

            Draw();
           
            CalculateFPS();
        }

        private void Draw()
        {
            if(_bitmapRender == null) { return; }
            using (var graphics = System.Drawing.Graphics.FromImage(_bitmapRender))
            {
                graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                DrawTiles(graphics);
                DrawGrid(graphics);
                if (_modeHandler.CurrentMode == ModeHandler.MODE.DRAW) { DrawSelectedTileTexture(graphics); }
                DrawHoverSquare(graphics);
                if (_modeHandler.CurrentMode == ModeHandler.MODE.SELECT) { DrawSelectedSquare(graphics); } 
                DrawFPS(graphics);
            }

            UpdateCanvasImage();
        }

        /// <summary>
        /// Calculates FPS
        /// </summary>
        private void CalculateFPS()
        {
            if(!_stopWatch.IsRunning) { _stopWatch.Start(); }

            if(_stopWatch.ElapsedMilliseconds >= 3000)
            {
                _fpsCounter = ((((float)_frames*1000)/3) / (_stopWatch.ElapsedMilliseconds));
                _frames = 0;
                _stopWatch.Restart();
            }

            _frames++;
        }

        /// <summary>
        /// Updates the image in the canvas with the new bitmap
        /// </summary>
        /// <param name="bitmap"></param>
        private void CreateCanvasImage(WriteableBitmap bitmap)
        {
            if(_canvasRender == null) { _canvasRender = new Image(); }
            
            _canvasRender.Width = _canvas.ActualWidth;
            _canvasRender.Height = _canvas.ActualHeight;
            _canvasRender.Source = bitmap;

            if(_canvas.Children.Count > 0) { _canvas.Children[0] = _canvasRender; }
            else { _canvas.Children.Add(_canvasRender); }
            
            Canvas.SetLeft(_canvasRender, 0);
            Canvas.SetTop(_canvasRender, 0);
        }

        /// <summary>
        /// Updates the image on the canvas with the new bitmap
        /// </summary>
        private void UpdateCanvasImage()
        {
            if(_bitmapRender == null) { return; }

            var bitmapSource = BitmapToSource(_bitmapRender);

            int stride = bitmapSource.PixelWidth * (bitmapSource.Format.BitsPerPixel + 7) / 8;

            byte[] data = new byte[stride * bitmapSource.PixelHeight];

            bitmapSource.CopyPixels(data, stride, 0);

            if(_writeableBitmap == null)
            {
                _writeableBitmap = new WriteableBitmap
                (
                     bitmapSource.PixelWidth,
                     bitmapSource.PixelHeight,
                     bitmapSource.DpiX, bitmapSource.DpiY,
                     bitmapSource.Format, null
                 );
            }

            _writeableBitmap.WritePixels( new Int32Rect(0, 0, bitmapSource.PixelWidth, bitmapSource.PixelHeight),data, stride, 0);

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

            var bitmapSource = BitmapSource.Create(bitmapData.Width, bitmapData.Height, bitmap.HorizontalResolution, bitmap.VerticalResolution, PixelFormats.Bgr32, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);
            return bitmapSource;
        }

        /// <summary>
        /// Draws the first loaded tile
        /// </summary>
        private void DrawTiles(System.Drawing.Graphics graphics)
        {
            if (_tilesetHandler.TileBitmaps.Count <= 0) { return; }
            if(_tileHandler.TileDictionary.Count <= 0) { return; }

            foreach (var tile in _tileHandler.TileDictionary.Values)
            {
                if (!_gridHandler.IsTileInsideGrid(tile.Position)) { continue; }
                DrawTile(_gridHandler.GetCoordsFromPoint(tile.Position), (int)_gridHandler.TileSize, (System.Drawing.Rectangle)_tilesetHandler.TileBitmaps[tile.TextureId], graphics);
            }
        }

        /// <summary>
        /// Draws the first loaded tile
        /// </summary>
        private void DrawSelectedTileTexture(System.Drawing.Graphics graphics)
        {
            if (_tilesetHandler.TileBitmaps.Count <= 0) { return; }
            if(_gridHandler.HoverTile == new Point(-1, -1)) { return; }

            DrawTile(_gridHandler.GetCoordsFromPoint(_gridHandler.HoverTile), (int)_gridHandler.TileSize, (System.Drawing.Rectangle)_tilesetHandler.TileBitmaps[SelectedTileTextureId], graphics);
        }

        /// <summary>
        /// Draws the square around the highlighted square
        /// </summary>
        private void DrawHoverSquare(System.Drawing.Graphics graphics)
        {
            if(_gridHandler.HoverTile == new Point(-1, -1) || _bitmapRender == null) { return; };

            DrawHollowSquare(_gridHandler.GetCoordsFromPoint(_gridHandler.HoverTile), (int)_gridHandler.TileSize, _hoverPen, graphics);
        }

        /// <summary>
        /// Draws a string with the fps count
        /// </summary>
        private void DrawFPS(System.Drawing.Graphics graphics)
        {
            if (_bitmapRender == null) { return; }
            graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;

            DrawText(new Point(0, 0), 250, 80, _fpsFont, $"FPS: {_fpsCounter}", graphics);
        }

        /// <summary>
        /// Draws the square around the highlighted square
        /// </summary>
        private void DrawSelectedSquare(System.Drawing.Graphics graphics)
        {
            if (_gridHandler.SelectedTilePoint == new Point(-1, -1) || _bitmapRender == null) { return; };

            DrawHollowSquare(_gridHandler.GetCoordsFromPoint(_gridHandler.SelectedTilePoint), (int)_gridHandler.TileSize, _selectedPen, graphics);
        }

        /// <summary>
        /// Draws a grid based on the defined size
        /// </summary>
        public void DrawGrid(System.Drawing.Graphics graphics)
        {
            if (_bitmapRender == null) { return; }

            double width = _gridHandler.TileSize * _gridHandler.GridWidth;
            double height = _gridHandler.TileSize * _gridHandler.GridHeight;

            // Columns
            for (int i = 0; i <= _gridHandler.GridWidth; i++)
            {
                int x = (i * (int)_gridHandler.TileSize) + (int)_cameraHandler.Position.X;
                int y = (int)_cameraHandler.Position.Y;
                DrawLine(new Point(x, y), new Point(x, height+y), graphics);
            }

            // Rows
            for (int i = 0; i <= _gridHandler.GridHeight; i++)
            {
                int x = (int)_cameraHandler.Position.X;
                int y = (i * (int)_gridHandler.TileSize) + (int)_cameraHandler.Position.Y;
                DrawLine(new Point(x, y), new Point(width+x, y), graphics);
            }
        }

        /// <summary>
        /// Draws a line on top of the bitmap
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        private void DrawLine(Point start, Point end, System.Drawing.Graphics graphics)
        {
            graphics.DrawLine(_gridPen, (float)start.X, (float)start.Y, (float)end.X, (float)end.Y);
        }

        /// <summary>
        /// Draws a hollow square with size and position
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="pen"></param>
        private void DrawHollowSquare(Point position, int size, System.Drawing.Pen pen, System.Drawing.Graphics graphics)
        {
            graphics.DrawRectangle(pen, new System.Drawing.Rectangle((int)position.X, (int)position.Y, size, size));
        }

        /// <summary>
        /// Draws given bitmap tile to the canvasRenderBitmap
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="bitmap"></param>
        private void DrawTile(Point position, int size, System.Drawing.Rectangle bitmap, System.Drawing.Graphics graphics)
        {
            System.Drawing.Rectangle positionRectangle = new System.Drawing.Rectangle((int)position.X, (int)position.Y, size, size);
            graphics.DrawImage(_tilesetHandler.Tileset, positionRectangle, bitmap.X, bitmap.Y, bitmap.Width, bitmap.Height, System.Drawing.GraphicsUnit.Pixel);
        }

        /// <summary>
        /// Draws a string onto the bitmap
        /// </summary>
        /// <param name="position"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="font"></param>
        /// <param name="text"></param>
        private void DrawText(Point position, int width, int height, System.Drawing.Font font, string text, System.Drawing.Graphics graphics)
        {
            graphics.DrawString(text, font, System.Drawing.Brushes.Black, new System.Drawing.Rectangle((int)position.X, (int)position.Y, width, height));
        }

        /// <summary>
        /// Clears the canvas
        /// </summary>
        public void Clear()
        {
            CreateEmptyBitmap();
        }
        
        /// <summary>
        /// Creates the empty bitmap to render the content onto
        /// </summary>
        /// <returns></returns>
        private void CreateEmptyBitmap()
        { 
            int width = (int)_canvas.ActualWidth;
            int height = (int)_canvas.ActualHeight;

            if(width == 0) { return; }

            if(_bitmapRender == null) { _bitmapRender = new System.Drawing.Bitmap(width, height); }
            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(_bitmapRender))
            {
                System.Drawing.Rectangle ImageSize = new System.Drawing.Rectangle(0, 0, width, height);
                graphics.FillRectangle(System.Drawing.Brushes.DimGray, ImageSize);
            }
        }

        public void Dispose()
        {
            _bitmapRender.Dispose();
            _gridPen.Dispose();
            _hoverPen.Dispose();
            _selectedPen.Dispose();
            _fpsFont.Dispose();
        }
    }

}
