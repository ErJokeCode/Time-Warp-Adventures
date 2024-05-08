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
    public class World
    {
        public Player NowPlayer { get; set; }

        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }
        private int width;
        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        public float PositionX { get; set; }
        public int LiteralBorder { get; set; }

        public List<Player> Players = new List<Player>();

        public List<Town> Towns = new List<Town>();
        public List<Monster> Monsters = new List<Monster>();

        private float velisityScroll;
        public float VelisityScroll { get { return velisityScroll; } }


        public void LoadContent()
        {
            WindowWidth = 1920;
            WindowHeight = 1080;
            Width = 3000;
            LiteralBorder = WindowWidth / 10;
            LoadGround();
        }

        private void LoadGround()
        {
            Ground.Gravity = new Vector2(0, 2);
            Ground.Initialize();
        }

        public void NewPlayer()
        {
            var num = Players.IndexOf(NowPlayer);
            NowPlayer = Players[(num + 1) % Players.Count];
        }

        public void NewPlayer(int index)
        {
            var num = index % Players.Count;
            NowPlayer = Players[num % Players.Count];
        }

        public void Scroll(float velosity)
        {
            velisityScroll = velosity;
            PositionX += velisityScroll;

            foreach (Player player in Players)
            {
                player.Position = new Vector2(player.Position.X - velosity, player.Position.Y);
            }
        }

        public void NoScroll()
        {
            velisityScroll = 0;
        }


        public bool CollideMonsterWithPlayer(Monster monster)
        {
            Rectangle boxPlayer = new Rectangle((int)NowPlayer.Position.X,
                (int)NowPlayer.Position.Y, NowPlayer.BackGround.Width, NowPlayer.BackGround.Height);
            Rectangle boxMonster = new Rectangle((int)(monster.AbsolutePosition.X),
                (int)monster.WindowPosition.Y, monster.BackGround.Width, monster.BackGround.Height);

            return boxPlayer.Intersects(boxMonster);
        }

        public void DiedPlayer()
        {
            var index = Players.IndexOf(NowPlayer);
            Players.RemoveAt(index);
            if (Players.Count > 0)
                NowPlayer = Players[index % Players.Count];
            else
                GameState.StartGame();
        }


        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            Ground.Draw(spriteBatch);

            foreach (var town in Towns)
                town.Draw(spriteBatch);

            foreach (var monster in Monsters)
                monster.Draw(spriteBatch);

            foreach (var player in Players)
                player.Draw(spriteBatch);
        }

        public void Update(List<Direction> directs)
        {
            Ground.Update();

            foreach (var town in Towns)
                town.Update();

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
