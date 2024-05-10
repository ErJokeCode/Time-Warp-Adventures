using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;
using System.Security.Cryptography;
using System.Reflection;
using TimeWarpAdventures.Models;

namespace TimeWarpAdventures.Classes
{
    class MenuItem
    {
        public int Index { get; }
        public Vector2 Position { get; }

        public MenuItem(int index, Vector2 position)
        {
            Index = index;
            Position = position;
        }
    }

    static class MainMenu
    {
        public static SpriteFont Font {  get; set; }
        public static string[] MenuItems = { "Start", "SaveGame", "LoadGame" };

        private static int selectedIndex { get; set; } = 0;

        public static void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch)
        {
            foreach (var menuItem in GetMenuItems())
            {
                var color = (selectedIndex == menuItem.Index) ? Color.Yellow : Color.White;
                _spriteBatch.DrawString(Font, MenuItems[menuItem.Index], menuItem.Position, color);
            }
        }

        public static void Update(int navigate, Vector2 mousePosition, GameManagerDate _gameManager, bool isClick = false)
        {
            if (selectedIndex + navigate >= 0 && selectedIndex + navigate < MenuItems.Length)
                selectedIndex += navigate;

            var nowSelectedIndex = GetSelectedIndex(mousePosition);

            if(nowSelectedIndex != -1)
                selectedIndex = nowSelectedIndex;
            if (isClick && nowSelectedIndex != -1)
                HandleMenuSelection(_gameManager);
        }

        public static void HandleMenuSelection(GameManagerDate _gameManager)
        {
            switch (selectedIndex)
            {
                case 0:
                    World.StartGame();
                    break;
                case 1:
                    _gameManager.SaveGameState();
                    break;
                case 2:
                    _gameManager.RestoreDate();
                    break;
            }
        }

        public static void СhangeSelectedIndex(int index) => selectedIndex = index;

        private static IEnumerable<MenuItem> GetMenuItems()
        {
            for (int i = 0; i < MenuItems.Length; i++)
                yield return new MenuItem(i, new Vector2(100, 200 + i * 70));
        }

        private static int GetSelectedIndex(Vector2 mousePosition)
        {
            foreach (var menuItem in GetMenuItems())
            {
                var position = menuItem.Position;
                var index = menuItem.Index;
                if (new Rectangle((int)position.X, (int)position.Y, (int)Font.MeasureString(MenuItems[index]).X, 
                    (int)Font.MeasureString(MenuItems[index]).Y).Contains(mousePosition))
                    return index;
            }

            return -1;
        }
    }
}
