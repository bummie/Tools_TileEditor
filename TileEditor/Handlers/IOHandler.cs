using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEditor.Handlers
{
    public class IOHandler
    {
        //private readonly string MAPS_PATH = System.IO.Path.Combine(System.IO.Path.GetFullPath(@"..\..\"), @"Resources\Maps\");

        public IOHandler()
        { }


        /// <summary>
        /// Writes to given file the given data
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="data"></param>
        public void WriteToFile(string filePath, string data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads data from given file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>Content from the file being read from</returns>
        public string ReadFromFile(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
