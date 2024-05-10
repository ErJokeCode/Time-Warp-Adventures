using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TimeWarpAdventures.Classes;

namespace TimeWarpAdventures.Models;
public class Soil
{
    public Texture2D BackGround { get; set; }

    public int Top { get; set; }

    public int Width;
    public int Height;

    private Rectangle rect;
    private Vector2 position;

    private Color color;

    public Soil(float position, int height)
    {
        Width = 100;
        Height = 100;
        Top = World.WindowHeight - height;
        rect = new Rectangle(0, 0, Width, height);
        this.position = new Vector2(position, Top);
        color = Color.White;
    }

    public float GetRightPosition() => position.X + Width;
    public float GetLeftPosition() => position.X;

    public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch)
    {
        _spriteBatch.Draw(BackGround, position, rect, color);
    }

    public void Update()
    {
        position = new Vector2(position.X - World.VelisityScroll, Top);
        //rect = new Rectangle((int)World.PositionX % lenOneSprite, 0, widthWindow / 12, heightGround);
    }
}
