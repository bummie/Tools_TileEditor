using System.Drawing;
using System.Windows.Media;
using System.Windows;
namespace TileEditor.Model
{
    public class TileTextureItem
    {
        public int TextureId { get; set; }
        public ImageSource Bitmap { get; set; }
        public Int32Rect Rectangle { get; set; }

        public TileTextureItem(int textureId, Int32Rect rect, ImageSource bitmap)
        {
            TextureId = textureId;
            Bitmap = bitmap;
            Rectangle = rect;
        }
    }
}
