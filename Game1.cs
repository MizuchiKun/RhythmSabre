#region Includes
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Security.Cryptography.X509Certificates;
#endregion

namespace ProjektRoguelike
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public Game1()
        {
            // Store yourself in Globals.
            Globals.Game1 = this;

            // Set Global's Graphics and Content.
            Globals.Content = Content;
            Globals.Content.RootDirectory = "Content";

            // Create the GraphicsDeviceManager.
            Globals.Graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = false,
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720
            };

            Globals.appDataFilePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content. Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // The save class.
            Globals.save = new Save(1, Globals.gameName);

            // Set the sounds class.
            Globals.sounds = new Sounds();

            // Set the SpriteBatch.
            Globals.SpriteBatch = new SpriteBatch(Globals.Graphics.GraphicsDevice);

            // Set the UI.
            Globals.UI = new UI();

            if (Globals.save.GetFile("xml\\options.xml") != null
                && Globals.save.GetFile("xml\\options.xml").Element("Root").Element("Options").Element("Option").Element("name").Equals("Full Screen")
                && Globals.save.GetFile("xml\\options.xml").Element("Root").Element("Options").Element("Option").Element("selected").Equals("1"))
            {
                Globals.Graphics.IsFullScreen = true;
            }
            else
            {
                Globals.Graphics.IsFullScreen = false;
            }

            // Set the initial Scene.
            Globals.CurrentScene = new Mainmenu();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() { }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() { }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Save the current GameTime and states.
            Globals.GameTime = gameTime;
            Globals.CurrentKeyboardState = Keyboard.GetState();
            Globals.CurrentMouseState = Mouse.GetState();
            Globals.CurrentGamePadState = GamePad.GetState(PlayerIndex.One);

            switch (Globals.gamestate)
            {
                case Gamestate.Active:
                    Globals.CurrentScene.Update();
                    if (Globals.GetKeyDown(Keys.Escape))
                    {
                        Level.SaveData();
                        Level.Player.SaveData();
                        // [PLACEHOLDER] Save the data of the previous, the current and the next rooms of the level and all its objects excluding the player
                        Globals.sounds.StopSong();
                        Globals.CurrentScene = new Mainmenu();
                        Globals.gamestate = Gamestate.Mainmenu;
                    }
                    break;
                case Gamestate.Paused:
                    Globals.UI.Update();
                    if (Globals.GetKeyUp(Keys.P))
                    {
                        Globals.gamestate = Gamestate.Active;
                    }
                    break;
                case Gamestate.Mainmenu:
                case Gamestate.Optionsmenu:
                case Gamestate.Challengesmenu:
                case Gamestate.Statsmenu:
                case Gamestate.Win:
                    // Call the current Scene's Update method.
                    Globals.CurrentScene.Update();

                    // Call the UI's Update method.
                    Globals.UI.Update();
                    break;
                case Gamestate.Dead:
                    // Call the UI's Update method.
                    Globals.UI.Update();
                    break;
                default:
                    break;
            }



            // Save the current states as the previous states (so they will be accessable as previous states in the next Update()).
            Globals.PreviousKeyboardState = Globals.CurrentKeyboardState;
            Globals.PreviousMouseState = Globals.CurrentMouseState;
            Globals.PreviousGamePadState = Globals.CurrentGamePadState;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Clear the GraphicsDevice.
            Globals.Graphics.GraphicsDevice.Clear(Color.Blue);

            // Begin the SpriteBatch.
            Globals.SpriteBatch.Begin(
                sortMode: SpriteSortMode.BackToFront,
                transformMatrix: Matrix.CreateTranslation(new Vector3(-Camera.Position, 0))
                                 * Matrix.CreateScale(Camera.Scale));





            switch (Globals.gamestate)
            {
                case Gamestate.Active:
                case Gamestate.Paused:
                case Gamestate.Mainmenu:
                case Gamestate.Optionsmenu:
                case Gamestate.Challengesmenu:
                case Gamestate.Statsmenu:
                case Gamestate.Win:
                case Gamestate.Dead:
                    Globals.CurrentScene.Draw();

                    Globals.UI.Draw();
                    break;
                default:
                    break;
            }


            // End the SpriteBatch.
            Globals.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
