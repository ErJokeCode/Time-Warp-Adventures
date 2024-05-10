using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using static TimeWarpAdventures.Game1;
using System.Diagnostics.Contracts;
using System.Threading;
using TimeWarpAdventures.Models;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework.Content;

namespace TimeWarpAdventures.Classes
{
    public class Player : Essence
    {
        public int Health { get; set; } = 100;

        private Texture2D backGround;
        public string NameTexture { get; set; }

        private Color color = Color.White;

        private int widthWindow = World.WindowWidth;

        public Player(Texture2D backGround, int startPosX, int maxVelosity, int acceleration, int jump) 
        {
            this.backGround = backGround;
            this.MaxVelosity = maxVelosity;
            this.Acceleration = acceleration;
            this.Jump = jump;
            Position = new Vector2(startPosX + World.PositionX, backGround.Height);
            Width = this.backGround.Width;
            Height = this.backGround.Height;
            NameTexture = backGround.Name;
        }

        public Player()
        {
            MaxVelosity = 10;
            Acceleration = 2;
            Jump = 30;
            Position = new Vector2(700 + World.PositionX, -200); 
        }

        public void AddBackGround(ContentManager content)
        {
            backGround = content.Load<Texture2D>(NameTexture);
            Width = backGround.Width;
            Height = backGround.Height;
        }

        public Texture2D GetBackGround() => backGround;

        private void UpdateScroll() 
        {
            if (Velosity.X < 0 && Position.X > 0)
                ScrollWorld(IsTouchingLeftBorder());
            else if (Velosity.X > 0 && Position.X < widthWindow - Width)
                ScrollWorld(IsTouchingRightBorder());
        }

        private bool IsTouchingLeftBorder() => World.PositionX > 0 && Position.X < World.LiteralBorder;

        private bool IsTouchingRightBorder()
        {
            var rightBorder = widthWindow - World.LiteralBorder - Width;
            return World.PositionX < World.Width - World.WindowWidth && Position.X + Velosity.Y >= rightBorder;
        }

        private void ScrollWorld(bool touchBorder)
        {
            if (touchBorder)
            {
                World.Scroll(Velosity.X);
            }
            else
                World.NoScroll();
        }

        public void LimitWorld()
        {
            if (Position.X + Velosity.X <= 0 && Velosity.X != 0)
            {
                Position = new Vector2(0, Position.Y);
                Velosity = new Vector2(0, Velosity.Y);
            }
            else if (Position.X + Velosity.X >= widthWindow - Width && Velosity.X != 0)
            {
                Position = new Vector2(widthWindow - Width, Position.Y);
                Velosity = new Vector2(0, Velosity.Y);
            }
        }

        public void MonsterKick(Monster monster)
        {
            Velosity -= new Vector2(Velosity.X - 100 * monster.Velosity.X, 500);
            Health -= monster.Hit;
            if (Health < 0)
                World.DiedPlayer();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backGround, Position, color);
        }

        public void Update(List<Direction> dirs)
        {
            UpdateVelosity(dirs);
            UpdateScroll();
            LimitWorld();
            Position += Velosity;
        }
    }
}
