using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TimeWarpAdventures.Classes;

namespace TimeWarpAdventures.Models;
public class GameState
{
    public static World world = new World();

    private static bool pause = true;

    public GameState(ContentManager content) 
    {
        LoadContent(content);
    }

    public void LoadContent(ContentManager content)
    {
        MainMenu.Font = content.Load<SpriteFont>("Font");
        world.LoadContent();

        var player1 = new Player(content.Load<Texture2D>("Player"), 1000, 10, 2, 30);
        var player2 = new Player(content.Load<Texture2D>("Player"), 700, 10, 2, 30);
        Ground.BackGround = content.Load<Texture2D>("Ground");

        var town = new Town(100, 1, content.Load<Texture2D>("Ellipse"));
        var monster = new Monster(content.Load<Texture2D>("SmallMonster"), 0, 500, 10, 10);

        world.Players.Add(player1);
        world.Players.Add(player2);
        world.NowPlayer = player1;

        world.Towns.Add(town);
        world.Monsters.Add(monster);
    }

    public static void StartGame()
    {
        if (world.Players.Count > 0) pause = false;
    }

    public static void StopGame() => pause = true;

    public static bool IsPause() => pause;

    public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch)
    {
        if (IsPause())
            MainMenu.Draw(_spriteBatch);
        else
            world.Draw(_spriteBatch);
    }
}
