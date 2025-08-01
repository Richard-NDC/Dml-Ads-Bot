namespace WindowsFormsApp1
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.otto = new System.Windows.Forms.RadioButton();
            this.hatching = new System.Windows.Forms.RadioButton();
            this.earngems = new System.Windows.Forms.RadioButton();
            this.spamcardads = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(488, 35);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(83, 26);
            this.button1.TabIndex = 0;
            this.button1.Text = "Bắt đầu";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // otto
            // 
            this.otto.AutoSize = true;
            this.otto.Checked = true;
            this.otto.Cursor = System.Windows.Forms.Cursors.Hand;
            this.otto.Location = new System.Drawing.Point(12, 12);
            this.otto.Name = "otto";
            this.otto.Size = new System.Drawing.Size(75, 17);
            this.otto.TabIndex = 1;
            this.otto.TabStop = true;
            this.otto.Text = "Otto\'s lotto";
            this.otto.UseVisualStyleBackColor = true;
            this.otto.Click += new System.EventHandler(this.otto_Click);
            // 
            // hatching
            // 
            this.hatching.AutoSize = true;
            this.hatching.Cursor = System.Windows.Forms.Cursors.Hand;
            this.hatching.Location = new System.Drawing.Point(101, 12);
            this.hatching.Name = "hatching";
            this.hatching.Size = new System.Drawing.Size(140, 17);
            this.hatching.TabIndex = 2;
            this.hatching.Text = "Skip Egg Hatching Time";
            this.hatching.UseVisualStyleBackColor = true;
            this.hatching.Click += new System.EventHandler(this.hatching_Click);
            // 
            // earngems
            // 
            this.earngems.AutoSize = true;
            this.earngems.Cursor = System.Windows.Forms.Cursors.Hand;
            this.earngems.Location = new System.Drawing.Point(247, 12);
            this.earngems.Name = "earngems";
            this.earngems.Size = new System.Drawing.Size(117, 17);
            this.earngems.TabIndex = 3;
            this.earngems.Text = "Earn Gems (Popup)";
            this.earngems.UseVisualStyleBackColor = true;
            this.earngems.Click += new System.EventHandler(this.radioButton1_Click);
            // 
            // spamcardads
            // 
            this.spamcardads.AutoSize = true;
            this.spamcardads.Cursor = System.Windows.Forms.Cursors.Hand;
            this.spamcardads.Location = new System.Drawing.Point(370, 12);
            this.spamcardads.Name = "spamcardads";
            this.spamcardads.Size = new System.Drawing.Size(98, 17);
            this.spamcardads.TabIndex = 4;
            this.spamcardads.Text = "Spam Card Ads";
            this.spamcardads.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radioButton2.Location = new System.Drawing.Point(474, 12);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(103, 17);
            this.radioButton2.TabIndex = 5;
            this.radioButton2.Text = "Spam Chest Ads";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 76);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.spamcardads);
            this.Controls.Add(this.earngems);
            this.Controls.Add(this.hatching);
            this.Controls.Add(this.otto);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton otto;
        private System.Windows.Forms.RadioButton hatching;
        private System.Windows.Forms.RadioButton earngems;
        private System.Windows.Forms.RadioButton spamcardads;
        private System.Windows.Forms.RadioButton radioButton2;
    }
}

