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

static class Controller
{
    public static void Update(GameManagerDate _gameManager, Game game, GameTime gameTime)
    {
        ControllerMenu.UpdateMenu(_gameManager, game);

        ControllerWorld.UpdateWorld(_gameManager, gameTime);

        ControllerWorld.ChangePlayer();
    }
}

