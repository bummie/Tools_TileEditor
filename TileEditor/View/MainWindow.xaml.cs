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

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();

            _gridHandler = new GridHandler(5, 10, 64);
            _drawHandler = new DrawHandler(DrawCanvas, _gridHandler);

            KeyDown += new KeyEventHandler(OnButtonKeyDown);
            KeyUp += new KeyEventHandler(OnButtonKeyRelease);
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space)
            {
                _drawHandler.Update();
            }
        }

        private void OnButtonKeyRelease(object sender, KeyEventArgs e)
        {

        }
    }
}