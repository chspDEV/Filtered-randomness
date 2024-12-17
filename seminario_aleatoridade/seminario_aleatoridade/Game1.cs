using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace seminario_aleatoridade
{
   
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        UIManager ui;
        DataManager data;
        TextureManager texture;
        int randomizers = 5;
        float maxTimeToGenerate = 2f;
        float numberToChangeMaxTime = 1f;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            var displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            graphics.PreferredBackBufferWidth = displayMode.Width;
            graphics.PreferredBackBufferHeight = displayMode.Height;

            // Ativar tela cheia
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            base.Initialize();

            data = new DataManager();

            data.gameHeight = graphics.PreferredBackBufferHeight;
            data.gameWidth = graphics.PreferredBackBufferWidth;

            for (int i = 0; i < randomizers; i++)
            {
                Randomizer r = new Randomizer(60f, 0.25f, 50, 15);
                data.GetRandomizers().Add(r);
            }
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            var fntBig = Content.Load<SpriteFont>(@"Fonts\Big");
            var fntSmall = Content.Load<SpriteFont>(@"Fonts\Small");

            texture = new TextureManager(GraphicsDevice);

            var fontList = new List<SpriteFont>()
            {
                fntBig,
                fntSmall
            };
                
            ui = new UIManager(fontList);
            
        }

        protected override void UnloadContent()
        {
           
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            #region Debugs
            if (Keyboard.GetState().IsKeyDown(Keys.K))
            {
                ui.GraphicMode = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.L))
            {
                ui.GraphicMode = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.NumPad1)) //add maxtime
            {
                maxTimeToGenerate += numberToChangeMaxTime;
                data.UpdateMaxTime(maxTimeToGenerate);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad2)) //remove maxtime
            {
                //fiz isso aqui pra nao ficar numero = 0 ou negativo
                if (maxTimeToGenerate - numberToChangeMaxTime <= 0) { numberToChangeMaxTime /= 10; }
                maxTimeToGenerate -= numberToChangeMaxTime;
                data.UpdateMaxTime(maxTimeToGenerate);
            }

           
            if (Keyboard.GetState().IsKeyDown(Keys.F9)) //reset
            {
                data.ResetNumberList();
            }


            #endregion

            data.Update(gameTime);
            ui.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            ui.Draw(spriteBatch, GraphicsDevice);

            base.Draw(gameTime);
        }
    }
}
