using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using TileEditor.Model;

namespace TileEditor.Handlers
{
    public class InputHandler
    {
        #region Handlers

        private readonly GridHandler _gridHandler;
        private readonly CameraHandler _cameraHandler;
        private readonly TileHandler _tileHandler;
        private readonly ModeHandler _modeHandler;

        #endregion

        private readonly Information _information;
        private readonly Canvas _canvas;
        private bool _mouseDown = false;

        public InputHandler(GridHandler gridHandler, CameraHandler cameraHandler, TileHandler tileHandler, ModeHandler modeHandler, Canvas canvas, Information information)
        {
            _gridHandler = gridHandler;
            _cameraHandler = cameraHandler;
            _tileHandler = tileHandler;
            _modeHandler = modeHandler;
            _canvas = canvas;
            _information = information;
        }

        /// <summary>
        /// Key pressed down
        /// </summary>
        /// <param name="e"></param>
        public void KeyDown(EventArgs e)
        {
            var pressedKey = (e != null) ? (KeyEventArgs)e : null;

            _cameraHandler.UpdateMovement(pressedKey.Key);
            _information.InfoCameraPosition = _cameraHandler.Position.ToString() + " Zoom: " + _cameraHandler.Zoom;
        }

        /// <summary>
        /// Key release
        /// </summary>
        /// <param name="e"></param>
        public void KeyUp(EventArgs e)
        {
            var pressedKey = (e != null) ? (KeyEventArgs)e : null;
        }

        /// <summary>
        /// The event fires when the mouse moves over the canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MouseMove(EventArgs e)
        {
            var mouseEvent = (e != null) ? (MouseEventArgs)e : null;
            if (mouseEvent == null) { return; }

            AddTile(mouseEvent);
            _gridHandler.HoverTile = _gridHandler.GetPointFromCoords(mouseEvent.GetPosition(_canvas));

            _information.InfoMousePos = mouseEvent.GetPosition(_canvas).ToString();
            _information.InfoTilePos = _gridHandler.GetPointFromCoords(mouseEvent.GetPosition(_canvas)).ToString();
        }

        /// <summary>
        /// Hits when left mousebutton is down
        /// </summary>
        /// <param name="e"></param>
        public void MouseDown(EventArgs e)
        {
            var mouseEvent = (e != null) ? (MouseEventArgs)e : null;
            if (mouseEvent == null) { return; }

            _mouseDown = true;
            AddTile(mouseEvent);
            Fill(mouseEvent);
        }

        /// <summary>
        /// Adds a tile to the grid
        /// </summary>
        public void AddTile(MouseEventArgs mouseEvent)
        {
            if (_mouseDown && _modeHandler.CurrentMode == ModeHandler.MODE.DRAW)
            {
                _tileHandler.AddTile(_gridHandler.GetPointFromCoords(mouseEvent.GetPosition(_canvas)));
            }
        }

        /// <summary>
        /// Fills the tiles
        /// </summary>
        private void Fill(MouseEventArgs mouseEvent)
        {
            if (_mouseDown && _modeHandler.CurrentMode == ModeHandler.MODE.FILL)
            {
                Point targetPoint = _gridHandler.GetPointFromCoords(mouseEvent.GetPosition(_canvas));
                int targetTexture = (!_tileHandler.TileDictionary.ContainsKey(targetPoint)) ? -1 : _tileHandler.TileDictionary[targetPoint].TextureId;
                _tileHandler.FillTiles(_gridHandler.GetPointFromCoords(mouseEvent.GetPosition(_canvas)), targetTexture);
            }
        }

        /// <summary>
        /// Hits when left mouse button is up
        /// </summary>
        /// <param name="e"></param>
        public void MouseUp(EventArgs e)
        {
            var mouseEvent = (e != null) ? (MouseEventArgs)e : null;

            if (mouseEvent == null) { return; }

            _gridHandler.SelectedTilePoint = _gridHandler.GetPointFromCoords(mouseEvent.GetPosition(_canvas));
            _mouseDown = false;
        }

    }
}
