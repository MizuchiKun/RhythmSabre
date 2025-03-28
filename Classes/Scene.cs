using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    /// <summary>
    /// A container for <see cref="GameObject"/>s to update and draw to the screen.
    /// </summary>
    public abstract class Scene
    {
        /// <summary>
        /// This Scene's <see cref="GameObject"/>s.
        /// </summary>
        protected List<GameObject> _gameObjects;

        /// <summary>
        /// Creates a new <see cref="Scene"/> object with the given <see cref="GameObject"/>s.
        /// </summary>
        /// <param name="gameObjects">
        /// The given List of <see cref="GameObject"/>s which will be added to the Scene.<br></br>
        /// If null, it will be set to an empty List of <see cref="GameObject"/>s.
        /// </param>
        public Scene(List<GameObject> gameObjects = null)
        {
            // Store the parameters.
            _gameObjects = (gameObjects != null) ? gameObjects : new List<GameObject>();
        }

        /// <summary>
        /// Calls the <see cref="Scene"/>'s <see cref="GameObject"/>s' Update() methods.
        /// </summary>
        public virtual void Update()
        {
            // Call your GameObjects' Update() methods.
            for (ushort i = 0; i < _gameObjects.Count; i++)
            {
                _gameObjects[i].Update();
            }
        }

        /// <summary>
        /// Calls the <see cref="Scene"/>'s <see cref="GameObject"/>s' Draw() methods.
        /// </summary>
        public virtual void Draw()
        {
            // Call your GameObjects' Draw() methods.
            foreach (GameObject gameObject in _gameObjects)
            {
                gameObject.Draw();
            }
        }
    }
}
