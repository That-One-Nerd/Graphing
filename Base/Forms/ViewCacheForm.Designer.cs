namespace Graphing.Forms
{
    partial class ViewCacheForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewCacheForm));
            CachePie = new Controls.PieChart();
            TotalCacheText = new Label();
            EraseAllCacheButton = new Button();
            SpecificCachePanel = new Panel();
            SuspendLayout();
            // 
            // CachePie
            // 
            CachePie.Location = new Point(50, 50);
            CachePie.Name = "CachePie";
            CachePie.Size = new Size(450, 450);
            CachePie.TabIndex = 0;
            // 
            // TotalCacheText
            // 
            TotalCacheText.Font = new Font("Segoe UI Semibold", 10.125F, FontStyle.Bold, GraphicsUnit.Point, 0);
            TotalCacheText.Location = new Point(62, 540);
            TotalCacheText.Name = "TotalCacheText";
            TotalCacheText.Size = new Size(425, 45);
            TotalCacheText.TabIndex = 1;
            TotalCacheText.Text = "Total Cache: Something";
            TotalCacheText.TextAlign = ContentAlignment.TopCenter;
            // 
            // EraseAllCacheButton
            // 
            EraseAllCacheButton.Location = new Point(200, 580);
            EraseAllCacheButton.Name = "EraseAllCacheButton";
            EraseAllCacheButton.Size = new Size(150, 46);
            EraseAllCacheButton.TabIndex = 2;
            EraseAllCacheButton.Text = "Erase All";
            EraseAllCacheButton.UseVisualStyleBackColor = true;
            EraseAllCacheButton.Click += EraseAllCacheButton_Click;
            // 
            // SpecificCachePanel
            // 
            SpecificCachePanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            SpecificCachePanel.AutoScroll = true;
            SpecificCachePanel.Location = new Point(520, 12);
            SpecificCachePanel.Name = "SpecificCachePanel";
            SpecificCachePanel.Size = new Size(542, 657);
            SpecificCachePanel.TabIndex = 3;
            // 
            // ViewCacheForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1074, 679);
            Controls.Add(SpecificCachePanel);
            Controls.Add(EraseAllCacheButton);
            Controls.Add(TotalCacheText);
            Controls.Add(CachePie);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            MinimumSize = new Size(885, 750);
            Name = "ViewCacheForm";
            Text = "Graph Caches";
            ResumeLayout(false);
        }

        #endregion

        private Controls.PieChart CachePie;
        private Label TotalCacheText;
        private Button EraseAllCacheButton;
        private Panel SpecificCachePanel;
    }
}