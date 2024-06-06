using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TimeWarpAdventures.Classes;
using System.Collections.Generic;
using SpriteBatch = SharpDX.Direct2D1.SpriteBatch;
using SharpDX.Direct2D1;
using System;
using TimeWarpAdventures.Models;
using System.Threading;

namespace TimeWarpAdventures.Classes
{
    public class Ground
    {
        public static Texture2D BackGround { get; set; }

        public static Vector2 Gravity { get; set; }

        public static int Top { get { return World.WindowHeight - HeightGround; } }

        private static int widthWindow = World.WindowWidth;
        public static int HeightGround { get; set; }

        private static Rectangle rect;

        public static void Initialize()
        {
            rect = new Rectangle(0, 0, widthWindow, HeightGround);
        }

        public static bool IsTouch(Essence essense)
        {
            return essense.Position.Y + essense.Height + essense.Velosity.Y > Top;
            //return essense.Position.Y + essense.Velosity.Y > heightWindow - heightGround - essense.Height;
        }

        public static Rectangle GetNewRectForView() => rect;

        public static void Update()
        {
            rect = new Rectangle((int)World.PositionX % (BackGround.Width / 3), 0, widthWindow, HeightGround);
        }
    }
}
