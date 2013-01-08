using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Diagnostics;

namespace HeartbeatsClient
{
    class AudioManager
    {
        /// <summary>
        /// A list of all beats availible
        /// </summary>
        List<Beat> m_BeatsList;

        /// <summary>
        /// The mood manager object, responsible for telling the 
        /// </summary>
        MoodManager m_MoodManager;

        /// <summary>
        /// The xml parser object, responsible for reading an xml file and returning beat objects
        /// </summary>
        XmlParser m_XmlParser;

        /// <summary>
        /// The audio player object, responsible for playing and changing tracks given passed moods
        /// </summary>
        AudioPlayer m_AudioPlayer;

        /// <summary>
        /// Flag used to determine whether this object has been initalised properly
        /// </summary>
        bool Initalised = false;

        /// <summary>
        /// The audio timer is used to pass info to the AudioPlayer and Moodrequestor at regular intervals
        /// </summary>
        private Timer m_AudioTimer;

        /// <summary>
        /// Default constructor for the AudioManager object, initalises the XmlParser
        /// </summary>
        public AudioManager()
        {
            m_XmlParser = new XmlParser();
        }

        /// <summary>
        /// Destructor for the AudioManager, disposes of the AudioTimer if it has been initalised
        /// </summary>
        ~AudioManager()
        {
            if (Initalised)
            {
                m_AudioTimer.Dispose();
            }
        }

        /// <summary>
        /// Late initalisation function for the AudioManager
        /// Tries to init the XmlParser, if it succeeds then we can init each other object that relies on it
        /// </summary>
        /// <param name="ServerURL">The URL of the heartbeats device</param>
        /// <param name="XmlFileDir">The directory of the heartbeats xml file</param>
        /// <returns>True if sucessful, false if not</returns>
        public bool Init(string ServerURL, string XmlFileDir)
        {
            if (!Initalised)
            {
                if (m_XmlParser.Init(XmlFileDir))
                {
                    Initalised = true;
                    m_BeatsList = m_XmlParser.GetBeatsList();
                    m_MoodManager = new MoodManager(ServerURL);
                    m_AudioPlayer = new AudioPlayer(GetBeatByMood(Moods.Low));
                    m_AudioTimer = new Timer();
                    m_AudioTimer.Elapsed +=new ElapsedEventHandler(m_AudioTimer_Elapsed);
                    //1000 milliseconds, one second
                    m_AudioTimer.Interval = 1000;
                    m_AudioTimer.Start();
                    Debugger.AddStringToDebugger("Sucessfully initalised the Audio Manager, beginging timed Check event");
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// The on tick method for a the timer object 
        /// </summary>
        /// <param name="sender">The object that fired the event</param>
        /// <param name="e">The event arguments</param>
        void  m_AudioTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            m_AudioPlayer.Tick(m_AudioTimer.Interval);
            GetMoodAndPassBeatToAudioPlayer();
        }

        /// <summary>
        /// Retrieves a mood from the server
        /// Passes a beat corresponding to that mood to the audio player
        /// </summary>
        private void GetMoodAndPassBeatToAudioPlayer()
        {
            //Get Mood
            Moods m_CurrentMood = m_MoodManager.GetMoodfromServer();
            //Add a track that matches the current mood
            m_AudioPlayer.TryPlayBeat(GetBeatByMood(m_CurrentMood));
        }
        
        /// <summary>
        /// Takes a mood and find a beat which matches that mood
        /// </summary>
        /// <param name="Mood">The mood to use while finding a beat</param>
        /// <returns>A beat with the mood passed, if a beat cant be found
        /// then a beat with a null directory is returned and an error is surface to the debugger</returns>
        private Beat GetBeatByMood(Moods Mood)
        {
            List<Beat> BeatCandidates = new List<Beat>();
            //Each beat which matches the passed mood is added to the BeatCandidates list
            foreach (Beat ThisBeat in m_BeatsList)
            {
                if (ThisBeat.GetMood() == Mood)
                {
                    BeatCandidates.Add(ThisBeat);
                }
            }

            //If we found a few then lets return a random song
            if (BeatCandidates.Count > 0)
            {
                Random Rand = new Random();
                return BeatCandidates[Rand.Next(0, (BeatCandidates.Count))];
            }
            else
            {
                Debugger.AddStringToDebugger("Either the BeatCandidates list or the Beats list was empty, is your XML file empty?");
                return new Beat("", Moods.High);
            }

        }
    }
}
