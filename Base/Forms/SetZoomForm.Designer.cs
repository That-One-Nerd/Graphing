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
            SuspendLayout();
            // 
            // EnableBoxSelect
            // 
            EnableBoxSelect.Location = new System.Drawing.Point(12, 12);
            EnableBoxSelect.Name = "EnableBoxSelect";
            EnableBoxSelect.Size = new System.Drawing.Size(150, 46);
            EnableBoxSelect.TabIndex = 0;
            EnableBoxSelect.Text = "Box Select";
            EnableBoxSelect.UseVisualStyleBackColor = true;
            EnableBoxSelect.Click += EnableBoxSelect_Click;
            // 
            // MatchAspectButton
            // 
            MatchAspectButton.Location = new System.Drawing.Point(168, 12);
            MatchAspectButton.Name = "MatchAspectButton";
            MatchAspectButton.Size = new System.Drawing.Size(187, 46);
            MatchAspectButton.TabIndex = 1;
            MatchAspectButton.Text = "Match Aspect";
            MatchAspectButton.UseVisualStyleBackColor = true;
            MatchAspectButton.Click += MatchAspectButton_Click;
            // 
            // ResetButton
            // 
            ResetButton.Location = new System.Drawing.Point(517, 12);
            ResetButton.Name = "ResetButton";
            ResetButton.Size = new System.Drawing.Size(150, 46);
            ResetButton.TabIndex = 2;
            ResetButton.Text = "Reset";
            ResetButton.UseVisualStyleBackColor = true;
            ResetButton.Click += ResetButton_Click;
            // 
            // NormalizeButton
            // 
            NormalizeButton.Location = new System.Drawing.Point(361, 12);
            NormalizeButton.Name = "NormalizeButton";
            NormalizeButton.Size = new System.Drawing.Size(150, 46);
            NormalizeButton.TabIndex = 3;
            NormalizeButton.Text = "Normalize";
            NormalizeButton.UseVisualStyleBackColor = true;
            NormalizeButton.Click += NormalizeButton_Click;
            // 
            // SetZoomForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(NormalizeButton);
            Controls.Add(ResetButton);
            Controls.Add(MatchAspectButton);
            Controls.Add(EnableBoxSelect);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Name = "SetZoomForm";
            Text = "Set Viewport Zoom";
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button EnableBoxSelect;
        private System.Windows.Forms.Button MatchAspectButton;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.Button NormalizeButton;
    }
}