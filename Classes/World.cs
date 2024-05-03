using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;
using GroundBattle.Classes;
using System.Collections.Generic;
using System;
using SharpDX.Direct3D9;
using static GroundBattle.Game1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using System.Threading;

namespace GroundBattle.Classes
{
    public static class World
    {
        private static Player nowPlayer;
        public static int NowPlayer
        {
            get { return Players.IndexOf(nowPlayer); }
            set 
            { 
                var num = value % Players.Count;
                nowPlayer = Players[num];
                /*if (playerInCenter < 0)
                    PositionX = 0;
                else if (playerInCenter > Width)
                    PositionX = Width;
                else
                    PositionX = playerInCenter;*/
            }
        }

        public static int WindowWidth { get; set; }
        public static int WindowHeight { get; set; }
        private static int width;
        public static int Width 
        { 
            get {  return width; } 
            set { width = value; }
        }
        public static float PositionX {  get; set; }
        public static int LiteralBorder { get; set; }

        public static List<Player> Players = new List<Player>();

        public static List<Town> Towns = new List<Town>();
        public static List<Monster> Monsters = new List<Monster>();

        private static float velisityScroll;
        public static float VelisityScroll { get { return velisityScroll; } }


        public static void LoadContent()
        {
            WindowWidth = 1920;
            WindowHeight = 1080;
            Width = 3000;
            LiteralBorder = WindowWidth / 10;
            LoadGround();
        }

        private static void LoadGround()
        {
            Ground.Gravity = new Vector2(0, 2);
            Ground.Initialize();
        }

        public static void NewPlayer()
        {
            var num = Players.IndexOf(nowPlayer);
            nowPlayer = Players[(num + 1) % Players.Count];
        }



        public static void Scroll(float velosity)
        {
            velisityScroll = velosity;
            PositionX += velisityScroll;

            foreach(Player player in Players)
            {
                player.Position = new Vector2(player.Position.X - velosity, player.Position.Y);
            }
        }

        public static void NoScroll()
        {
            velisityScroll = 0;
        }



        public static bool Collide()
        {
            Rectangle boxPlayer = new Rectangle((int)nowPlayer.Position.X,
                (int)nowPlayer.Position.Y, nowPlayer.BackGround.Width, nowPlayer.BackGround.Height);
            Rectangle boxMonster = new Rectangle((int)(Monsters[0].AbsolutePosition.X),
                (int)Monsters[0].WindowPosition.Y, Monsters[0].BackGround.Width, Monsters[0].BackGround.Height);

            return boxPlayer.Intersects(boxMonster);
        }


        public static void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            Ground.Draw(spriteBatch);

            foreach (var town in Towns)
                town.Draw(spriteBatch);

            foreach (var monster in Monsters)
                monster.Draw(spriteBatch);

            foreach (var player in Players)
                player.Draw(spriteBatch);
        }

        public static void Update(List<Direction> directs)
        {
            if(Collide())
            {
                if(Math.Abs(nowPlayer.Velosity.X) > 1)
                    nowPlayer.Velosity -= new Vector2(2*nowPlayer.Velosity.X, 100);
            }

            Ground.Update();

            foreach (var town in Towns)
                town.Update();

            foreach (var monster in Monsters)
                monster.Update();

            foreach (var player in Players)
            {
                if(nowPlayer ==  player)
                    player.Update(directs);
                else
                    player.Update(new List<Direction>());
            }
        }
    }
}
