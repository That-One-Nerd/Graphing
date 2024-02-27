namespace Graphing.Forms
{
    partial class SetZoomForm
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
            MessageLabel = new Label();
            ZoomTrackBar = new TrackBar();
            ValueLabel = new Label();
            ZoomMinValue = new TextBox();
            ZoomMaxValue = new TextBox();
            ((System.ComponentModel.ISupportInitialize)ZoomTrackBar).BeginInit();
            SuspendLayout();
            // 
            // MessageLabel
            // 
            MessageLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            MessageLabel.Location = new Point(52, 20);
            MessageLabel.Name = "MessageLabel";
            MessageLabel.Size = new Size(413, 35);
            MessageLabel.TabIndex = 0;
            MessageLabel.Text = "Set the zoom level for the graph.";
            MessageLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ZoomTrackBar
            // 
            ZoomTrackBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ZoomTrackBar.LargeChange = 1000;
            ZoomTrackBar.Location = new Point(12, 127);
            ZoomTrackBar.Maximum = 10000;
            ZoomTrackBar.Name = "ZoomTrackBar";
            ZoomTrackBar.Size = new Size(489, 90);
            ZoomTrackBar.TabIndex = 1;
            ZoomTrackBar.TickStyle = TickStyle.None;
            ZoomTrackBar.Scroll += ZoomTrackBar_Scroll;
            // 
            // ValueLabel
            // 
            ValueLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            ValueLabel.Location = new Point(52, 91);
            ValueLabel.Name = "ValueLabel";
            ValueLabel.Size = new Size(413, 33);
            ValueLabel.TabIndex = 2;
            ValueLabel.Text = "1.00x";
            ValueLabel.TextAlign = ContentAlignment.TopCenter;
            // 
            // ZoomMinValue
            // 
            ZoomMinValue.Location = new Point(12, 178);
            ZoomMinValue.Name = "ZoomMinValue";
            ZoomMinValue.Size = new Size(83, 39);
            ZoomMinValue.TabIndex = 3;
            ZoomMinValue.Text = "0.50";
            ZoomMinValue.TextChanged += ZoomMinValue_TextChanged;
            // 
            // ZoomMaxValue
            // 
            ZoomMaxValue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ZoomMaxValue.Location = new Point(418, 178);
            ZoomMaxValue.Name = "ZoomMaxValue";
            ZoomMaxValue.Size = new Size(83, 39);
            ZoomMaxValue.TabIndex = 4;
            ZoomMaxValue.Text = "2.00";
            ZoomMaxValue.TextAlign = HorizontalAlignment.Right;
            ZoomMaxValue.TextChanged += ZoomMaxValue_TextChanged;
            // 
            // SetZoomForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(513, 230);
            Controls.Add(ZoomMaxValue);
            Controls.Add(ZoomMinValue);
            Controls.Add(ValueLabel);
            Controls.Add(ZoomTrackBar);
            Controls.Add(MessageLabel);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "SetZoomForm";
            Text = "Zoom Level";
            ((System.ComponentModel.ISupportInitialize)ZoomTrackBar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label MessageLabel;
        private TrackBar ZoomTrackBar;
        private Label ValueLabel;
        private TextBox ZoomMinValue;
        private TextBox ZoomMaxValue;
    }
}