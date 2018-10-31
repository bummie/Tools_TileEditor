using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TileEditor.Handlers
{
    public static class ImagePool
    {
        private static List<Image> _available = new List<Image>();
        private static List<Image> _inUse = new List<Image>();

        /// <summary>
        /// Returns a new or free image object
        /// </summary>
        /// <returns></returns>
        public static Image GetObject()
        {
            lock (_available)
            {
                if (_available.Count != 0)
                {
                    Image po = _available[0];
                    _inUse.Add(po);
                    _available.RemoveAt(0);
                    return po;
                }
                else
                {
                    Image po = new Image();
                    _inUse.Add(po);
                    return po;
                }
            }
        }

        /// <summary>
        /// Releases all image objects
        /// </summary>
        public static void ReleaseAll()
        {
            lock (_available)
            {
                foreach(Image image in _inUse)
                {
                    _available.Add(image);
                }

                _inUse.Clear();
            }
        }
    }
}
