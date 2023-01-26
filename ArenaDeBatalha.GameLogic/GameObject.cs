using System.Drawing;
using System.IO;
using System.Media;

namespace ArenaDeBatalha.GameLogic
{
    public abstract class GameObject
    {
        #region Game object properties 
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
        #endregion

        #region Game Object Methods

        public abstract Bitmap GetSprite();

        public GameObject(Size bounds, Graphics screen)
        {
            this.Bounds = bounds;
            this.Screen= screen;
            this.Active = true;
            this.soundPlayer = new SoundPlayer();
            this.Sprite = GetSprite();
            this.Rectangle = new Rectangle(this.Left, this.Top, this.Width, this.Height);
        }

        public virtual void UpdateObject()
        {
            this.Rectangle = new Rectangle(this.Left, this.Top, this.Width, this.Height);
            this.Screen.DrawImage(this.Sprite, this.Rectangle);
        }

        public virtual void MoveLeft()
        {
            if (this.Left > 0) this.Left -= this.Speed;
        }

        public virtual void MoveRight()
        {
            if (this.Left < this.Bounds.Width - this.Width) this.Left += this.Speed;
        }

        public virtual void MoveDown()
        {
            this.Top += this.Speed;
        }

        public virtual void MoveUp()
        {
            this.Top -= this.Speed;
        }

        public bool IsOutOfBounds()
        {
            return (this.Top > this.Bounds.Height + this.Height) || (this.Top < -this.Height)
                || (this.Left > this.Bounds.Width + this.Width) || (this.Left < -this.Width);
        }

        public void PlaySound()
        {
            soundPlayer.Stream = this.Sound;
            soundPlayer.Play();
        }

        public bool IsCollidingWith(GameObject gameObject)
        {
            if (this.Rectangle.IntersectsWith(gameObject.Rectangle))
            {
                this.PlaySound();
                return true;
            }
            return false;
        }

        public void Destroy()
        {
            this.Active = false;
        }

        #endregion
    }
}
