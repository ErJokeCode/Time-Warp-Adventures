using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;
using GroundBattle.Classes;
using System;

namespace GroundBattle
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch;

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
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;

            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(GraphicsDevice);
            MainMenu.Font = Content.Load<SpriteFont>("Font");

            World.LoadContent();
            

            var player1 = new Player(Content.Load<Texture2D>("Player"), 1000, 10, 2, 30);
            Ground.BackGround = Content.Load<Texture2D>("Ground");

            var town = new Town(100, 1, Content.Load<Texture2D>("Ellipse"));
            var monster = new Monster(Content.Load<Texture2D>("SmallMonster"), 0, 500);

            World.Players.Add(player1);
            World.NowPlayer = 0;

            World.Towns.Add(town);
            World.Monsters.Add(monster);
        }

        protected override void Update(GameTime gameTime)
        {
            if (!MainMenu.IsGameStart)
                MainMenu.Update();

            var directs = new System.Collections.Generic.List<Direction>();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Pause))
                MainMenu.IsGameStart = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                directs.Add(Direction.Right); 
            }   
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                directs.Add(Direction.Left);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                directs.Add(Direction.Up);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Tab))
            {
                World.NewPlayer();
            }

            if (MainMenu.IsGameStart)
            {
                World.Update(directs);
            }
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            if (!MainMenu.IsGameStart)
                MainMenu.Draw(_spriteBatch);
            else
                World.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}