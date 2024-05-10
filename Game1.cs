using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;
using TimeWarpAdventures.Classes;
using System;
using TimeWarpAdventures.Contriller;
using TimeWarpAdventures.Models;
using TimeWarpAdventures.View;
using Microsoft.Xna.Framework.Content;
using System.Reflection.Metadata;

namespace TimeWarpAdventures
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch;
        private GameManagerDate _gameManager;

        public enum Direction
        {
            Empty,
            Right, 
            Left, 
            Up,
            Down
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;

            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(GraphicsDevice);

            MainMenu.Font = Content.Load<SpriteFont>("Font");
            Ground.BackGround = Content.Load<Texture2D>("Ground");
            World.LoadContent(GraphicsDevice);
           


            _gameManager = new GameManagerDate(Content);
            var gameState = _gameManager.GetState();
            

            if(gameState.World.Players.Count != 0)
            {
                _gameManager.LoadDate();
                gameState.LoadTexture(Content);
            }     
            else
            {
                var player1 = new Player(Content.Load<Texture2D>("Player"), 1000, 10, 2, 30); 
                var player2 = new Player(Content.Load<Texture2D>("Player"), 700, 10, 2, 30);
                World.Players.Add(player1);
                World.Players.Add(player2);
                World.NowPlayer = player1;

                var monster = new Monster(Content.Load<Texture2D>("SmallMonster"), 0, 500, 10, 10);
                World.Monsters.Add(monster);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Controller.Update(_gameManager); 

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            View.View.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}