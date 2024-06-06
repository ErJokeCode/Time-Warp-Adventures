using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using static TimeWarpAdventures.Game1;
using System.Diagnostics.Contracts;
using System.Threading;
using TimeWarpAdventures.Models;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework.Content;
using System.Linq;

namespace TimeWarpAdventures.Classes
{
    public class Player : Essence
    {
        public int Lamps { get; set; } = 0;
        public bool IsActiveAI { get; set; }

        private Color color = Color.White;

        private AI ai = new AI();

        private int widthWindow = World.WindowWidth;

        public Player(Texture2D backGround, int countFrameWidth, int countFrameHeight, int startPosX, int maxVelosity, int acceleration, int jump) 
            : base(backGround, countFrameWidth, countFrameHeight, new Vector2(startPosX + World.PositionX, backGround.Height), maxVelosity)
        {
            this.Acceleration = acceleration;
            this.Jump = jump;
        }

        public Player() { }


        private void UpdateScroll() 
        {
            if (Velosity.X < 0 && Position.X > 0)
                ScrollWorld(IsTouchingLeftBorder());
            else if (Velosity.X > 0 && Position.X < widthWindow - Width)
                ScrollWorld(IsTouchingRightBorder());
        }

        private bool IsTouchingLeftBorder() => World.PositionX > 0 && Position.X < World.LiteralBorder;

        private bool IsTouchingRightBorder()
        {
            var rightBorder = widthWindow - World.LiteralBorder - Width;
            return World.PositionX < World.Width - World.WindowWidth && Position.X + Velosity.Y >= rightBorder;
        }

        private void ScrollWorld(bool touchBorder)
        {
            if (touchBorder)
            {
                World.Scroll(Velosity.X);
            }
            else
                World.NoScroll();
        }

        public void LimitWorld()
        {
            if (Position.X + Velosity.X <= 0 && Velosity.X != 0)
            {
                Position = new Vector2(0, Position.Y);
                Velosity = new Vector2(0, Velosity.Y);
            }
            else if (Position.X + Velosity.X >= widthWindow - Width && Velosity.X != 0)
            {
                Position = new Vector2(widthWindow - Width, Position.Y);
                Velosity = new Vector2(0, Velosity.Y);
            }
        }

        private void GetLamp()
        {
            var rectPlayer = new Rectangle((int)(Position.X), (int)(Position.Y), Width, Height);
            foreach (var lamp in World.Lamps)
            {
                var rectLamp = new Rectangle((int)(lamp.Position.X - World.PositionX), (int)(lamp.Position.Y),
                lamp.Width, lamp.Height);
                if (rectPlayer.Intersects(rectLamp))
                {
                    if (lamp.IsClicked)
                    {
                        lamp.Hover();
                        continue;
                    }
                    Lamps += 1;
                    World.Lamps.Remove(lamp);
                    break;
                }
                else if(lamp.IsClicked && lamp.IsHover()) 
                    lamp.NotHover();
            }
        }

        public void ChangeStateAI() => IsActiveAI = !IsActiveAI;

        public void NotActiveAi() => IsActiveAI = false;

        public void ActiveHit() => IsHit = true;

        public bool IntersectionWithThing(Thing thing)
        {
            var rectPlayer = new Rectangle((int)(Position.X), (int)(Position.Y), Width, Height);
            var rectThing = new Rectangle((int)(thing.Position.X - World.PositionX), (int)(thing.Position.Y),
                thing.Width, thing.Height);
            return rectPlayer.Intersects(rectThing);
        }

        private Lamp GetLampWithMinDist() =>
            World.Lamps.Where(lamp => !lamp.IsClicked)
            .OrderBy(x => Vector2.Distance(Position, x.Position))
            .FirstOrDefault();

        public void Update(GameTime gameTime, List<Direction> dirs = null)
        {
            if(dirs != null)
            {
                if (IsActiveAI)
                {
                    var lamp = GetLampWithMinDist();
                    if (lamp != null)
                        dirs = ai.GetDirectionsTo(World.NowLevel.GetMap(), this, lamp, World.Monsters);
                }
                    
                UpdateFrame(gameTime);
                UpdateVelosity(dirs);
                UpdateScroll();
                LimitWorld();
                Position += Velosity;
                GetLamp();
            }
            else
            {
                UpdateFrame(gameTime);
                if(World.Lamps.Count > 0 && IsActiveAI)
                    dirs = ai.GetDirectionsTo(World.NowLevel.GetMap(), this, GetLampWithMinDist(), World.Monsters);
                else
                    dirs = new List<Direction>();
                UpdateVelosity(dirs);
                Position += Velosity;
            }
        }
    }
}
