using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;

namespace GroundBattle.Classes
{
    static class MainMenu
    {
        public static SpriteFont Font {  get; set; }
        public static bool IsGameStart { get; set; } = false;
        private static int selectedIndex = 0;
        private static string[] menuItems = { "main", "persons", "start" };
        private static bool isHolding = false;


        public static void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch)
        {
            for (int i = 0; i < menuItems.Length; i++)
            {
                Vector2 menuItemPosition = new Vector2(100, 200 + i * 50);
                Color color = (selectedIndex == i) ? Color.Yellow : Color.White;
                _spriteBatch.DrawString(Font, menuItems[i], menuItemPosition, color);
            }
        }

        public static void Update()
        {
            MouseState mouseState = Mouse.GetState();
            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);

            if (Keyboard.GetState().IsKeyDown(Keys.Up) && selectedIndex > 0 && !isHolding)
            {
                selectedIndex--;
                isHolding = true;
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.Down) && selectedIndex < menuItems.Length - 1 && !isHolding)
            {
                selectedIndex++;
                isHolding = true;
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Up) && Keyboard.GetState().IsKeyUp(Keys.Down) && isHolding)
                isHolding = false;
                

            for (int i = 0; i < menuItems.Length; i++)
            {
                Vector2 menuItemPosition = new Vector2(100, 200 + i * 50);
                if (new Rectangle((int)menuItemPosition.X, (int)menuItemPosition.Y, (int)Font.MeasureString(menuItems[i]).X, (int)Font.MeasureString(menuItems[i]).Y).Contains(mousePosition))
                {
                    selectedIndex = i;
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        HandleMenuSelection();
                    }
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                HandleMenuSelection();
            }
        }

        private static void HandleMenuSelection()
        {
            switch (selectedIndex)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    IsGameStart = true;
                    break;
            }
        }
    }
}
