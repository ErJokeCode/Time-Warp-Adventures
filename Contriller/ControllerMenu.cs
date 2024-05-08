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

class ControllerMenu
{
    private static bool isHoldingInMenu = false;

    public static void UpdateMenu()
    {
        var navigate = GetNavigating();

        if (GameState.IsPause())
        {
            var mouseState = Mouse.GetState();
            var mousePosition = new Vector2(mouseState.X, mouseState.Y);

            if (mouseState.LeftButton == ButtonState.Pressed)
                MainMenu.Update(navigate, mousePosition, true);
            else
                MainMenu.Update(navigate, mousePosition);

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                MainMenu.HandleMenuSelection();
        }


        if (Keyboard.GetState().IsKeyDown(Keys.Pause))
            GameState.StopGame();
    }

    private static int GetNavigating()
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Up) && !isHoldingInMenu)
        {
            isHoldingInMenu = true;
            return -1;
        }

        else if (Keyboard.GetState().IsKeyDown(Keys.Down) && !isHoldingInMenu)
        {
            isHoldingInMenu = true;
            return 1;
        }
        else if (Keyboard.GetState().IsKeyUp(Keys.Up) && Keyboard.GetState().IsKeyUp(Keys.Down) && isHoldingInMenu)
            isHoldingInMenu = false;
        return 0;
    }
}

