using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace HeartbeatsClient
{
    class XmlParser
    {
        /// <summary>
        /// The list containing all beats loaded from the XML file
        /// </summary>
        List<Beat> m_BeatsList;

        /// <summary>
        /// Default constructor for the XMLParser, requires that a Directory be specified
        /// Loads the XML file in the constructor.
        /// </summary>
        /// <param name="Dir">The directory location of the XML file to be loaded</param>
        public XmlParser()
        {

        }

        /// <summary>
        /// Initalisation fucntion
        /// </summary>
        /// <param name="Dir">The directory location of the Heartbeats xml file</param>
        /// <returns>The output of the LoadXML fucntion, either ture(The file was loaded succesfully) or false (The file could not be loaded)</returns>
        public bool Init(string Dir)
        {
            m_BeatsList = new List<Beat>();
            return LoadXML(Dir);
        }

        /// <summary>
        /// Getter function for the beats list
        /// </summary>
        /// <returns></returns>
        public List<Beat> GetBeatsList()
        {
            return m_BeatsList;
        }

        /// <summary>
        /// Attempts to load and operate on an xml file at a specified directory. 
        /// If the try block succeeds each node of type "Beat" is passed to the CreateBeatFromXmlSubtree() function
        /// The return value is then set to true to show that the function has succeeded. 
        /// Otherwise exceptions are caught and info is passed to the Debugger cache
        /// </summary>
        /// <param name="Dir">The directory of the XML file</param>
        private bool LoadXML(string Dir)
        {
            //TODO COmment out
            Dir = "C:\\HeartBeats.xml";
            bool ReturnValue = false;
            try
            {
                XmlReader xmlReader = XmlReader.Create(Dir);
                while (xmlReader.Read())
                {
                    if ((xmlReader.NodeType == XmlNodeType.Element) && (xmlReader.Name == "Beat"))
                    {
                        CreateBeatFromXmlSubtree(xmlReader.ReadSubtree());

                        //Found at least one beat in the XML file, its ok to return a true value now
                        ReturnValue = true;
                    }
                }
            }
            catch (System.ArgumentNullException)
            {
                Debugger.AddStringToDebugger("ArgumentNullException occured while loading the xml file");
            }
            catch (System.Security.SecurityException)
            {
                Debugger.AddStringToDebugger("SecurityException occured while loading the xml file ");
            }
            catch (System.IO.FileNotFoundException)
            {
                Debugger.AddStringToDebugger("FileNotFoundException occured while loading the xml file ");
            }
            catch (System.UriFormatException)
            {
                Debugger.AddStringToDebugger("UriFormatException occured while loading the xml file ");
            }
            catch (System.ArgumentException)
            {
                Debugger.AddStringToDebugger("ArgumentException occured while loading the xml file ");
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                Debugger.AddStringToDebugger("Looks like the xml directory you supplied was malformed");
            }
            return ReturnValue;
        }

        /// <summary>
        /// Reads an Xml Subtree and looks for a Node with the name "Location" and "Mood"
        /// If both nodes are found then a Beat object is created and added to the m_BeatsList
        /// </summary>
        /// <param name="Subtree">The XML subtree to Read</param>
        private void CreateBeatFromXmlSubtree(XmlReader Subtree)
        {
            string Location = "";
            string MoodString = "";

            while (Subtree.Read())
            {
                if (Subtree.NodeType == XmlNodeType.Element)
                {
                    switch(Subtree.Name)
                    {
                        case "Location":
                            Location = Subtree.ReadElementContentAsString();
                            break;
                        case "Mood":
                             MoodString = Subtree.ReadElementContentAsString();
                            break;
                    }
                }
            }

            //Ensure both values have been set
            if (Location != "" && MoodString != "")
            {
                m_BeatsList.Add(new Beat(Location, ParseMood(MoodString)));
            }
        }

        /// <summary>
        /// Parses a string and returns a mood enum.
        /// If the string is malformed or empty then an enum of type "High" is returned by default
        /// </summary>
        /// <param name="StringToParse">The string to parse</param>
        /// <returns>The enum mood</returns>
        private Moods ParseMood(string StringToParse)
        {
            //Default value
            Moods ReturnMood = Moods.High;
            
            if (StringToParse == "LOW")
            {
                ReturnMood = Moods.Low;
            }
            else if (StringToParse == "MID")
            {
                ReturnMood = Moods.Mid;
            }
            else if (StringToParse == "HIGH")
            {
                ReturnMood = Moods.High;
            }

            return ReturnMood;
        }
    }
}
