namespace Continuum_DeShift
{
    partial class SettingsDialog
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
            this.bCancel = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            this.cbOpenWith = new System.Windows.Forms.CheckBox();
            this.cbEnableContext = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(213, 87);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 0;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bOK
            // 
            this.bOK.Location = new System.Drawing.Point(132, 87);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 1;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // cbOpenWith
            // 
            this.cbOpenWith.AutoSize = true;
            this.cbOpenWith.Location = new System.Drawing.Point(13, 23);
            this.cbOpenWith.Name = "cbOpenWith";
            this.cbOpenWith.Size = new System.Drawing.Size(142, 17);
            this.cbOpenWith.TabIndex = 2;
            this.cbOpenWith.Text = "Associate with .pac files.";
            this.cbOpenWith.UseVisualStyleBackColor = true;
            this.cbOpenWith.CheckedChanged += new System.EventHandler(this.cbOpenWith_CheckedChanged);
            // 
            // cbEnableContext
            // 
            this.cbEnableContext.AutoSize = true;
            this.cbEnableContext.Enabled = false;
            this.cbEnableContext.Location = new System.Drawing.Point(12, 56);
            this.cbEnableContext.Name = "cbEnableContext";
            this.cbEnableContext.Size = new System.Drawing.Size(190, 17);
            this.cbEnableContext.TabIndex = 3;
            this.cbEnableContext.Text = "Enable right click menu in explorer.";
            this.cbEnableContext.UseVisualStyleBackColor = true;
            this.cbEnableContext.CheckedChanged += new System.EventHandler(this.cbEnableContext_CheckedChanged);
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 122);
            this.Controls.Add(this.cbEnableContext);
            this.Controls.Add(this.cbOpenWith);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.bCancel);
            this.Name = "SettingsDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.CheckBox cbOpenWith;
        private System.Windows.Forms.CheckBox cbEnableContext;
    }
}