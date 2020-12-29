using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using NAudio;
using NAudio.Wave;
using NAudio.FileFormats;
using NAudio.CoreAudioApi;
using System.IO;
using Google.Cloud.Speech.V1;


namespace VoiceMaganerLib
{
    class VoiceManager
    {

        private WaveInEvent waveIn;
        private String outputFilePath;
        private WaveFileWriter writer;
        private float max_v; //for volume
        private bool isWriting;
        private float TimeDelay;
        private DateTime LastWritingDateTime;
        private SpeechClient speech;
        private RecognitionConfig config;
        private String strRecgnResult;
        private float IdleTimeAmount;
        private bool RestatAfterStopped;


        public VoiceManager()
        {
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
            IdleTimeAmount = 10; //seconds
            RestatAfterStopped = true;

            speech = SpeechClient.Create();
            config = new RecognitionConfig
            {
                LanguageCode = LanguageCodes.Russian.Russia
            };


        }

         ~VoiceManager()
        {
            DisableSystem();
        }

        public void DisableSystem()
        {
            RestatAfterStopped = true;
            isWriting = false;
            waveIn.StopRecording();

        }

        private void StopRecord()
        {
            waveIn.StopRecording();
  
        }

        public void StartRecord()
        {
            writer = new WaveFileWriter(outputFilePath, waveIn.WaveFormat);
            isWriting = false;
            waveIn.StartRecording();
            LastWritingDateTime = DateTime.Now;
            strRecgnResult = "";
            RestatAfterStopped = true;
            max_v = 0.0f;
        }

        public string GetRecognitionResult()
        {
            return strRecgnResult;
        }

        public void ClearRecognitionResult()
        {
            strRecgnResult = "";
        }

        public float GetRecordVolume()
        {
            return max_v;
        }

        private void waveIn_RecordingStopped(object sender, EventArgs e)

        {
            waveIn.Dispose();
            writer.Close();
            writer = null;
            if (this.isWriting)
                SpeechToText();
//            else if (!this.RestatAfterStopped)
//                StartRecord();
        }


        private void waveIn_DataAvailable(object sender, WaveInEventArgs e)
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
            else if (!isWriting & ts.Seconds >= IdleTimeAmount)// если молчание длиится долго, перезапускаем Запись
            {
                StopRecord();

            }

        }
        private void SpeechToText()
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

          //  StartRecord();
        }

    }
}
