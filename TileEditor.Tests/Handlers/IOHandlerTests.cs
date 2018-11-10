using Microsoft.VisualStudio.TestTools.UnitTesting;
using TileEditor.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEditor.Handlers.Tests
{
    [TestClass()]
    public class IOHandlerTests
    {
        [TestMethod()]
        public void WriteToFile_WriteDataToFileAndRead_DataReadBackIsEqual()
        {
            IOHandler.WriteToFile("test.txt", "test");

            Assert.AreEqual("test", IOHandler.ReadFromFile("test.txt"));
        }
    }
}