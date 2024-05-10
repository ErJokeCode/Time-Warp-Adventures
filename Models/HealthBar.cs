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
public class HealthBar
{
    private Texture2D colorHealth;
    private Texture2D colorDied;

    public void AddTexture(GraphicsDevice graphicsDevice)
    {
        var data = new Color[] { Color.Red };
        colorHealth = new Texture2D(graphicsDevice, 1, 1);
        colorHealth.SetData(data);

        data = new Color[] { Color.Gray };
        colorDied = new Texture2D(graphicsDevice, 1, 1);
        colorDied.SetData(data);
    }

    public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch)
    {
        _spriteBatch.Draw(colorDied, new Rectangle(50, 50, 100, 50), Color.White);
        _spriteBatch.Draw(colorHealth, new Rectangle(50, 50, World.NowPlayer.Health, 50), Color.White);
    }
}
