using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TimeWarpAdventures.Classes;

namespace TimeWarpAdventures.Models;
public class GameState
{
    public WorldInfo World {  get; set; }

    public GameState()
    {
        World = new WorldInfo();
    }

    public void LoadTexture(ContentManager Content)
    {
        foreach(var player in World.Players)
        {
            player.AddBackGround(Content);
        }

        foreach(var monster in World.Monstres) 
        {
            monster.AddBackGround(Content);
        }
    }
}
