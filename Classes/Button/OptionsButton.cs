using System;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    class OptionsButton : Button
    {
        public string Title;

        public int Selected, SelectedMax;

        public OptionsButton(string title, int selected, int selectedMax, ButtonState ButtonState, Vector2 relativePosition) : base(ButtonState, null, selected, relativePosition)
        {
            Title = title;
            Selected = selected;
            SelectedMax = selectedMax;
        }

        public override void Update()
        {
            switch (buttonState)
            {
                case ButtonState.Unselected:
                    break;
                case ButtonState.Selected:
                    // press enter to activate the button
                    if (Globals.GetKeyUp(Microsoft.Xna.Framework.Input.Keys.Enter))
                    {
                        buttonState = ButtonState.Activated;
                    }
                    ChangeValue();
                    break;
                case ButtonState.Activated:
                    // run the buttons effect and get back to being only selected
                    buttonState = ButtonState.Selected;
                    break;
                default:
                    break;
            }
        }

        public override void Draw()
        {
            // Update the position relative to the Camera.
            Vector2 absolutePosition = Camera.Position + RelativePosition;

            Globals.SpriteBatch.DrawString(Globals.Content.Load<SpriteFont>("Fonts/Consolas24"), Title, absolutePosition + new Vector2(75, 10), Color.White);
            if (SelectedMax == 1)
            {
                switch (Selected)
                {
                    case 0:
                        Globals.SpriteBatch.DrawString(Globals.Content.Load<SpriteFont>("Fonts/Consolas24"), "No", absolutePosition + new Vector2(425, 10), Color.White);
                        Globals.Fullscreen = false;
                        break;
                    case 1:
                        Globals.SpriteBatch.DrawString(Globals.Content.Load<SpriteFont>("Fonts/Consolas24"), "Yes", absolutePosition + new Vector2(425, 10), Color.White);
                        Globals.Fullscreen = true;
                        break;
                }
            }
            else
            {
                Globals.SpriteBatch.DrawString(Globals.Content.Load<SpriteFont>("Fonts/Consolas24"), Selected.ToString(), absolutePosition + new Vector2(425, 10), Color.White);
            }

            // only draw the button, if it is either activated or selected.
            switch (buttonState)
            {
                case ButtonState.Unselected:
                    break;
                case ButtonState.Selected:
                    Globals.SpriteBatch.Draw(activated, new Rectangle((int)absolutePosition.X, (int)absolutePosition.Y, 50, 50), null, Color.White, 0, new Vector2(.5f), new SpriteEffects(), 0.05f);
                    break;
                case ButtonState.Activated:
                    Globals.SpriteBatch.Draw(activated, new Rectangle((int)absolutePosition.X, (int)absolutePosition.Y, 50, 50), null, Color.White, 0, new Vector2(.5f), new SpriteEffects(), 0.05f);
                    break;
                default:
                    break;
            }
        }

        public void ChangeValue()
        {
            if (buttonState == ButtonState.Selected && (Globals.GetKeyDown(Keys.A) || (Globals.GetKeyDown(Keys.Left))))
            {
                // Decrease the selected amount.
                Selected--;

                // If the amount goes below 0, reset it to 0.
                if (Selected < 0)
                {
                    Selected = 0;
                }
            }
            if (buttonState == ButtonState.Selected && (Globals.GetKeyDown(Keys.D) || (Globals.GetKeyDown(Keys.Right))))
            {
                // Increase the selected amount.
                Selected++;

                // If the amount is increased further than the max, reset it to the max amount.
                if (Selected > SelectedMax)
                {
                    Selected = SelectedMax;
                }
            }
        }

        public XElement ReturnToXML()
        {
            XElement xml = new XElement("Option",
                                    new XElement("name", Title),
                                    new XElement("selected", Selected));

            return xml;
        }
    }
}
