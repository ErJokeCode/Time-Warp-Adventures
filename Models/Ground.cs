using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TimeWarpAdventures.Classes;
using System.Collections.Generic;
using SpriteBatch = SharpDX.Direct2D1.SpriteBatch;
using SharpDX.Direct2D1;
using System;
using TimeWarpAdventures.Models;

namespace TimeWarpAdventures.Classes
{
    public class Ground
    {
        public static Texture2D BackGround { get; set; }
        
        public static Vector2 Gravity { get; set; }

        public static int Top { get { return heightWindow - heightGround; } }

        private static int widthWindow = World.WindowWidth;
        private static int heightWindow = World.WindowHeight;
        private static int heightGround = World.WindowHeight / 12;
        private static int lenOneSprite = 720;
        
        private static Rectangle rect ;
        private static Vector2 vec;
        private static Color color;

        public static void Initialize()
        {
            rect = new Rectangle(0, 0, widthWindow, heightGround);
            vec = new Vector2(0, heightWindow - heightGround);
            color = Color.White;
        }

        public static void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BackGround, vec, rect, color);
        }

        public static void Update()
        {
            rect = new Rectangle((int)World.PositionX % lenOneSprite, 0, widthWindow, heightGround);
        }
    }
}
