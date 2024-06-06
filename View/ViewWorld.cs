using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using TimeWarpAdventures.Classes;
using TimeWarpAdventures.Models;
using SharpDX.Direct2D1;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.XAudio2;
using SharpDX.Mathematics.Interop;
using SharpDX.Direct3D9;
using System.Windows.Forms.VisualStyles;
using System.Net.NetworkInformation;

namespace TimeWarpAdventures.View;
public static class ViewWorld
{
    private static Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch;

    private const string textUse = "Use (E)";
    private const string textOpenLevel = "Open level (E)";
    private static List<string> commands = new List<string>() 
    { 
        "Change of player (Tab)", 
        "Hit (Space)", 
        "Use AI for player (R)",
        "Save (Ctrl + S)"
    };


    private const int maxWidthLamp = 100;

    private static Vector2 positionGround = new Vector2(0, World.WindowHeight - Ground.HeightGround);

    public static void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch)
    {
        spriteBatch = _spriteBatch;

        DrawWorldBackGround();

        DrawGround();

        DrawThings();

        DrawLevels();

        DrawBoxes();

        DrawLamps();

        DrawPlayers();

        DrawMonsters();

        DrawPlayer(World.NowPlayer);

        DrawBarPlayer();

        //DrawMap();
    }

    private static void DrawThings()
    {
        foreach (var thing in World.Things)
        {
            spriteBatch.Draw(thing.GetTexture(),
                new Vector2(thing.Position.X - World.PositionX, thing.Position.Y), thing.GetRect(), Color.White);
            if (thing.IsCanUse() && thing.IsUsed())
            {
                var sizeString = World.FontUse.MeasureString(textUse);
                var newPosString = new Vector2(thing.Position.X - World.PositionX, thing.Position.Y - sizeString.Y);
                spriteBatch.DrawString(World.FontUse, textUse, newPosString, Color.White);
            }
        }
            
    }

    private static void DrawLamps()
    {
        foreach (var lamp in World.Lamps)
        {
            var newPos = new Vector2(lamp.Position.X - World.PositionX, lamp.Position.Y);
            spriteBatch.Draw(lamp.GetBackGround(),
                newPos, Color.White);
            if(lamp.IsClicked && lamp.IsHover())
            {
                var sizeString = World.FontUse.MeasureString(textUse);
                var newPosString = new Vector2(lamp.Position.X - World.PositionX, lamp.Position.Y - sizeString.Y);
                spriteBatch.DrawString(World.FontUse, textUse, newPosString, Color.White);
            }  
        }   
    }

    private static void DrawWorldBackGround()
    {
        spriteBatch.Draw(World.BackGround, new Vector2(0, 0),
            new Rectangle((int)World.PositionX % World.BackGround.Width, 0, World.WindowWidth, World.WindowHeight),
            Color.White);
    }

    private static void DrawGround()
    {
        spriteBatch.Draw(Ground.BackGround,
            positionGround, Ground.GetNewRectForView(),
            Color.White);
    }

    private static void DrawLevels()
    {
        foreach (var link in World.GetItemLinkFromLinks(World.NowLevel).Links)
        {
            var level = World.DictLevels[link];
            spriteBatch.Draw(level.GetTexture(),
                new Vector2(level.Position.X - World.PositionX, level.Position.Y),
            Color.White);

            if (level.IsUsed())
            {
                var sizeString = World.FontUse.MeasureString(textUse);
                var newPosString = new Vector2(World.NowPlayer.Position.X, World.NowPlayer.Position.Y - sizeString.Y) ;
                spriteBatch.DrawString(World.FontUse, textOpenLevel, newPosString, Color.White);
            }

            DrawLampsForLevel(spriteBatch, level);
        }   
    }

    private static void DrawLampsForLevel(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Level level)
    {
        var cntLamps = level.GetAvailableLamps().Count();
        var widthLamp = cntLamps != 0 ? level.Width / cntLamps : 0;
        var correct = 0;
        if (widthLamp > maxWidthLamp)
        {
            widthLamp = maxWidthLamp;
            correct = (level.Width - widthLamp) / (cntLamps + 1);
        }

        var i = 0;
        foreach (var lamp in level.GetAvailableLamps())
        {
            spriteBatch.Draw(lamp.GetBackGround(),
                new Rectangle((int)(level.Position.X + widthLamp * i + correct - World.PositionX), (int)level.Position.Y - widthLamp,
                widthLamp, widthLamp), Color.White);
            i++;
        }
    }

    private static void DrawPlayers()
    {
        foreach (var player in World.Players)
        {
            if (player != World.NowPlayer)
                DrawPlayer(player);
        }
    }

    private static void DrawPlayer(Player player)
    {
        spriteBatch.Draw(player.GetTexture(), player.Position, player.GetRectSprite(), Color.White);
    }

    private static void DrawMonsters()
    {
        foreach (var monster in World.Monsters)
            spriteBatch.Draw(monster.GetTexture(), monster.Position, monster.GetRectSprite() , Color.White);
    }

    public static void DrawBarPlayer()
    {
        spriteBatch.Draw(World.BarPlayer.ColorDied, new Rectangle(30, 30, 400, 25), Color.White);
        spriteBatch.Draw(World.BarPlayer.ColorHealth, new Rectangle(35, 35, World.NowPlayer.Health * 4 - 10, 15), Color.White);
        for(var i = 0; i < World.NowPlayer.Lamps;  i++)
            spriteBatch.Draw(World.BarPlayer.TextureLamp, new Rectangle(30 + 20*i, 70, 20, 20), Color.White);

        for(int i = 0; i < commands.Count(); i++)
        {
            var command = commands[i];
            var sizeString = World.FontUse.MeasureString(command);
            var newPos = new Vector2(World.WindowWidth - 300, 30 + i * sizeString.Y);
            spriteBatch.DrawString(World.FontUse, command, newPos, Color.White);
        }
            
    }

    private static void DrawBoxes()
    {
        foreach(var box in World.Boxes)
            spriteBatch.Draw(box.GetTexture(), 
                new Vector2(box.Position.X - World.PositionX, box.Position.Y), Color.White);
    }

    private static void DrawMap()
    {
        var map = World.NowLevel.GetMap();
        for (int x = 0; x < map.Dungeon.GetLength(0); x++)
        {
            for (int y = 0; y < map.Dungeon.GetLength(1); y++)
            {
                if (map.Dungeon[x, y].Type == Game1.Type.Ground)
                    spriteBatch.Draw(World.BarPlayer.ColorHealth,
                        new Rectangle(map.WidthCell*x, map.HeightCell*y, map.WidthCell, map.HeightCell), Color.White);
                if(map.Dungeon[x, y].Type == Game1.Type.Box)
                    spriteBatch.Draw(World.BarPlayer.ColorDied,
                        new Rectangle((int)(map.WidthCell * x - World.PositionX), map.HeightCell * y, map.WidthCell, map.HeightCell), Color.White);
                if (map.Dungeon[x, y].Type == Game1.Type.Monster)
                    spriteBatch.Draw(World.BarPlayer.ColorDied,
                        new Rectangle((int)(map.WidthCell * x - World.PositionX), map.HeightCell * y, map.WidthCell, map.HeightCell), Color.White);
            }
        }
    }
}
