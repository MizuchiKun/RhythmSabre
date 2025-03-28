using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    public delegate void PassObject(object i);
    public delegate object PassObjectsAndReturn(object i);

    /// <summary>
    /// An enum containing the directions up, right, down and left.
    /// </summary>
    public enum Directions : byte
    {
        Up, Right, Down, Left
    }

    /// <summary>
    /// An enum containing the different states of the game: active, paused, mainmenu, dead.
    /// </summary>
    public enum Gamestate : byte
    {
        Active, Paused, Mainmenu, Optionsmenu, Challengesmenu, Statsmenu,/*Cutsceen,*/ Dead, Win
    }

    /// <summary>
    /// Contains information and functions of global importance.
    /// </summary>
    public static class Globals
    {
        /// <summary>
        /// The <see cref="Sounds"> class for this project
        /// </summary>
        public static Sounds sounds;

        /// <summary>
        /// Make the window fullscreen, if the fullscreen option is chosen.
        /// </summary>
        public static bool Fullscreen = false;

        /// <summary>
        /// The name of the game.
        /// </summary>
        public static string gameName = "ProjektRoguelike";

        /// <summary>
        /// 
        /// </summary>
        public static Save save;

        /// <summary>
        /// 
        /// </summary>
        public static string appDataFilePath;

        /// <summary>
        /// 
        /// </summary>
        public static System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");

        /// <summary>
        /// The current <see cref="gamestate"> of this project.
        /// </summary>
        public static Gamestate gamestate { get; set; } = Gamestate.Mainmenu;

        /// <summary>
        /// The <see cref="UI"> of this project.
        /// </summary>
        public static UI UI { get; set; }

        /// <summary>
        /// The <see cref="Game1"/> of this project.
        /// </summary>
        public static Game1 Game1 { get; set; }

        /// <summary>
        /// The <see cref="ContentManager"/> of this project.
        /// </summary>
        public static ContentManager Content { get; set; }

        /// <summary>
        /// The <see cref="GraphicsDeviceManager"/> of this project.
        /// </summary>
        public static GraphicsDeviceManager Graphics { get; set; }

        /// <summary>
        /// The base dimensions of the window.
        /// </summary>
        public static Vector2 BaseWindowDimensions
        {
            get
            {
                return new Vector2(1280, 720);
            }
        }

        /// <summary>
        /// The current dimensions of the window.
        /// </summary>
        public static Vector2 WindowDimensions
        {
            get
            {
                return new Vector2(Graphics.PreferredBackBufferWidth,
                                   Graphics.PreferredBackBufferHeight);
            }
        }

        /// <summary>
        /// The current global scale factors.
        /// </summary>
        public static Vector2 Scale
        {
            get
            {
                return WindowDimensions / BaseWindowDimensions;
            }
        }

        /// <summary>
        /// The <see cref="SpriteBatch"/> of this project.
        /// </summary>
        public static SpriteBatch SpriteBatch { get; set; }

        /// <summary>
        /// The current <see cref="Scene"/>.
        /// </summary>
        public static Scene CurrentScene { get; set; }

        /// <summary>
        /// The current <see cref="GameTime"/>.
        /// </summary>
        public static GameTime GameTime { get; set; }

        /// <summary>
        /// The previous state of the keyboard.
        /// </summary>
        public static KeyboardState PreviousKeyboardState { get; set; }

        /// <summary>
        /// The current state of the keyboard.
        /// </summary>
        public static KeyboardState CurrentKeyboardState { get; set; }

        /// <summary>
        /// The previous state of the mouse.
        /// </summary>
        public static MouseState PreviousMouseState { get; set; }

        /// <summary>
        /// The current state of the mouse.
        /// </summary>
        public static MouseState CurrentMouseState { get; set; }

        /// <summary>
        /// The previous state of the gamepad.
        /// </summary>
        public static GamePadState PreviousGamePadState { get; set; }

        /// <summary>
        /// The current state of the gamepad.
        /// </summary>
        public static GamePadState CurrentGamePadState { get; set; }

        /// <summary>
        /// The four <see cref="Vector2"/>s that correspond to the four <see cref="Directions"/>.
        /// </summary>
        public static Vector2[] DirectionVectors { get; } =
        {
            // Up.
            -Vector2.UnitY,
            // Right.
            Vector2.UnitX,
            // Down.
            Vector2.UnitY,
            // Left.
            -Vector2.UnitX
        };


        /// <summary>
        /// Gets whether the given key is currently pressed.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <return>True if the key is currently pressed, false otherwise.</return>
        public static bool GetKey(Keys key)
        {
            return (PreviousKeyboardState.IsKeyDown(key) && CurrentKeyboardState.IsKeyDown(key));
        }

        /// <summary>
        /// Gets whether the given key was just pressed down.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <returns>True if it was just pressed down, false otherwise.</returns>
        public static bool GetKeyDown(Keys key)
        {
            return (PreviousKeyboardState.IsKeyUp(key) && CurrentKeyboardState.IsKeyDown(key));
        }

        /// <summary>
        /// Gets whether the given key was just released.
        /// </summary>
        /// <param name="key">The given key.</param>
        /// <returns>True if it was just released, false otherwise.</returns>
        public static bool GetKeyUp(Keys key)
        {
            return (PreviousKeyboardState.IsKeyDown(key) && CurrentKeyboardState.IsKeyUp(key));
        }

        /// <summary>
        /// Gets whether the given mouse button is currently pressed.
        /// </summary>
        /// <param name="button">The given mouse button.</param>
        /// <returns>True if the mouse button is currently pressed, false otherwise.</returns>
        public static bool GetMouseButton(MouseButton button)
        {
            // Which button?
            switch (button)
            {
                case MouseButton.Left:
                    return (PreviousMouseState.LeftButton == ButtonState.Pressed && CurrentMouseState.LeftButton == ButtonState.Pressed);
                case MouseButton.Middle:
                    return (PreviousMouseState.MiddleButton == ButtonState.Pressed && CurrentMouseState.MiddleButton == ButtonState.Pressed);
                case MouseButton.Right:
                    return (PreviousMouseState.RightButton == ButtonState.Pressed && CurrentMouseState.RightButton == ButtonState.Pressed);
                case MouseButton.XButton1:
                    return (PreviousMouseState.XButton1 == ButtonState.Pressed && CurrentMouseState.XButton1 == ButtonState.Pressed);
                case MouseButton.XButton2:
                    return (PreviousMouseState.XButton2 == ButtonState.Pressed && CurrentMouseState.XButton2 == ButtonState.Pressed);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Gets whether the given mouse button was just pressed down.
        /// </summary>
        /// <param name="button">The given mouse button.</param>
        /// <returns>True if the mouse button was just pressed down, false otherwise.</returns>
        public static bool GetMouseButtonDown(MouseButton button)
        {
            // Which button?
            switch (button)
            {
                case MouseButton.Left:
                    return (PreviousMouseState.LeftButton == ButtonState.Released && CurrentMouseState.LeftButton == ButtonState.Pressed);
                case MouseButton.Middle:
                    return (PreviousMouseState.MiddleButton == ButtonState.Released && CurrentMouseState.MiddleButton == ButtonState.Pressed);
                case MouseButton.Right:
                    return (PreviousMouseState.RightButton == ButtonState.Released && CurrentMouseState.RightButton == ButtonState.Pressed);
                case MouseButton.XButton1:
                    return (PreviousMouseState.XButton1 == ButtonState.Released && CurrentMouseState.XButton1 == ButtonState.Pressed);
                case MouseButton.XButton2:
                    return (PreviousMouseState.XButton2 == ButtonState.Released && CurrentMouseState.XButton2 == ButtonState.Pressed);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Gets whether the given mouse button was just released.
        /// </summary>
        /// <param name="button">The given mouse button.</param>
        /// <returns>True if the mouse button was just released, false otherwise.</returns>
        public static bool GetMouseButtonUp(MouseButton button)
        {
            // Which button?
            switch (button)
            {
                case MouseButton.Left:
                    return (PreviousMouseState.LeftButton == ButtonState.Pressed && CurrentMouseState.LeftButton == ButtonState.Released);
                case MouseButton.Middle:
                    return (PreviousMouseState.MiddleButton == ButtonState.Pressed && CurrentMouseState.MiddleButton == ButtonState.Released);
                case MouseButton.Right:
                    return (PreviousMouseState.RightButton == ButtonState.Pressed && CurrentMouseState.RightButton == ButtonState.Released);
                case MouseButton.XButton1:
                    return (PreviousMouseState.XButton1 == ButtonState.Pressed && CurrentMouseState.XButton1 == ButtonState.Released);
                case MouseButton.XButton2:
                    return (PreviousMouseState.XButton2 == ButtonState.Pressed && CurrentMouseState.XButton2 == ButtonState.Released);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Gets whether the given button is currently pressed.
        /// </summary>
        /// <param name="button">The given button.</param>
        /// <returns>True if the button is currently pressed, false otherwise.</returns>
        public static bool GetButton(Buttons button)
        {
            return (PreviousGamePadState.IsButtonDown(button) && CurrentGamePadState.IsButtonDown(button));
        }

        /// <summary>
        /// Gets whether the given button was just pressed down.
        /// </summary>
        /// <param name="button">The given button.</param>
        /// <returns>True if the button was just pressed down, false otherwise.</returns>
        public static bool GetButtonDown(Buttons button)
        {
            return (PreviousGamePadState.IsButtonUp(button) && CurrentGamePadState.IsButtonDown(button));
        }

        /// <summary>
        /// Gets whether the given button was just released.
        /// </summary>
        /// <param name="button">The given button.</param>
        /// <returns>True if the button was just released, false otherwise.</returns>
        public static bool GetButtonDownUp(Buttons button)
        {
            return (PreviousGamePadState.IsButtonDown(button) && CurrentGamePadState.IsButtonUp(button));
        }

        /// <summary>
        /// Gets whether the given time span has passed since the given point in time.
        /// </summary>
        /// <param name="time">The given time span.</param>
        /// <param name="pointInTime">The given point in time.</param>
        /// <returns>True if the given time span has passed, false otherwise.</returns>
        public static bool HasTimePassed(TimeSpan time, DateTime pointInTime)
        {
            return (DateTime.Now - pointInTime >= time);
        }

        /// <summary>
        /// Gets the distance between the two given <see cref="Vector2"/>.
        /// </summary>
        /// <param name="origin">The origin <see cref="Vector2"/>.</param>
        /// <param name="target">The target <see cref="Vector2"/>.</param>
        /// <returns>The distance between the two <see cref="Vector2"/>.</returns>
        public static float GetDistance(Vector2 origin, Vector2 target)
        {
            return (origin - target).Length();
        }

        /// <summary>
        /// Gets the distance between the two given <see cref="Vector3"/>.
        /// </summary>
        /// <param name="origin">The origin <see cref="Vector3"/>.</param>
        /// <param name="target">The target <see cref="Vector3"/>.</param>
        /// <returns>The distance between the two <see cref="Vector3"/>.</returns>
        public static float GetDistance(Vector3 origin, Vector3 target)
        {
            return (origin - target).Length();
        }

        /// <summary>
        /// Gets the degrees of the given <see cref="Vector2"/> direction.
        /// </summary>
        /// <param name="direction">The <see cref="Vector2"/> direction.</param>
        /// <returns>The degrees of the direction.</returns>
        public static float Vector2ToDegrees(Vector2 direction)
        {
            // Return the angle.
            return MathHelper.ToDegrees((float)Math.Atan2(direction.Y, direction.X));
        }

        /// <summary>
        /// Converts the given degrees to a <see cref="Vector2"/>.
        /// </summary>
        /// <param name="degrees">The degrees that will be converted.</param>
        /// <returns>The <see cref="Vector2"/> direction equivalent to the degrees.</returns>
        public static Vector2 DegreesToVector2(float degrees)
        {
            // Return the degrees as Vector2.
            return new Vector2((float)Math.Cos(MathHelper.ToRadians(degrees)), (float)Math.Sin(MathHelper.ToRadians(degrees)));
        }

        /// <summary>
        /// Converts the given degrees to a <see cref="Vector2"/>.<br></br>
        /// </summary>
        /// <param name="focus">The point the entity needs to move to.</param>
        /// <param name="pos">The current position.</param>
        /// <param name="speed">The speed it should move in.</param>
        /// <returns>The <see cref="Vector2"/> which the unit has to move to, to reach the focus in given speed.</returns>
        public static Vector2 RadialMovement(Vector2 focus, Vector2 pos, float speed)
        {
            float dist = Globals.GetDistance(pos, focus);

            if (dist <= speed)
            {
                return focus - pos;
            }
            else
            {
                return (focus - pos) * speed / dist;
            }
        }

        /// <summary>
        /// Rotates the object around the origin
        /// </summary>
        /// <param name="point">The Point from which the rotation around the origin starts</param>
        /// <param name="origin">The origin of the rotation</param>
        /// <param name="rotation">The speed at which the rotation works</param>
        /// <returns>The <see cref="Vector2"/> of the rotation.</returns>
        public static Vector2 RotateAboutOrigin(Vector2 point, Vector2 origin, float rotation)
        {
            return Vector2.Transform(point - origin, Matrix.CreateRotationZ(rotation)) + origin;
        }
    }

    /// <summary>
    /// Represents the buttons of a typical mouse.
    /// </summary>
    public enum MouseButton : byte
    {
        Left, Middle, Right, XButton1, XButton2
    }
}
