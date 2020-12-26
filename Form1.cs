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


namespace NAudioRecordTest
{

    public partial class Form1 : Form
    {
        WaveInEvent waveIn;
        String outputFilePath;
        WaveFileWriter writer;
        float max_v; //for volume
        bool isWriting;
        float TimeDelay;
        DateTime LastWritingDateTime;
        SpeechClient speech;
        RecognitionConfig config;
        String strRecgnResult;
 



        public Form1()
        {
            InitializeComponent();
            var outputFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "NAudio");
            Directory.CreateDirectory(outputFolder);
            outputFilePath = Path.Combine(outputFolder, "recorded.wav");
            waveIn = new WaveInEvent();
            waveIn.WaveFormat = new WaveFormat(44100, 16, 1);
            waveIn.DataAvailable += waveIn_DataAvailable;
            waveIn.RecordingStopped += waveIn_RecordingStopped;

            writer = null;
            max_v = 0;
            isWriting = false;
            TimeDelay = 1; //seconds
            strRecgnResult = "";

            speech = SpeechClient.Create();
            config = new RecognitionConfig
            {
               // Encoding = RecognitionConfig.Types.AudioEncoding.Flac,
                //SampleRateHertz = 16000,
                LanguageCode = LanguageCodes.Russian.Russia
            };



        }

        private void Form1_Load(object sender, EventArgs e)
        {


        }

        void StopRecord()
        {
            waveIn.StopRecording();
            isWriting = false;

        }

        void StartRecord()
        {
            writer = new WaveFileWriter(outputFilePath, waveIn.WaveFormat);
            waveIn.StartRecording();
            LastWritingDateTime = DateTime.Now;
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            ButtonRecord.Enabled = true;
            StopRecord();
        }

        private void ButtonRecord_Click(object sender, EventArgs e)
        {
            StartRecord();
        //    ButtonRecord.Enabled = false;
          //  ButtonStop.Enabled = true;
        }

        void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            TimeSpan ts = TimeSpan.Zero;
            max_v = 0;
            // interpret as 16 bit audio
            for (int index = 0; index < e.BytesRecorded; index += 2)
            {
                short sample = (short)((e.Buffer[index + 1] << 8) |
                                        e.Buffer[index + 0]);
                // to floating point
                var sample32 = sample / 32768f;
                // absolute value 
                if (sample32 < 0) sample32 = -sample32;
                // is this the max value?
                if (sample32 > max_v) max_v = sample32;
            }

            ts = DateTime.Now - LastWritingDateTime;

            if (max_v > 0.1 || (isWriting & ts.Seconds < TimeDelay))
            {
                writer.Write(e.Buffer, 0, e.BytesRecorded);
                if (max_v > 0.1)
                    LastWritingDateTime = DateTime.Now;
                if (!isWriting)
                    isWriting = true;
            }
            else if (isWriting & (max_v <= 0.1 & ts.Seconds >= TimeDelay))
            {
                  StopRecord();
            }

            update_volume_progress_bar();
        }

        void update_volume_progress_bar()
        {
            if (progressBarVolume.InvokeRequired)
                progressBarVolume.Invoke(new Action(update_volume_progress_bar));
            else
                progressBarVolume.Value = Convert.ToInt32(100 * max_v);
        }


        void waveIn_RecordingStopped(object sender, EventArgs e)
        {
            waveIn.Dispose();
            writer.Close();
            writer = null;
            SpeechToText();
        }

        void SpeechToText()
        {
            strRecgnResult = "";
            var audio = RecognitionAudio.FromFile(outputFilePath);

            var response = speech.Recognize(config, audio);

            foreach (var result in response.Results)
            {
                foreach (var alternative in result.Alternatives)
                {
                    strRecgnResult += ((alternative.Transcript) + "...");
                }
            }

            RecognizeResultTextBox.Invoke((MethodInvoker)delegate { RecognizeResultTextBox.AppendText(RecognizeResultTextBox.Text + strRecgnResult + " \r\n"); });

        }
    }
}

