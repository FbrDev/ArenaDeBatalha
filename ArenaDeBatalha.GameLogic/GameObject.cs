using System.Drawing;
using System.IO;
using System.Media;

namespace ArenaDeBatalha.GameLogic
{
    public class GameObject
    {
        public Bitmap Sprite { get; set; }
        public bool Active { get; set; }
        public int Speed { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get { return this.Sprite.Width; } }
        public int Height { get { return this.Sprite.Height; } }
        public Size Bounds { get; set; }
        public Rectangle Rectangle { get; set; }
        public Stream Sound { get; set; }
        public Graphics Screen { get; set; }

        private SoundPlayer soundPlayer { get; set; }
    }
}
