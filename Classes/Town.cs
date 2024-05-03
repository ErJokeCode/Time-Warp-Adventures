using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GroundBattle.Game1;

namespace GroundBattle.Classes
{
    public class Town
    {
        public Texture2D BackGround { get; }

        public Vector2 Position {  get; }
        public int Width { get; }

        private Color color = Color.White;

        public Town(int position, int width, Texture2D BackGround)
        {
            Position = new Vector2 (position, Ground.Top - BackGround.Height);
            Width = width;
            this.BackGround = BackGround;
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BackGround, Position, color);
        }

        public void Update()
        {
            
        }
    }
}
