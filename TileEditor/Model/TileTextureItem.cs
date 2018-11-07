using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TileEditor.Model
{
    public class TileTextureItem
    {
        public int TextureId { get; set; }
        public Rectangle Rectangle { get; set; }
        public ImageSource Bitmap { get; set; }

        public TileTextureItem(int textureId, Rectangle rectangle, ImageSource bitmap)
        {
            TextureId = textureId;
            Rectangle = rectangle;
            Bitmap = bitmap;
        }
    }
}
