using Microsoft.Xna.Framework.Content;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TimeWarpAdventures.Classes;

namespace TimeWarpAdventures.Models;
public class GameManagerDate
{
    private const string SaveFilePath = "game_state.xml";
    private GameState gameState;
    private ContentManager content;

    public GameManagerDate(ContentManager content)
    {
        this.content = content;
        LoadGameState();
    }

    private void LoadGameState()
    {
        if (File.Exists(SaveFilePath))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GameState));
            using (FileStream stream = new FileStream(SaveFilePath, FileMode.Open))
            {
                gameState = (GameState)serializer.Deserialize(stream);
            }
        }
        else
        {
            gameState = new GameState();
        }
    }

    public void SaveGameState()
    {
        gameState.World = new WorldInfo();

        XmlSerializer serializer = new XmlSerializer(typeof(GameState));
        using (FileStream stream = new FileStream(SaveFilePath, FileMode.Create))
        {
            serializer.Serialize(stream, gameState);
        }
    }

    public void LoadDate()
    {
        World.Players = gameState.World.Players;
        World.NowPlayer = gameState.World.Players[0];

        World.Monsters = gameState.World.Monstres;

        World.PositionX = gameState.World.Position;
    }

    public void RestoreDate()
    {
        LoadGameState();
        gameState.LoadTexture(content);
        LoadDate();
    }

    public GameState GetState() => gameState;
}

