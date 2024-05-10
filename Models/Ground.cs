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

        public static List<Soil> Soils { get; set; }

        public static LinkedList<Soil> DrawSoils { get; set; }
        private static int numLastDrawSoil;

        public static Vector2 Gravity { get; set; }

        private static int widthWindow { get { return World.WindowWidth; } set { } }
        private static int heightWindow { get { return World.WindowHeight; } set { } }
        private static int heightGround { get { return World.WindowHeight / 12; } set { } }
        private static int lenOneSprite = 720;

        private static Rectangle rect;
        private static Vector2 vec;
        private static Color color;

        public static void Initialize()
        {
            rect = new Rectangle(0, 0, widthWindow, heightGround);
            vec = new Vector2(0, heightWindow - heightGround);
            color = Color.White;
            CreateGround();
        }

        private static void CreateGround()
        {
            float h = 100;
            float rightPosBox = 0;
            var numBox = 0;
            Soils = new List<Soil>();
            DrawSoils = new LinkedList<Soil>();
            Soil newSoil;
            while (rightPosBox < World.Width)
            {
                newSoil = new Soil(numBox == 0 ? -100 : rightPosBox = Soils[numBox - 1].GetRightPosition(), (int)h);
                newSoil.BackGround = BackGround;
                Soils.Add(newSoil);
                if (Soils[numBox].GetRightPosition() >= -50 && Soils[numBox].GetLeftPosition() <= World.PositionX + World.WindowWidth + 100)
                    DrawSoils.AddLast(newSoil);
                numBox++;
                h += 5;
            }
            numLastDrawSoil = DrawSoils.Count - 1;
        }

        public static (bool, int) IsTouch(Essence player, Vector2 newPos)
        {
            Rectangle boxPlayer = new Rectangle((int)newPos.X,
                (int)newPos.Y, player.Width, player.Height);
            foreach (var soil in Soils)
            {
                Rectangle boxSoil = new Rectangle((int)(soil.GetLeftPosition()),
                    (int)soil.Top, soil.Width, soil.Height);
                if (boxPlayer.Intersects(boxSoil))
                    return (true, soil.Top);
            }
            return (false, 0);
        }

        public static void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch)
        {
            foreach (var soil in DrawSoils)
            {
                soil.Draw(_spriteBatch);
            }
            //_spriteBatch.Draw(BackGround, vec, rect, color);
        }

        public static void Update()
        {
            var left = DrawSoils.First.Value.GetRightPosition();
            var right = DrawSoils.Last.Value.GetLeftPosition();

            if ((left < 0)
                && numLastDrawSoil < Soils.Count - 1)
            {
                DrawSoils.RemoveFirst();
                numLastDrawSoil++;
                DrawSoils.AddLast(Soils[numLastDrawSoil]);
            }
            else if ((right > World.WindowWidth + 100))
            {
                DrawSoils.RemoveLast();
                numLastDrawSoil--;
                DrawSoils.AddFirst(Soils[numLastDrawSoil - DrawSoils.Count]);
            }


            foreach(var soil in Soils)
            {
                soil.Update();
            }
            //rect = new Rectangle((int)World.PositionX % lenOneSprite, 0, widthWindow, heightGround);
        }
    }
}
