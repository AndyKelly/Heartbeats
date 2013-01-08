using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Touch;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;
using Gadgeteer.Modules.Seeed;

namespace aPriori_Client
{
    /// <summary>
    /// Bio data objects are created as a simple method of cacheing the needed data(Pulse rate) recieved from the pulse oximeter.
    /// </summary>
    public class BioData
    {
        /// <summary>
        /// The beats per minute
        /// </summary>
        int m_BPM = 0;


        public void Init(PulseOximeter.Reading Reading)
        {
            m_BPM = Reading.PulseRate;
        }

        /// <summary>
        /// Serialises the data held within this object and returns a string
        /// </summary>
        /// <returns>A string containing all tne data within the object with semi colon delimiters</returns>
        public string SerialiseBpm()
        {
            return m_BPM.ToString();
        }

        /// <summary>
        /// Getter for BPM
        /// </summary>
        /// <returns>m_BPM</returns>
        public int GetBPM()
        {
            return m_BPM;
        }

    }
}
