namespace JoystickTest
{
    partial class MultiJoyForm
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
            this.panelScroll = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelScroll
            // 
            this.panelScroll.AutoScroll = true;
            this.panelScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelScroll.Location = new System.Drawing.Point(0, 0);
            this.panelScroll.Name = "panelScroll";
            this.panelScroll.Size = new System.Drawing.Size(984, 533);
            this.panelScroll.TabIndex = 0;
            // 
            // MultiJoyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 533);
            this.Controls.Add(this.panelScroll);
            this.Name = "MultiJoyForm";
            this.Text = "Multi Joy Tester";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelScroll;
    }
}

