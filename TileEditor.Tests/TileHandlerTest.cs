// <copyright file="TileHandlerTest.cs">Copyright ©  2018</copyright>
using System;
using System.Windows;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TileEditor.Handlers;
using TileEditor.Model;

namespace TileEditor.Handlers.Tests
{
    [TestClass]
    public partial class TileHandlerTest
    {
        [TestMethod]
        public void AddTile_IsAddedToDictionary_DictionaryCountIncreases()
        {
            var tileHandler = new TileHandler(new GridHandler(new CameraHandler()));

            tileHandler.AddTile(new Point(0, 0));

            Assert.AreEqual(1, tileHandler.TileDictionary.Count);
            Assert.AreEqual(1, tileHandler.TilePropertyDictionary.Count);
        }

        [TestMethod]
        public void AddTileProperty_IsAddedToDictionary_DictionaryCountIncreases()
        {
            var tileHandler = new TileHandler(new GridHandler(new CameraHandler()));

            tileHandler.AddTileProperty(new TileProperty(0));

            Assert.AreEqual(1, tileHandler.TilePropertyDictionary.Count);
        }

        [TestMethod]
        public void GetTile_TileDataIsEqual_RetrievesCorrectTile()
        {
            var tileHandler = new TileHandler(new GridHandler(new CameraHandler()));

            tileHandler.AddTile(new Point(0, 0), 3);

            var retrievedTile = tileHandler.GetTile(new Point(0, 0));

            Assert.AreEqual(3, retrievedTile.TextureId);
        }

        [TestMethod]
        public void GetTileProperty_TilePropertyDataIsEqual_RetrievesCorrectTileProperty()
        {
            var tileHandler = new TileHandler(new GridHandler(new CameraHandler()));

            tileHandler.AddTileProperty(new TileProperty(4));

            var retrievedTile = tileHandler.GetTileProperty(4);

            Assert.AreEqual(4, retrievedTile.TextureId);
        }

        [TestMethod]
        public void FillTiles_FilleTheWholeGridWithTiles_IncreasedAmountOfTiles()
        {
            Random rnd = new Random();
            int width = rnd.Next(1, 100);
            int height = rnd.Next(1, 100);
            var gridHandler = new GridHandler(width, height, 16, new CameraHandler());
            var tileHandler = new TileHandler(gridHandler);
            tileHandler.SelectedTileTextureId = 0;

            tileHandler.FillTiles(new Point(0, 0), -1);
            
            Assert.AreEqual(width*height, tileHandler.TileDictionary.Count);
        }

        [TestMethod]
        public void Reset_ClearsTheTileDictionaries_DictionariesEmpty()
        {
            var gridHandler = new GridHandler(new CameraHandler());
            var tileHandler = new TileHandler(gridHandler);

            tileHandler.AddTile(new Point(0, 0));
            Assert.AreEqual(1, tileHandler.TileDictionary.Count);

            tileHandler.Reset();
            Assert.AreEqual(0, tileHandler.TileDictionary.Count);
            Assert.AreEqual(0, tileHandler.TilePropertyDictionary.Count);
        }
    }
}
