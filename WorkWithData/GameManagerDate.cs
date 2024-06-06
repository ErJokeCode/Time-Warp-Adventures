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

namespace TimeWarpAdventures.WorkWithData;
public class GameManagerDate
{
    private const string SaveFilePath = "game_state.xml";
    private GameState gameState;

    public GameManagerDate()
    {
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
            gameState = null;
    }

    public void SaveGameState()
    {
        gameState = new GameState();

        XmlSerializer serializer = new XmlSerializer(typeof(GameState));
        using (FileStream stream = new FileStream(SaveFilePath, FileMode.Create))
        {
            serializer.Serialize(stream, gameState);
        }
    }

    public void LoadDate(GameState gameState)
    {
        World.Players = gameState.Players;
        World.NowPlayer = gameState.Players[gameState.NowPlayer];
        World.PositionX = gameState.Position;
        World.ListLevel = gameState.ListLevel;
        World.Links = gameState.Links;
        World.NowLevel = World.DictLevels[gameState.NowLevel.Name];
    }

    public void RestoreDate()
    {
        ClearAll();
        GameState newGameState;
        if (File.Exists(SaveFilePath))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GameState));
            using (FileStream stream = new FileStream(SaveFilePath, FileMode.Open))
            {
                newGameState = (GameState)serializer.Deserialize(stream);
            }
        }
        else
            newGameState = null;

        
        LoadDate(newGameState);
        World.Loader.LoadFromHistory();
    }

    private void ClearAll()
    {
        World.Players = new List<Player>();
        World.NowPlayer = new Player();
        World.PositionX = 0;
        World.ListLevel = new List<Models.ItemLevel>();
        World.Links = new List<Models.ItemLink>();
        World.NowLevel = new Models.Level();
    }

    public GameState GetState() => gameState;
}

