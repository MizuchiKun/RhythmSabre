using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    class Button
    {
        public enum ButtonState
        {
            Unselected, Selected, Activated
        }

        /// <summary>
        /// The state of the button.
        /// </summary>
        public ButtonState buttonState { get; set; }

        public PassObject buttonActivated;

        protected Texture2D activated;

        public object Info;

        public Vector2 RelativePosition { get; set; }

        public Button(ButtonState ButtonState, PassObject ButtonActivated, object info, Vector2 relativePosition)
        {
            Info = info;
            RelativePosition = relativePosition;
            buttonState = ButtonState;
            buttonActivated = ButtonActivated;

            activated = Globals.Content.Load<Texture2D>("Sprites/Misc/Arrowbutton");
        }

        public virtual void Update()
        {
            switch (buttonState)
            {
                case ButtonState.Unselected:
                    break;
                case ButtonState.Selected:
                    // press enter to activate the button
                    if (Globals.GetKeyUp(Keys.Enter))
                    {
                        buttonState = ButtonState.Activated;
                    }
                    break;
                case ButtonState.Activated:
                    // run the buttons effect and get back to being only selected
                    ButtonUse();
                    buttonState = ButtonState.Selected;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// If the delegate "buttonActivated" is not null, run the code in it.
        /// </summary>
        public virtual void ButtonUse()
        {
            if (buttonActivated != null)
            {
                buttonActivated(Info);
            }
        }

        public virtual void Draw()
        {
            // only draw the button, if it is either activated or selected.
            switch (buttonState)
            {
                case ButtonState.Unselected:
                    break;
                case ButtonState.Selected:
                case ButtonState.Activated:
                    Globals.SpriteBatch.Draw(activated, new Rectangle((Camera.Position + RelativePosition).ToPoint(), new Point(50, 50)), null, Color.White, 0, new Vector2(.5f), new SpriteEffects(), 0.05f);
                    break;
                default:
                    break;
            }
        }
    }
}
