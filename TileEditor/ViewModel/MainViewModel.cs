using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
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
        public RelayCommand CmdButtonFill { get; set; }
        public RelayCommand CmdButtonSave { get; set; }
        public RelayCommand CmdButtonLoad { get; set; }
        public RelayCommand CmdButtonErase { get; set; }
        public RelayCommand CmdButtonClear { get; set; }
        public RelayCommand CmdButtonUpdateEditor { get; set; }
        public RelayCommand CmdButtonUpdateTileProperty { get; set; }

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

                if(_selectedTileTextureItem == null) { return; }
                _drawHandler.SelectedTileTextureId = _selectedTileTextureItem.TextureId;
                _tileHandler.SelectedTileTextureId = _selectedTileTextureItem.TextureId;
            }
        }

        #region Models
        public Information Information { get; set; }
        public MapData MapData { get; set; }
        public TileProperty TileProperty { get; set; }
        #endregion

        #region Handlers
        private DrawHandler _drawHandler;
        private GridHandler _gridHandler;
        private CameraHandler _cameraHandler;
        private TilesetLoader _tilesetLoader;
        private TileHandler _tileHandler;
        private MapLoader _mapLoader;
        private ModeHandler _modeHandler;
        private InputHandler _inputHandler;

        public InputHandler InputHandler
        {
            get { return _inputHandler; }
            set { _inputHandler = value; RaisePropertyChanged("Inputhandler"); }
        }

        #endregion

        public MainViewModel()
        {
            CompositionTarget.Rendering += Update;
            InitCommands();
            InitModels();
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
        /// Initialized the models
        /// </summary>
        private void InitModels()
        {
            Information = new Information();
            MapData = new MapData();
            TileProperty = new TileProperty(0);
        }

        /// <summary>
        /// Initializes the commands
        /// </summary>
        private void InitCommands()
        {
            CmdKeyDown = new RelayCommand<EventArgs>((e) => { if (InputHandler != null) { InputHandler.KeyDown(e); }});
            CmdKeyUp = new RelayCommand<EventArgs>((e) => { if (InputHandler != null) { InputHandler.KeyUp(e); }});

            CmdMouseDown = new RelayCommand<EventArgs>((e) => { if (InputHandler != null) {  InputHandler.MouseDown(e); }});
            CmdMouseUp = new RelayCommand<EventArgs>((e) => { if (InputHandler != null) {  InputHandler.MouseUp(e); }});
            CmdMouseMove = new RelayCommand<EventArgs>((e) => { if (InputHandler != null) { InputHandler.MouseMove(e); }});

            CmdButtonSelect = new RelayCommand(() => { if (_modeHandler != null) { _modeHandler.CurrentMode = ModeHandler.MODE.SELECT; } });
            CmdButtonDraw = new RelayCommand(() => { if (_modeHandler != null) { _modeHandler.CurrentMode = ModeHandler.MODE.DRAW; } });
            CmdButtonFill = new RelayCommand(() => { if (_modeHandler != null) { _modeHandler.CurrentMode = ModeHandler.MODE.FILL; } });
            CmdButtonErase = new RelayCommand(() => { if (_modeHandler != null) { _modeHandler.CurrentMode = ModeHandler.MODE.ERASE; } });
            CmdButtonSave = new RelayCommand(() => { if (_mapLoader != null) { _mapLoader.SaveMap();} });
            CmdButtonLoad = new RelayCommand(() => { if (_mapLoader != null) { _mapLoader.LoadMap(); UpdateEditor(); } });
            CmdButtonClear = new RelayCommand(() => { if (_tileHandler != null) { _tileHandler.Clear(); } });

            CmdButtonUpdateEditor = new RelayCommand(() => { UpdateEditor(); });
            CmdButtonUpdateTileProperty = new RelayCommand(() => { UpdateTileProperty(); });
        }

        /// <summary>
        /// Updates the data from the UI to the selected tile
        /// </summary>
        private void UpdateTileProperty()
        {
            Tile selectedTile = _tileHandler.GetTile(_gridHandler.SelectedTilePoint);
            if (selectedTile == null) { return; }

            TileProperty tileProp = _tileHandler.GetTileProperty(selectedTile.TextureId);
            if (tileProp == null) { return; }

            tileProp.CopyData(TileProperty);
        }

        /// <summary>
        /// Initializes the handles with given values from map loaded
        /// </summary>
        private void InitHandlers()
        {
            _modeHandler = new ModeHandler();
            _cameraHandler = new CameraHandler();
            _gridHandler = new GridHandler(_cameraHandler);
            _tileHandler = new TileHandler(_gridHandler);
            _tilesetLoader = new TilesetLoader();
            _mapLoader = new MapLoader(_tileHandler, _gridHandler, _tilesetLoader, MapData);
            _drawHandler = new DrawHandler(DrawCanvas, _gridHandler, _cameraHandler, _tilesetLoader, _tileHandler, _modeHandler);
            InputHandler = new InputHandler(_gridHandler, _cameraHandler, _tileHandler, _modeHandler, DrawCanvas, Information, TileProperty);
        }

        /// <summary>
        /// Updates the editor with new values
        /// </summary>
        private void UpdateEditor()
        {
            _mapLoader.ResetEditor();
            SelectableTileTextures.Clear();
            FillSelectableTileTextures();
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

        /// <summary>
        /// Retrieves the source of the given bitmap
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
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