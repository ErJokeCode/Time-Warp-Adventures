using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;
using TimeWarpAdventures.Classes;
using System;
using static TimeWarpAdventures.Game1;
using SharpDX.Direct3D9;
using SharpDX.Mathematics.Interop;
using TimeWarpAdventures.Models;

namespace TimeWarpAdventures.Contriller;
internal class ControllerWorld
{
    private static bool isHoldingTab = false;

    public static void UpdateWorld()
    {
        var directs = GetDerects();

        if (!GameState.IsPause())
        {
            GameState.world.Update(directs);
        }
    }

    public static void ChangePlayer()
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Tab) && !isHoldingTab)
        {
            isHoldingTab = true;
            GameState.world.NewPlayer();
        }
        else if (Keyboard.GetState().IsKeyUp(Keys.Tab) && isHoldingTab)
            isHoldingTab = false;
    }

    private static System.Collections.Generic.List<Direction> GetDerects()
    {
        var directs = new System.Collections.Generic.List<Direction>();

        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            directs.Add(Direction.Right);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            directs.Add(Direction.Left);
        }
        if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            directs.Add(Direction.Up);
        }

        return directs;
    }
}

