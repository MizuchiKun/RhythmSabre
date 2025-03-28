using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    /// <summary>
    /// The user interface
    /// </summary>
    public class UI : GameObject
    {
        public override Rectangle Hitbox => Rectangle.Empty;

        Texture2D heart, bomb, key, coin = new Texture2D(Globals.Graphics.GraphicsDevice, 256, 256);

        Button restartButton;

        PassObject resetObject;

        public UI()
        {
            heart = Globals.Content.Load<Texture2D>("Sprites/Items/Heart");
            bomb = Globals.Content.Load<Texture2D>("Sprites/Pickups/Bomb");
            key = Globals.Content.Load<Texture2D>("Sprites/Pickups/Key");
            coin = Globals.Content.Load<Texture2D>("Sprites/Pickups/Coin");

            resetObject = ResetWorld;
            restartButton = new Button(Button.ButtonState.Selected, ResetWorld, null, new Vector2((int)Camera.Position.X + Globals.Graphics.PreferredBackBufferWidth / 2 - 130, (int)Camera.Position.Y + Globals.Graphics.PreferredBackBufferHeight / 2));
        }

        public override void Update()
        {
            switch (Globals.gamestate)
            {
                case Gamestate.Active:
                case Gamestate.Paused:
                case Gamestate.Mainmenu:
                case Gamestate.Optionsmenu:
                case Gamestate.Challengesmenu:
                case Gamestate.Statsmenu:
                case Gamestate.Win:
                    break;
                case Gamestate.Dead:
                    Globals.save.DeleteFile("xml\\level.xml");
                    Globals.save.DeleteFile("xml\\stats.xml");
                    restartButton.Update();
                    if (restartButton.buttonState == Button.ButtonState.Activated)
                    {
                        ResetWorld(null);
                    }
                    if (Globals.GetKeyUp(Keys.Escape))
                    {
                        Globals.CurrentScene = new Mainmenu();
                        Globals.gamestate = Gamestate.Mainmenu;
                    }
                    break;
                default:
                    break;
            }
        }

        public override void Draw()
        {
            // Call the current Scene's Draw method.
            Globals.CurrentScene.Draw();

            switch (Globals.gamestate)
            {
                case Gamestate.Active:
                    #region Healthbar
                    for (int i = 0; i < Level.Player.HealthMax; i++)
                    {
                        //Globals.SpriteBatch.Draw(heart, new Rectangle((int)Level.Player.Position.X - 50 + (i * 25), (int)Level.Player.Position.Y - 100, 25, 25), null, Color.Black, 0, new Vector2(.5f), new SpriteEffects(), 0.1f);
                        Globals.SpriteBatch.Draw(heart, new Rectangle((int)Camera.Position.X, (int)Camera.Position.Y + (i * 30), 40, 40), null, Color.Black, 0, new Vector2(.5f), new SpriteEffects(), 0.1f);
                    }
                    for (int i = 0; i < Level.Player.Health; i++)
                    {
                        Globals.SpriteBatch.Draw(heart, new Rectangle((int)Camera.Position.X, (int)Camera.Position.Y + (i * 30), 40, 40), null, Color.White, 0, new Vector2(.5f), new SpriteEffects(), 0.05f);
                    }
                    #endregion
                    #region PickupItems

                    // Bombs.
                    Globals.SpriteBatch.Draw(bomb, new Rectangle((int)Camera.Position.X, (int)Camera.Position.Y + 675, 40, 40), null, Color.White, 0, new Vector2(.5f), new SpriteEffects(), 0.05f);
                    Globals.SpriteBatch.DrawString(Globals.Content.Load<SpriteFont>("Fonts/Consolas24"), Level.Player.Bombs.ToString(), new Vector2((int)Camera.Position.X + 12, (int)Camera.Position.Y + 645), Color.White);

                    // Keys.
                    Globals.SpriteBatch.Draw(key, new Rectangle((int)Camera.Position.X, (int)Camera.Position.Y + 595, 40, 40), null, Color.White, 0, new Vector2(.5f), new SpriteEffects(), 0.05f);
                    Globals.SpriteBatch.DrawString(Globals.Content.Load<SpriteFont>("Fonts/Consolas24"), Level.Player.Keys.ToString(), new Vector2((int)Camera.Position.X + 12, (int)Camera.Position.Y + 565), Color.White);

                    // Coins.
                    Globals.SpriteBatch.Draw(coin, new Rectangle((int)Camera.Position.X, (int)Camera.Position.Y + 525, 40, 40), null, Color.White, 0, new Vector2(.5f), new SpriteEffects(), 0.05f);
                    Globals.SpriteBatch.DrawString(Globals.Content.Load<SpriteFont>("Fonts/Consolas24"), Level.Player.Gold.ToString(), new Vector2((int)Camera.Position.X + 12, (int)Camera.Position.Y + 500), Color.White);

                    #endregion
                    #region Items
                    for (int i = 0; i < Level.Player.items.Count; i++)
                    {
                        Level.Player.items[i].Scale = new Vector2(.2f);
                        Level.Player.items[i].Position = new Vector2(Camera.Position.X + 1260, Camera.Position.Y + 30 + (i * 45));
                        Level.Player.items[i].Draw();
                    }

                    #endregion
                    break;
                case Gamestate.Paused:
                    #region Healthbar
                    for (int i = 0; i < Level.Player.HealthMax; i++)
                    {
                        //Globals.SpriteBatch.Draw(heart, new Rectangle((int)Level.Player.Position.X - 50 + (i * 25), (int)Level.Player.Position.Y - 100, 25, 25), null, Color.Black, 0, new Vector2(.5f), new SpriteEffects(), 0.1f);
                        Globals.SpriteBatch.Draw(heart, new Rectangle((int)Camera.Position.X + (Globals.Graphics.PreferredBackBufferWidth / 2 / Level.Player.HealthMax * i + (Globals.Graphics.PreferredBackBufferWidth / 4)), (int)Camera.Position.Y + (Globals.Graphics.PreferredBackBufferHeight / 4), 40, 40), null, Color.Black, 0, new Vector2(.5f), new SpriteEffects(), 0.1f);
                    }
                    for (int i = 0; i < Level.Player.Health; i++)
                    {
                        Globals.SpriteBatch.Draw(heart, new Rectangle((int)Camera.Position.X + (Globals.Graphics.PreferredBackBufferWidth / 2 / Level.Player.HealthMax * i + (Globals.Graphics.PreferredBackBufferWidth / 4)), (int)Camera.Position.Y + (Globals.Graphics.PreferredBackBufferHeight / 4), 40, 40), null, Color.White, 0, new Vector2(.5f), new SpriteEffects(), 0.05f);
                    }
                    #endregion
                    #region PickupItems

                    // Bombs.
                    Globals.SpriteBatch.Draw(bomb, new Rectangle((int)Camera.Position.X + (Globals.Graphics.PreferredBackBufferWidth / 2 / Level.Player.HealthMax + (Globals.Graphics.PreferredBackBufferWidth / 4)), (int)Camera.Position.Y + (Globals.Graphics.PreferredBackBufferHeight / 2 ), 40, 40), null, Color.White, 0, new Vector2(.5f), new SpriteEffects(), 0.05f);
                    Globals.SpriteBatch.DrawString(Globals.Content.Load<SpriteFont>("Fonts/Consolas24"), Level.Player.Bombs.ToString(), new Vector2((int)Camera.Position.X + (Globals.Graphics.PreferredBackBufferWidth / 2 / Level.Player.HealthMax + (Globals.Graphics.PreferredBackBufferWidth / 4)) + 10, (int)Camera.Position.Y + (Globals.Graphics.PreferredBackBufferHeight / 2) - 40), Color.White);

                    // Keys.
                    Globals.SpriteBatch.Draw(key, new Rectangle((int)Camera.Position.X + Globals.Graphics.PreferredBackBufferWidth / 2, (int)Camera.Position.Y + (Globals.Graphics.PreferredBackBufferHeight / 2), 40, 40), null, Color.White, 0, new Vector2(.5f), new SpriteEffects(), 0.05f);
                    Globals.SpriteBatch.DrawString(Globals.Content.Load<SpriteFont>("Fonts/Consolas24"), Level.Player.Keys.ToString(), new Vector2((int)Camera.Position.X + Globals.Graphics.PreferredBackBufferWidth / 2 + 10, (int)Camera.Position.Y + (Globals.Graphics.PreferredBackBufferHeight / 2) - 40), Color.White);

                    // Coins.
                    Globals.SpriteBatch.Draw(coin, new Rectangle((int)Camera.Position.X + Globals.Graphics.PreferredBackBufferWidth / 3 * 2, (int)Camera.Position.Y + (Globals.Graphics.PreferredBackBufferHeight / 2), 40, 40), null, Color.White, 0, new Vector2(.5f), new SpriteEffects(), 0.05f);
                    Globals.SpriteBatch.DrawString(Globals.Content.Load<SpriteFont>("Fonts/Consolas24"), Level.Player.Gold.ToString(), new Vector2((int)Camera.Position.X + Globals.Graphics.PreferredBackBufferWidth / 3 * 2 + 10, (int)Camera.Position.Y + (Globals.Graphics.PreferredBackBufferHeight / 2) - 40), Color.White);

                    #endregion
                    #region Items
                    for (int i = 0; i < Level.Player.items.Count; i++)
                    {
                        Level.Player.items[i].Scale = new Vector2(.2f);
                        Level.Player.items[i].Position = new Vector2(Camera.Position.X + (Globals.Graphics.PreferredBackBufferWidth / 2 / Level.Player.items.Count * i + (Globals.Graphics.PreferredBackBufferWidth / 4)), Camera.Position.Y + (Globals.Graphics.PreferredBackBufferHeight / 4 * 3));
                        Level.Player.items[i].Draw();
                    }

                    #endregion
                    Globals.SpriteBatch.DrawString(Globals.Content.Load<SpriteFont>("Fonts/Consolas24"), "Paused", new Vector2((int)Camera.Position.X + Globals.Graphics.PreferredBackBufferWidth / 2 - 30, (int)Camera.Position.Y + Globals.Graphics.PreferredBackBufferHeight / 2 - 100), Color.White);
                    break;
                case Gamestate.Mainmenu:
                case Gamestate.Optionsmenu:
                case Gamestate.Challengesmenu:
                case Gamestate.Statsmenu:
                case Gamestate.Win:
                    break;
                case Gamestate.Dead:
                    restartButton.Draw();
                    Globals.SpriteBatch.DrawString(Globals.Content.Load<SpriteFont>("Fonts/Consolas24"), "Press enter to restart", new Vector2((int)Camera.Position.X + Globals.Graphics.PreferredBackBufferWidth / 2 - 70, (int)Camera.Position.Y + Globals.Graphics.PreferredBackBufferHeight / 2 + 10), Color.White);
                    Globals.SpriteBatch.DrawString(Globals.Content.Load<SpriteFont>("Fonts/Consolas24"), "Game Over", new Vector2((int)Camera.Position.X + Globals.Graphics.PreferredBackBufferWidth / 2 - 30, (int)Camera.Position.Y + Globals.Graphics.PreferredBackBufferHeight / 2 - 100), Color.White);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Empty the entities list, create a new level and set the gamestate to active.
        /// </summary>
        /// <param name="info"> Only here, so I can use my delegate. lol </param>
        public virtual void ResetWorld(object info)
        {
            Level.CurrentRoom.Entities.Clear();
            Globals.CurrentScene = new Level(0);
            Globals.gamestate = Gamestate.Active;
        }
    }
}
