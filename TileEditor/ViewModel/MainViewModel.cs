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
        public RelayCommand<EventArgs> CmdMouseMove { get; set; }

        public RelayCommand CmdButtonSelect { get; set; }
        public RelayCommand CmdButtonDraw { get; set; }
        public RelayCommand CmdButtonSave { get; set; }
        public RelayCommand CmdButtonLoad { get; set; }
        public RelayCommand CmdButtonHost { get; set; }
        public RelayCommand CmdButtonJoin { get; set; }
        public RelayCommand CmdButtonClear { get; set; }
        
        #endregion

        public Canvas DrawCanvas { get; set; }
        public ObservableCollection<TileTextureItem> SelectableTileTextures { get; set; }

        private TileTextureItem _selectedTileTextureItem = null;
        public TileTextureItem SelectedTileTexture
        {
            get { return _selectedTileTextureItem; }
            set
            {
                _selectedTileTextureItem = value;
                _drawHandler.SelectedTileTextureId = _selectedTileTextureItem.TextureId;
            }
        }

        #region Handlers
        private DrawHandler _drawHandler;
        private GridHandler _gridHandler;
        private CameraHandler _cameraHandler;
        private TilesetLoader _tilesetLoader;
        private TileHandler _tileHandler;
        private MapLoader _mapLoader;
        private ModeHandler _modeHandler;
        private InputHandler _inputHandler;
        #endregion

        public MainViewModel()
        {
            CompositionTarget.Rendering += Update;
            InitCommands();
            SelectableTileTextures = new ObservableCollection<TileTextureItem>();
            Messenger.Default.Register<Canvas>(this, (canvas) => { InitEditor(canvas); });
        }

        /// <summary>
        /// Init the editor when we have received the canvas context from the view
        /// </summary>
        /// <param name="canvas"></param>
        private void InitEditor(Canvas canvas)
        {
            DrawCanvas = canvas;
            InitHandlers();
            FillSelectableTileTextures();
        }

        /// <summary>
        /// Redraw the content in the canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Update(object sender, EventArgs e)
        {
            if (DrawCanvas == null) { return; }
            _drawHandler.Update();
        }

        /// <summary>
        /// Initializes the commands
        /// </summary>
        private void InitCommands()
        {
            CmdKeyDown = new RelayCommand<EventArgs>((e) => { if (_inputHandler != null) {  _inputHandler.KeyDown(e); }});
            CmdKeyUp = new RelayCommand<EventArgs>((e) => { if (_inputHandler != null) {  _inputHandler.KeyUp(e); }});

            CmdMouseDown = new RelayCommand<EventArgs>((e) => { if (_inputHandler != null) {  _inputHandler.MouseDown(e); }});
            CmdMouseUp = new RelayCommand<EventArgs>((e) => { if (_inputHandler != null) {  _inputHandler.MouseUp(e); }});
            CmdMouseMove = new RelayCommand<EventArgs>((e) => { if (_inputHandler != null) { _inputHandler.MouseMove(e); }});

            CmdButtonSelect = new RelayCommand(() => { if (_modeHandler != null) { _modeHandler.CurrentMode = ModeHandler.MODE.SELECT; } });
            CmdButtonDraw = new RelayCommand(() => { if (_modeHandler != null) { _modeHandler.CurrentMode = ModeHandler.MODE.DRAW; } });

            CmdButtonClear = new RelayCommand(() => { if (_tileHandler != null) { _tileHandler.Reset(); } });
        }

        /// <summary>
        /// Initializes the handles with given values from map loaded
        /// </summary>
        private void InitHandlers()
        {
            _modeHandler = new ModeHandler();
            _cameraHandler = new CameraHandler();
            _tileHandler = new TileHandler();
            _gridHandler = new GridHandler(_cameraHandler);
            _tilesetLoader = new TilesetLoader();
            _mapLoader = new MapLoader(_tileHandler, _gridHandler, _tilesetLoader);
            _drawHandler = new DrawHandler(DrawCanvas, _gridHandler, _cameraHandler, _tilesetLoader, _tileHandler, _modeHandler);
            _inputHandler = new InputHandler(_gridHandler, _cameraHandler, _tileHandler, _modeHandler, DrawCanvas);
        }

        /// <summary>
        /// Fills the selectable tile textures with textures from tileset
        /// </summary>
        private void FillSelectableTileTextures()
        {
            if (_tilesetLoader.Tileset == null) { return; }

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
    }
}