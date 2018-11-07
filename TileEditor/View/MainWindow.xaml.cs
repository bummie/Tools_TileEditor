using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using TileEditor.ViewModel;
using TileEditor.Handlers;
using TileEditor.Loaders;
using System;
using GalaSoft.MvvmLight.Messaging;

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

            Messenger.Default.Send<Canvas>(DrawCanvas);
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
           
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

        }

        private new void MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private new void MouseUp(object sender, MouseButtonEventArgs e)
        {
         
        }

    }
}