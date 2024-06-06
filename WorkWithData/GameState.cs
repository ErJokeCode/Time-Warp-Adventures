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
using TimeWarpAdventures.Models;

namespace TimeWarpAdventures.WorkWithData;
public class GameState
{
    public float Position { get; set; }
    //public int WindowWidth { get { return World.WindowWidth; } }
    //public int WindowHeight { get { return World.WindowHeight; } }
    public int Width { get; set; }
    //public int Border { get { return World.LiteralBorder; } }

    public int NowPlayer { get; set; }
    public List<Player> Players { get; set; }
    public Level NowLevel { get; set; }
    public List<ItemLevel> ListLevel { get; set; }
    public List<ItemLink> Links { get; set; }

    public GameState()
    {
        Position = World.PositionX;
        Width = World.Width;
        NowPlayer = World.GetIndexNowPlayer();
        Players = World.Players;
        NowLevel = World.NowLevel;
        ListLevel = World.ListLevel;
        Links = World.Links;
    }
}
