using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeartbeatsClient
{
    /// <summary>
    /// The mood enum defines the types of moods a beat can be.
    /// </summary>
    enum Moods { High, Mid, Low };

    class Beat
    {
        /// <summary>
        /// The mood this beat is.
        /// </summary>
        Moods m_Mood;

        /// <summary>
        /// The directory location of the Beat's sound file
        /// </summary>
        string m_Location;

        /// <summary>
        /// Default constructor for a beat object
        /// </summary>
        /// <param name="Location">The directory location of this sound file</param>
        /// <param name="Mood">The mood associated with this beat</param>
        public Beat(string Location, Moods Mood)
        {
            m_Location = Location;
            m_Mood = Mood;
        }

        /// <summary>
        /// Retrieves the directory location associated with this instance
        /// </summary>
        /// <returns>m_Location</returns>
        public string GetLocation()
        {
            return m_Location;
        }

        /// <summary>
        /// Retrieves the mood associated with this instance
        /// </summary>
        /// <returns>m_Mood</returns>
        public Moods GetMood()
        {
            return m_Mood;
        }
    }
}
