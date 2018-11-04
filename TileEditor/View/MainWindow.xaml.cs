using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using TileEditor.ViewModel;
using TileEditor.Handlers;
using TileEditor.Loaders;
using System;

namespace TileEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DrawHandler _drawHandler;
        private GridHandler _gridHandler;
        private CameraHandler _cameraHandler;
        private TilesetLoader _tilesetHander;
        private TileHandler _tileHandler;
        private MapLoader _mapLoader;

        private bool _mouseDown = false;
        private int selectedTileId = 0;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();

            _cameraHandler = new CameraHandler();
            _mapLoader = new MapLoader();
            ReloadEditor();

            KeyDown += new KeyEventHandler(OnButtonKeyDown);
            KeyUp += new KeyEventHandler(OnButtonKeyRelease);
            CompositionTarget.Rendering += Update;
        }

        /// <summary>
        /// Initializes the handles with given values from map loaded
        /// </summary>
        private void ReloadEditor()
        {
            _gridHandler = new GridHandler(_mapLoader.GridWidth, _mapLoader.GridWidth, _mapLoader.TileSize, _cameraHandler);
            _tilesetHander = new TilesetLoader(_mapLoader.Tileset, _mapLoader.TileSize);
            _tileHandler = new TileHandler();
            _mapLoader.TileHandler = _tileHandler;
            _drawHandler = new DrawHandler(DrawCanvas, _gridHandler, _cameraHandler, _tilesetHander, _tileHandler);
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            _cameraHandler.UpdateMovement(e.Key);

            switch (e.Key)
            {
                case Key.Q:
                        if(selectedTileId > 0) { selectedTileId--; }
                        _drawHandler.SelectedTileTextureId = selectedTileId;
                    break;
                case Key.E:
                        if (selectedTileId < _tilesetHander.TileBitmaps.Count - 1) { selectedTileId++; }
                        _drawHandler.SelectedTileTextureId = selectedTileId;
                    break;
            }
        }

        /// <summary>
        /// Redraw the content in the canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Update(object sender, EventArgs e)
        {
            _drawHandler.Update();
        }


        /// <summary>
        /// The event fires when the canvas is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnButtonKeyRelease(object sender, KeyEventArgs e)
        {

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