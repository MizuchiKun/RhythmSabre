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
    public class Optionsmenu : Scene
    {
        Texture2D bkg;

        private int fullscreen, music, sfx;

        OptionsButton FullScreen, MusicVolume, SFXVolume;

        List<OptionsButton> buttons = new List<OptionsButton>();

        PassObject saveObject;
        Button Savebutton;

        XDocument xml;

        public Optionsmenu()
        {
            bkg = Globals.Content.Load<Texture2D>("Sprites/Misc/Options");

            xml = Globals.save.GetFile("xml\\options.xml");

            saveObject = SaveOptions;
            Savebutton = new Button(Button.ButtonState.Selected, saveObject, null, new Vector2(400, 520));

            FullScreen = new OptionsButton("Full Screen", fullscreen, 1, Button.ButtonState.Selected, new Vector2(400, 220));
            buttons.Add(FullScreen);

            MusicVolume = new OptionsButton("Music Volume", music, 20, Button.ButtonState.Unselected, new Vector2(400, 320));
            buttons.Add(MusicVolume);

            SFXVolume = new OptionsButton("SFX Volume", sfx, 20, Button.ButtonState.Unselected, new Vector2(400, 420));
            buttons.Add(SFXVolume);

            LoadData(xml);
        }

        public override void Update()
        {
            Savebutton.Update();
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

            // Switch back to the mainmenu by hitting the escape key. 
            if (Globals.GetKeyDown(Keys.Escape))
            {
                //SaveOptions(null);
                //Globals.gamestate = Gamestate.Mainmenu;
                Globals.CurrentScene = new Mainmenu();
            }
        }

        public override void Draw()
        {
            Savebutton.Draw();
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].Draw();
            }
            Globals.SpriteBatch.Draw(bkg, new Rectangle(Camera.Position.ToPoint(), (Globals.WindowDimensions + Vector2.One).ToPoint()), null, Color.White, 0, new Vector2(0f), SpriteEffects.None, 0.1f);
            Globals.SpriteBatch.DrawString(Globals.Content.Load<SpriteFont>("Fonts/Consolas24"), "Press enter to save", Camera.Position + Savebutton.RelativePosition + new Vector2(75, 15), Color.White);
        }

        public void SaveOptions(object info)
        {
            // Apply the options.
            // Apply the full screen.
            Globals.Graphics.ApplyChanges();
            // Apply the audio options (if necessary).
            //...

            // Save the options.
            XDocument xml = new XDocument(new XElement("Root",
                                                new XElement("Options", "")));

            for (int i = 0; i < buttons.Count; i++)
            {
                xml.Element("Root").Element("Options").Add(buttons[i].ReturnToXML());
            }

            Globals.save.HandleSaveFormates(xml, "options.xml");

            Globals.sounds.LoadData();

            Globals.CurrentScene = new Mainmenu();
        }

        public void LoadData(XDocument data)
        {
            if (data != null)
            {
                List<string> allOptions = new List<string>();
                for (int i = 0; i < buttons.Count; i++)
                {
                    allOptions.Add(buttons[i].Title);
                }

                for (int i = 0; i < allOptions.Count; i++)
                {
                    List<XElement> optionList = (from t in data.Element("Root").Element("Options").Descendants("Option")
                                                    where t.Element("name").Value == allOptions[i]
                                                    select t).ToList<XElement>();

                    if (optionList.Count > 0)
                    {
                        for (int j = 0; j < buttons.Count; j++)
                        {
                            if (buttons[j].Title == allOptions[i])
                            {
                                buttons[j].Selected = Convert.ToInt32(optionList[0].Element("selected").Value, Globals.culture);
                            }
                        }
                    }
                }
            }
        }
    }
}