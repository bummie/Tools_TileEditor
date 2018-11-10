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
    public class GridHandlerTests
    {
        [TestMethod()]
        public void GridHandler_SetsDefaultValues_DefaultValues()
        {
            var gridHandler = new GridHandler(new CameraHandler());

            Assert.AreEqual(16, gridHandler.TileSize);
            Assert.AreEqual(16, gridHandler.GridWidth);
            Assert.AreEqual(16, gridHandler.GridHeight);
        }

        [TestMethod]
        public void GetPointFromCoords_ReturnsTileFromMouseCoords_ReturnsCorrectPoint()
        {
            var cameraHandler = new CameraHandler();
            cameraHandler.Position = new Point(0, 0);

            var gridHandler = new GridHandler(cameraHandler);

            var point = gridHandler.GetPointFromCoords(new Point(14, 7));
            Assert.AreEqual(new Point(0, 0), point);

            var point2 = gridHandler.GetPointFromCoords(new Point(16, 7));
            Assert.AreEqual(new Point(1, 0), point2);

            var point3 = gridHandler.GetPointFromCoords(new Point(-100, 700));
            Assert.AreEqual(new Point(-1, -1), point3);
        }

        [TestMethod]
        public void GetCoordsFromPoint_ReturnsCoordsFromTile_ReturnsCorrectCoord()
        {
            var cameraHandler = new CameraHandler();
            cameraHandler.Position = new Point(0, 0);

            var gridHandler = new GridHandler(cameraHandler);

            var point = gridHandler.GetCoordsFromPoint(new Point(0, 0));
            Assert.AreEqual(new Point(0, 0), point);

            var point2 = gridHandler.GetCoordsFromPoint(new Point(1, 0));
            Assert.AreEqual(new Point(16, 0), point2);
        }

        [TestMethod]
        public void IsTileInsideGrid_SeeIfTileIsInsideGrid_ReturnsBool()
        {
            var cameraHandler = new CameraHandler();
            var gridHandler = new GridHandler(cameraHandler);

            var point = new Point(0, 0);
            var point2 = new Point(-1, 0);
            var point3 = new Point(32, 32);

            Assert.IsTrue(gridHandler.IsTileInsideGrid(point));
            Assert.IsFalse(gridHandler.IsTileInsideGrid(point2));
            Assert.IsFalse(gridHandler.IsTileInsideGrid(point3));
        }
    }
}