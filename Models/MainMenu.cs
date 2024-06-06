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
using TimeWarpAdventures.WorkWithData;

namespace TimeWarpAdventures.Classes
{
    class MenuItem
    {
        public int Index { get; }
        public Vector2 Position { get; }
        public string Name { get; }

        public MenuItem(int index, Vector2 position, string name)
        {
            Index = index;
            Position = position;
            Name = name;
        }
    }

    static class MainMenu
    {
        public static SpriteFont Font { get; set; }
        public static Texture2D BackGround {  get; set; }

        private static string[] menuItems = { "Play", "Save", "To last save", "Exit" };

        public static int SelectedIndex { get; set; } = 0;

        public static void СhangeSelectedIndex(int index) => SelectedIndex = index;

        public static IEnumerable<MenuItem> GetMenuItems()
        {
            for (int i = 0; i < menuItems.Length; i++)
                yield return new MenuItem(i, new Vector2(100, 200 + i * 70), menuItems[i]);
        }

        private static int? GetSelectedIndex(Vector2 mousePosition)
        {
            foreach (var menuItem in GetMenuItems())
            {
                var position = menuItem.Position;
                var index = menuItem.Index;

                if (new Rectangle((int)position.X, (int)position.Y, (int)Font.MeasureString(menuItems[index]).X, 
                    (int)Font.MeasureString(menuItems[index]).Y).Contains(mousePosition))
                    return index;
            }

            return null;
        }

        public static void Update(int navigate, Vector2 mousePosition)
        {
            SelectedIndex = (SelectedIndex + navigate) % menuItems.Length;
            if (SelectedIndex < 0)
                SelectedIndex = menuItems.Length - 1;

            var nowSelectedIndex = GetSelectedIndex(mousePosition);

            if (nowSelectedIndex != null)
                SelectedIndex = (int)nowSelectedIndex;
        }
    }
}
