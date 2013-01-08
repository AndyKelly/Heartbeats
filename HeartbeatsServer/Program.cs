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
    public partial class Program
    {
        /// <summary>
        /// Set to true when the pulse oximeter is attached & false otherwise
        /// </summary>
        public bool ProbeAttached = false;

        /// <summary>
        /// The networking webevent used to perform http requests
        /// </summary>
        GT.Networking.WebEvent SendBioFeedback;

        /// <summary>
        /// The bio data which is parsed and sent via http to a client
        /// </summary>
        public BioData CurrentReading;

        /// <summary>
        /// Entry point for the application
        /// Initalises the CurrentReading BioData object and creates event handlers
        /// </summary>
        void ProgramStarted()
        {
            Debug.Print("Program Started");
            CurrentReading = new BioData();
            led.TurnGreen();
            
            pulseOximeter.ProbeAttached += new PulseOximeter.ProbeAttachedHandler(pulseOximeter_ProbeAttached);
            pulseOximeter.ProbeDetached += new PulseOximeter.ProbeDetachedHandler(pulseOximeter_ProbeDetached);
            pulseOximeter.Heartbeat += new PulseOximeter.HeartbeatHandler(pulseOximeter_Heartbeat);
            button.ButtonPressed += new Button.ButtonEventHandler(button_ButtonPressed);
           
        }

        /// <summary>
        /// Button event handler, 
        /// Upon a press, the ethernet device is setup with a static IP
        /// Events are created for the network being up and down.
        /// </summary>
        /// <param name="sender">The button object that firred the event</param>
        /// <param name="state">The state in which the above mentioned button is in</param>
        void button_ButtonPressed(Button sender, Button.ButtonState state)
        {
            ethernet.UseStaticIP("192.168.1.22", "255.255.255.0", "192.168.1.254");
            ethernet.NetworkUp += new GTM.Module.NetworkModule.NetworkEventHandler(ethernet_NetworkUp);
            ethernet.NetworkDown += new GTM.Module.NetworkModule.NetworkEventHandler(ethernet_NetworkDown);
        }

        /// <summary>
        /// An event which is fired when the network is down, turns the status led red
        /// </summary>
        /// <param name="sender">The network module that fired the event</param>
        /// <param name="state">The state in which the network module is in</param>
        void ethernet_NetworkDown(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state)
        {
            led.TurnRed();
        }

        /// <summary>
        /// An event which is fired when the network is up, turns the status led blue, starts a http server operation on port 80
        /// Creates the web event bio, acessed via http://192.168.1.22:80/bio and its response handler 
        /// </summary>
        /// <param name="sender">The network module that fired the event</param>
        /// <param name="state">The state the network module was in</param>
        void ethernet_NetworkUp(GTM.Module.NetworkModule sender, GTM.Module.NetworkModule.NetworkState state)
        {
            led.TurnBlue();
            WebServer.StartLocalServer(ethernet.NetworkSettings.IPAddress, 80);
            //http://192.168.1.22:80/bio
            SendBioFeedback = WebServer.SetupWebEvent("bio");
            SendBioFeedback.WebEventReceived += new WebEvent.ReceivedWebEventHandler(SendBioFeedback_WebEventReceived);
        }
        
        /// <summary>
        /// The response event for http://192.168.1.22:80/bio,
        /// Serialisese the bpm from the latest reading to a string, converts it to a byte array and passes it to the responder
        /// </summary>
        /// <param name="path">http://192.168.1.22:80/bio</param>
        /// <param name="method">Enum that represents the supported http methods, i.e. post, get, delete, put</param>
        /// <param name="responder">An object which responds to the request</param>
        void SendBioFeedback_WebEventReceived(string path, WebServer.HttpMethod method, Responder responder)
        {
            string content = CurrentReading.SerialiseBpm();
            byte[] bytes = new System.Text.UTF8Encoding().GetBytes(content);
            responder.Respond(bytes, "text/html");
        }

        /// <summary>
        /// Checks if the probe is attached, if it is, the reading object is used to set the currentReading object
        /// and led is pulsed also to show the user that the pulse is being read
        /// </summary>
        /// <param name="sender">The object that fired the event</param>
        /// <param name="reading">The reading taken by the object</param>
        void pulseOximeter_Heartbeat(PulseOximeter sender, PulseOximeter.Reading reading)
        {
            PulseDebugLED();
            if (ProbeAttached)
            {
                CurrentReading.Init(reading);
                lED7R.Animate(reading.PulseRate, true, true, false);
                Debug.Print("heartrate: " + reading.PulseRate + " SPO2 " + reading.SPO2 + " signal " + reading.SignalStrength);
                Debug.Print("Serialised string :" + CurrentReading.SerialiseBpm());
            }
        }

        /// <summary>
        /// Sets the global flag which verifys the probe is attached
        /// </summary>
        /// <param name="sender">The object that fired the event</param>
        void pulseOximeter_ProbeDetached(PulseOximeter sender)
        {
            Debug.Print("Probe Detached");
            ProbeAttached = false;
        }

        /// <summary>
        /// Sets the global flag which verifys the probe is not attached
        /// </summary>
        /// <param name="sender">The object that fired the event</param>
        void pulseOximeter_ProbeAttached(PulseOximeter sender)
        {
            Debug.Print("Probe Atttached, begining to take vitals");
            ProbeAttached = true;
        }

    }
}
