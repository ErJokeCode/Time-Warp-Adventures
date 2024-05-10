using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeWarpAdventures.Classes;

namespace TimeWarpAdventures.View;
static class View
{
    public static Microsoft.Xna.Framework.Graphics.SpriteBatch SpriteBatch;

    public static void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch)
    {
        SpriteBatch = _spriteBatch;
        if (World.IsPause())
            MainMenu.Draw(SpriteBatch);
        else
            WorldDraw();
    }

    private static void WorldDraw()
    {
        var monsters = World.Monsters;
        var players = World.Players;
        Ground.Draw(SpriteBatch);

        foreach (var monster in monsters)
            monster.Draw(SpriteBatch);

        foreach (var player in players)
            player.Draw(SpriteBatch);

        World.HealthBar.Draw(SpriteBatch);
    }
}

