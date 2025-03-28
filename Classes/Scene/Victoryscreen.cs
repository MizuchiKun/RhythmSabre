using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    public class Victoryscreen : Scene
    {
        Texture2D bkg;

        List<Button> buttons = new List<Button>();

        Button mainButton, restartButton;

        PassObject mainObject, restartObject;

        XDocument xml;

        public Victoryscreen()
        {
            bkg = Globals.Content.Load<Texture2D>("Sprites/Misc/Victoryscreen");

            mainObject = ToMainmenu;
            mainButton = new Button(Button.ButtonState.Selected, mainObject, null, 
                                    new Vector2((int)Camera.Position.X + Globals.Graphics.PreferredBackBufferWidth / 2 - 375, (int)Camera.Position.Y + Globals.Graphics.PreferredBackBufferHeight / 2 + 10));
            buttons.Add(mainButton);

            restartObject = Restart;
            restartButton = new Button(Button.ButtonState.Unselected, restartObject, null,
                                        new Vector2((int)Camera.Position.X + Globals.Graphics.PreferredBackBufferWidth / 2 - 75, (int)Camera.Position.Y + Globals.Graphics.PreferredBackBufferHeight / 2 + 10));
            buttons.Add(restartButton);
        }

        public override void Update()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Update();
                if (buttons[i].buttonState == Button.ButtonState.Selected && (Globals.GetKeyUp(Keys.A) || Globals.GetKeyUp(Keys.Left)) && i > 0)
                {
                    buttons[i - 1].buttonState = Button.ButtonState.Selected;
                    buttons[i].buttonState = Button.ButtonState.Unselected;

                    // Break because the player can move the arrow only once per frame.
                    break;
                }
                if (buttons[i].buttonState == Button.ButtonState.Selected && (Globals.GetKeyUp(Keys.D) || Globals.GetKeyUp(Keys.Right)) && i < buttons.Count - 1)
                {
                    buttons[i + 1].buttonState = Button.ButtonState.Selected;
                    buttons[i].buttonState = Button.ButtonState.Unselected;

                    // Break because the player can move the arrow only once per frame.
                    break;
                }
            }
            // Switch back to the mainmenu by hitting the escape key. 
            if (Globals.GetKeyUp(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                //Globals.gamestate = Gamestate.Mainmenu;
                Globals.CurrentScene = new Mainmenu();
            }
        }

        public override void Draw()
        {
            Globals.SpriteBatch.Draw(bkg, new Rectangle(Camera.Position.ToPoint(), (Globals.WindowDimensions + Vector2.One).ToPoint()), null, Color.White, 0, new Vector2(0f), SpriteEffects.None, 0.1f);

            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Draw();
            }

            Globals.SpriteBatch.DrawString(Globals.Content.Load<SpriteFont>("Fonts/Consolas24"), "Mainmenu", mainButton.RelativePosition + new Vector2(60, 10), Color.White);
            Globals.SpriteBatch.DrawString(Globals.Content.Load<SpriteFont>("Fonts/Consolas24"), "And another one!", restartButton.RelativePosition + new Vector2(60, 10), Color.White);
        }

        private void ToMainmenu(object info)
        {
            Globals.CurrentScene = new Mainmenu();
            Globals.gamestate = Gamestate.Mainmenu;
        }

        private void Restart(object info)
        {
            Level.CurrentRoom.Entities.Clear();
            Globals.CurrentScene = new Level(0);
            Globals.gamestate = Gamestate.Active;
        }
    }
}