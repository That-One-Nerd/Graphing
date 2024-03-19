using System.Drawing;
using System.Windows.Forms;

namespace Graphing.Forms
{
    partial class GraphColorPickerForm
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
            PresetButtonPanel = new Panel();
            RgbSliders = new Panel();
            BlueValueBox = new TextBox();
            GreenValueBox = new TextBox();
            RedValueBox = new TextBox();
            BlueTrackBar = new TrackBar();
            RedTrackBar = new TrackBar();
            GreenTrackBar = new TrackBar();
            ResultView = new Panel();
            BottomPanel = new Panel();
            OkButton = new Button();
            CancelButton = new Button();
            RgbSliders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)BlueTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RedTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)GreenTrackBar).BeginInit();
            BottomPanel.SuspendLayout();
            SuspendLayout();
            // 
            // MessageLabel
            // 
            MessageLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            MessageLabel.AutoEllipsis = true;
            MessageLabel.Location = new Point(84, 28);
            MessageLabel.Margin = new Padding(75, 28, 75, 28);
            MessageLabel.Name = "MessageLabel";
            MessageLabel.Size = new Size(375, 101);
            MessageLabel.TabIndex = 0;
            MessageLabel.Text = "Pick a color for GRAPHABLE NAME HERE";
            MessageLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // PresetButtonPanel
            // 
            PresetButtonPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            PresetButtonPanel.AutoScroll = true;
            PresetButtonPanel.Location = new Point(12, 160);
            PresetButtonPanel.Name = "PresetButtonPanel";
            PresetButtonPanel.Size = new Size(114, 334);
            PresetButtonPanel.TabIndex = 1;
            // 
            // RgbSliders
            // 
            RgbSliders.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            RgbSliders.Controls.Add(BlueValueBox);
            RgbSliders.Controls.Add(GreenValueBox);
            RgbSliders.Controls.Add(RedValueBox);
            RgbSliders.Controls.Add(BlueTrackBar);
            RgbSliders.Controls.Add(RedTrackBar);
            RgbSliders.Controls.Add(GreenTrackBar);
            RgbSliders.Controls.Add(ResultView);
            RgbSliders.Location = new Point(152, 160);
            RgbSliders.Name = "RgbSliders";
            RgbSliders.Size = new Size(379, 334);
            RgbSliders.TabIndex = 2;
            // 
            // BlueValueBox
            // 
            BlueValueBox.Anchor = AnchorStyles.Right;
            BlueValueBox.Location = new Point(154, 257);
            BlueValueBox.MaxLength = 3;
            BlueValueBox.Name = "BlueValueBox";
            BlueValueBox.Size = new Size(48, 39);
            BlueValueBox.TabIndex = 6;
            BlueValueBox.TextAlign = HorizontalAlignment.Right;
            BlueValueBox.TextChanged += BlueValueBox_TextChanged;
            // 
            // GreenValueBox
            // 
            GreenValueBox.Anchor = AnchorStyles.Right;
            GreenValueBox.Location = new Point(154, 167);
            GreenValueBox.MaxLength = 3;
            GreenValueBox.Name = "GreenValueBox";
            GreenValueBox.Size = new Size(48, 39);
            GreenValueBox.TabIndex = 5;
            GreenValueBox.TextAlign = HorizontalAlignment.Right;
            GreenValueBox.TextChanged += GreenValueBox_TextChanged;
            // 
            // RedValueBox
            // 
            RedValueBox.Anchor = AnchorStyles.Right;
            RedValueBox.Location = new Point(154, 77);
            RedValueBox.MaxLength = 3;
            RedValueBox.Name = "RedValueBox";
            RedValueBox.Size = new Size(48, 39);
            RedValueBox.TabIndex = 4;
            RedValueBox.Text = "288";
            RedValueBox.TextAlign = HorizontalAlignment.Right;
            RedValueBox.TextChanged += RedValueBox_TextChanged;
            // 
            // BlueTrackBar
            // 
            BlueTrackBar.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            BlueTrackBar.Cursor = Cursors.SizeWE;
            BlueTrackBar.LargeChange = 25;
            BlueTrackBar.Location = new Point(3, 212);
            BlueTrackBar.Maximum = 255;
            BlueTrackBar.Name = "BlueTrackBar";
            BlueTrackBar.Size = new Size(215, 90);
            BlueTrackBar.TabIndex = 3;
            BlueTrackBar.TickStyle = TickStyle.None;
            BlueTrackBar.Scroll += BlueTrackBar_Scroll;
            // 
            // RedTrackBar
            // 
            RedTrackBar.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            RedTrackBar.Cursor = Cursors.SizeWE;
            RedTrackBar.LargeChange = 25;
            RedTrackBar.Location = new Point(3, 32);
            RedTrackBar.Maximum = 255;
            RedTrackBar.Name = "RedTrackBar";
            RedTrackBar.Size = new Size(215, 90);
            RedTrackBar.TabIndex = 2;
            RedTrackBar.TickStyle = TickStyle.None;
            RedTrackBar.Scroll += RedTrackBar_Scroll;
            // 
            // GreenTrackBar
            // 
            GreenTrackBar.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            GreenTrackBar.Cursor = Cursors.SizeWE;
            GreenTrackBar.LargeChange = 25;
            GreenTrackBar.Location = new Point(3, 122);
            GreenTrackBar.Maximum = 255;
            GreenTrackBar.Name = "GreenTrackBar";
            GreenTrackBar.Size = new Size(215, 90);
            GreenTrackBar.TabIndex = 1;
            GreenTrackBar.TickStyle = TickStyle.None;
            GreenTrackBar.Scroll += GreenTrackBar_Scroll;
            // 
            // ResultView
            // 
            ResultView.Anchor = AnchorStyles.Right;
            ResultView.Location = new Point(224, 105);
            ResultView.Name = "ResultView";
            ResultView.Size = new Size(125, 125);
            ResultView.TabIndex = 0;
            // 
            // BottomPanel
            // 
            BottomPanel.BackColor = SystemColors.Window;
            BottomPanel.Controls.Add(OkButton);
            BottomPanel.Controls.Add(CancelButton);
            BottomPanel.Dock = DockStyle.Bottom;
            BottomPanel.Location = new Point(0, 517);
            BottomPanel.Margin = new Padding(0);
            BottomPanel.Name = "BottomPanel";
            BottomPanel.Size = new Size(543, 64);
            BottomPanel.TabIndex = 3;
            // 
            // OkButton
            // 
            OkButton.Anchor = AnchorStyles.Right;
            OkButton.Location = new Point(220, 9);
            OkButton.Margin = new Padding(0);
            OkButton.Name = "OkButton";
            OkButton.Size = new Size(150, 46);
            OkButton.TabIndex = 1;
            OkButton.Text = "OK";
            OkButton.UseVisualStyleBackColor = true;
            OkButton.Click += OkButton_Click;
            // 
            // CancelButton
            // 
            CancelButton.Anchor = AnchorStyles.Right;
            CancelButton.Location = new Point(384, 9);
            CancelButton.Margin = new Padding(0);
            CancelButton.Name = "CancelButton";
            CancelButton.Size = new Size(150, 46);
            CancelButton.TabIndex = 0;
            CancelButton.Text = "Cancel";
            CancelButton.UseVisualStyleBackColor = true;
            CancelButton.Click += CancelButton_Click;
            // 
            // GraphColorPickerForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(543, 581);
            Controls.Add(BottomPanel);
            Controls.Add(RgbSliders);
            Controls.Add(PresetButtonPanel);
            Controls.Add(MessageLabel);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            MaximumSize = new Size(1000, 1000);
            MinimumSize = new Size(450, 577);
            Name = "GraphColorPickerForm";
            Text = "GraphColorPickerForm";
            RgbSliders.ResumeLayout(false);
            RgbSliders.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)BlueTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)RedTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)GreenTrackBar).EndInit();
            BottomPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Label MessageLabel;
        private Panel PresetButtonPanel;
        private Panel RgbSliders;
        private Panel ResultView;
        private TrackBar GreenTrackBar;
        private TrackBar BlueTrackBar;
        private TrackBar RedTrackBar;
        private Panel BottomPanel;
        private Button CancelButton;
        private Button OkButton;
        private TextBox RedValueBox;
        private TextBox BlueValueBox;
        private TextBox GreenValueBox;
    }
}