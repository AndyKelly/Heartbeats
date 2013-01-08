using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace HeartbeatsClient
{
    public partial class HeartbeatsForm : Form
    {
        AudioManager m_AudioManager;

        /// <summary>
        /// Entry point for application
        /// </summary>
        public HeartbeatsForm()
        {
            InitializeComponent();
            m_AudioManager = new AudioManager();
        }

        /// <summary>
        /// The submit button on press event, attempts to initalise the AudioManager
        /// using the Text supplied in the clientAddressTextBox & xmlLocationTextBox
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void SubmitAddress_Click_1(object sender, EventArgs e)
        {
            if (m_AudioManager.Init(clientAddressTextBox.Text, xmlLocationTextBox.Text))
            {
                Debugger.AddStringToDebugger("Correctley initalised the AudioManager");
            }
            else 
            {
                Debugger.AddStringToDebugger("Failed to Inialise the AudioManager");
            }
        }

        /// <summary>
        /// The timer on tick method, this even is used to dump the contents of the debugger singleton
        /// to the debug textbox object.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            List<string> DisplayList = Debugger.RetrieveDebugStrings();
            DebugTextBox.Text = "";
            foreach (string ThisString in DisplayList)
            {
                DebugTextBox.Text += ThisString + "\n";
            }
        }
    }
}
