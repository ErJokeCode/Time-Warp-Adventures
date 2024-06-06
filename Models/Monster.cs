using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TimeWarpAdventures.Game1;
using TimeWarpAdventures.Models;
using Microsoft.Xna.Framework.Content;
using SharpDX.Direct3D9;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using System.Threading;

namespace TimeWarpAdventures.Classes
{
    public class Monster : Essence
    {
        public int canView { get; }

        private int currentTime;
        private const int changeMoveTime = 3000;
        private Random rnd = new Random();
        private bool moveToRight = false;

        private AI ai = new AI();

        public Monster(Texture2D backGround, int countFrameWidth, int countFrameHeight, int position, int canView, 
            int health, int hit, int acceleration = 1, int maxVelosity = 4, int jamp = 20)
            :base(backGround, countFrameWidth, countFrameHeight, new Vector2(position, 0), maxVelosity)
        {
            Acceleration = acceleration;
            Jump = jamp;
            this.canView = canView;
            Health = health;
            Hit = hit;
        }

        public Monster() : base()
        {
            this.canView = 500;
            Health = 100;
            Hit = 100;
        }

        private List<Direction> Persuit(Player player)
        {
            var kill = 200;
            var raznVect = World.NowPlayer.Position - Position;
            var correct = new Vector2(World.NowPlayer.Width / 2 - Width/2, World.NowPlayer.Height / 2 - Height/2);
            raznVect += correct;
            var makeKill = Math.Sqrt(raznVect.X * raznVect.X + raznVect.Y * raznVect.Y) <= kill;
            var dirs = ai.GetDirectionsTo(World.NowLevel.GetMap(), this, player);
            if (!IsHit && makeKill) 
                IsHit = true;
            else if(IsHit && !makeKill) IsHit = false;
            return dirs;
        }

        private bool IsFindPlayer(Player player) => Math.Abs(Position.X - player.Position.X) < canView;

        private List<Direction> CreateVelosity()
        {
            var dirs = new List<Direction>();
            if(moveToRight)
                dirs.Add(Direction.Right);
            else
                dirs.Add(Direction.Left);
            return dirs;
        }

        public void LimitWorld()
        {
            if (World.PositionX + Position.X + Velosity.X <= 0 && Velosity.X != 0)
            {
                Position = new Vector2(-World.PositionX, Position.Y);
                Velosity = new Vector2(0, Velosity.Y);
            }
            else if (Position.X + Velosity.X + World.PositionX >= World.Width - Width && Velosity.X != 0)
            {
                Position = new Vector2(World.Width - World.PositionX - Width, Position.Y);
                Velosity = new Vector2(0, Velosity.Y);
            }
        }

        public void Update(GameTime gameTime)
        {
            currentTime += gameTime.ElapsedGameTime.Milliseconds;

            if(currentTime > changeMoveTime)
            {
                moveToRight = (rnd.NextDouble() * 2 - 1) >= 0;
                currentTime = 0;
            } 

            if (IsFindPlayer(World.NowPlayer))
                UpdateVelosity(Persuit(World.NowPlayer));
            else
                UpdateVelosity(CreateVelosity());
            Position += Velosity - new Vector2(World.VelisityScroll, 0);

            UpdateFrame(gameTime);
            LimitWorld();
        }
    }
}
