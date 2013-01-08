using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace HeartbeatsClient
{
    class MoodRequester
    {
        /// <summary>
        /// The URL of the surver to make requests on. Set within the constructor.
        /// </summary>
        string m_ServerURL;

        /// <summary>
        /// Default constructor for a MoodRequestor object,
        /// Validates and assignes the passed server URL
        /// </summary>
        public MoodRequester(string ServerURL)
        {
            Debugger.AddStringToDebugger("MoodRequester initalised using the URL:" + ServerURL);
            m_ServerURL = ValidateURL(ServerURL);
        }

        /// <summary>
        /// Tries to perform a get request on a specified URL. 
        /// The heartbeats device should return a string such as "88"
        /// This function atempts to parse the string returned from the get request to a float and return it
        /// </summary>
        /// <returns>A float which holds the value of a users heartrate, when a sucessful get request on the correct address occurs</returns>
        public float GetBpmFromServer()
        {
            WebRequest wrGETURL;
            try
            {
                wrGETURL = WebRequest.Create(m_ServerURL);
                Stream objStream = wrGETURL.GetResponse().GetResponseStream();
                StreamReader objReader = new StreamReader(objStream);
                return ParseStringToFloat(objReader.ReadLine());
            }
            catch (System.Net.WebException)
            {
                Debugger.AddStringToDebugger("Couldnt resolve the specified URL. \nPlease ensure youre specifying the correct URL. \nProceeding with randomly generated values");
                return GetRandomBPM();
            }
            catch (System.UriFormatException)
            {
                Debugger.AddStringToDebugger("The URL you supplied seems to be malformed or empty. \nPlease ensure youre specifying the correct URL. \nProceeding with randomly generated values");
                return GetRandomBPM();
            }

        }

        /// <summary>
        /// Checks if a specified URL contains the string "http://", if so it returns the unchanged url, if not it prepends it
        /// </summary>
        /// <param name="URL">The URL to validate</param>
        /// <returns>The URL with "http://"</returns>
        private string ValidateURL(string URL)
        {
            //If "http://" is within the middle of the string this will break. Not fixing for clarity sake.
            if (URL.Contains("http://"))
            {
                return URL;
            }
            else 
            {
                return "http://" + URL;
            }
        }

        /// <summary>
        /// Creates and returns a pseudo random float between the range 80 - 100
        /// </summary>
        /// <returns>The pseudo random value</returns>
        private float GetRandomBPM()
        {
            Random Rand = new Random();
            return Rand.Next(80, 100);
        }

        /// <summary>
        /// Attempts to parse a string to a float
        /// if the parse attempt is unsucessful a random value is returned. 
        /// </summary>
        /// <param name="StringToParse">The string to be parsed</param>
        /// <returns>The value returned from the string parse operation or a random value if the former is unsucessful</returns>
        private float ParseStringToFloat(string StringToParse)
        {
            try
            {
                return float.Parse(StringToParse);
            }
            catch (System.UriFormatException)
            {
                return GetRandomBPM();
            }
            catch 
            {
                return GetRandomBPM();
            }
        }
    }
}
