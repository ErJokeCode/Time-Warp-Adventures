using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TimeWarpAdventures.Classes;
using TimeWarpAdventures.Models;
using TimeWarpAdventures.View;

namespace TimeWarpAdventures.WorkWithData;
public class LoaderContent
{
    private ContentManager Content;
    private GraphicsDeviceManager _graphics;

    public LoaderContent(ContentManager content, GraphicsDeviceManager _graphics)
    {
        Content = content;
        this._graphics = _graphics;
    }

    private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
    
    public void LoadMenu()
    {
        MainMenu.Font = Content.Load<SpriteFont>("Font");
        MainMenu.BackGround = Content.Load<Texture2D>("Menu");
    }

    public void LoadWorld()
    {
        World.FontUse = Content.Load<SpriteFont>("FontForUseThing");
        LoadTexture();

        World.WindowWidth = _graphics.PreferredBackBufferWidth;
        World.WindowHeight = _graphics.PreferredBackBufferHeight;
        World.Width = 3000;
        World.LiteralBorder = World.WindowWidth / 10;
        World.BarPlayer = new BarPlayer();
        World.Loader = this;
        World.BackGround = textures["World"];

        LoadHealthBar();
        LoadGround();
    }

    private void LoadTexture()
    {
        textures.Add("World", Content.Load<Texture2D>("World"));
        textures.Add("Lamp", Content.Load<Texture2D>("Lamp"));
        textures.Add("NotActiveLamp", Content.Load<Texture2D>("NotActiveLamp"));        
        textures.Add("MainPlayer", Content.Load<Texture2D>("PlayerPeople"));
        textures.Add("Mag", Content.Load<Texture2D>("PlayerMag"));
        textures.Add("Small", Content.Load<Texture2D>("Player2"));
        textures.Add("Monster", Content.Load<Texture2D>("MonsterRun"));
        textures.Add("House1", Content.Load<Texture2D>("House"));
        textures.Add("House2", Content.Load<Texture2D>("House3"));
        textures.Add("Box", Content.Load<Texture2D>("box"));
        textures.Add("Ground", Content.Load<Texture2D>("Ground"));
        textures.Add("Door", Content.Load<Texture2D>("Door"));
    }

    private void LoadHealthBar()
    {
        var barPlayer = World.BarPlayer;
        var data = new Color[] { Color.Red };
        barPlayer.ColorHealth = new Texture2D(_graphics.GraphicsDevice, 1, 1);
        barPlayer.ColorHealth.SetData(data);
        barPlayer.TextureLamp = textures["Lamp"];

        data = new Color[] { Color.Gray };
        barPlayer.ColorDied = new Texture2D(_graphics.GraphicsDevice, 1, 1);
        barPlayer.ColorDied.SetData(data);
    }

    private void LoadGround()
    {
        Ground.Gravity = new Vector2(0, 2);
        Ground.HeightGround = World.WindowHeight / 12;
        Ground.BackGround = textures["Ground"];
        Ground.Initialize();
    }

    public void LoadFromHistory()
    {
        World.BackGround = Content.Load<Texture2D>(World.NowLevel.NameBackground);
        Ground.BackGround = Content.Load<Texture2D>(World.NowLevel.NameTextureGround);
        
        foreach (var player in World.Players)
            player.AddBackGround(Content);

        World.NowPlayer.AddBackGround(Content);

        foreach (var level in World.DictLevels.Values)
        {
            level.AddBackGround(Content);

            foreach (var monster in level.Monsters)
                monster.AddBackGround(Content);

            foreach (var box in level.Boxes)
                box.AddBackGround(Content);

            foreach (var lamp in level.Lamps)
                lamp.AddBackGround(Content);

            foreach (var thing in level.Things)
                thing.AddBackGround(Content);
        }   
    }

    public void AddItemsInWorld()
    {
        var players = new List<Player>
        {
            new Player(textures["MainPlayer"], 8, 4, 300, 10, 2, 30),
        };

        World.Players = players;
        World.NowPlayer = players[0];

        var levels = new Dictionary<string, Level>();

        var mainWorld = CreateLevelWorld();
        var level1 = CreateLevel1();
        var level2 = CreateLevel2();

        var links = new List<ItemLink>();
        links.Add(new ItemLink(mainWorld.Name, new List<string>() { level1.Name, level2.Name },
            new List<int> { (int)level1.Position.X, (int)level2.Position.X }));
        links.Add(new ItemLink(level1.Name, new List<string>() { mainWorld.Name }, new List<int> { 100 }));
        links.Add(new ItemLink(level2.Name, new List<string>() { mainWorld.Name }, new List<int> { 100 }));
        World.Links = links;

        levels.Add(mainWorld.Name, mainWorld);
        levels.Add(level1.Name, level1);
        levels.Add(level2.Name, level2);

        World.NowLevel = mainWorld;
        World.DictLevels = levels;
    }

    private Level CreateLevelWorld()
    {
        var level = new Level(textures["Door"], 800, "World", "Ground");
        level.Name = "MainLevel";
        var monsters = new List<Monster>();

        level.Monsters = monsters;
        return level;
    }

