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
using TimeWarpAdventures.WorkWithData;
using System.Linq;

namespace TimeWarpAdventures.Contriller;
internal class ControllerWorld
{
    private static bool isHoldingTab = false;
    private static bool isHoldingE = false;
    private static bool isHoldingR = false;

    private const Keys keyUp = Keys.W;
    private const Keys keyDown = Keys.S;
    private const Keys keyLeft = Keys.A;
    private const Keys keyRight = Keys.D;
    private const Keys keySave1 = Keys.S;
    private const Keys keySave2 = Keys.LeftControl;
    private const Keys keyUse = Keys.E;
    private const Keys keyChangePlayer = Keys.Tab;
    private const Keys keyActiveAI = Keys.R;
    private const Keys keyHit = Keys.Space;

    public static void UpdateWorld(GameManagerDate _gameManager, GameTime gameTime)
    {
        var directs = GetDerects();

        if (!World.IsPause())
            World.Update(directs, gameTime);

        if (Keyboard.GetState().IsKeyDown(keySave1) && Keyboard.GetState().IsKeyDown(keySave2))
            _gameManager.SaveGameState();

        if (Keyboard.GetState().IsKeyDown(keyHit))
            World.NowPlayer.ActiveHit();

        UseButton(World.NowPlayer.ChangeStateAI, keyActiveAI, ref isHoldingR);

        UseButton(World.UseThing, keyUse, ref isHoldingE);
    }

    private static void UseButton(Action func, Keys key , ref bool flag)
    {
        if (Keyboard.GetState().IsKeyDown(key) && !flag)
        {
            func();
            flag = true;
        }
        else if (Keyboard.GetState().IsKeyUp(key) && flag)
            flag = false;
    }

    public static void ChangePlayer()
    {
        if (Keyboard.GetState().IsKeyDown(keyChangePlayer) && !isHoldingTab)
        {
            isHoldingTab = true;
            World.NewPlayer();
        }
        else if (Keyboard.GetState().IsKeyUp(keyChangePlayer) && isHoldingTab)
            isHoldingTab = false;
    }

    private static System.Collections.Generic.List<Direction> GetDerects()
    {
        var directs = new System.Collections.Generic.List<Direction>();

        if (Keyboard.GetState().IsKeyDown(keyRight))
            directs.Add(Direction.Right);

        if (Keyboard.GetState().IsKeyDown(keyLeft))
            directs.Add(Direction.Left);

        if (Keyboard.GetState().IsKeyDown(keyUp))
            directs.Add(Direction.Up);


        return directs;
    }
}

