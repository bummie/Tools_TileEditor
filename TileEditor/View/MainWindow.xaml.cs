﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using TileEditor.ViewModel;
using TileEditor.Handlers;
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
        private TilesetHandler _tilesetHander;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();

            _cameraHandler = new CameraHandler();
            _gridHandler = new GridHandler(32, 32, 16, _cameraHandler);
            _tilesetHander = new TilesetHandler("set.gif", 16);
            _drawHandler = new DrawHandler(DrawCanvas, _gridHandler, _cameraHandler, _tilesetHander);

            KeyDown += new KeyEventHandler(OnButtonKeyDown);
            KeyUp += new KeyEventHandler(OnButtonKeyRelease);
            CompositionTarget.Rendering += Draw;
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            _cameraHandler.UpdateMovement(e.Key);
        }

        /// <summary>
        /// Redraw the content in the canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Draw(object sender, EventArgs e)
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
            _gridHandler.HoverTile = _gridHandler.GetPointFromCoords(e.GetPosition(DrawCanvas));
        }

        private new void MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private new void MouseUp(object sender, MouseButtonEventArgs e)
        {
            _gridHandler.SelectedTilePoint = _gridHandler.GetPointFromCoords(e.GetPosition(DrawCanvas));
            //MessageBox.Show(e.GetPosition(DrawCanvas).ToString() + "\nTile: " + _gridHandler.GetPointFromCoords(e.GetPosition(DrawCanvas)) + "\nCamera: " + _cameraHandler.Position.ToString() + "\n TileSize: " + _gridHandler.TileSize + "\n Camera Zoom: " + _cameraHandler.Zoom);
        }
    }
}