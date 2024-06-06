using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;
using TimeWarpAdventures.Classes;
using System;
using TimeWarpAdventures.Contriller;
using TimeWarpAdventures.View;
using Microsoft.Xna.Framework.Content;
using System.Reflection.Metadata;
using System.Net.Mail;
using TimeWarpAdventures.WorkWithData;

namespace TimeWarpAdventures
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch;
        private GameManagerDate _gameManager;
        private View.View viewer;

        public enum Direction
        {
            Right, 
            Left, 
            Up,
            Down
        }

        public enum Type
        {
            Ground, 
            Box, 
            Empty, 
            Monster
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();

            //Window.ClientSizeChanged += Window_ClientSizeChanged;
            base.Initialize();
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            World.WindowWidth = Window.ClientBounds.Width;
            World.WindowHeight = Window.ClientBounds.Height;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(GraphicsDevice);
            viewer = new View.View(_spriteBatch, _graphics);

            var loaderContent = new LoaderContent(Content, _graphics);

            loaderContent.LoadMenu();
            loaderContent.LoadWorld();

            _gameManager = new GameManagerDate();
            var gameState = _gameManager.GetState();
            

            if(gameState != null)
            {
                _gameManager.LoadDate(gameState);
                loaderContent.LoadFromHistory();
            }     
            else
                loaderContent.AddItemsInWorld();
        }

        protected override void Update(GameTime gameTime)
        {
            Controller.Update(_gameManager, this, gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            viewer.Draw();

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}