namespace Graphing.Forms
{
    partial class SlopeFieldDetailForm
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
            Message = new System.Windows.Forms.Label();
            TrackSlopeDetail = new System.Windows.Forms.TrackBar();
            MinDetailBox = new System.Windows.Forms.TextBox();
            MaxDetailBox = new System.Windows.Forms.TextBox();
            CurrentDetailBox = new System.Windows.Forms.TextBox();
            IncrementButton = new System.Windows.Forms.Button();
            DecrementButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)TrackSlopeDetail).BeginInit();
            SuspendLayout();
            // 
            // Message
            // 
            Message.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            Message.Location = new System.Drawing.Point(119, 25);
            Message.Margin = new System.Windows.Forms.Padding(110);
            Message.Name = "Message";
            Message.Size = new System.Drawing.Size(516, 109);
            Message.TabIndex = 1;
            Message.Text = "Change the Detail of %name%\r\nA higher value means more lines per unit.";
            Message.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TrackSlopeDetail
            // 
            TrackSlopeDetail.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TrackSlopeDetail.LargeChange = 250;
            TrackSlopeDetail.Location = new System.Drawing.Point(59, 158);
            TrackSlopeDetail.Margin = new System.Windows.Forms.Padding(50);
            TrackSlopeDetail.Maximum = 1000;
            TrackSlopeDetail.Name = "TrackSlopeDetail";
            TrackSlopeDetail.Size = new System.Drawing.Size(636, 90);
            TrackSlopeDetail.SmallChange = 0;
            TrackSlopeDetail.TabIndex = 0;
            TrackSlopeDetail.TickFrequency = 0;
            TrackSlopeDetail.TickStyle = System.Windows.Forms.TickStyle.Both;
            TrackSlopeDetail.Scroll += TrackSlopeDetail_Scroll;
            // 
            // MinDetailBox
            // 
            MinDetailBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            MinDetailBox.Location = new System.Drawing.Point(12, 228);
            MinDetailBox.Name = "MinDetailBox";
            MinDetailBox.Size = new System.Drawing.Size(100, 39);
            MinDetailBox.TabIndex = 2;
            // 
            // MaxDetailBox
            // 
            MaxDetailBox.Anchor = System.Windows.Forms.AnchorStyles.Right;
            MaxDetailBox.Location = new System.Drawing.Point(642, 228);
            MaxDetailBox.Name = "MaxDetailBox";
            MaxDetailBox.Size = new System.Drawing.Size(100, 39);
            MaxDetailBox.TabIndex = 3;
            MaxDetailBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // CurrentDetailBox
            // 
            CurrentDetailBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            CurrentDetailBox.Location = new System.Drawing.Point(330, 228);
            CurrentDetailBox.Name = "CurrentDetailBox";
            CurrentDetailBox.Size = new System.Drawing.Size(100, 39);
            CurrentDetailBox.TabIndex = 4;
            CurrentDetailBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // IncrementButton
            // 
            IncrementButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            IncrementButton.Font = new System.Drawing.Font("Segoe UI", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            IncrementButton.Location = new System.Drawing.Point(436, 228);
            IncrementButton.Name = "IncrementButton";
            IncrementButton.Size = new System.Drawing.Size(40, 40);
            IncrementButton.TabIndex = 5;
            IncrementButton.Text = "+";
            IncrementButton.UseVisualStyleBackColor = true;
            IncrementButton.Click += IncrementButton_Click;
            // 
            // DecrementButton
            // 
            DecrementButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            DecrementButton.Font = new System.Drawing.Font("Segoe UI", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            DecrementButton.Location = new System.Drawing.Point(284, 228);
            DecrementButton.Name = "DecrementButton";
            DecrementButton.Size = new System.Drawing.Size(40, 40);
            DecrementButton.TabIndex = 6;
            DecrementButton.Text = "-";
            DecrementButton.UseVisualStyleBackColor = true;
            DecrementButton.Click += DecrementButton_Click;
            // 
            // SlopeFieldDetailForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(754, 282);
            Controls.Add(DecrementButton);
            Controls.Add(IncrementButton);
            Controls.Add(CurrentDetailBox);
            Controls.Add(MaxDetailBox);
            Controls.Add(MinDetailBox);
            Controls.Add(Message);
            Controls.Add(TrackSlopeDetail);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Name = "SlopeFieldDetailForm";
            Text = "Change Slope Field Detail";
            ((System.ComponentModel.ISupportInitialize)TrackSlopeDetail).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label Message;
        private System.Windows.Forms.TrackBar TrackSlopeDetail;
        private System.Windows.Forms.TextBox MinDetailBox;
        private System.Windows.Forms.TextBox MaxDetailBox;
        private System.Windows.Forms.TextBox CurrentDetailBox;
        private System.Windows.Forms.Button IncrementButton;
        private System.Windows.Forms.Button DecrementButton;
    }
}