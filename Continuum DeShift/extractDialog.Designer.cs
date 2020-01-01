namespace Continuum_DeShift
{
    partial class extractDialog
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
            this.bBrowse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPacFile = new System.Windows.Forms.TextBox();
            this.bExtract = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // bBrowse
            // 
            this.bBrowse.Location = new System.Drawing.Point(279, 22);
            this.bBrowse.Name = "bBrowse";
            this.bBrowse.Size = new System.Drawing.Size(75, 23);
            this.bBrowse.TabIndex = 6;
            this.bBrowse.Text = "Browse";
            this.bBrowse.UseVisualStyleBackColor = true;
            this.bBrowse.Click += new System.EventHandler(this.bBrowse_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Extract To:";
            // 
            // tbPacFile
            // 
            this.tbPacFile.Location = new System.Drawing.Point(15, 25);
            this.tbPacFile.Name = "tbPacFile";
            this.tbPacFile.Size = new System.Drawing.Size(258, 20);
            this.tbPacFile.TabIndex = 4;
            // 
            // bExtract
            // 
            this.bExtract.Location = new System.Drawing.Point(198, 51);
            this.bExtract.Name = "bExtract";
            this.bExtract.Size = new System.Drawing.Size(75, 23);
            this.bExtract.TabIndex = 7;
            this.bExtract.Text = "Extract";
            this.bExtract.UseVisualStyleBackColor = true;
            this.bExtract.Click += new System.EventHandler(this.bExtract_Click);
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(279, 51);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 8;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // extractDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 80);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bExtract);
            this.Controls.Add(this.bBrowse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbPacFile);
            this.Name = "extractDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Extract";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bBrowse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPacFile;
        private System.Windows.Forms.Button bExtract;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}