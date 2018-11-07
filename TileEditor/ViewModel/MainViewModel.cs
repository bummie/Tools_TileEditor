using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TileEditor.Handlers;
using TileEditor.Loaders;

namespace TileEditor.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        public RelayCommand<EventArgs> CmdKeyDown { get; set; }
        public RelayCommand<EventArgs> CmdKeyUp { get; set; }


        private Canvas _canvas = null;
        public Canvas DrawCanvas { get; set; }

        private DrawHandler _drawHandler;
        private GridHandler _gridHandler;
        private CameraHandler _cameraHandler;
        private TilesetLoader _tilesetLoader;
        private TileHandler _tileHandler;
        private MapLoader _mapLoader;

        private bool _mouseDown = false;
        private int selectedTileId = 0;

        public MainViewModel()
        {
            CompositionTarget.Rendering += Update;

            InitCommands();

            Messenger.Default.Register<Canvas>(this, (canvas) => { DrawCanvas = canvas; InitHandlers(); });
        }

        private void InitCommands()
        {
            CmdKeyDown = new RelayCommand<EventArgs>(KeyDown);
            CmdKeyUp = new RelayCommand<EventArgs>(KeyUp);

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
        private new void MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDown)
            {
                _tileHandler.AddTile(_gridHandler.GetPointFromCoords(e.GetPosition(DrawCanvas)), selectedTileId);
            }

            _gridHandler.HoverTile = _gridHandler.GetPointFromCoords(e.GetPosition(DrawCanvas));
        }

        private new void MouseDown(object sender, MouseButtonEventArgs e)
        {
            _mouseDown = true;
        }

        private new void MouseUp(object sender, MouseButtonEventArgs e)
        {
            _gridHandler.SelectedTilePoint = _gridHandler.GetPointFromCoords(e.GetPosition(DrawCanvas));
            _mouseDown = false;
        }
    }
}