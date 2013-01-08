using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeartbeatsClient
{
    /// <summary>
    /// Responsible for determining which mood a reading resides under
    /// This class uses the MoodRequestor to retrieve a float containing a users BPM
    /// Then the class defines a range for High Mid & Low which a bpm can be classed under
    /// </summary>
    class MoodManager
    {
        /// <summary>
        /// The mood requestor object, responsible for getting a BPM float from the server
        /// </summary>
        MoodRequester m_Requestor;

        /// <summary>
        /// The point at which a BPM reading ceases to be Low and becomes Mid or High
        /// </summary>
        float UpperLowThreshold;

        /// <summary>
        /// The point at which a BPM reading ceases to be Mid and becomes High
        /// </summary>
        float UpperMidThreshold;

        /// <summary>
        /// The highest value of BPM read from the User
        /// </summary>
        float HighestBPM;

        /// <summary>
        /// The lowest value of BPM read from the User
        /// </summary>
        float LowestBPM;

        /// <summary>
        /// The default constructor for a MoodManager object, Takes a ServerURL string and passes it to the MoodRequestor
        /// Then uses the requestor to set the HighestBPM and LowestBPM variables to an inital value.
        /// </summary>
        /// <param name="ServerURL">The URL of the server the Requestor should perform get requests on</param>
        public MoodManager(string ServerURL)
        {
            Debugger.AddStringToDebugger("Moodmanager created using the ServerURL: " + ServerURL);
            m_Requestor = new MoodRequester(ServerURL);
            HighestBPM = m_Requestor.GetBpmFromServer();
            LowestBPM = HighestBPM;
            SetThresholds();
        }

        /// <summary>
        /// Retrieves a mood from the server.
        /// Takes a reading from the server, Sets the threshold readings then uses the value,
        /// then classes it as a Mood using the GetMoodFromReading function
        /// </summary>
        /// <returns></returns>
        public Moods GetMoodfromServer()
        {
            float CurrentReading = m_Requestor.GetBpmFromServer();
            CheckForBetterThresholdsReadings(CurrentReading);
            return GetMoodFromReading(CurrentReading);
        }

        /// <summary>
        /// Parses a numerical value (float) to an enum Moods var
        /// </summary>
        /// <param name="Reading">The float to parse</param>
        /// <returns>The mood the value corresponds to</returns>
        public Moods GetMoodFromReading(float Reading)
        {
            if (Reading < UpperLowThreshold)
            {
                return Moods.Low;
            }
            else if (Reading < UpperMidThreshold)
            {
                return Moods.Mid;
            }
            else
            {
                return Moods.High;
            }
        }
        
        /// <summary>
        /// Determines if the thresholds are correct given the new value passed
        /// A reading Higher or Lower than the perviest highest/lowest would indicate that new thresholds must be set
        /// </summary>
        /// <param name="Reading">The value of the reading in bpm</param>
        private void CheckForBetterThresholdsReadings(float Reading)
        {
            if (Reading > HighestBPM)
            {
                HighestBPM = Reading;
                SetThresholds();
            }

            else if (Reading < LowestBPM)
            {
                LowestBPM = Reading;
                SetThresholds();
            }
        }

        /// <summary>
        /// Sets the threshold variables using the Highest and LowestBPM variables
        /// </summary>
        private void SetThresholds()
        {
            float Difference = (HighestBPM - LowestBPM);
            float ThirdOfDifference = Difference / 3;
            UpperLowThreshold = LowestBPM + ThirdOfDifference;
            UpperMidThreshold = LowestBPM + (ThirdOfDifference * 2);
        }
    }
}
