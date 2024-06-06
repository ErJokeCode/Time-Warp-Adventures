using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeWarpAdventures.Classes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace TimeWarpAdventures.Models;
public class Thing
{
    public string Name { get; set; } = "";
    public int CountFrameWidth;
    public int CountFrameHeight;
    public Vector2 Position { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string NameTexture { get; set; }
    private bool canUse { get; set; } = false;
    private bool used = false;

    private Texture2D backGround;

    public Thing() { }

    public Thing(Texture2D texture, Vector2 position, string name = "", int countFrameWidth = 1, int countFrameHeight = 1) 
    { 
        Position = position;
        backGround = texture;
        CountFrameWidth = countFrameWidth;
        CountFrameHeight = countFrameHeight;
        Width = backGround.Width / countFrameWidth;
        Height = backGround.Height / countFrameHeight;
        NameTexture = texture.Name;
        Name = name;

    }

    public void AddBackGround(ContentManager content)
    {
        backGround = content.Load<Texture2D>(NameTexture);
        Width = backGround.Width / CountFrameWidth;
        Height = backGround.Height / CountFrameHeight;
    }

    public bool IsCanUse() => canUse;

    public void SetCanUse(bool canUse) => this.canUse = canUse;

    public void SetUsed(bool used) => this.used = used;

    public bool IsUsed() => used;

    public Texture2D GetTexture() => backGround;

    public Rectangle GetRect() => new Rectangle(0, 0, Width, Height);

    public void Update()
    {
        var isClikedLamp = World.NowLevel.Lamps.Where(lamp => lamp.IsClicked).Count();
        var isPressedLamp = World.NowLevel.Lamps.Where(lamp => lamp.IsClicked)
            .Where(lamp => lamp.IsPressed).Count();
        if (isClikedLamp == isPressedLamp)
        {
            canUse = true;
            if(World.NowPlayer.IntersectionWithThing(this))
                used = true;
            else
                used = false;
        }    
        else
            canUse = false;
    }
}
