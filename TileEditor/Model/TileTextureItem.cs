using System.Drawing;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;

namespace TileEditor.Model
{
    public class TileTextureItem
    {
        public int TextureId { get; set; }
        public BitmapSource Bitmap { get; set; }
        public Int32Rect Rectangle { get; set; }

        public TileTextureItem(int textureId, Int32Rect rect, BitmapSource bitmap)
        {
            TextureId = textureId;
            Bitmap = bitmap;
            Rectangle = rect;
        }
    }
}
