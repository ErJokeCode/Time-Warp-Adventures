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

namespace TimeWarpAdventures.Classes
{
    public class Player
    {
        public int Health { get; set; } = 100; 

        private Vector2 position;
        public Vector2 Position 
        { 
            get { return position; } 
            set {  position = value; } 
        }

        public Texture2D BackGround {  get; }

        private Vector2 velosity;
        public Vector2 Velosity { get { return velosity; } set { velosity = value; } }


        private Color color = Color.White;
        private int maxVelosity;
        private int acceleration;
        private int jump;

        private int widthWindow = World.WindowWidth;

        public Player(Texture2D backGround, int startPosX, int maxVelosity, int acceleration, int jump) 
        {
            BackGround = backGround;
            this.maxVelosity = maxVelosity;
            this.acceleration = acceleration;
            this.jump = jump;
            Position = new Vector2(startPosX + World.PositionX, Ground.Top - BackGround.Height);
        }

        private Vector2 GetSpeed(List<Direction> dirs)
        {
            var velocCorect = new Vector2(0, 0);
            foreach(var dir in dirs)
            {
                if (dir == Direction.Left)
                    velocCorect.X -= acceleration;
                if (dir == Direction.Right)
                    velocCorect.X += acceleration;
                if (dir == Direction.Up)
                    velocCorect.Y -= jump;
                if (dir == Direction.Down)
                    velocCorect.Y += acceleration;
            }
            return velocCorect;
        }

        private Vector2 GetTrueSpeed(Vector2 vect)
        {
            vect.Y = vect.Y < -jump ? -jump : vect.Y;
            vect.X = vect.X > maxVelosity ? maxVelosity : vect.X;
            vect.X = vect.X < -maxVelosity ? -maxVelosity : vect.X;

            return vect;
        }

        private void UpdatePosition(Vector2 cortrect) 
        {
            var friction = new Vector2(0.9f, 1);
            if (IsTouchingGround())
            {
                if (Velosity.Y > 0)
                    cortrect.Y = -Velosity.Y;
                Velosity = Velosity * friction;
            }
            else
                cortrect = new Vector2(0, Ground.Gravity.Y);

            Velosity += cortrect;
            Velosity = GetTrueSpeed(Velosity);

            if (Velosity.X < 0 && position.X > 0)
                ScrollWorld(IsTouchingLeftBorder());
            else if (Velosity.X > 0 && position.X < widthWindow - BackGround.Width)
                ScrollWorld(IsTouchingRightBorder());
            else if (position.X + Velosity.X <= 0 && Velosity.X != 0) 
            {
                position.X = 0;
                velosity.X = 0;
            }
            else if (position.X + Velosity.X >= widthWindow - BackGround.Width && Velosity.X != 0)
            {
                position.X = widthWindow - BackGround.Width;
                velosity.X = 0;
            }

            position += Velosity;
        }

        private bool IsTouchingGround() => position.Y + BackGround.Height + Velosity.Y > Ground.Top;

        private bool IsTouchingLeftBorder() => World.PositionX > 0 && position.X < World.LiteralBorder;

        private bool IsTouchingRightBorder()
        {
            var rightBorder = widthWindow - World.LiteralBorder - BackGround.Width;
            return World.PositionX < World.Width - World.WindowWidth && position.X + Velosity.Y >= rightBorder;
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

        public void MonsterKick(Monster monster)
        {
            Velosity -= new Vector2(Velosity.X - 100 * monster.Velosity.X, 500);
            Health -= monster.Hit;
            if (Health < 0)
                World.DiedPlayer();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BackGround, Position, color);
        }

        public void Update(List<Direction> dirs)
        {
            UpdatePosition(GetSpeed(dirs));
        }
    }
}
