﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;
using TimeWarpAdventures.Classes;
using System;
using static TimeWarpAdventures.Game1;
using SharpDX.Direct3D9;
using SharpDX.Mathematics.Interop;

namespace TimeWarpAdventures.Contriller;

static class Controller
{
    public static void Update()
    {
        ControllerMenu.UpdateMenu();

        ControllerWorld.UpdateWorld();

        ControllerWorld.ChangePlayer();

    }


}
