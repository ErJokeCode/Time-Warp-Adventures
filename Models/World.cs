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
using SharpDX.WIC;
using TimeWarpAdventures.WorkWithData;
using System.Linq;
using System.Xml.Serialization;

namespace TimeWarpAdventures.Classes
{
    public static class World
    {
        public static Texture2D BackGround { get; set; }

        public static SpriteFont FontUse { get; set; }

        public static BarPlayer BarPlayer { get; set; }

        public static Player NowPlayer { get; set; }

        public static Level NowLevel { get; set; }

        public static Dictionary<string, Level> DictLevels { get; set; }

        public static List<ItemLink> Links { get; set; }

        public static List<ItemLevel> ListLevel 
        { 
            get 
            {
                if (DictLevels == null) return null;
                var list = new List<ItemLevel>();
                foreach (var level in DictLevels)
                    list.Add(new ItemLevel(level.Key, level.Value));
                return list;
            } 
            set 
            {
                var dict = new Dictionary<string, Level>();
                foreach (var level in value)
                    dict.Add(level.Key, level.Value);   
                DictLevels = dict;
            } 
        }

        public static List<Player> Players { get; set; } = new List<Player>();

        public static List<Monster> Monsters { get { return NowLevel.Monsters; } }

        public static List<Models.Box> Boxes { get { return NowLevel.Boxes; } }

        public static List<Lamp> Lamps { get { return NowLevel.Lamps; } }

        public static List<Thing> Things { get { return NowLevel.Things; } }

        public static LoaderContent Loader { get; set; }

        public static int WindowWidth { get; set; }
        public static int WindowHeight { get; set; }
        private static int width;
        public static int Width
        {
            get { return width; }
            set { width = value; }
        }
        public static int LiteralBorder { get; set; }

        public static float PositionX { get; set; }
        private static float velisityScroll;
        public static float VelisityScroll { get { return velisityScroll; } }

        private static bool isScrollToPlayer;
        private static bool pause = true;
        private static int indexNowPlayer;

        public static void NewPlayer()
        {
            var num = Players.IndexOf(NowPlayer);
            NowPlayer = Players[(num + 1) % Players.Count];
            indexNowPlayer = (num + 1) % Players.Count;
            isScrollToPlayer = true;
        }

        public static int GetIndexNowPlayer() => indexNowPlayer;

        public static void Scroll(float velosity)
        {
            velisityScroll = velosity;
            PositionX += velisityScroll;

            foreach (var player in Players)
                player.Position = new Vector2(player.Position.X - velosity, player.Position.Y);
        }

        public static void NoScroll()
        {
            velisityScroll = 0;
        }

        public static void ScrollToPlayer()
        {
            if(NowPlayer.Position.X > WindowWidth / 2 - 100 && NowPlayer.Position.X < WindowWidth / 2 + 100)
            {
                NoScroll();
                isScrollToPlayer = false;
            }
            if (NowPlayer.Position.X - WindowWidth / 2 < 0 && PositionX > 0 && isScrollToPlayer)
                Scroll(-10);
            else if (NowPlayer.Position.X - WindowWidth / 2 + NowPlayer.Width > 0 && PositionX < Width - WindowWidth && isScrollToPlayer)
                Scroll(10);
        }

        public static bool IsCollideMonsterWithPlayer(Monster monster)
        {
            var boxPlayer = new Rectangle((int)NowPlayer.Position.X,
                (int)NowPlayer.Position.Y, NowPlayer.Width, NowPlayer.Height);
            var boxMonster = new Rectangle((int)(monster.Position.X),
                (int)monster.Position.Y, monster.Width, monster.Height);

            return boxPlayer.Intersects(boxMonster);
        }

