using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    /// <summary>
    /// A room of a level.
    /// </summary>
    public class GameObjectContainer : GameObject
    {
        /// <summary>
        /// The hitbox of a <see cref="GameObjectContainer"/>.
        /// </summary>
        public override Rectangle Hitbox 
        { 
            get
            {
                return Rectangle.Empty;
            }
        }

        /// <summary>
        /// A list of all <see cref="GameObject"/>s of this room.
        /// </summary>
        protected List<GameObject> _gameObjects = new List<GameObject>();

        /// <summary>
        /// Creates a new <see cref="GameObjectContainer"/> with the given <see cref="GameObject"/>s.
        /// </summary>
        /// <param name="gameObjects">
        /// The <see cref="GameObject"/>s that will be added to the container.<br></br>
        /// If null, it will be set to an empty List of <see cref="GameObject"/>s.
        /// </param>
        public GameObjectContainer (List<GameObject> gameObjects = null)
        {
            // Store the parameters.
            _gameObjects = (gameObjects != null) ? gameObjects : new List<GameObject>();
        }

        /// <summary>
        /// Calls the <see cref="GameObjectContainer"/>'s <see cref="GameObject"/>s' Update() methods.
        /// </summary>
        public override void Update()
        {            
            // Call your GameObjects' Update() methods.
            for (int i = 0; i < _gameObjects.Count; i++)
            {
                _gameObjects[i].Update();
            }
        }

        /// <summary>
        /// Calls the <see cref="GameObjectContainer"/>'s <see cref="GameObject"/>s' Draw() methods.
        /// </summary>
        public override void Draw()
        {
            // Call your GameObjects' Draw() methods.
            foreach (GameObject gameObject in _gameObjects)
            {
                gameObject.Draw();
            }
        }
    }
}
