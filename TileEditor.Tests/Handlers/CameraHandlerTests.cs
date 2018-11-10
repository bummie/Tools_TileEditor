using Microsoft.VisualStudio.TestTools.UnitTesting;
using TileEditor.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TileEditor.Handlers.Tests
{
    [TestClass()]
    public class CameraHandlerTests
    {
        [TestMethod()]
        public void UpdateMovement_SeeIfCameraMovesCorrectly_PositionIsCorrect()
        {
            var cameraHandler = new CameraHandler
            {
                Position = new Point(0, 0)
            };

            cameraHandler.UpdateMovement(System.Windows.Input.Key.W);
            Assert.AreEqual(new Point(0, cameraHandler.MOVE_AMOUNT), cameraHandler.Position);

            cameraHandler.UpdateMovement(System.Windows.Input.Key.A);
            Assert.AreEqual(new Point(cameraHandler.MOVE_AMOUNT, cameraHandler.MOVE_AMOUNT), cameraHandler.Position);

            cameraHandler.UpdateMovement(System.Windows.Input.Key.S);
            Assert.AreEqual(new Point(cameraHandler.MOVE_AMOUNT, 0), cameraHandler.Position);

            cameraHandler.UpdateMovement(System.Windows.Input.Key.D);
            Assert.AreEqual(new Point(0, 0), cameraHandler.Position);

            cameraHandler.UpdateMovement(System.Windows.Input.Key.Q);
            Assert.AreEqual(1 + CameraHandler.ZOOM_LEVEL, cameraHandler.Zoom);

            cameraHandler.UpdateMovement(System.Windows.Input.Key.E);
            Assert.AreEqual(1, cameraHandler.Zoom);
        }

        [TestMethod()]
        public void Zoom_SeeIfZoomIsClamped_ValueBetweenMaxAndMin()
        {
            var cameraHandler = new CameraHandler
            {
                Zoom = -100
            };

            Assert.AreEqual(cameraHandler.MIN_ZOOM, cameraHandler.Zoom);

            cameraHandler.Zoom = 1000;
            Assert.AreEqual(cameraHandler.MAX_ZOOM, cameraHandler.Zoom);
        }
    }
}