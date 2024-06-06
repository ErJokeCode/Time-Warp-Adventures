using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;
using TimeWarpAdventures.Classes;
using System;
using static TimeWarpAdventures.Game1;
using SharpDX.Direct3D9;
using SharpDX.Mathematics.Interop;
using TimeWarpAdventures.WorkWithData;

namespace TimeWarpAdventures.Contriller;

class ControllerMenu
{
    private static bool isHoldingInMenu = false;

    private const Keys keyPause = Keys.Escape;
    private const Keys keyUp = Keys.W;
    private const Keys keyDown = Keys.S;
    private const Keys keyEnter = Keys.Enter;

    public static void UpdateMenu(GameManagerDate _gameManager, Game game)
    {
        var navigate = GetNavigating();

        if (World.IsPause())
        {
            var mouseState = Mouse.GetState();
            var mousePosition = new Vector2(mouseState.X, mouseState.Y);
            MainMenu.Update(navigate, mousePosition);
            if (mouseState.LeftButton == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(keyEnter))
                HandleMenuSelection(_gameManager, game);
        }


        if (Keyboard.GetState().IsKeyDown(keyPause))
            World.StopGame();
    }

    private static int GetNavigating()
    {
        if (Keyboard.GetState().IsKeyDown(keyUp) && !isHoldingInMenu)
        {
            isHoldingInMenu = true;
            return -1;
        }

        else if (Keyboard.GetState().IsKeyDown(keyDown) && !isHoldingInMenu)
        {
            isHoldingInMenu = true;
            return 1;
        }
        else if (Keyboard.GetState().IsKeyUp(keyUp) && Keyboard.GetState().IsKeyUp(keyDown) && isHoldingInMenu)
            isHoldingInMenu = false;
        return 0;
    }

    private static void HandleMenuSelection(GameManagerDate _gameManager, Game game)
    {
        switch (MainMenu.SelectedIndex)
        {
            case 0:
                World.StartGame();
                break;
            case 1:
                _gameManager.SaveGameState();
                break;
            case 2:
                _gameManager.RestoreDate();
                World.StartGame();
                break;
            case 3:
                game.Exit();
                break;
        }
    }
}

