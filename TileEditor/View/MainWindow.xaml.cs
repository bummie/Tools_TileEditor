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
        private int i = 0;
        private DrawHandler _drawHandler;
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();

            _drawHandler = new DrawHandler(DrawCanvas);

            KeyDown += new KeyEventHandler(OnButtonKeyDown);
            KeyUp += new KeyEventHandler(OnButtonKeyRelease);
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            _drawHandler.Update();
            Ellipse circle = new Ellipse()
            {
                Width = 10,
                Height = 10,
                Stroke = Brushes.Red,
                StrokeThickness = 6
            };

            i++;
            circle.SetValue(Canvas.LeftProperty, (double)i);
            DrawCanvas.Children.Add(circle);
        }

        private void OnButtonKeyRelease(object sender, KeyEventArgs e)
        {

        }
    }
}