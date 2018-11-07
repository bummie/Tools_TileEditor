using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TileEditor.Handlers;
using TileEditor.Loaders;
using TileEditor.Model;

namespace TileEditor.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        #region Commands
        public RelayCommand<EventArgs> CmdKeyDown { get; set; }
        public RelayCommand<EventArgs> CmdKeyUp { get; set; }
        public RelayCommand<EventArgs> CmdMouseDown { get; set; }
        public RelayCommand<EventArgs> CmdMouseUp { get; set; }
        public RelayCommand<EventArgs> CmdMouseMove{ get; set; }
        #endregion

        public Canvas DrawCanvas { get; set; }
        public ObservableCollection<TileTextureItem> SelectableTileTextures { get; set; }

        #region Handlers
        private DrawHandler _drawHandler;
        private GridHandler _gridHandler;
        private CameraHandler _cameraHandler;
        private TilesetLoader _tilesetLoader;
        private TileHandler _tileHandler;
        private MapLoader _mapLoader;
        #endregion

        private bool _mouseDown = false;
        private int selectedTileId = 0;

        public MainViewModel()
        {
            CompositionTarget.Rendering += Update;

            InitCommands();

            SelectableTileTextures = new ObservableCollection<TileTextureItem>();

            Messenger.Default.Register<Canvas>(this, (canvas) => { DrawCanvas = canvas; InitHandlers(); });
        }

        /// <summary>
        /// Initializes the commands
        /// </summary>
        private void InitCommands()
        {
            CmdKeyDown = new RelayCommand<EventArgs>(KeyDown);
            CmdKeyUp = new RelayCommand<EventArgs>(KeyUp);

            CmdMouseDown = new RelayCommand<EventArgs>(MouseDown);
            CmdMouseUp = new RelayCommand<EventArgs>(MouseUp);
            CmdMouseMove = new RelayCommand<EventArgs>(MouseMove);
        }

        /// <summary>
        /// Initializes the handles with given values from map loaded
        /// </summary>
        private void InitHandlers()
        {
            _cameraHandler = new CameraHandler();
            _tileHandler = new TileHandler();

            _gridHandler = new GridHandler(_cameraHandler);
            _tilesetLoader = new TilesetLoader();

            _mapLoader = new MapLoader(_tileHandler, _gridHandler, _tilesetLoader);
            _drawHandler = new DrawHandler(DrawCanvas, _gridHandler, _cameraHandler, _tilesetLoader, _tileHandler);

            FillSelectableTileTextures();
        }

        /// <summary>
        /// Fills the selectable tile textures with textures from tileset
        /// </summary>
        private void FillSelectableTileTextures()
        {
            if(_tilesetLoader.Tileset ==  null) { return; }

            var source = ImageSourceForBitmap(_tilesetLoader.Tileset);

            for (int i = 0; i < _tilesetLoader.TileBitmaps.Count; i++)
            {
                Int32Rect rect = new Int32Rect
                {
                    X = ((Rectangle)_tilesetLoader.TileBitmaps[i]).X,
                    Y = ((Rectangle)_tilesetLoader.TileBitmaps[i]).Y,
                    Width = ((Rectangle)_tilesetLoader.TileBitmaps[i]).Width,
                    Height = ((Rectangle)_tilesetLoader.TileBitmaps[i]).Height
                };

                SelectableTileTextures.Add(new TileTextureItem(i, rect, source));
            }
        }

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public BitmapSource ImageSourceForBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }

        /// <summary>
        /// Key pressed down
        /// </summary>
        /// <param name="e"></param>
        private void KeyDown(EventArgs e)
        {
            var pressedKey = (e != null) ? (KeyEventArgs)e : null;

            _cameraHandler.UpdateMovement(pressedKey.Key);

            switch (pressedKey.Key)
            {
                case Key.Q:
                    if (selectedTileId > 0) { selectedTileId--; }
                    _drawHandler.SelectedTileTextureId = selectedTileId;
                    break;
                case Key.E:
                    if (selectedTileId < _tilesetLoader.TileBitmaps.Count - 1) { selectedTileId++; }
                    _drawHandler.SelectedTileTextureId = selectedTileId;
                    break;

                case Key.P:
                    _mapLoader.SaveMap();
                    break;

                case Key.O:
                    _mapLoader.LoadMap("Unnamed");
                    break;
            }
        }

        /// <summary>
        /// Key release
        /// </summary>
        /// <param name="e"></param>
        private void KeyUp(EventArgs e)
        {
            var pressedKey = (e != null) ? (KeyEventArgs)e : null;
        }

        /// <summary>
        /// Redraw the content in the canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Update(object sender, EventArgs e)
        {
            if(DrawCanvas == null) { return; }
            _drawHandler.Update();
        }
        
        /// <summary>
        /// The event fires when the mouse moves over the canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseMove(EventArgs e)
        {
            var mouseEvent = (e != null) ? (MouseEventArgs)e : null;
            if (mouseEvent == null) { return; }

            if (_mouseDown)
            {
                _tileHandler.AddTile(_gridHandler.GetPointFromCoords(mouseEvent.GetPosition(DrawCanvas)), selectedTileId);
            }

            _gridHandler.HoverTile = _gridHandler.GetPointFromCoords(mouseEvent.GetPosition(DrawCanvas));
        }

        /// <summary>
        /// Hits when left mousebutton is down
        /// </summary>
        /// <param name="e"></param>
        private void MouseDown(EventArgs e)
        {
            _mouseDown = true;
        }

        /// <summary>
        /// Hits when left mouse button is up
        /// </summary>
        /// <param name="e"></param>
        private void MouseUp(EventArgs e)
        {
            var mouseEvent = (e != null) ? (MouseEventArgs)e : null;

            if(mouseEvent == null) { return; }

            _gridHandler.SelectedTilePoint = _gridHandler.GetPointFromCoords(mouseEvent.GetPosition(DrawCanvas));
            _mouseDown = false;
        }
    }
}