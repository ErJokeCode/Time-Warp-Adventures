using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using TimeWarpAdventures.Classes;
using Microsoft.Xna.Framework.Content;

namespace TimeWarpAdventures.Models;
public class Lamp : Thing
{
    public string NameGrayTextrure { get; set; }
    private Texture2D background;
    private Texture2D bgNotAvailable;
    private Texture2D nowBackground;
    public bool IsClicked { get; set; }
    private bool isHover = false;
    public bool IsPressed { get; set; } = false;

    public Lamp() { }

    public Lamp(Vector2 position, Texture2D backgroung, Texture2D bgNotAvailable = null, bool isClicked = false, bool isPressed = false) 
        : base(backgroung, position)
    {
        this.background = backgroung;
        this.bgNotAvailable = bgNotAvailable;
        this.nowBackground = !isClicked && !isPressed ? background : bgNotAvailable;
        Position = new Vector2(position.X, Ground.Top - background.Height - position.Y);
        Width = background.Width;
        Height = background.Height;
        NameTexture = background.Name;
        NameGrayTextrure = bgNotAvailable != null ? bgNotAvailable.Name : background.Name;
        IsClicked = isClicked;
    }

    public new void AddBackGround(ContentManager content)
    {
        background = content.Load<Texture2D>(NameTexture);
        bgNotAvailable = content.Load<Texture2D>(NameGrayTextrure);
        nowBackground = !IsClicked && !IsPressed ? background : bgNotAvailable;
        Width = background.Width;
        Height = background.Height;
    }

    public Texture2D GetBackGround() => !IsClicked ? nowBackground : IsPressed ? background : bgNotAvailable;

    public void Hover() => isHover = true;

    public void NotHover() => isHover = false;

    public bool IsHover() => isHover;

    public void ChangeState() => IsPressed = !IsPressed;
}