        public static void DiedPlayer()
        {
            var index = Players.IndexOf(NowPlayer);
            Players.RemoveAt(index);
            for (int i = 0; i < NowPlayer.Lamps; i++)
            {
                var pos = new Vector2(NowPlayer.Position.X + i * 20, WindowHeight - NowPlayer.Position.Y);
                Lamps.Add(new Lamp(pos, BarPlayer.TextureLamp));
            }
                
            if (Players.Count > 0)
                NowPlayer = Players[index % Players.Count];
            else
                StopGame();
        }

        private static Monster diedMonster;
        public static void DiedMonster(Monster monster)
        {
            var index = Monsters.IndexOf(monster);
            Monsters.RemoveAt(index);
            diedMonster = null;
        }

        public static void StartGame()
        {
            if (Players.Count > 0) pause = false;
        }

        public static void StopGame() => pause = true;

        public static bool IsPause() => pause;

        public static void UseThing()
        {
            CheckLamp();
            CheckLevel();
            CheckThing();
        }

        private static void CheckThing()
        {
            foreach(var thing in Things)
            {
                if(thing.IsCanUse() && thing.IsUsed())
                {
                    Loader.AddThing(thing);
                    Things.Remove(thing);
                    break;
                }    
            }
        }

        private static void CheckLevel()
        {
            var boxPlayer = new Rectangle((int)NowPlayer.Position.X,
                (int)NowPlayer.Position.Y, NowPlayer.Width, NowPlayer.Height);
            var nowLevel = NowLevel;
            Rectangle boxLevels;
            foreach (var link in GetItemLinkFromLinks(NowLevel).Links)
            {
                var level = DictLevels[link];
                boxLevels = new Rectangle((int)(level.Position.X - PositionX),
                (int)level.Position.Y, level.Width, level.Height);

                if (boxLevels.Intersects(boxPlayer))
                {
                    Loader.LoadLevel(level);
                    foreach (var player in Players)
                        player.Position = new Vector2(World.GetItemLinkFromLinks(level).GetPosition(nowLevel.Name), player.Position.Y);
                }
            }
        }

        private static void CheckLamp()
        {
            foreach(var lamp in Lamps)
            {
                if (lamp.IsHover())
                {
                    if (!lamp.IsPressed && NowPlayer.Lamps > 0)
                    {
                        lamp.ChangeState();
                        NowPlayer.Lamps -= 1;
                    }
                    else
                    {
                        lamp.ChangeState();
                        NowPlayer.Lamps += 1;
                    }
                }
            }
        }

        public static void CollideMonsterWithPlayer(Monster monster) 
        {
            if (IsCollideMonsterWithPlayer(monster) && !isScrollToPlayer)
            {
                if (NowPlayer.IsHit)
                {
                    monster.Kick(NowPlayer);
                    if (monster.Health <= 0)
                        diedMonster = monster;
                }
                else
                {
                    NowPlayer.Kick(monster);
                    if (NowPlayer.Health <= 0)
                        DiedPlayer();
                }     
            }
        }

        public static ItemLink GetItemLinkFromLinks(Level level) => Links.Where(x => x.Level == level.Name)
            .FirstOrDefault();

        private static void UpdateEssense(List<Direction> directs, GameTime gameTime)
        {
            foreach (var monster in Monsters)
            {
                CollideMonsterWithPlayer(monster);

                monster.Update(gameTime);
            }

            if (diedMonster != null)
                DiedMonster(diedMonster);

            foreach (var player in Players)
            {
                if (NowPlayer == player)
                    player.Update(gameTime, directs);
                else
                    player.Update(gameTime);
            }
        }

        private static void UpdateLevel()
        {
            foreach (var level in DictLevels.Values)
                level.Update();

            foreach(var thing in Things)
                thing.Update();

            if (isScrollToPlayer) ScrollToPlayer();
        }

        public static void Update(List<Direction> directs, GameTime gameTime)
        {
            NowLevel.GetMap().Update();
            Ground.Update();

            UpdateEssense(directs, gameTime);

            UpdateLevel();
        }
    }
}
