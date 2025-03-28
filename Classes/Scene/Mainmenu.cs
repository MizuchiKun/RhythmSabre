using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProjektRoguelike
{
    public class Mainmenu : Scene
    {
        Texture2D bkg;

        Button NewRun, Continue, Challenges, Stats, Options;

        PassObject NewRunObject, ContinueObject, ChallengesObject, StatsObject, OptionsObject;

        List<Button> buttons = new List<Button>();

        public static XDocument xmlPlayer, xmlLevel;

        public Mainmenu()
        {
            //Mainmenu1 needs to be added to the project
            bkg = Globals.Content.Load<Texture2D>("Sprites/Misc/Mainmenu1");

            NewRunObject = NewGame;
            NewRun = new Button(Button.ButtonState.Selected, NewRunObject, null, new Vector2(380, 220));
            buttons.Add(NewRun);

            ContinueObject = ContinueGame;
            Continue = new Button(Button.ButtonState.Unselected, ContinueObject, null, new Vector2(380, 330));
            buttons.Add(Continue);

            ChallengesObject = SwitchToChallenges;
            Challenges = new Button(Button.ButtonState.Unselected, ChallengesObject, null, new Vector2(380, 410));
            buttons.Add(Challenges);

            StatsObject = SwitchToStats;
            Stats = new Button(Button.ButtonState.Unselected, StatsObject, null, new Vector2(380, 470));
            buttons.Add(Stats);

            OptionsObject = SwitchToOptions;
            Options = new Button(Button.ButtonState.Unselected, OptionsObject, null, new Vector2(380, 550));
            buttons.Add(Options);

            xmlPlayer = Globals.save.GetFile("xml\\stats.xml");
        }

        public override void Update()
        {
            // Update the buttons and check which button is supposed to be selected.
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Update();
                if (buttons[i].buttonState == Button.ButtonState.Selected && (Globals.GetKeyUp(Keys.W) || Globals.GetKeyUp(Keys.Up)) && i > 0)
                {
                    buttons[i - 1].buttonState = Button.ButtonState.Selected;
                    buttons[i].buttonState = Button.ButtonState.Unselected;

                    // Break because the player can move the arrow only once per frame.
                    break;
                }
                if (buttons[i].buttonState == Button.ButtonState.Selected && (Globals.GetKeyUp(Keys.S) || Globals.GetKeyUp(Keys.Down)) && i < buttons.Count - 1)
                {
                    buttons[i + 1].buttonState = Button.ButtonState.Selected;
                    buttons[i].buttonState = Button.ButtonState.Unselected;

                    // Break because the player can move the arrow only once per frame.
                    break;
                }
            }

            // Close the game by hitting the escape key. 
            if (Globals.GetKeyDown(Keys.Escape))
            {
                System.Environment.Exit(0);
            }
        }

        public override void Draw()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Draw();
            }
            Globals.SpriteBatch.Draw(bkg, new Rectangle(Camera.Position.ToPoint(), (Globals.WindowDimensions + Vector2.One).ToPoint()), null, Color.White, 0, new Vector2(0f), SpriteEffects.None, 0.1f);
        }

        /// <summary>
        /// Empty the Entities List, create a new level and set the gamestate to active.
        /// </summary>
        /// <param name="info"> Only here, so I can use my delegate. lol </param>
        private void NewGame(object info)
        {
            Globals.sounds.PlaySong("audio", true);
            Globals.save.DeleteFile("xml\\level.xml");
            Globals.save.DeleteFile("xml\\stats.xml"); // I, as a player, would like to keep my stats over multiple runs.
            Globals.CurrentScene = new Level(0);
            Globals.gamestate = Gamestate.Active;
        }

        private void ContinueGame(object info)
        {
            // Try to get the file.
            Mainmenu.xmlLevel = Globals.save.GetFile("xml\\level.xml");

            // If there is a Level to load.
            if (xmlLevel != null)
            {
                Globals.sounds.PlaySong("audio", true);
                Globals.CurrentScene = new Level(0, true);
                Level.Player.LoadData(xmlPlayer);
                Globals.gamestate = Gamestate.Active;
            }
        }

        private void SwitchToOptions(object info)
        {
            //Globals.gamestate = Gamestate.Optionsmenu;
            Globals.CurrentScene = new Optionsmenu();
        }

        private void SwitchToChallenges(object info)
        {
            //Globals.gamestate = Gamestate.Challengesmenu;
            Globals.CurrentScene = new Challengesmenu();
        }

        private void SwitchToStats(object info)
        {
            //Globals.gamestate = Gamestate.Statsmenu;
            Globals.CurrentScene = new Statsmenu();
        }
    }
}