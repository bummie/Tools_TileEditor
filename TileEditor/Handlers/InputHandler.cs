using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

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

        #region InfoStrings
        private string _infoMousePos;
        public string InfoMousePos { get => _infoMousePos; set { _infoMousePos = $"[{value}]"; /*RaisePropertyChanged("InfoMousePos");*/ } }
        private string _infoTilePos;
        public string InfoTilePos { get => _infoTilePos; set { _infoTilePos = $"[{value}]"; /* RaisePropertyChanged("InfoTilePos"); */} }

        private string _infoCameraPosition;

        public string InfoCameraPosition
        {
            get => _infoCameraPosition; set { _infoCameraPosition = $"[{value}]"; /*RaisePropertyChanged("InfoCameraPosition"); */}
        }

        #endregion

        private readonly Canvas _canvas;
        private bool _mouseDown = false;

        public InputHandler(GridHandler gridHandler, CameraHandler cameraHandler, TileHandler tileHandler, ModeHandler modeHandler, Canvas canvas)
        {
            _gridHandler = gridHandler;
            _cameraHandler = cameraHandler;
            _tileHandler = tileHandler;
            _modeHandler = modeHandler;
            _canvas = canvas;
        }

        /// <summary>
        /// Key pressed down
        /// </summary>
        /// <param name="e"></param>
        public void KeyDown(EventArgs e)
        {
            var pressedKey = (e != null) ? (KeyEventArgs)e : null;

            _cameraHandler.UpdateMovement(pressedKey.Key);
            InfoCameraPosition = _cameraHandler.Position.ToString() + " Zoom: " + _cameraHandler.Zoom;
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

            InfoMousePos = mouseEvent.GetPosition(_canvas).ToString();
            InfoTilePos = _gridHandler.GetPointFromCoords(mouseEvent.GetPosition(_canvas)).ToString();
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
