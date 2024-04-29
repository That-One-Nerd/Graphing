using System.Drawing;
using System.Windows.Forms;

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
            MenuElements = new ToolStripMenuItem();
            MenuElementsColors = new ToolStripMenuItem();
            MenuElementsRemove = new ToolStripMenuItem();
            MenuOperations = new ToolStripMenuItem();
            MenuOperationsDerivative = new ToolStripMenuItem();
            MenuOperationsIntegral = new ToolStripMenuItem();
            MenuOperationsTranslate = new ToolStripMenuItem();
            MenuConvert = new ToolStripMenuItem();
            MenuConvertEquation = new ToolStripMenuItem();
            MenuConvertSlopeField = new ToolStripMenuItem();
            MenuMisc = new ToolStripMenuItem();
            MenuMiscCaches = new ToolStripMenuItem();
            MiscMenuPreload = new ToolStripMenuItem();
            UpdaterPopup = new Panel();
            UpdaterPopupDownloadButton = new Button();
            UpdaterPopupCloseButton = new Button();
            UpdaterPopupMessage = new Label();
            MenuElementsDetail = new ToolStripMenuItem();
            GraphMenu.SuspendLayout();
            UpdaterPopup.SuspendLayout();
            SuspendLayout();
            // 
            // ResetViewportButton
            // 
            ResetViewportButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ResetViewportButton.Font = new Font("Segoe UI Emoji", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ResetViewportButton.Location = new Point(1372, 43);
            ResetViewportButton.Margin = new Padding(4, 2, 4, 2);
            ResetViewportButton.Name = "ResetViewportButton";
            ResetViewportButton.Size = new Size(63, 64);
            ResetViewportButton.TabIndex = 0;
            ResetViewportButton.Text = "🏠";
            ResetViewportButton.UseVisualStyleBackColor = true;
            ResetViewportButton.Click += ResetViewportButton_Click;
            // 
            // GraphMenu
            // 
            GraphMenu.ImageScalingSize = new Size(32, 32);
            GraphMenu.Items.AddRange(new ToolStripItem[] { MenuViewport, MenuElements, MenuOperations, MenuConvert, MenuMisc });
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
            ButtonViewportSetZoom.Size = new Size(359, 44);
            ButtonViewportSetZoom.Text = "Set Zoom";
            ButtonViewportSetZoom.Click += ButtonViewportSetZoom_Click;
            // 
            // ButtonViewportSetCenter
            // 
            ButtonViewportSetCenter.Name = "ButtonViewportSetCenter";
            ButtonViewportSetCenter.Size = new Size(359, 44);
            ButtonViewportSetCenter.Text = "Set Center Position";
            ButtonViewportSetCenter.Click += ButtonViewportSetCenter_Click;
            // 
            // ButtonViewportReset
            // 
            ButtonViewportReset.Name = "ButtonViewportReset";
            ButtonViewportReset.Size = new Size(359, 44);
            ButtonViewportReset.Text = "Reset Viewport";
            ButtonViewportReset.Click += ButtonViewportReset_Click;
            // 
            // ButtonViewportResetWindow
            // 
            ButtonViewportResetWindow.Name = "ButtonViewportResetWindow";
            ButtonViewportResetWindow.Size = new Size(359, 44);
            ButtonViewportResetWindow.Text = "Reset Window Size";
            ButtonViewportResetWindow.Click += ButtonViewportResetWindow_Click;
            // 
            // MenuElements
            // 
            MenuElements.DropDownItems.AddRange(new ToolStripItem[] { MenuElementsColors, MenuElementsDetail, MenuElementsRemove });
            MenuElements.Name = "MenuElements";
            MenuElements.Size = new Size(131, 38);
            MenuElements.Text = "Elements";
            // 
            // MenuElementsColors
            // 
            MenuElementsColors.Name = "MenuElementsColors";
            MenuElementsColors.Size = new Size(359, 44);
            MenuElementsColors.Text = "Colors";
            // 
            // MenuElementsRemove
            // 
            MenuElementsRemove.Name = "MenuElementsRemove";
            MenuElementsRemove.Size = new Size(359, 44);
            MenuElementsRemove.Text = "Remove";
            // 
            // MenuOperations
            // 
            MenuOperations.DropDownItems.AddRange(new ToolStripItem[] { MenuOperationsDerivative, MenuOperationsIntegral, MenuOperationsTranslate });
            MenuOperations.Name = "MenuOperations";
            MenuOperations.Size = new Size(151, 38);
            MenuOperations.Text = "Operations";
            // 
            // MenuOperationsDerivative
            // 
            MenuOperationsDerivative.Name = "MenuOperationsDerivative";
            MenuOperationsDerivative.Size = new Size(360, 44);
            MenuOperationsDerivative.Text = "Compute Derivative";
            // 
            // MenuOperationsIntegral
            // 
            MenuOperationsIntegral.Name = "MenuOperationsIntegral";
            MenuOperationsIntegral.Size = new Size(360, 44);
            MenuOperationsIntegral.Text = "Compute Integral";
            // 
            // MenuOperationsTranslate
            // 
            MenuOperationsTranslate.Name = "MenuOperationsTranslate";
            MenuOperationsTranslate.Size = new Size(360, 44);
            MenuOperationsTranslate.Text = "Translate";
            // 
            // MenuConvert
            // 
            MenuConvert.DropDownItems.AddRange(new ToolStripItem[] { MenuConvertEquation, MenuConvertSlopeField });
            MenuConvert.Name = "MenuConvert";
            MenuConvert.Size = new Size(118, 38);
            MenuConvert.Text = "Convert";
            // 
            // MenuConvertEquation
            // 
            MenuConvertEquation.Name = "MenuConvertEquation";
            MenuConvertEquation.Size = new Size(297, 44);
            MenuConvertEquation.Text = "To Equation";
            // 
            // MenuConvertSlopeField
            // 
            MenuConvertSlopeField.Name = "MenuConvertSlopeField";
            MenuConvertSlopeField.Size = new Size(297, 44);
            MenuConvertSlopeField.Text = "To Slope Field";
            // 
            // MenuMisc
            // 
            MenuMisc.DropDownItems.AddRange(new ToolStripItem[] { MenuMiscCaches, MiscMenuPreload });
            MenuMisc.Name = "MenuMisc";
            MenuMisc.Size = new Size(83, 38);
            MenuMisc.Text = "Misc";
            // 
            // MenuMiscCaches
            // 
            MenuMiscCaches.Name = "MenuMiscCaches";
            MenuMiscCaches.Size = new Size(299, 44);
            MenuMiscCaches.Text = "View Caches";
            MenuMiscCaches.Click += MenuMiscCaches_Click;
            // 
            // MiscMenuPreload
            // 
            MiscMenuPreload.Name = "MiscMenuPreload";
            MiscMenuPreload.Size = new Size(299, 44);
            MiscMenuPreload.Text = "Preload Cache";
            MiscMenuPreload.Click += MiscMenuPreload_Click;
            // 
            // UpdaterPopup
            // 
            UpdaterPopup.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            UpdaterPopup.BackColor = SystemColors.HighlightText;
            UpdaterPopup.BorderStyle = BorderStyle.FixedSingle;
            UpdaterPopup.Controls.Add(UpdaterPopupDownloadButton);
            UpdaterPopup.Controls.Add(UpdaterPopupCloseButton);
            UpdaterPopup.Controls.Add(UpdaterPopupMessage);
            UpdaterPopup.Location = new Point(966, 791);
            UpdaterPopup.Margin = new Padding(6, 6, 6, 6);
            UpdaterPopup.Name = "UpdaterPopup";
            UpdaterPopup.Size = new Size(483, 115);
            UpdaterPopup.TabIndex = 2;
            UpdaterPopup.Visible = false;
            // 
            // UpdaterPopupDownloadButton
            // 
            UpdaterPopupDownloadButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            UpdaterPopupDownloadButton.Location = new Point(336, 58);
            UpdaterPopupDownloadButton.Margin = new Padding(6, 6, 6, 6);
            UpdaterPopupDownloadButton.Name = "UpdaterPopupDownloadButton";
            UpdaterPopupDownloadButton.Size = new Size(139, 49);
            UpdaterPopupDownloadButton.TabIndex = 2;
            UpdaterPopupDownloadButton.Text = "Visit";
            UpdaterPopupDownloadButton.UseVisualStyleBackColor = true;
            // 
            // UpdaterPopupCloseButton
            // 
            UpdaterPopupCloseButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            UpdaterPopupCloseButton.Location = new Point(435, 2);
            UpdaterPopupCloseButton.Margin = new Padding(2, 2, 2, 2);
            UpdaterPopupCloseButton.Name = "UpdaterPopupCloseButton";
            UpdaterPopupCloseButton.Size = new Size(45, 51);
            UpdaterPopupCloseButton.TabIndex = 1;
            UpdaterPopupCloseButton.Text = "X";
            UpdaterPopupCloseButton.UseVisualStyleBackColor = true;
            UpdaterPopupCloseButton.Click += UpdaterPopupCloseButton_Click;
            // 
            // UpdaterPopupMessage
            // 
            UpdaterPopupMessage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            UpdaterPopupMessage.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            UpdaterPopupMessage.Location = new Point(6, 6);
            UpdaterPopupMessage.Margin = new Padding(6, 6, 6, 6);
            UpdaterPopupMessage.Name = "UpdaterPopupMessage";
            UpdaterPopupMessage.Size = new Size(423, 100);
            UpdaterPopupMessage.TabIndex = 0;
            UpdaterPopupMessage.Text = "A <type> update is available!\r\nA.B.C → E.F.G";
            // 
            // MenuElementsDetail
            // 
            MenuElementsDetail.Name = "MenuElementsDetail";
            MenuElementsDetail.Size = new Size(359, 44);
            MenuElementsDetail.Text = "Detail";
            // 
            // GraphForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1449, 907);
            Controls.Add(UpdaterPopup);
            Controls.Add(ResetViewportButton);
            Controls.Add(GraphMenu);
            MainMenuStrip = GraphMenu;
            Margin = new Padding(4, 2, 4, 2);
            Name = "GraphForm";
            Text = "GraphFormBase";
            GraphMenu.ResumeLayout(false);
            GraphMenu.PerformLayout();
            UpdaterPopup.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button ResetViewportButton;
        private MenuStrip GraphMenu;
        private ToolStripMenuItem MenuViewport;
        private ToolStripMenuItem ButtonViewportSetZoom;
        private ToolStripMenuItem ButtonViewportSetCenter;
        private ToolStripMenuItem ButtonViewportReset;
        private ToolStripMenuItem ButtonViewportResetWindow;
        private ToolStripMenuItem MenuOperations;
        private ToolStripMenuItem MenuOperationsDerivative;
        private ToolStripMenuItem MenuOperationsIntegral;
        private ToolStripMenuItem MenuMisc;
        private ToolStripMenuItem MenuMiscCaches;
        private ToolStripMenuItem MiscMenuPreload;
        private ToolStripMenuItem MenuConvert;
        private ToolStripMenuItem MenuConvertEquation;
        private ToolStripMenuItem MenuElements;
        private ToolStripMenuItem MenuElementsColors;
        private ToolStripMenuItem MenuElementsRemove;
        private ToolStripMenuItem MenuOperationsTranslate;
        private ToolStripMenuItem MenuConvertSlopeField;
        private Panel UpdaterPopup;
        private Label UpdaterPopupMessage;
        private Button UpdaterPopupCloseButton;
        private Button UpdaterPopupDownloadButton;
        private ToolStripMenuItem MenuElementsDetail;
    }
}