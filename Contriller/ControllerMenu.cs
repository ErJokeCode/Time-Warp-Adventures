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

    public static void UpdateMenu(GameManagerDate _gameManager)
    {
        var navigate = GetNavigating();

        if (World.IsPause())
        {
            var mouseState = Mouse.GetState();
            var mousePosition = new Vector2(mouseState.X, mouseState.Y);

            if (mouseState.LeftButton == ButtonState.Pressed)
                MainMenu.Update(navigate, mousePosition, _gameManager, true);
            else
                MainMenu.Update(navigate, mousePosition, _gameManager);

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                MainMenu.HandleMenuSelection(_gameManager);
        }


        if (Keyboard.GetState().IsKeyDown(Keys.Pause))
            World.StopGame();
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

