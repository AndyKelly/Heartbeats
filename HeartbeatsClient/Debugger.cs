using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeartbeatsClient
{
    /// <summary>
    /// The Debugger singleton
    /// </summary>
    public sealed class Debugger
    {
        /// <summary>
        /// The singleton instance
        /// </summary>
        static readonly Debugger _instance = new Debugger();

        /// <summary>
        /// The cache of Debug strings
        /// </summary>
        static List<string> DebugStringList = new List<string>();

        public static Debugger Instance
        {
            get
            {
                return _instance;
            }
        }

        Debugger()
        {
        }

        /// <summary>
        /// Adds a string to the DebugStringList, provided it is unique
        /// If the list is larger than 20 elements then it will remove the first element from the list and add another to the end
        /// </summary>
        /// <param name="StringToAdd">The string which will be added to the list</param>
        public static void AddStringToDebugger(string StringToAdd)
        {
            //No need to display non unique errors
            if (!DebugStringList.Contains(StringToAdd))
            {
                //Debug string list is capped at 20 elements
                if (DebugStringList.Count >= 20)
                {
                    DebugStringList.RemoveAt(0);
                }
                
                DebugStringList.Add(StringToAdd);
            }
        }

        /// <summary>
        /// Returns a list containing debug strings
        /// </summary>
        /// <returns>A list of strings</returns>
        public static List<string> RetrieveDebugStrings()
        {
            return DebugStringList;
        }
    }
}
