using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;
using TimeWarpAdventures.Classes;
using System.Collections.Generic;
using System;
using SharpDX.Direct3D9;
using static TimeWarpAdventures.Game1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using System.Threading;

namespace TimeWarpAdventures.Classes
{
    public static class World
    {
        public static Player NowPlayer { get; set; }

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
            var num = Players.IndexOf(NowPlayer);
            NowPlayer = Players[(num + 1) % Players.Count];
        }

        public static void NewPlayer(int index)
        {
            var num = index % Players.Count;
            NowPlayer = Players[num % Players.Count];
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


        public static bool CollideMonsterWithPlayer(Monster monster)
        {
            Rectangle boxPlayer = new Rectangle((int)NowPlayer.Position.X,
                (int)NowPlayer.Position.Y, NowPlayer.BackGround.Width, NowPlayer.BackGround.Height);
            Rectangle boxMonster = new Rectangle((int)(monster.AbsolutePosition.X),
                (int)monster.WindowPosition.Y, monster.BackGround.Width, monster.BackGround.Height);

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
            

            Ground.Update();

            foreach (var town in Towns)
                town.Update();

            foreach (var monster in Monsters)
            {
                if (CollideMonsterWithPlayer(monster))
                    NowPlayer.Velosity -= new Vector2(2 * NowPlayer.Velosity.X + monster.Velosity, 100);
                monster.Update();
            }
                

            foreach (var player in Players)
            {
                if(NowPlayer ==  player)
                    player.Update(directs);
                else
                    player.Update(new List<Direction>());
            }
        }
    }
}
