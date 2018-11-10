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
    /// <summary>This class contains parameterized unit tests for TileHandler</summary>
    [PexClass(typeof(TileHandler))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class TileHandlerTest
    {
        /// <summary>Test stub for AddTileProperty(TileProperty)</summary>
        [PexMethod]
        public void AddTilePropertyTest(
            [PexAssumeUnderTest]TileHandler target,
            TileProperty tileProperty
        )
        {
            target.AddTileProperty(tileProperty);
            // TODO: add assertions to method TileHandlerTest.AddTilePropertyTest(TileHandler, TileProperty)
        }

        /// <summary>Test stub for AddTile(Point)</summary>
        [PexMethod]
        public void AddTileTest([PexAssumeUnderTest]TileHandler target, Point position)
        {
            target.AddTile(position);
            // TODO: add assertions to method TileHandlerTest.AddTileTest(TileHandler, Point)
        }

        /// <summary>Test stub for AddTile(Point, Int32)</summary>
        [PexMethod]
        public void AddTileTest01(
            [PexAssumeUnderTest]TileHandler target,
            Point position,
            int textureId
        )
        {
            target.AddTile(position, textureId);
            // TODO: add assertions to method TileHandlerTest.AddTileTest01(TileHandler, Point, Int32)
        }

        /// <summary>Test stub for .ctor(TileTextureItem, GridHandler)</summary>
        [PexMethod]
        public TileHandler ConstructorTest(TileTextureItem selectedTexture, GridHandler gridHandler)
        {
            TileHandler target = new TileHandler(selectedTexture, gridHandler);
            return target;
            // TODO: add assertions to method TileHandlerTest.ConstructorTest(TileTextureItem, GridHandler)
        }

        /// <summary>Test stub for FillTiles(Point, Int32)</summary>
        [PexMethod]
        public void FillTilesTest(
            [PexAssumeUnderTest]TileHandler target,
            Point tile,
            int targetTexture
        )
        {
            target.FillTiles(tile, targetTexture);
            // TODO: add assertions to method TileHandlerTest.FillTilesTest(TileHandler, Point, Int32)
        }

        /// <summary>Test stub for GetTileProperty(Int32)</summary>
        [PexMethod]
        public TileProperty GetTilePropertyTest([PexAssumeUnderTest]TileHandler target, int textureId)
        {
            TileProperty result = target.GetTileProperty(textureId);
            return result;
            // TODO: add assertions to method TileHandlerTest.GetTilePropertyTest(TileHandler, Int32)
        }

        /// <summary>Test stub for GetTile(Point)</summary>
        [PexMethod]
        public Tile GetTileTest([PexAssumeUnderTest]TileHandler target, Point position)
        {
            Tile result = target.GetTile(position);
            return result;
            // TODO: add assertions to method TileHandlerTest.GetTileTest(TileHandler, Point)
        }

        /// <summary>Test stub for Reset()</summary>
        [PexMethod]
        public void ResetTest([PexAssumeUnderTest]TileHandler target)
        {
            target.Reset();
            // TODO: add assertions to method TileHandlerTest.ResetTest(TileHandler)
        }
    }
}
