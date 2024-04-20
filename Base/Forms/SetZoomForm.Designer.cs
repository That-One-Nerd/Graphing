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
            EnableBoxSelect = new System.Windows.Forms.Button();
            MatchAspectButton = new System.Windows.Forms.Button();
            ResetButton = new System.Windows.Forms.Button();
            NormalizeButton = new System.Windows.Forms.Button();
            MinBoxX = new System.Windows.Forms.TextBox();
            TextX = new System.Windows.Forms.Label();
            MaxBoxX = new System.Windows.Forms.TextBox();
            MaxBoxY = new System.Windows.Forms.TextBox();
            TextY = new System.Windows.Forms.Label();
            MinBoxY = new System.Windows.Forms.TextBox();
            ViewportLock = new System.Windows.Forms.CheckBox();
            SuspendLayout();
            // 
            // EnableBoxSelect
            // 
            EnableBoxSelect.Location = new System.Drawing.Point(12, 12);
            EnableBoxSelect.Name = "EnableBoxSelect";
            EnableBoxSelect.Size = new System.Drawing.Size(187, 46);
            EnableBoxSelect.TabIndex = 0;
            EnableBoxSelect.Text = "Box Select";
            EnableBoxSelect.UseVisualStyleBackColor = true;
            EnableBoxSelect.Click += EnableBoxSelect_Click;
            // 
            // MatchAspectButton
            // 
            MatchAspectButton.Location = new System.Drawing.Point(12, 64);
            MatchAspectButton.Name = "MatchAspectButton";
            MatchAspectButton.Size = new System.Drawing.Size(187, 46);
            MatchAspectButton.TabIndex = 1;
            MatchAspectButton.Text = "Match Aspect";
            MatchAspectButton.UseVisualStyleBackColor = true;
            MatchAspectButton.Click += MatchAspectButton_Click;
            // 
            // ResetButton
            // 
            ResetButton.Location = new System.Drawing.Point(12, 168);
            ResetButton.Name = "ResetButton";
            ResetButton.Size = new System.Drawing.Size(187, 46);
            ResetButton.TabIndex = 2;
            ResetButton.Text = "Reset";
            ResetButton.UseVisualStyleBackColor = true;
            ResetButton.Click += ResetButton_Click;
            // 
            // NormalizeButton
            // 
            NormalizeButton.Location = new System.Drawing.Point(12, 116);
            NormalizeButton.Name = "NormalizeButton";
            NormalizeButton.Size = new System.Drawing.Size(187, 46);
            NormalizeButton.TabIndex = 3;
            NormalizeButton.Text = "Normalize";
            NormalizeButton.UseVisualStyleBackColor = true;
            NormalizeButton.Click += NormalizeButton_Click;
            // 
            // MinBoxX
            // 
            MinBoxX.Location = new System.Drawing.Point(227, 49);
            MinBoxX.Margin = new System.Windows.Forms.Padding(25, 3, 0, 3);
            MinBoxX.Name = "MinBoxX";
            MinBoxX.Size = new System.Drawing.Size(108, 39);
            MinBoxX.TabIndex = 4;
            // 
            // TextX
            // 
            TextX.Location = new System.Drawing.Point(335, 49);
            TextX.Margin = new System.Windows.Forms.Padding(0);
            TextX.Name = "TextX";
            TextX.Size = new System.Drawing.Size(77, 39);
            TextX.TabIndex = 5;
            TextX.Text = "≤ x ≤";
            TextX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MaxBoxX
            // 
            MaxBoxX.Location = new System.Drawing.Point(412, 49);
            MaxBoxX.Margin = new System.Windows.Forms.Padding(0, 3, 25, 3);
            MaxBoxX.Name = "MaxBoxX";
            MaxBoxX.Size = new System.Drawing.Size(108, 39);
            MaxBoxX.TabIndex = 6;
            // 
            // MaxBoxY
            // 
            MaxBoxY.Location = new System.Drawing.Point(412, 94);
            MaxBoxY.Margin = new System.Windows.Forms.Padding(0, 3, 25, 3);
            MaxBoxY.Name = "MaxBoxY";
            MaxBoxY.Size = new System.Drawing.Size(108, 39);
            MaxBoxY.TabIndex = 9;
            // 
            // TextY
            // 
            TextY.Location = new System.Drawing.Point(335, 94);
            TextY.Margin = new System.Windows.Forms.Padding(0);
            TextY.Name = "TextY";
            TextY.Size = new System.Drawing.Size(77, 39);
            TextY.TabIndex = 8;
            TextY.Text = "≤ y ≤";
            TextY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MinBoxY
            // 
            MinBoxY.Location = new System.Drawing.Point(227, 94);
            MinBoxY.Margin = new System.Windows.Forms.Padding(25, 3, 0, 3);
            MinBoxY.Name = "MinBoxY";
            MinBoxY.Size = new System.Drawing.Size(108, 39);
            MinBoxY.TabIndex = 7;
            // 
            // ViewportLock
            // 
            ViewportLock.Location = new System.Drawing.Point(227, 139);
            ViewportLock.Name = "ViewportLock";
            ViewportLock.Size = new System.Drawing.Size(293, 39);
            ViewportLock.TabIndex = 10;
            ViewportLock.Text = "Lock Viewport";
            ViewportLock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            ViewportLock.UseVisualStyleBackColor = true;
            ViewportLock.CheckedChanged += ViewportLock_CheckedChanged;
            // 
            // SetZoomForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(533, 227);
            Controls.Add(ViewportLock);
            Controls.Add(MaxBoxY);
            Controls.Add(TextY);
            Controls.Add(MinBoxY);
            Controls.Add(MaxBoxX);
            Controls.Add(TextX);
            Controls.Add(MinBoxX);
            Controls.Add(NormalizeButton);
            Controls.Add(ResetButton);
            Controls.Add(MatchAspectButton);
            Controls.Add(EnableBoxSelect);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Name = "SetZoomForm";
            Text = "Set Viewport Zoom";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button EnableBoxSelect;
        private System.Windows.Forms.Button MatchAspectButton;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.Button NormalizeButton;
        private System.Windows.Forms.TextBox MinBoxX;
        private System.Windows.Forms.Label TextX;
        private System.Windows.Forms.TextBox MaxBoxX;
        private System.Windows.Forms.TextBox MaxBoxY;
        private System.Windows.Forms.Label TextY;
        private System.Windows.Forms.TextBox MinBoxY;
        private System.Windows.Forms.CheckBox ViewportLock;
    }
}