namespace Graphing.Forms
{
    partial class TranslateForm
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
            TrackX = new System.Windows.Forms.TrackBar();
            LabelX = new System.Windows.Forms.Label();
            MinBoxX = new System.Windows.Forms.TextBox();
            MaxBoxX = new System.Windows.Forms.TextBox();
            ThisValueX = new System.Windows.Forms.TextBox();
            ThisValueY = new System.Windows.Forms.TextBox();
            MaxBoxY = new System.Windows.Forms.TextBox();
            MinBoxY = new System.Windows.Forms.TextBox();
            LabelY = new System.Windows.Forms.Label();
            TrackY = new System.Windows.Forms.TrackBar();
            TitleLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)TrackX).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TrackY).BeginInit();
            SuspendLayout();
            // 
            // TrackX
            // 
            TrackX.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TrackX.LargeChange = 250;
            TrackX.Location = new System.Drawing.Point(15, 193);
            TrackX.Margin = new System.Windows.Forms.Padding(0);
            TrackX.Maximum = 1000;
            TrackX.Name = "TrackX";
            TrackX.Size = new System.Drawing.Size(644, 90);
            TrackX.SmallChange = 50;
            TrackX.TabIndex = 0;
            TrackX.TabStop = false;
            TrackX.TickFrequency = 50;
            TrackX.TickStyle = System.Windows.Forms.TickStyle.Both;
            TrackX.Value = 1;
            TrackX.Scroll += TrackX_Scroll;
            // 
            // LabelX
            // 
            LabelX.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LabelX.Location = new System.Drawing.Point(15, 157);
            LabelX.Name = "LabelX";
            LabelX.Size = new System.Drawing.Size(644, 36);
            LabelX.TabIndex = 1;
            LabelX.Text = "X Offset";
            LabelX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MinBoxX
            // 
            MinBoxX.Location = new System.Drawing.Point(15, 259);
            MinBoxX.Name = "MinBoxX";
            MinBoxX.Size = new System.Drawing.Size(100, 39);
            MinBoxX.TabIndex = 2;
            // 
            // MaxBoxX
            // 
            MaxBoxX.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            MaxBoxX.Location = new System.Drawing.Point(556, 259);
            MaxBoxX.Name = "MaxBoxX";
            MaxBoxX.Size = new System.Drawing.Size(100, 39);
            MaxBoxX.TabIndex = 3;
            MaxBoxX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ThisValueX
            // 
            ThisValueX.Anchor = System.Windows.Forms.AnchorStyles.Top;
            ThisValueX.Location = new System.Drawing.Point(289, 259);
            ThisValueX.Name = "ThisValueX";
            ThisValueX.Size = new System.Drawing.Size(100, 39);
            ThisValueX.TabIndex = 4;
            ThisValueX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ThisValueY
            // 
            ThisValueY.Anchor = System.Windows.Forms.AnchorStyles.Top;
            ThisValueY.Location = new System.Drawing.Point(289, 449);
            ThisValueY.Name = "ThisValueY";
            ThisValueY.Size = new System.Drawing.Size(100, 39);
            ThisValueY.TabIndex = 9;
            ThisValueY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // MaxBoxY
            // 
            MaxBoxY.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            MaxBoxY.Location = new System.Drawing.Point(556, 449);
            MaxBoxY.Name = "MaxBoxY";
            MaxBoxY.Size = new System.Drawing.Size(100, 39);
            MaxBoxY.TabIndex = 8;
            MaxBoxY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // MinBoxY
            // 
            MinBoxY.Location = new System.Drawing.Point(15, 449);
            MinBoxY.Name = "MinBoxY";
            MinBoxY.Size = new System.Drawing.Size(100, 39);
            MinBoxY.TabIndex = 7;
            // 
            // LabelY
            // 
            LabelY.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            LabelY.Location = new System.Drawing.Point(15, 347);
            LabelY.Name = "LabelY";
            LabelY.Size = new System.Drawing.Size(644, 36);
            LabelY.TabIndex = 6;
            LabelY.Text = "Y Offset";
            LabelY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TrackY
            // 
            TrackY.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TrackY.LargeChange = 250;
            TrackY.Location = new System.Drawing.Point(15, 383);
            TrackY.Margin = new System.Windows.Forms.Padding(0);
            TrackY.Maximum = 1000;
            TrackY.Name = "TrackY";
            TrackY.Size = new System.Drawing.Size(644, 90);
            TrackY.SmallChange = 50;
            TrackY.TabIndex = 5;
            TrackY.TabStop = false;
            TrackY.TickFrequency = 50;
            TrackY.TickStyle = System.Windows.Forms.TickStyle.Both;
            TrackY.Value = 1;
            TrackY.Scroll += TrackY_Scroll;
            // 
            // TitleLabel
            // 
            TitleLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TitleLabel.Location = new System.Drawing.Point(12, 39);
            TitleLabel.Name = "TitleLabel";
            TitleLabel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 18);
            TitleLabel.Size = new System.Drawing.Size(644, 89);
            TitleLabel.TabIndex = 10;
            TitleLabel.Text = "Change the Location of %name%";
            TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TranslateForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ClientSize = new System.Drawing.Size(674, 531);
            Controls.Add(TitleLabel);
            Controls.Add(ThisValueY);
            Controls.Add(MaxBoxY);
            Controls.Add(MinBoxY);
            Controls.Add(LabelY);
            Controls.Add(TrackY);
            Controls.Add(ThisValueX);
            Controls.Add(MaxBoxX);
            Controls.Add(MinBoxX);
            Controls.Add(LabelX);
            Controls.Add(TrackX);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Name = "TranslateForm";
            Padding = new System.Windows.Forms.Padding(15);
            Text = "Herm";
            TopMost = true;
            ((System.ComponentModel.ISupportInitialize)TrackX).EndInit();
            ((System.ComponentModel.ISupportInitialize)TrackY).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TrackBar TrackX;
        private System.Windows.Forms.Label LabelX;
        private System.Windows.Forms.TextBox MinBoxX;
        private System.Windows.Forms.TextBox MaxBoxX;
        private System.Windows.Forms.TextBox ThisValueX;
        private System.Windows.Forms.TextBox ThisValueY;
        private System.Windows.Forms.TextBox MaxBoxY;
        private System.Windows.Forms.TextBox MinBoxY;
        private System.Windows.Forms.Label LabelY;
        private System.Windows.Forms.TrackBar TrackY;
        private System.Windows.Forms.Label TitleLabel;
    }
}