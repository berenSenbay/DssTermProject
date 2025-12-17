namespace csharp_ml_algorithm_comparator
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.pnlAttributes = new System.Windows.Forms.FlowLayoutPanel();
            this.btnDiscover = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblResultTitle = new System.Windows.Forms.Label();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();

            // 
            // panelHeader (Modern Başlık Çubuğu)
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(480, 60);

            // 
            // lblTitle (Uygulama İsmi)
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(12, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(264, 30);
            this.lblTitle.Text = "AI CLASSIFIER STUDIO";

            // 
            // txtFilePath (Dosya Yolu Kutusu)
            // 
            this.txtFilePath.BackColor = System.Drawing.Color.White;
            this.txtFilePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFilePath.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtFilePath.Location = new System.Drawing.Point(20, 80);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(320, 23);

            // 
            // btnBrowse (Modern Mavi Buton)
            // 
            this.btnBrowse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnBrowse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBrowse.FlatAppearance.BorderSize = 0;
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowse.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnBrowse.ForeColor = System.Drawing.Color.White;
            this.btnBrowse.Location = new System.Drawing.Point(350, 78);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(110, 28);
            this.btnBrowse.Text = "LOAD DATASET";
            this.btnBrowse.UseVisualStyleBackColor = false;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);

            // 
            // lblStatus (Durum Çubuğu)
            // 
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI Italic", 9.5F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(140)))), ((int)(((byte)(141)))));
            this.lblStatus.Location = new System.Drawing.Point(20, 115);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(440, 25);
            this.lblStatus.Text = "Ready to analyze...";

            // 
            // pnlAttributes (Giriş Alanı Paneli)
            // 
            this.pnlAttributes.AutoScroll = true;
            this.pnlAttributes.BackColor = System.Drawing.Color.White;
            this.pnlAttributes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAttributes.Location = new System.Drawing.Point(20, 145);
            this.pnlAttributes.Name = "pnlAttributes";
            this.pnlAttributes.Padding = new System.Windows.Forms.Padding(10);
            this.pnlAttributes.Size = new System.Drawing.Size(440, 220);

            // 
            // btnDiscover (Modern Yeşil Buton)
            // 
            this.btnDiscover.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnDiscover.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDiscover.FlatAppearance.BorderSize = 0;
            this.btnDiscover.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDiscover.Font = new System.Drawing.Font("Segoe UI Bold", 11F, System.Drawing.FontStyle.Bold);
            this.btnDiscover.ForeColor = System.Drawing.Color.White;
            this.btnDiscover.Location = new System.Drawing.Point(20, 380);
            this.btnDiscover.Name = "btnDiscover";
            this.btnDiscover.Size = new System.Drawing.Size(440, 40);
            this.btnDiscover.Text = "DISCOVER CLASS";
            this.btnDiscover.UseVisualStyleBackColor = false;
            this.btnDiscover.Click += new System.EventHandler(this.btnDiscover_Click);

            // 
            // lblResultTitle
            // 
            this.lblResultTitle.AutoSize = true;
            this.lblResultTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblResultTitle.Location = new System.Drawing.Point(20, 435);
            this.lblResultTitle.Text = "Analysis Result:";

            // 
            // lblResult (Sonuç Alanı)
            // 
            this.lblResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.lblResult.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80)))));
            this.lblResult.Location = new System.Drawing.Point(20, 455);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(440, 45);
            this.lblResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblResult.Text = "- Waiting -";

            // 
            // Form1
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(480, 520);
            this.Controls.Add(this.lblResultTitle);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.btnDiscover);
            this.Controls.Add(this.pnlAttributes);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Machine Learning Algorithm Comparator";
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.FlowLayoutPanel pnlAttributes;
        private System.Windows.Forms.Button btnDiscover;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblResultTitle;
    }
}