using System;
using System.IO;
using System.Windows;

namespace TileEditor.Handlers
{
    public static class IOHandler
    {
        /// <summary>
        /// Writes to given file the given data
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="data"></param>
        public static void WriteToFile(string filePath, string data)
        {
            try
            {
                StreamWriter file = new StreamWriter(filePath);
                file.Write(data);
                file.Close();
            }
            catch(DirectoryNotFoundException exception)
            {
                Console.WriteLine("IOHandler: " + exception.Message);
                MessageBox.Show("Could not save map.");
            }
        }

        /// <summary>
        /// Reads data from given file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>Content from the file being read from</returns>
        public static string ReadFromFile(string filePath)
        {
            try
            {
                return File.ReadAllText(filePath);
            }
            catch (FileNotFoundException exception)
            {
                Console.WriteLine("IOHandler: " + exception.Message);
                MessageBox.Show("Map was not found!");
            }

            return null;
        }
    }
}
