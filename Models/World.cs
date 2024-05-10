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
using TimeWarpAdventures.Models;

namespace TimeWarpAdventures.Classes
{
    public class WorldInfo
    {
        public float Position { get { return World.PositionX; } }
        //public int WindowWidth { get { return World.WindowWidth; } }
        //public int WindowHeight { get { return World.WindowHeight; } }
        public int Width { get { return World.Width; } set { } }
        //public int Border { get { return World.LiteralBorder; } }

        public Player NowPlayer { get { return World.NowPlayer; } }
        public List<Player> Players { get {  return World.Players; } }
        public List<Monster> Monstres { get { return World.Monsters; } }
    }

    public static class World
    {
        private static bool pause = true;

        public static HealthBar HealthBar { get; set; }

        public static Player NowPlayer { get; set; }

        public static int WindowWidth { get; set; }
        public static int WindowHeight { get; set; }
        private static int width;
        public static int Width
        {
            get { return width; }
            set { width = value; }
        }
        public static float PositionX { get; set; }
        public static int LiteralBorder { get; set; }

        public static List<Player> Players = new List<Player>();

        public static List<Monster> Monsters = new List<Monster>();

        private static float velisityScroll;
        public static float VelisityScroll { get { return velisityScroll; } }


        public static void LoadContent(GraphicsDevice graphicsDevice)
        {
            WindowWidth = 1920;
            WindowHeight = 1080;
            Width = 3000;
            LiteralBorder = WindowWidth / 10;

            HealthBar = new HealthBar();
            HealthBar.AddTexture(graphicsDevice);

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

            foreach (Player player in Players)
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
                (int)NowPlayer.Position.Y, NowPlayer.Width, NowPlayer.Height);
            Rectangle boxMonster = new Rectangle((int)(monster.Position.X),
                (int)monster.Position.Y, monster.Width, monster.Height);

            return boxPlayer.Intersects(boxMonster);
        }

        public static void DiedPlayer()
        {
            var index = Players.IndexOf(NowPlayer);
            Players.RemoveAt(index);
            if (Players.Count > 0)
                NowPlayer = Players[index % Players.Count];
            else
                StopGame();
        }

        public static void StartGame()
        {
            if (Players.Count > 0) pause = false;
        }

        public static void StopGame() => pause = true;

        public static bool IsPause() => pause;

        public static void Update(List<Direction> directs)
        {
            Ground.Update();

            foreach (var monster in Monsters)
            {
                if (CollideMonsterWithPlayer(monster))
                    NowPlayer.MonsterKick(monster);
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
