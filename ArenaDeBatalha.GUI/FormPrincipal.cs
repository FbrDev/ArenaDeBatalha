using ArenaDeBatalha.GameLogic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Threading;

namespace ArenaDeBatalha.GUI
{
    public partial class FormPrincipal : Form
    {
        DispatcherTimer gameLoopTimer { get; set; }
        Bitmap screenBuffer { get; set; }
        Graphics screenPainter { get; set; }
        Background background { get; set; }
        List<GameObject> gameObjects { get; set; }

        public FormPrincipal()
        {
            InitializeComponent();

            this.ClientSize = Media.fundo.Size;
            this.screenBuffer = new Bitmap(Media.fundo.Width, Media.fundo.Height);
            this.screenPainter = Graphics.FromImage(this.screenBuffer);
            this.gameObjects = new List<GameObject>();
            this.background = new Background(this.screenBuffer.Size, this.screenPainter);

            this.gameLoopTimer = new DispatcherTimer(DispatcherPriority.Render);
            this.gameLoopTimer.Interval = TimeSpan.FromMilliseconds(16.66666);
            this.gameLoopTimer.Tick += GameLoop;

            this.gameObjects.Add(this.background);

            StartGame();
        }

        public void StartGame()
        {
            this.gameLoopTimer.Start();
        }

        public void GameLoop(object sender, EventArgs e)
        {
            foreach(GameObject go in this.gameObjects)
            {
                go.UpdateObject();
                this.Invalidate();
            }
        }

        private void FormPrincipal_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawImage(this.screenBuffer, 0, 0);
        }
    }
}
