using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using TimeWarpAdventures.Classes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TimeWarpAdventures.Models;
public class Box: Thing
{
    public Box() { }

    public Box(Vector2 position, Texture2D backgroung)
        :base(backgroung, position)
    {
        Position = new Vector2(position.X, Ground.Top - backgroung.Height - position.Y);
    }
}
