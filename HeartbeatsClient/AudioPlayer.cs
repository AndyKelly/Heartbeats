using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IrrKlang;
using System.Diagnostics;

namespace HeartbeatsClient
{
    /// <summary>
    /// The audio player acts like a dj, it takes recieves beats from the audio manager, if enough requests for a track of a "High"
    /// mood are aquired then it locks in a "High" track and plays it. 
    /// </summary>
    class AudioPlayer
    {
        /// <summary>
        /// The IrrKlang audio engine
        /// </summary>
        ISoundEngine m_SoundEngine;

        /// <summary>
        /// The ammount of time passed since the last beat has began playing
        /// </summary>
        TimeSpan m_TimePassed;

        /// <summary>
        /// The ammount of time the player waits before changing the beat playing to one of an identical mood
        /// </summary>
        TimeSpan m_ChangeOverPeriod;

        /// <summary>
        /// The ammount of time the player waits before changing the beat playing to one of a different mood
        /// </summary>
        TimeSpan m_ShortChangeOverPeriod;

        /// <summary>
        /// The next track to be played, only allowed to be set when, m_NextTrackLocked is false
        /// </summary>
        Beat m_NextTrack;

        /// <summary>
        /// The currently playing track
        /// </summary>
        Beat m_CurrentTrack;

        /// <summary>
        /// Determines whether the next m_NextTrack object has been confirmed or not
        /// </summary>
        bool m_NextTrackLocked;

        /// <summary>
        /// Counts the number of times a High mood has made it to the audio player,
        /// used to determine whether a track should be locked in
        /// </summary>
        int m_HighPoints;

        /// <summary>
        /// Counts the number of times a Mid mood has made it to the audio player,
        /// used to determine whether a track should be locked in
        /// </summary>
        int m_MidPoints;

        /// <summary>
        /// Counts the number of times a Low mood has made it to the audio player,
        /// used to determine whether a track should be locked in
        /// </summary>
        int m_LowPoints;

        /// <summary>
        /// The number of Points a mood must acrue before it can be locked in.
        /// </summary>
        int m_PointsLimit;

        /// <summary>
        /// Default constructor for the audio player object, sets up inital values
        /// </summary>
        /// <param name="FirstTrack">The first track to play.</param>
        public AudioPlayer(Beat FirstTrack)
        {
            m_HighPoints = 0;
            m_MidPoints = 0;
            m_LowPoints = 0;
            //Within a changeover period of 1min 31sec (91 seconds) there will be 91 readings aprox, readings are taken once a second
            //Therefore a value of 31 as a reading limit makes sense as, there can be a maximum number of (30+30+31) = 91
            m_PointsLimit = 31;
            m_TimePassed = new TimeSpan(0, 0, 0);
            m_ChangeOverPeriod = new TimeSpan(0, 1, 31);
            m_ShortChangeOverPeriod = new TimeSpan(0, 0, 45);
            m_SoundEngine = new ISoundEngine();
            m_NextTrack = FirstTrack;
            m_CurrentTrack = FirstTrack;
            PlayNextBeat();
        }

        /// <summary>
        /// The tick function of the AudioPlayer
        /// Takes a time delta and applies it to the m_TimePassed var
        /// Then determines whether or not a new track should be played
        /// </summary>
        /// <param name="TimeDelta">The time since the last Tick time this function has been called</param>
        public void Tick(double TimeDelta)
        {
            //Increment time passed
            m_TimePassed = m_TimePassed.Add(ConvertTimeDeltaInSecondsToTimeSpan(TimeDelta));

            //Either a new song should be played because the changeover period has elapsed or a track has been locked in
            if (m_TimePassed >= m_ChangeOverPeriod || m_NextTrackLocked)
            {
                //Mood has changed, play the next song after a short changover
                if (m_CurrentTrack.GetMood() != m_NextTrack.GetMood() && m_TimePassed >= m_ShortChangeOverPeriod)
                {
                    PlayNextBeat();
                }
                //No track has been locked in but the changover time has elapsed, play a new song
                else if (m_TimePassed >= m_ChangeOverPeriod)
                {
                    PlayNextBeat();
                }
            }
        }

