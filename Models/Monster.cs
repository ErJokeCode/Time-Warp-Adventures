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

namespace TimeWarpAdventures.Classes
{
    public class Monster
    {
        private World world = GameState.world;
        public int Health { get; set; } = 10;

        public int Hit {  get; } = 0;

        public Texture2D BackGround { get; }

        private Vector2 windowPosition;
        public Vector2 WindowPosition
        {
            get { return windowPosition; }
            set { windowPosition = value; }
        }

        public Vector2 AbsolutePosition { get { return windowPosition - new Vector2(world.PositionX, 0); } }

        public int canView { get; }

        private Color color;
        private Random rnd;
        private Vector2 velocity;
        public Vector2 Velosity { get { return velocity; } set { velocity = value; } }

        public Monster(Texture2D BackGround, int position, int canView, int health, int hit)
        {
            WindowPosition = new Vector2(position + world.PositionX, Ground.Top - BackGround.Height);
            this.BackGround = BackGround;
            this.canView = canView;
            color = Color.White;
            rnd = new Random();
            Health = health;
            Hit = hit;
        }

        private void UpdatePosition()
        {

            Persuit();
            if(!IsPersuit)
                SomethingDoing();
        }

        private bool IsPersuit = false;

        private void Persuit()
        {
            var acceleration = 5;
            var player = world.NowPlayer;

            if (Math.Abs(world.PositionX + player.Position.X - WindowPosition.X) < canView)
            {
                IsPersuit = true;
                if (world.PositionX + player.Position.X < WindowPosition.X) 
                    velocity.X = -acceleration;
                else if (world.PositionX + player.Position.X > WindowPosition.X) 
                    velocity.X = acceleration;
                windowPosition.X += velocity.X;
            }
            else
                IsPersuit = false;
        }

        private void SomethingDoing()
        {
            velocity.X += (float)(Math.Round(rnd.NextDouble() * 2 - 1));
            if(velocity.X > 2) velocity.X = 2;
            else if(velocity.X < -2) velocity.X = -2;
            if (windowPosition.X + velocity.X >= 0 && windowPosition.X + velocity.X <= world.Width - BackGround.Width)
            {
                windowPosition.X += velocity.X;
            }
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BackGround, AbsolutePosition, color);
        }

        public void Update()
        {
            UpdatePosition();
        }
    }
}
