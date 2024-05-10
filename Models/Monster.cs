using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TimeWarpAdventures.Game1;
using TimeWarpAdventures.Models;
using Microsoft.Xna.Framework.Content;
using SharpDX.Direct3D9;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace TimeWarpAdventures.Classes
{
    public class Monster : Essence
    {
        public int Health { get; set; } = 10;

        public int Hit { get; set; } = 0;

        private Texture2D backGround;
        public string NameTexture { get; set; }

        public int canView { get; }

        private Color color;
        private Random rnd;

        public Monster(Texture2D backGround, int position, int canView, int health, int hit)
        {
            Acceleration = 1;
            MaxVelosity = 4;

            this.backGround = backGround;
            this.canView = canView;
            color = Color.White;
            rnd = new Random();
            Health = health;
            Hit = hit;
            NameTexture = backGround.Name;
            Width = backGround.Width;
            Height = backGround.Height;
        }

        public Monster()
        {
            this.canView = 500;
            color = Color.White;
            rnd = new Random();
            Health = 100;
            Hit = 100;
        }

        public void AddBackGround(ContentManager content)
        {
            backGround = content.Load<Texture2D>(NameTexture);
            Width = backGround.Width;
            Height = backGround.Height;
        }

        private bool IsPersuit = false;

        private List<Direction> Persuit()
        {
            var dirs = new List<Direction>();
            var player = World.NowPlayer;

            if (Math.Abs(Position.X - player.Position.X) < canView)
            {
                IsPersuit = true;
                if (Position.X > player.Position.X)
                    dirs.Add(Direction.Left);
                else if (Position.X < player.Position.X)
                    dirs.Add(Direction.Right);
            }
            else
                IsPersuit = false;
            return dirs;
        }

        private List<Direction> CreateVelosity()
        {
            var dirs = new List<Direction>();
            dirs.Add(Math.Round(rnd.NextDouble() * 2 - 1) > 0 ? Direction.Right : Direction.Left);
            dirs.Add(Math.Round(rnd.NextDouble() * 2 - 1) > 0 ? Direction.Up : Direction.Down);
            return dirs;
        }

        public void LimitWorld()
        {
            if (World.PositionX + Position.X + Velosity.X <= 0 && Velosity.X != 0)
            {
                Position = new Vector2(0, Position.Y);
                Velosity = new Vector2(0, Velosity.Y);
            }
            else if (Position.X + Velosity.X >= World.Width - World.PositionX- Width && Velosity.X != 0)
            {
                Position = new Vector2(World.Width - World.PositionX - Width, Position.Y);
                Velosity = new Vector2(0, Velosity.Y);
            }
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backGround, Position, color);
        }

        public void Update()
        {
            UpdateVelosity(Persuit());
            if (!IsPersuit)
                UpdateVelosity(CreateVelosity());
            Position += Velosity - new Vector2(World.VelisityScroll, 0);
            LimitWorld();
        }
    }
}
