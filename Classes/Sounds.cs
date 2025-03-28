using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace ProjektRoguelike
{
    /// <summary>
    /// Class to play sounds.
    /// </summary>
    public class Sounds
    {
        public float musicVolume, sfxVolume, sfxToMusicRatio;

        bool play = true;

        public Sounds()
        {
            LoadData();

            // Set how loud the SFX should be, compared to the music.
            sfxToMusicRatio = 0.5f;
        }

        public void PlaySoundEffect(string filename, bool repeating = false)
        {
            SoundEffect soundToPlay = Globals.Content.Load<SoundEffect>("Sounds/SoundEffect/" + filename);
            var soundInstance = soundToPlay.CreateInstance();

            soundInstance.Volume = sfxVolume * sfxToMusicRatio;
            soundInstance.IsLooped = repeating;
            soundInstance.Play();
        }

        public void PlaySoundEffectOnce(string filename)
        {
            SoundEffect soundToPlay = Globals.Content.Load<SoundEffect>("Sounds/SoundEffect/" + filename);
            var soundInstance = soundToPlay.CreateInstance();

            soundInstance.Volume = sfxVolume * sfxToMusicRatio;

            if (play)
            {
                play = false;
                soundInstance.Play();
            }
            if (soundInstance.State == SoundState.Stopped)
            {
                play = true;               
            }
        }

        public void PlaySong(string filename, bool repeating = false)
        {
            Song songToPlay = Globals.Content.Load<Song>("Sounds/Song/" + filename);
            MediaPlayer.Volume = musicVolume;
            MediaPlayer.IsRepeating = repeating;
            MediaPlayer.Play(songToPlay);
        }

        /*
        public void PlaySong(List<Song> songs, bool repeating = false)
        {
            List<Song> songsToPlay = new List<Song>();
            //SongCollection songstoplay = new SongCollection();

            for (int i = 0; i < songs.Count; i++)
            {
                songsToPlay.Add(songs[i]);
            }

            MediaPlayer.Volume = musicVolume * 5 / 100;
            MediaPlayer.IsRepeating = repeating;
            MediaPlayer.Play(songsToPlay);
        }
        */

        public void StopSong()
        {
            MediaPlayer.Stop();
        }

        public void LoadData()
        {
            XDocument data = Globals.save.GetFile("xml\\options.xml");

            if (data != null)
            {
                List<XElement> optionList = (from t in data.Element("Root").Element("Options").Descendants("Option")
                                           select t).ToList<XElement>();

                musicVolume = Convert.ToInt32(optionList[1].Element("selected").Value, Globals.culture) / 20f;
                sfxVolume = Convert.ToInt32(optionList[2].Element("selected").Value, Globals.culture) / 20f;
            }
        }
    }
}