    private Level CreateLevel1()
    {
        var monsters = new List<Monster>
        {
            new Monster(textures["Monster"], 6, 4, 2500, 500, 100, 10),
            new Monster(textures["Monster"], 6, 4, 2600, 500, 100, 10)
        };

        var boxes = new List<Box>
        {
            new Box(new Vector2(500, 0), textures["Box"]),
            new Box(new Vector2(600, 0), textures["Box"]),
            new Box(new Vector2(600, 100), textures["Box"]),
            new Box(new Vector2(600, 200), textures["Box"]),

            new Box(new Vector2(1000, 0), textures["Box"]),
            new Box(new Vector2(1100, 0), textures["Box"]),
            new Box(new Vector2(1200, 0), textures["Box"]),
            new Box(new Vector2(1100, 100), textures["Box"]),
            new Box(new Vector2(1200, 100), textures["Box"]),
            new Box(new Vector2(1200, 200), textures["Box"])
        };

        var lamps = new List<Lamp>
        {
            new Lamp(new Vector2(300, 0), textures["Lamp"]),
            new Lamp(new Vector2(600, 0), textures["Lamp"]),
            new Lamp(new Vector2(1200, 300), textures["Lamp"], textures["NotActiveLamp"], true),
            new Lamp(new Vector2(600, 300), textures["Lamp"], textures["NotActiveLamp"], true),
            new Lamp(new Vector2(500, 0), textures["Lamp"])
        };

        var things = new List<Thing>();
        things.Add(new Thing(textures["Mag"], new Vector2(1500, Ground.Top - textures["Mag"].Height/4), "Player", 8, 4));

        var level = new Level(textures["House1"], 20, "Level1", "Stone", monsters, boxes, lamps, things);
        level.Name = "Level1";
        return level;
    }

    private Level CreateLevel2()
    {
        var monsters = new List<Monster>
        {
            new Monster(textures["Monster"], 6, 4, 2500, 500, 100, 10),
            new Monster(textures["Monster"], 6, 4, 2600, 500, 100, 10),
            new Monster(textures["Monster"], 6, 4, 600, 500, 100, 10),
            new Monster(textures["Monster"], 6, 4, 1200, 500, 100, 10)
        };

        var boxes = new List<Box>
        {
            new Box(new Vector2(500, 0), textures["Box"]),
            new Box(new Vector2(600, 0), textures["Box"]),
            new Box(new Vector2(600, 100), textures["Box"]),
            new Box(new Vector2(600, 200), textures["Box"]),

            new Box(new Vector2(1000, 0), textures["Box"]),
            new Box(new Vector2(1100, 0), textures["Box"]),
            new Box(new Vector2(1200, 0), textures["Box"]),
            new Box(new Vector2(1100, 100), textures["Box"]),

            new Box(new Vector2(1800, 0), textures["Box"]),
            new Box(new Vector2(1900, 0), textures["Box"]),
            new Box(new Vector2(2000, 0), textures["Box"]),
            new Box(new Vector2(2100, 0), textures["Box"]),
            new Box(new Vector2(1800, 100), textures["Box"]),
            new Box(new Vector2(1900, 100), textures["Box"]),
            new Box(new Vector2(2000, 100), textures["Box"]),
            new Box(new Vector2(2000, 200), textures["Box"])
        };

        var lamps = new List<Lamp>
        {
            new Lamp(new Vector2(1000, 0), textures["Lamp"]),
            new Lamp(new Vector2(700, 0), textures["Lamp"]),
            new Lamp(new Vector2(1200, 300), textures["Lamp"], textures["NotActiveLamp"], true),
            new Lamp(new Vector2(600, 300), textures["Lamp"], textures["NotActiveLamp"], true),
            new Lamp(new Vector2(1000, 300), textures["Lamp"], textures["NotActiveLamp"], true),
            new Lamp(new Vector2(2000, 300), textures["Lamp"], textures["NotActiveLamp"], true),
            new Lamp(new Vector2(500, 0), textures["Lamp"]),
            new Lamp(new Vector2(2000, 0), textures["Lamp"]),
            new Lamp(new Vector2(2700, 0), textures["Lamp"]),
            new Lamp(new Vector2(2300, 200), textures["Lamp"]),
            new Lamp(new Vector2(1700, 0), textures["Lamp"]),
        };

        var things = new List<Thing>();
        things.Add(new Thing(textures["Mag"], new Vector2(800, Ground.Top - textures["Mag"].Height / 4), "Player", 8, 4));

        var level = new Level(textures["House2"], 900, "Level1", "Stone", monsters, boxes, lamps, things);
        level.Name = "Level2";
        return level;
    }

    public void LoadLevel(Level level)
    {
        var back = level.NameBackground;
        var ground = level.NameTextureGround;

        World.BackGround = Content.Load<Texture2D>(back);
        World.NowLevel = level;
        foreach(var (name, pos) in World.GetItemLinkFromLinks(level).GetDict())
        {
            var levelIn = World.DictLevels[name];
            levelIn.Position = new Vector2(pos, levelIn.Position.Y);
        }
        Ground.BackGround = Content.Load<Texture2D>(ground);
        World.PositionX = 0;
    }

    public void AddThing(Thing thing)
    {
        if(thing.Name == "Player")
        {
            var player = new Player(thing.GetTexture(), thing.CountFrameWidth, thing.CountFrameHeight, 
                (int)(thing.Position.X - World.PositionX), 10, 2, 30);
            World.Players.Add(player);
            World.NowPlayer = player;
        }      
    }
}

