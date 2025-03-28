using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    /// <summary>
    /// A container for all camera related information.
    /// </summary>
    public static class Camera
    {
        /// <summary>
        /// The current position of the camera.
        /// </summary>
        public static Vector2 Position { get; set; } = Vector2.Zero;

        /// <summary>
        /// The current scale of the camera.
        /// </summary>
        public static float Scale { get; set; } = 1.0f;
    }
}