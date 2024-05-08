using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TimeWarpAdventures.Classes;

namespace TimeWarpAdventures.Models;
class GameManager
{
    public ContentManager Content;
    private const string SaveFilePath = "game_state.xml";
    private GameState gameState;

    public GameManager(ContentManager content)
    {
        Content = content;
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
            gameState = new GameState(Content);
        }
    }

    public void SaveGameState()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(GameState));
        using (FileStream stream = new FileStream(SaveFilePath, FileMode.Create))
        {
            serializer.Serialize(stream, gameState);
        }
    }

    public GameState GetState() => gameState;
}

