using Microsoft.Xna.Framework;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeWarpAdventures.Classes;
using TimeWarpAdventures.Models;

namespace TimeWarpAdventures.View;
public class View
{
    private Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch;
    private GraphicsDeviceManager _graphics;

    public View(Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch, GraphicsDeviceManager _graphics) 
    {
        this._spriteBatch = _spriteBatch;
        this._graphics = _graphics;
    }

    public void Draw()
    {
        if (World.IsPause())
            ViewMenu.Draw(_spriteBatch, _graphics);
        else
            ViewWorld.Draw(_spriteBatch);
    }
}

