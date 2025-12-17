namespace csharp_ml_algorithm_comparator
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }

        private void InitializeComponent()
        {
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.pnlAttributes = new System.Windows.Forms.FlowLayoutPanel();
            this.btnDiscover = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.SuspendLayout();

            this.txtFilePath.Location = new System.Drawing.Point(20, 20);
            this.txtFilePath.Size = new System.Drawing.Size(300, 20);

            this.btnBrowse.Location = new System.Drawing.Point(330, 18);
            this.btnBrowse.Size = new System.Drawing.Size(80, 23);
            this.btnBrowse.Text = "BROWSE";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);

            this.lblStatus.Location = new System.Drawing.Point(20, 55);
            this.lblStatus.Size = new System.Drawing.Size(400, 30);
            this.lblStatus.Text = "Status...";

            this.pnlAttributes.Location = new System.Drawing.Point(20, 90);
            this.pnlAttributes.Size = new System.Drawing.Size(400, 200);
            this.pnlAttributes.AutoScroll = true;

            this.btnDiscover.Location = new System.Drawing.Point(20, 300);
            this.btnDiscover.Size = new System.Drawing.Size(100, 30);
            this.btnDiscover.Text = "Discover";
            this.btnDiscover.Click += new System.EventHandler(this.btnDiscover_Click);

            this.lblResult.Location = new System.Drawing.Point(20, 340);
            this.lblResult.Size = new System.Drawing.Size(200, 20);
            this.lblResult.Font = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);

            this.ClientSize = new System.Drawing.Size(450, 400);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pnlAttributes);
            this.Controls.Add(this.btnDiscover);
            this.Controls.Add(this.lblResult);
            this.Text = "ML Tool";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.FlowLayoutPanel pnlAttributes;
        private System.Windows.Forms.Button btnDiscover;
        private System.Windows.Forms.Label lblResult;
    }
}