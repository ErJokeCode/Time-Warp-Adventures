using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TimeWarpAdventures.Game1;

namespace TimeWarpAdventures.Classes
{
    public class Monster
    {
        public Texture2D BackGround { get; }

        private Vector2 windowPosition;
        public Vector2 WindowPosition
        {
            get { return windowPosition; }
            set { windowPosition = value; }
        }

        public Vector2 AbsolutePosition { get { return windowPosition - new Vector2(World.PositionX, 0); } }

        public int canView { get; }

        private Color color;
        private Random rnd;
        private float velocity;
        public float Velosity { get { return velocity; } set { velocity = value; } }

        public Monster(Texture2D BackGround, int position, int canView)
        {
            WindowPosition = new Vector2(position + World.PositionX, Ground.Top - BackGround.Height);
            this.BackGround = BackGround;
            this.canView = canView;
            color = Color.White;
            rnd = new Random();
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
            var player = World.NowPlayer;

                if (Math.Abs(World.PositionX + player.Position.X - WindowPosition.X) < canView)
                {
                    IsPersuit = true;
                    if (World.PositionX + player.Position.X < WindowPosition.X) windowPosition.X -= 5;
                    else if (World.PositionX + player.Position.X > WindowPosition.X) windowPosition.X += 5;
                }
                else
                    IsPersuit = false;
        }

        private void SomethingDoing()
        {
            //Creaet maxVelocity
            velocity += (float)(Math.Round(rnd.NextDouble() * 2 - 1));
            if(velocity > 2) velocity = 2;
            else if(velocity < -2) velocity = -2;
            if (windowPosition.X + velocity >= 0 && windowPosition.X + velocity <= World.Width - BackGround.Width)
            {
                windowPosition.X += velocity;
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
