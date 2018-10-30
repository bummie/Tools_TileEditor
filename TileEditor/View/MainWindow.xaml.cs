using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TileEditor.ViewModel;

namespace TileEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();

            Ellipse circle = new Ellipse()
            {
                Width = 10,
                Height = 10,
                Stroke = Brushes.Red,
                StrokeThickness = 6
            };

            circle.SetValue(Canvas.LeftProperty, 10.0);
            DrawCanvas.Children.Add(circle);
            //DrawCanvas.Children.Clear();
        }
    }
}