        /// <summary>
        /// Records the occurance of a certain mood being seen. Increments the m_HighPoints, m_MidPoints, m_LowPoints variables 
        /// depending on the passed enum.
        /// </summary>
        /// <param name="e_CurrentMood">The mood to be incremented</param>
        private void SetPoints(Moods e_CurrentMood)
        {
            Debug.Print("Highpoints at: " + m_HighPoints.ToString() + " & midpoints at: " + m_MidPoints.ToString() + " & lowpoints at: " + m_LowPoints.ToString());
            if (e_CurrentMood == Moods.High)
            {
                m_HighPoints++;
            }
            else if (e_CurrentMood == Moods.Mid)
            {
                m_MidPoints++;
            }
            else if (e_CurrentMood == Moods.Low)
            {
                m_LowPoints++;
            }
        }

        /// <summary>
        /// Attempts to add a track to be played.
        /// If a track has already been locked in then the passed parameter is ignored
        /// Otherwise it records the number of 
        /// </summary>
        /// <param name="BeatToPlay"></param>
        public void TryPlayBeat(Beat BeatToPlay)
        {
            //If a track is locked, it is garunteed to play. All other readings are ignored 
            if (!m_NextTrackLocked)
            {
                SetPoints(BeatToPlay.GetMood());
                TryToLockInBeat(BeatToPlay);
            }
        }

        /// <summary>
        /// Tries to lock a beat in. Checks if each of the points (i.e. m_HighPoints) variables have passed
        /// the threshold (m_PointsLimit). If the passed beat is of that type then we can lock it in. 
        /// </summary>
        /// <param name="BeatToLockIn">The beat object candidate</param>
        private void TryToLockInBeat(Beat BeatToLockIn)
        {
            if (BeatToLockIn.GetMood() == Moods.High && m_HighPoints >= m_PointsLimit)
            {
                m_NextTrack = BeatToLockIn;
                m_NextTrackLocked = true;
                ResetPoints();
            }
            else if (BeatToLockIn.GetMood() == Moods.Mid && m_MidPoints >= m_PointsLimit)
            {
                m_NextTrack = BeatToLockIn;
                m_NextTrackLocked = true;
                ResetPoints();
            }
            else if (BeatToLockIn.GetMood() == Moods.Low && m_LowPoints >= m_PointsLimit)
            {
                m_NextTrack = BeatToLockIn;
                m_NextTrackLocked = true;
                ResetPoints();
            }
            else 
            {
                //Default case, use whatever has been passed but allow other tracks to become locked in in the meantime
                m_NextTrack = BeatToLockIn;
            }
        }

        /// <summary>
        /// Resets each of the point variables to 0
        /// </summary>
        private void ResetPoints()
        {
            m_HighPoints = 0;
            m_MidPoints = 0;
            m_LowPoints = 0;
        }

        /// <summary>
        /// Resets a few utility variables, then trys to tell the Sound engine to play the next track (m_NextTrack) 
        /// </summary>
        private void PlayNextBeat()
        {
            m_NextTrackLocked = false;
            m_TimePassed = new TimeSpan(0, 0, 0);
            m_SoundEngine.StopAllSounds();

            try
            {
                m_SoundEngine.Play2D(m_NextTrack.GetLocation(), false);
            }
            catch 
            {
                Debugger.AddStringToDebugger("Failed to play a the track the the location: " + m_NextTrack.GetLocation());
            }

            m_CurrentTrack = m_NextTrack;
        }

        /// <summary>
        /// Conversion fuction for converting from a double containing a value in seconds to a timeSpan object
        /// </summary>
        /// <param name="TimeDelta"></param>
        /// <returns></returns>
        private TimeSpan ConvertTimeDeltaInSecondsToTimeSpan(double TimeDelta)
        {
            //Todo, exception handeling
            //To do, is this possible?
            //return new TimeSpan(0, int.Parse(TimeDelta.ToString()), 0);
            TimeDelta = TimeDelta / 1000;//Converting from milliseconds to seconds
            return new TimeSpan(0, 0, int.Parse(TimeDelta.ToString()));
        }
    }
}
