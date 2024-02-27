namespace Graphing.Forms
{
    partial class GraphForm
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
            ResetViewportButton = new Button();
            GraphMenu = new MenuStrip();
            MenuViewport = new ToolStripMenuItem();
            ButtonViewportSetZoom = new ToolStripMenuItem();
            ButtonViewportSetCenter = new ToolStripMenuItem();
            ButtonViewportReset = new ToolStripMenuItem();
            ButtonViewportResetWindow = new ToolStripMenuItem();
            MenuColors = new ToolStripMenuItem();
            MenuEquations = new ToolStripMenuItem();
            MenuEquationsDerivative = new ToolStripMenuItem();
            MenuEquationsIntegral = new ToolStripMenuItem();
            MenuMisc = new ToolStripMenuItem();
            MenuMiscCaches = new ToolStripMenuItem();
            GraphMenu.SuspendLayout();
            SuspendLayout();
            // 
            // ResetViewportButton
            // 
            ResetViewportButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ResetViewportButton.Font = new Font("Segoe UI Emoji", 13.875F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ResetViewportButton.Location = new Point(1373, 43);
            ResetViewportButton.Name = "ResetViewportButton";
            ResetViewportButton.Size = new Size(64, 64);
            ResetViewportButton.TabIndex = 0;
            ResetViewportButton.Text = "⌂";
            ResetViewportButton.TextAlign = ContentAlignment.TopRight;
            ResetViewportButton.UseVisualStyleBackColor = true;
            ResetViewportButton.Click += ResetViewportButton_Click;
            // 
            // GraphMenu
            // 
            GraphMenu.ImageScalingSize = new Size(32, 32);
            GraphMenu.Items.AddRange(new ToolStripItem[] { MenuViewport, MenuColors, MenuEquations, MenuMisc });
            GraphMenu.Location = new Point(0, 0);
            GraphMenu.Name = "GraphMenu";
            GraphMenu.Size = new Size(1449, 42);
            GraphMenu.TabIndex = 1;
            GraphMenu.Text = "menuStrip1";
            // 
            // MenuViewport
            // 
            MenuViewport.DropDownItems.AddRange(new ToolStripItem[] { ButtonViewportSetZoom, ButtonViewportSetCenter, ButtonViewportReset, ButtonViewportResetWindow });
            MenuViewport.Name = "MenuViewport";
            MenuViewport.Size = new Size(129, 38);
            MenuViewport.Text = "Viewport";
            // 
            // ButtonViewportSetZoom
            // 
            ButtonViewportSetZoom.Name = "ButtonViewportSetZoom";
            ButtonViewportSetZoom.Size = new Size(350, 44);
            ButtonViewportSetZoom.Text = "Set Zoom";
            ButtonViewportSetZoom.Click += ButtonViewportSetZoom_Click;
            // 
            // ButtonViewportSetCenter
            // 
            ButtonViewportSetCenter.Name = "ButtonViewportSetCenter";
            ButtonViewportSetCenter.Size = new Size(350, 44);
            ButtonViewportSetCenter.Text = "Set Center Position";
            ButtonViewportSetCenter.Click += ButtonViewportSetCenter_Click;
            // 
            // ButtonViewportReset
            // 
            ButtonViewportReset.Name = "ButtonViewportReset";
            ButtonViewportReset.Size = new Size(350, 44);
            ButtonViewportReset.Text = "Reset Viewport";
            ButtonViewportReset.Click += ButtonViewportReset_Click;
            // 
            // ButtonViewportResetWindow
            // 
            ButtonViewportResetWindow.Name = "ButtonViewportResetWindow";
            ButtonViewportResetWindow.Size = new Size(350, 44);
            ButtonViewportResetWindow.Text = "Reset Window Size";
            ButtonViewportResetWindow.Click += ButtonViewportResetWindow_Click;
            // 
            // MenuColors
            // 
            MenuColors.Name = "MenuColors";
            MenuColors.Size = new Size(101, 38);
            MenuColors.Text = "Colors";
            // 
            // MenuEquations
            // 
            MenuEquations.DropDownItems.AddRange(new ToolStripItem[] { MenuEquationsDerivative, MenuEquationsIntegral });
            MenuEquations.Name = "MenuEquations";
            MenuEquations.Size = new Size(138, 38);
            MenuEquations.Text = "Equations";
            // 
            // MenuEquationsDerivative
            // 
            MenuEquationsDerivative.Name = "MenuEquationsDerivative";
            MenuEquationsDerivative.Size = new Size(360, 44);
            MenuEquationsDerivative.Text = "Compute Derivative";
            // 
            // MenuEquationsIntegral
            // 
            MenuEquationsIntegral.Name = "MenuEquationsIntegral";
            MenuEquationsIntegral.Size = new Size(360, 44);
            MenuEquationsIntegral.Text = "Compute Integral";
            // 
            // MenuMisc
            // 
            MenuMisc.DropDownItems.AddRange(new ToolStripItem[] { MenuMiscCaches });
            MenuMisc.Name = "MenuMisc";
            MenuMisc.Size = new Size(83, 38);
            MenuMisc.Text = "Misc";
            // 
            // MenuMiscCaches
            // 
            MenuMiscCaches.Name = "MenuMiscCaches";
            MenuMiscCaches.Size = new Size(359, 44);
            MenuMiscCaches.Text = "View Caches";
            MenuMiscCaches.Click += MenuMiscCaches_Click;
            // 
            // GraphForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1449, 907);
            Controls.Add(ResetViewportButton);
            Controls.Add(GraphMenu);
            MainMenuStrip = GraphMenu;
            Name = "GraphForm";
            Text = "GraphFormBase";
            GraphMenu.ResumeLayout(false);
            GraphMenu.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button ResetViewportButton;
        private MenuStrip GraphMenu;
        private ToolStripMenuItem MenuColors;
        private ToolStripMenuItem MenuViewport;
        private ToolStripMenuItem ButtonViewportSetZoom;
        private ToolStripMenuItem ButtonViewportSetCenter;
        private ToolStripMenuItem ButtonViewportReset;
        private ToolStripMenuItem ButtonViewportResetWindow;
        private ToolStripMenuItem MenuEquations;
        private ToolStripMenuItem MenuEquationsDerivative;
        private ToolStripMenuItem MenuEquationsIntegral;
        private ToolStripMenuItem MenuMisc;
        private ToolStripMenuItem MenuMiscCaches;
    }
}