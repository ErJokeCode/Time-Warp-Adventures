using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeWarpAdventures.Classes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace TimeWarpAdventures.Models;
public class BarPlayer
{
    public Texture2D ColorHealth { get; set; }
    public Texture2D ColorDied { get; set; }
    public Texture2D TextureLamp { get; set; }
}
