using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using TileEditor.ViewModel;
using TileEditor.Handlers;

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

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();

            _cameraHandler = new CameraHandler();
            _gridHandler = new GridHandler(5, 10, 64);
            _drawHandler = new DrawHandler(DrawCanvas, _gridHandler, _cameraHandler);

            KeyDown += new KeyEventHandler(OnButtonKeyDown);
            KeyUp += new KeyEventHandler(OnButtonKeyRelease);
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                // Movement
                case Key.W:
                    _cameraHandler.Y++;
                    break;
                case Key.S:
                    _cameraHandler.Y--;
                    break;
                case Key.A:
                    _cameraHandler.X++;
                    break;
                case Key.D:
                    _cameraHandler.X--;
                    break;

                // Zoom
                case Key.Space:
                    _cameraHandler.Zoom += CameraHandler.ZOOM_LEVEL;
                    break;
                case Key.LeftCtrl:
                    _cameraHandler.Zoom -= CameraHandler.ZOOM_LEVEL;
                    break;
            }

            _drawHandler.Update();
        }

        private void OnButtonKeyRelease(object sender, KeyEventArgs e)
        {

        }
    }
}