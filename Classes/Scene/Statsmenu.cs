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
    public class Statsmenu : Scene
    {
        Texture2D bkg;

        //List<OptionsButton> buttons = new List<OptionsButton>();

        XDocument xml;

        public Statsmenu()
        {
            bkg = Globals.Content.Load<Texture2D>("Sprites/Misc/Stats");

            xml = Globals.save.GetFile("xml\\options.xml");

            //LoadData(xml);
        }

        public override void Update()
        {
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
        }

        /*
        public void SaveOptions(object info)
        {
            XDocument xml = new XDocument(new XElement("Root",
                                                new XElement("Options", "")));

            for (int i = 0; i < buttons.Count; i++)
            {
                xml.Element("Root").Element("Options").Add(buttons[i].ReturnToXML());
            }

            Globals.save.HandleSaveFormates(xml, "options.xml");

            Globals.gamestate = Gamestate.Mainmenu;
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
        */
    }
}