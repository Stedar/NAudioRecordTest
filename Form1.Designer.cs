namespace NAudioRecordTest
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ButtonStop = new System.Windows.Forms.Button();
            this.ButtonRecord = new System.Windows.Forms.Button();
            this.progressBarVolume = new System.Windows.Forms.ProgressBar();
            this.RecognizeResultTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ButtonStop
            // 
            this.ButtonStop.Location = new System.Drawing.Point(149, 12);
            this.ButtonStop.Name = "ButtonStop";
            this.ButtonStop.Size = new System.Drawing.Size(75, 23);
            this.ButtonStop.TabIndex = 1;
            this.ButtonStop.Text = "Stop";
            this.ButtonStop.UseVisualStyleBackColor = true;
            this.ButtonStop.Visible = false;
            this.ButtonStop.Click += new System.EventHandler(this.ButtonStop_Click);
            // 
            // ButtonRecord
            // 
            this.ButtonRecord.Location = new System.Drawing.Point(68, 12);
            this.ButtonRecord.Name = "ButtonRecord";
            this.ButtonRecord.Size = new System.Drawing.Size(75, 23);
            this.ButtonRecord.TabIndex = 2;
            this.ButtonRecord.Text = "Record";
            this.ButtonRecord.UseVisualStyleBackColor = true;
            this.ButtonRecord.Click += new System.EventHandler(this.ButtonRecord_Click);
            // 
            // progressBarVolume
            // 
            this.progressBarVolume.Location = new System.Drawing.Point(68, 49);
            this.progressBarVolume.Name = "progressBarVolume";
            this.progressBarVolume.Size = new System.Drawing.Size(498, 23);
            this.progressBarVolume.TabIndex = 3;
            // 
            // RecognizeResultTextBox
            // 
            this.RecognizeResultTextBox.Location = new System.Drawing.Point(68, 91);
            this.RecognizeResultTextBox.Multiline = true;
            this.RecognizeResultTextBox.Name = "RecognizeResultTextBox";
            this.RecognizeResultTextBox.Size = new System.Drawing.Size(498, 173);
            this.RecognizeResultTextBox.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.RecognizeResultTextBox);
            this.Controls.Add(this.progressBarVolume);
            this.Controls.Add(this.ButtonRecord);
            this.Controls.Add(this.ButtonStop);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button ButtonStop;
        private System.Windows.Forms.Button ButtonRecord;
        private System.Windows.Forms.ProgressBar progressBarVolume;
        private System.Windows.Forms.TextBox RecognizeResultTextBox;
    }
}

