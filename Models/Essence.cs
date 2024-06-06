using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TimeWarpAdventures.Classes;
using static TimeWarpAdventures.Game1;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics.Eventing.Reader;

namespace TimeWarpAdventures.Models;
public class Essence
{
    public Essence() { }

    public Essence(Texture2D backGround, int countFrameWidth, int countFrameHeight, Vector2 position, int maxVelosity, int health = 100, int hit = 5) 
    {
        MaxVelosity = maxVelosity;
        this.backGround = backGround;
        PeriondUpdateFrame = 1000 / MaxVelosity;
        this.CountFrameWidth = countFrameWidth;
        this.CountFrameHeight = countFrameHeight;
        NameTexture = backGround.Name;
        Width = backGround.Width / countFrameWidth;
        Height = backGround.Height / countFrameHeight;
        Position = new Vector2(position.X, Ground.Top - backGround.Height);
        Health = health;
        Hit = hit;
    }

    private Vector2 position;
    public Vector2 Position
    {
        get { return position; }
        set { position = value; }
    }

    public Vector2 Velosity { get; set; }

    public int Width { get; set; }
    public int Height { get; set; }

    public int MaxVelosity { get; set; }
    public int Acceleration { get; set; }
    public int Jump { get; set; }
    public int Hit { get; set; }
    public int Health { get; set; }
    public bool IsHit { get; set; }

    private Texture2D backGround;

    public string NameTexture { get; set; }

    private Point currentFrame = new Point();
    public int CountFrameWidth;
    public int CountFrameHeight;
    private int currentTime = 0;
    public int PeriondUpdateFrame;

    public Vector2 Friction { get; set; } = new Vector2(0.9f, 1);

    public void AddBackGround(ContentManager content)
    {
        backGround = content.Load<Texture2D>(NameTexture);
        Width = backGround.Width / CountFrameWidth;
        Height = backGround.Height / CountFrameHeight;
    }

    public Texture2D GetTexture() => backGround;

    public void UpdateVelosity(List<Direction> dirs)
    {
        Velosity = GetVelosity(dirs, Velosity);
        Velosity = GetTrueSpeed(Velosity);

        if (IsHit && Velosity.X < 0) currentFrame.Y = 3;
        else if (IsHit && Velosity.X > 0) currentFrame.Y = 2;
        else if (IsHit && Math.Abs(Velosity.X) < 1) 
            currentFrame.Y = currentFrame.Y == 0 ? 2 : 3;
        else
        {
            if (Math.Abs(Velosity.X) < 1)
                currentFrame.X = 0;

            if (Velosity.X < 0)
                currentFrame.Y = 0;
            else if(Velosity.X > 0)
                currentFrame.Y = 1;
            else currentFrame.Y = currentFrame.Y == 2 ? 1 : 0;
        }
    }

    private Vector2 GetSpeed(List<Direction> dirs)
    {
        var velocCorect = new Vector2(0, 0);
        foreach (var dir in dirs)
        {
            if (dir == Direction.Left)
                velocCorect.X -= Acceleration;
            if (dir == Direction.Right)
                velocCorect.X += Acceleration;
            if (dir == Direction.Up)
                velocCorect.Y -= Jump;
            if (dir == Direction.Down)
                velocCorect.Y += Acceleration;
        }
        return velocCorect;
    }

    public Vector2 GetVelosity(List<Direction> dirs, Vector2 velosity)
    {
        var correct = GetSpeed(dirs);
        Vector2 newVel;
        var isTouchBox = IsTouchBox(velosity);
        if (Ground.IsTouch(this))
        {
            if (velosity.Y > 0)
                correct.Y = -velosity.Y;

            newVel = (velosity + correct) * Friction;
        }
        else
            newVel = velosity + new Vector2(0, Ground.Gravity.Y);

        if (isTouchBox)
            newVel = GetVelosityTouchBox(velosity, correct);
        
        
        return newVel;
    }

    private Vector2 GetVelosityTouchBox(Vector2 velosity, Vector2 correct)
    {
        if (velosity.Y > 0)
            correct.Y = -velosity.Y;
        else if (!Ground.IsTouch(this) && velosity.Y < 0)
            correct.Y = Ground.Gravity.Y;

        return (velosity + correct)*Friction;
    }

    private Vector2 GetTrueSpeed(Vector2 vect)
    {
        vect.Y = vect.Y < -Jump ? -Jump : vect.Y;
        vect.X = vect.X > MaxVelosity ? MaxVelosity : vect.X;
        vect.X = vect.X < -MaxVelosity ? -MaxVelosity : vect.X;

        return vect;
    }

    private bool IsTouchBox(Vector2 vel)
    {
        var boxEssense = new Rectangle((int)(Position.X + vel.X), (int)(Position.Y + vel.Y), Width, Height);
        var errorHeight = 20;

        foreach (var box in World.Boxes)
        {
            var boxBox = new Rectangle((int)(box.Position.X - World.PositionX), (int)box.Position.Y, box.Width, box.Height);
            if(boxEssense.Intersects(boxBox) && box.Position.Y > Position.Y + Height - errorHeight)
                return true;
        }
        return false;
    }

    public void Kick(Essence essense)
    {
        Velosity -= new Vector2(Velosity.X - 100 * essense.Velosity.X, 500);
        Health -= essense.Hit;
    }

    public Rectangle GetRectSprite()
    {
        return IsHit ? new Rectangle(Width * 2 * currentFrame.X, Height * currentFrame.Y, Width * 2, Height) 
            : new Rectangle(Width * currentFrame.X, Height * currentFrame.Y, Width, Height);
    }

    public void UpdateFrame(GameTime gameTime)
    {
        currentTime += gameTime.ElapsedGameTime.Milliseconds;
        if(IsHit) { currentFrame.X %= (CountFrameWidth / 2); }
        if (IsHit && currentTime > PeriondUpdateFrame)
        {
            currentFrame.X = (currentFrame.X + 1) % (CountFrameWidth / 2);
            currentTime = 0;
            if(currentFrame.X == (CountFrameWidth / 2) - 1 )
                IsHit = false;
        }
        else if (!IsHit && currentTime > PeriondUpdateFrame)
        {
            currentFrame.X = (currentFrame.X + 1) % CountFrameWidth;
            currentTime = 0;
        }
        
    }
}

