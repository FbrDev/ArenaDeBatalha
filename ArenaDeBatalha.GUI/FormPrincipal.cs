﻿using ArenaDeBatalha.GameLogic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;

namespace ArenaDeBatalha.GUI
{
    public partial class FormPrincipal : Form
    {
        DispatcherTimer gameLoopTimer { get; set; }
        DispatcherTimer enemySpawnTimer { get; set; }
        Bitmap screenBuffer { get; set; }
        Graphics screenPainter { get; set; }
        Background background { get; set; }
        Player player { get; set; }
        GameOver gameOver { get; set; }
        List<GameObject> gameObjects { get; set; }
        public Random random { get; set; }

        bool canShoot;

        int score { get; set; }

        public FormPrincipal()
        {
            InitializeComponent();

            this.random = new Random();
            float scaleX = Screen.PrimaryScreen.WorkingArea.Width / 1920.0F;
            this.ClientSize = new Size((int)(775 / scaleX), (int)(572 / scaleX));        
            this.screenBuffer = new Bitmap(Media.fundo.Width, Media.fundo.Height);
            this.screenPainter = Graphics.FromImage(this.screenBuffer);
            this.gameObjects = new List<GameObject>();
            this.background = new Background(this.screenBuffer.Size, this.screenPainter);
            this.player = new Player(this.screenBuffer.Size, this.screenPainter);
            this.gameOver = new GameOver(this.screenBuffer.Size, this.screenPainter);

            this.gameLoopTimer = new DispatcherTimer(DispatcherPriority.Render);
            this.gameLoopTimer.Interval = TimeSpan.FromMilliseconds(16.66666);
            this.gameLoopTimer.Tick += GameLoop;

            this.enemySpawnTimer = new DispatcherTimer(DispatcherPriority.Render);
            this.enemySpawnTimer.Interval = TimeSpan.FromMilliseconds(1000);
            this.enemySpawnTimer.Tick += SpawnEnemy;

            this.Load += FormPrincipal_Load;

            StartGame();
        }

        public void StartGame()
        {
            this.gameObjects.Clear();
            this.gameObjects.Add(background);
            this.gameObjects.Add(player);
            this.player.SetStartPosition();
            this.player.Active = true;
            this.gameLoopTimer.Start();
            this.enemySpawnTimer.Start();
            this.canShoot = true;
        }

        public void EndGame()
        {
            this.gameObjects.Clear();
            this.gameLoopTimer.Stop();
            this.enemySpawnTimer.Stop();
            this.background.UpdateObject();
            this.gameOver.UpdateObject();
            Invalidate();
        }

        public void SpawnEnemy(object sender, EventArgs e)
        {
            Point enemyPosition = new Point(this.random.Next(10, this.screenBuffer.Width - 74), -62);
            Enemy enemy = new Enemy(this.screenBuffer.Size, this.screenPainter, enemyPosition);
            this.gameObjects.Add(enemy);
        }

        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            Label lblScore = new Label();
            lblScore.Text = "Score: 0";
            lblScore.Location = new Point(10, 10);
            lblScore.AutoSize = true;
            this.Controls.Add(lblScore);
        }

        private void UpdateScoreLabel()
        {
            foreach (Control control in this.Controls)
            {
                if (control is Label && control.Text.StartsWith("Score: "))
                {
                    control.Text = "Score: " + score;
                    break;
                }
            }
        }

        public void GameLoop(object sender, EventArgs e)
        {
            if (score > 0)
            {
                UpdateScoreLabel();
            }

            this.gameObjects.RemoveAll(x => !x.Active);

            this.ProcessControls();

            foreach (GameObject go in this.gameObjects)
            {
                go.UpdateObject();

                if (go.IsOutOfBounds())
                {
                    go.Destroy();
                }

                if (go is Enemy)
                {
                    if (go.IsCollidingWith(player))
                    {
                        player.Destroy();
                        player.PlaySound();
                        EndGame();
                        return;
                    }
                    foreach (GameObject bullet in this.gameObjects.Where(x => x is Bullet))
                    {
                        if (go.IsCollidingWith(bullet))
                        {
                            go.Destroy();
                            bullet.Destroy();

                            if (!go.Active)
                            {
                                score++;
                            }
                        }
                    }
                }
            }

            this.Invalidate();
        }

        private void FormPrincipal_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawImage(this.screenBuffer, 0, 0);
        }

        private void ProcessControls()
        {
            if (Keyboard.IsKeyDown(Key.Left)) player.MoveLeft();
            if (Keyboard.IsKeyDown(Key.Right)) player.MoveRight();
            if (Keyboard.IsKeyDown(Key.Up)) player.MoveUp();
            if (Keyboard.IsKeyDown(Key.Down)) player.MoveDown();
            if (Keyboard.IsKeyDown(Key.Space) && canShoot)
            {
                this.gameObjects.Insert(1, player.Shoot());
                this.canShoot = false;
            }
            if (Keyboard.IsKeyUp(Key.Space)) canShoot = true;
        }

        private void FormPrincipal_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R)
            {
                StartGame();
            }
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}
