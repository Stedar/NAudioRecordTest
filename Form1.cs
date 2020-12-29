using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio;
using NAudio.Wave;
using NAudio.FileFormats;
using NAudio.CoreAudioApi;
using System.IO;
using Google.Cloud.Speech.V1;
using VoiceMaganerLib;
using System.Timers;

namespace NAudioRecordTest
{

    public partial class Form1 : Form
    {
        VoiceManager voiceManager;
        System.Timers.Timer aTimer;
        System.Timers.Timer micvolumeTimer;
        bool ResultReceived;



        public Form1()
        {
            InitializeComponent();
            voiceManager = new VoiceManager();
            aTimer = new System.Timers.Timer(10);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;

            micvolumeTimer = new System.Timers.Timer(400);
            micvolumeTimer.Elapsed += OnMicTimedEvent;
            micvolumeTimer.AutoReset = true;
            micvolumeTimer.Enabled = true;


        }

        void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
   
            if (ResultReceived ==false)
            {
                String Result = this.voiceManager.GetRecognitionResult();

                if (Result!= "")
                {
                    ResultReceived = true;

                    RecognizeResultTextBox.Invoke((MethodInvoker)delegate { RecognizeResultTextBox.AppendText(Result + " \r\n"); });
                    this.voiceManager.ClearRecognitionResult();
                }
                //здесь делаем какие-то действия в 1с помтом возобновляем запись

                // this.voiceManager.StartRecord();
            }

        }

        void OnMicTimedEvent(Object source, ElapsedEventArgs e)
        {
            update_volume_progress_bar();
        }

        private void Form1_Load(object sender, EventArgs e)
        {


        }


        private void ButtonStop_Click(object sender, EventArgs e)
        {
            voiceManager.DisableSystem();
        }

        private void ButtonRecord_Click(object sender, EventArgs e)
        {
            voiceManager.StartRecord();
            ResultReceived = false;

        }


        void update_volume_progress_bar()
        {
           float max_v = this.voiceManager.GetRecordVolume();
            if (progressBarVolume.InvokeRequired)
                progressBarVolume.Invoke(new Action(update_volume_progress_bar));
            else
                progressBarVolume.Value = Convert.ToInt32(100 * max_v);
        }



    }
}

