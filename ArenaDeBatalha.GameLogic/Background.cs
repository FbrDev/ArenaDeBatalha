using System.Drawing;

namespace ArenaDeBatalha.GameLogic
{
    public class Background : GameObject
    {
        public Background(Size bounds, Graphics screen) : base(bounds, screen)
        {
            this.Left = 0;
            this.Top = 0;
            this.Speed = 0;
        }

        public override Bitmap GetSprite()
        {
            return Media.fundo;
        }
    }
}
