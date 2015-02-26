namespace WebScraper
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
            this.scrapeButton = new System.Windows.Forms.Button();
            this.tagBox = new System.Windows.Forms.TextBox();
            this.charCount = new System.Windows.Forms.Label();
            this.charCountBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // scrapeButton
            // 
            this.scrapeButton.Location = new System.Drawing.Point(48, 33);
            this.scrapeButton.Name = "scrapeButton";
            this.scrapeButton.Size = new System.Drawing.Size(239, 23);
            this.scrapeButton.TabIndex = 0;
            this.scrapeButton.Text = "Scrape";
            this.scrapeButton.UseVisualStyleBackColor = true;
            this.scrapeButton.Click += new System.EventHandler(this.scrapeButtonClick);
            // 
            // tagBox
            // 
            this.tagBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tagBox.Location = new System.Drawing.Point(12, 128);
            this.tagBox.Multiline = true;
            this.tagBox.Name = "tagBox";
            this.tagBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tagBox.Size = new System.Drawing.Size(337, 192);
            this.tagBox.TabIndex = 1;
            // 
            // charCount
            // 
            this.charCount.AutoSize = true;
            this.charCount.Location = new System.Drawing.Point(45, 80);
            this.charCount.Name = "charCount";
            this.charCount.Size = new System.Drawing.Size(81, 17);
            this.charCount.TabIndex = 2;
            this.charCount.Text = "Char count:";
            // 
            // charCountBox
            // 
            this.charCountBox.Location = new System.Drawing.Point(158, 75);
            this.charCountBox.Name = "charCountBox";
            this.charCountBox.Size = new System.Drawing.Size(129, 22);
            this.charCountBox.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 332);
            this.Controls.Add(this.charCountBox);
            this.Controls.Add(this.charCount);
            this.Controls.Add(this.tagBox);
            this.Controls.Add(this.scrapeButton);
            this.Name = "Form1";
            this.Text = "Web Page Tag Scraper";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button scrapeButton;
        private System.Windows.Forms.TextBox tagBox;
        private System.Windows.Forms.Label charCount;
        private System.Windows.Forms.TextBox charCountBox;
    }
}

