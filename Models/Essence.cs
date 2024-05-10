using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TimeWarpAdventures.Classes;
using static TimeWarpAdventures.Game1;

namespace TimeWarpAdventures.Models;
public class Essence
{
    private Vector2 position;
    public Vector2 Position
    {
        get { return position; }
        set { position = value; }
    }

    private Vector2 velosity;
    public Vector2 Velosity 
    { 
        get { return velosity; } 
        set { velosity = value; } 
    }

    public int Width { get; set; }
    public int Height { get; set; }

    public int MaxVelosity { get; set; }
    public int Acceleration { get; set; }
    public int Jump { get; set; }

    public void UpdateVelosity(List<Direction> dirs)
    {
        var cortrect = GetSpeed(dirs);
        var friction = new Vector2(0.9f, 1);
        var touch = IsTouchingGround(position + Velosity);
        if (touch.Item1)
        {
            if (Velosity.Y > 0)
            {
                cortrect.Y = -velosity.Y;
            }

            if (position.Y + Height - touch.Item2 > 3)
                position.Y = touch.Item2 - Height;

            Velosity = Velosity * friction;
        }
        else
            cortrect = new Vector2(0, Ground.Gravity.Y);

        Velosity += cortrect;
        Velosity = GetTrueSpeed(Velosity);
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

    private (bool, int) IsTouchingGround(Vector2 newPos) => Ground.IsTouch(this, newPos);

    private Vector2 GetTrueSpeed(Vector2 vect)
    {
        vect.Y = vect.Y < -Jump ? -Jump : vect.Y;
        vect.X = vect.X > MaxVelosity ? MaxVelosity : vect.X;
        vect.X = vect.X < -MaxVelosity ? -MaxVelosity : vect.X;

        return vect;
    }
}

