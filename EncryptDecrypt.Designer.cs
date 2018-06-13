using System.Windows.Forms;

namespace EncryptDecryptAppConfigFile
{
    partial class EncryptDecrypt
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtEncryption = new System.Windows.Forms.TextBox();
            this.cmdOpen = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.allLikeConfigFilesOptionGroupBox = new System.Windows.Forms.GroupBox();
            this.radioButton_Yes = new System.Windows.Forms.RadioButton();
            this.radioButton_No = new System.Windows.Forms.RadioButton();
            this.cmdDecrypt = new System.Windows.Forms.Button();
            this.cmdEncrypt = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.allLikeConfigFilesOptionGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDilog";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtEncryption);
            this.panel1.Controls.Add(this.cmdOpen);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(500, 70);
            this.panel1.TabIndex = 5;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Open .NET Config File";
            // 
            // txtEncryption
            // 
            this.txtEncryption.Location = new System.Drawing.Point(15, 38);
            this.txtEncryption.Name = "txtEncryption";
            this.txtEncryption.Size = new System.Drawing.Size(445, 20);
            this.txtEncryption.TabIndex = 6;
            // 
            // cmdOpen
            // 
            this.cmdOpen.Location = new System.Drawing.Point(466, 38);
            this.cmdOpen.Name = "cmdOpen";
            this.cmdOpen.Size = new System.Drawing.Size(22, 20);
            this.cmdOpen.TabIndex = 5;
            this.cmdOpen.Text = "&..";
            this.cmdOpen.UseVisualStyleBackColor = true;
            this.cmdOpen.Click += new System.EventHandler(this.cmdOpen_Click_1);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.allLikeConfigFilesOptionGroupBox);
            this.panel2.Controls.Add(this.cmdDecrypt);
            this.panel2.Controls.Add(this.cmdEncrypt);
            this.panel2.Location = new System.Drawing.Point(12, 88);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(500, 134);
            this.panel2.TabIndex = 6;
            // 
            // allLikeConfigFilesOptionGroupBox
            // 
            this.allLikeConfigFilesOptionGroupBox.Controls.Add(this.radioButton_Yes);
            this.allLikeConfigFilesOptionGroupBox.Controls.Add(this.radioButton_No);
            this.allLikeConfigFilesOptionGroupBox.Location = new System.Drawing.Point(27, 42);
            this.allLikeConfigFilesOptionGroupBox.Name = "allLikeConfigFilesOptionGroupBox";
            this.allLikeConfigFilesOptionGroupBox.Size = new System.Drawing.Size(288, 73);
            this.allLikeConfigFilesOptionGroupBox.TabIndex = 4;
            this.allLikeConfigFilesOptionGroupBox.TabStop = false;
            this.allLikeConfigFilesOptionGroupBox.Text = "Encrypt/Decrypt all .config files in the same directory?";
            // 
            // radioButton_Yes
            // 
            this.radioButton_Yes.AutoSize = true;
            this.radioButton_Yes.Location = new System.Drawing.Point(13, 42);
            this.radioButton_Yes.Name = "radioButton_Yes";
            this.radioButton_Yes.Size = new System.Drawing.Size(257, 17);
            this.radioButton_Yes.TabIndex = 1;
            this.radioButton_Yes.TabStop = true;
            this.radioButton_Yes.Text = "Yes, modify each .config file in the same directory";
            this.radioButton_Yes.UseVisualStyleBackColor = true;
            this.radioButton_Yes.CheckedChanged += new System.EventHandler(this.radioButtonYes_CheckedChanged);
            // 
            // radioButton_No
            // 
            this.radioButton_No.AutoSize = true;
            this.radioButton_No.Checked = true;
            this.radioButton_No.Location = new System.Drawing.Point(13, 19);
            this.radioButton_No.Name = "radioButton_No";
            this.radioButton_No.Size = new System.Drawing.Size(209, 17);
            this.radioButton_No.TabIndex = 0;
            this.radioButton_No.TabStop = true;
            this.radioButton_No.Text = "No, only modify the selected .config file";
            this.radioButton_No.UseVisualStyleBackColor = true;
            this.radioButton_No.CheckedChanged += new System.EventHandler(this.radioButtonNo_CheckedChanged);
            // 
            // cmdDecrypt
            // 
            this.cmdDecrypt.Location = new System.Drawing.Point(137, 13);
            this.cmdDecrypt.Name = "cmdDecrypt";
            this.cmdDecrypt.Size = new System.Drawing.Size(104, 23);
            this.cmdDecrypt.TabIndex = 3;
            this.cmdDecrypt.Text = "&Decrypt Config";
            this.cmdDecrypt.UseVisualStyleBackColor = true;
            this.cmdDecrypt.Click += new System.EventHandler(this.cmdDecrypt_Click_1);
            // 
            // cmdEncrypt
            // 
            this.cmdEncrypt.Location = new System.Drawing.Point(27, 13);
            this.cmdEncrypt.Name = "cmdEncrypt";
            this.cmdEncrypt.Size = new System.Drawing.Size(104, 23);
            this.cmdEncrypt.TabIndex = 2;
            this.cmdEncrypt.Text = "&Encrypt Config";
            this.cmdEncrypt.UseVisualStyleBackColor = true;
            this.cmdEncrypt.Click += new System.EventHandler(this.cmdEncrypt_Click_1);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(112, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Modify all .config files in this folder?";
            // 
            // EncryptDecrypt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 235);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "EncryptDecrypt";
            this.Text = "Encrypt Decrypt Connection String";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.allLikeConfigFilesOptionGroupBox.ResumeLayout(false);
            this.allLikeConfigFilesOptionGroupBox.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private OpenFileDialog openFileDialog1;
        private Panel panel1;
        private Label label1;
        private TextBox txtEncryption;
        private Button cmdOpen;
        private Panel panel2;
        private Button cmdDecrypt;
        private Button cmdEncrypt;

        private Label label2;
        protected RadioButton selectedRadioButton;
        private GroupBox allLikeConfigFilesOptionGroupBox;
        private RadioButton radioButton_Yes;
        protected RadioButton radioButton_No;
    }
}

