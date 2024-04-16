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
            // SetZoomForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(EnableBoxSelect);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Name = "SetZoomForm";
            Text = "Set Viewport Zoom";
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button EnableBoxSelect;
    }
}