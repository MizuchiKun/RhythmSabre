#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion

namespace ProjektRoguelike
{
    public class McTimer
    {
        public bool goodToGo;
        protected int mSec;
        protected TimeSpan timer = new TimeSpan();

        /// <summary>
        /// Creates a timer with a timevalue.<br></br>
        /// </summary>
        /// <param name="m">The time the timer looks for, when testing, if the timer is up.</param>
        public McTimer(int m)
        {
            goodToGo = false;
            mSec = m;
        }

        /// <summary>
        /// Creates a timer with a timevalue and a bool to start some action before updating the timer.<br></br>
        /// </summary>
        /// <param name="m">The time the timer looks for, when testing, if the timer is up.</param>
        /// <param name="STARTLOADED">Bool to skip the timerequirement a single time.</param>
        public McTimer(int m, bool STARTLOADED)
        {
            goodToGo = STARTLOADED;
            mSec = m;
        }

        public int MSec
        {
            get { return mSec; }
            set { mSec = value; }
        }
        public int Timer
        {
            get { return (int)timer.TotalMilliseconds; }
        }


        /// <summary>
        /// Updates the timer.<br></br>
        /// </summary>
        public void UpdateTimer()
        {
            timer += Globals.GameTime.ElapsedGameTime;
        }

        /// <summary>
        /// Updates the timer with a certain speed (.5f would cause time to pass by half as fast).<br></br>
        /// </summary>
        /// <param name="SPEED">The speed, with which time passes by. .5f would casue the time to pass by half as fast</param>
        public void UpdateTimer(float SPEED)
        {
            timer += TimeSpan.FromTicks((long)(Globals.GameTime.ElapsedGameTime.Ticks * SPEED));
        }

        /// <summary>
        /// Adds time to the timer, so it does not need the time it was contructed with, but something lower.<br></br>
        /// </summary>
        /// <param name="MSEC">The time added to the timer, which decreases the time it needs to be done.</param>
        public virtual void AddToTimer(int MSEC)
        {
            timer += TimeSpan.FromMilliseconds((long)(MSEC));
        }

        /// <summary>
        /// Tests for the time since the timer started updating/got reset.<br></br>
        /// </summary>
        /// <returns>True, if the set time for the timer is over, false if not.</returns>
        public bool Test()
        {
            if (timer.TotalMilliseconds >= mSec || goodToGo)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            timer = timer.Subtract(new TimeSpan(0, 0, mSec / 60000, mSec / 1000, mSec % 1000));
            if (timer.TotalMilliseconds < 0)
            {
                timer = TimeSpan.Zero;
            }
            goodToGo = false;
        }

        /// <summary>
        /// Resets the timer and gives it a new time it needs.<br></br>
        /// </summary>
        /// <param name="NEWTIMER">The new timervalue, the timer tests for.</param>
        public void Reset(int NEWTIMER)
        {
            timer = TimeSpan.Zero;
            MSec = NEWTIMER;
            goodToGo = false;
        }

        /// <summary>
        /// Resets the timer to 0, as if the timer was just created.<br></br>
        /// </summary>
        public void ResetToZero()
        {
            timer = TimeSpan.Zero;
            goodToGo = false;
        }

        /// <summary>
        /// Saves the timervalues in an XML file.<br></br>
        /// </summary>
        public virtual XElement ReturnXML()
        {
            XElement xml = new XElement("Timer",
                                    new XElement("mSec", mSec),
                                    new XElement("timer", Timer));



            return xml;
        }

        /// <summary>
        /// Sets the time, which has passed to <paramref name="TIME"/>.<br></br>
        /// </summary>
        /// <param name="TIME">The time that has passed since the timer has started.</param>
        public void SetTimer(TimeSpan TIME)
        {
            timer = TIME;
        }

        /// <summary>
        /// Sets the time, which has passed to <paramref name="TIME"/>.<br></br>
        /// </summary>
        /// <param name="TIME">The time that has passed since the timer has started.</param>
        public virtual void SetTimer(int MSEC)
        {
            timer = TimeSpan.FromMilliseconds((long)(MSEC));
        }
    }
}