using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeWarpAdventures.Classes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace TimeWarpAdventures.View;
public static class ViewMenu
{
    private static Color menuColor;
    public static void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch, GraphicsDeviceManager _graphics)
    {
        _spriteBatch.Draw(MainMenu.BackGround, 
            new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), 
            Color.White);

        foreach (var menuItem in MainMenu.GetMenuItems())
        {
            if (MainMenu.SelectedIndex == menuItem.Index)
                menuColor = Color.Yellow;
            else
                menuColor = Color.White;
            _spriteBatch.DrawString(MainMenu.Font, menuItem.Name, menuItem.Position, menuColor);
        }     
    }
}

