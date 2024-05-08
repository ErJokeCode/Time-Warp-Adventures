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

    public Microsoft.Xna.Framework.Vector2 Position {  get; set; }

    public GameState()
    {
        Position = new Vector2(1000, 0);
    }
}
