namespace WeiboAnnualReportApp
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnopenReport = new Button();
            btoCreateReport = new Button();
            SuspendLayout();
            // 
            // btnopenReport
            // 
            btnopenReport.Location = new Point(418, 359);
            btnopenReport.Margin = new Padding(6, 5, 6, 5);
            btnopenReport.Name = "btnopenReport";
            btnopenReport.Size = new Size(281, 80);
            btnopenReport.TabIndex = 0;
            btnopenReport.Text = "打开年度报告";
            btnopenReport.UseVisualStyleBackColor = true;
            btnopenReport.Click += btnOpenReport_Click;
            // 
            // btoCreateReport
            // 
            btoCreateReport.Location = new Point(418, 171);
            btoCreateReport.Name = "btoCreateReport";
            btoCreateReport.Size = new Size(281, 82);
            btoCreateReport.TabIndex = 1;
            btoCreateReport.Text = "生成年度报告";
            btoCreateReport.UseVisualStyleBackColor = true;
            btoCreateReport.Click += btnCreateReport_Click;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(13F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1159, 636);
            Controls.Add(btoCreateReport);
            Controls.Add(btnopenReport);
            Margin = new Padding(6, 5, 6, 5);
            Name = "FormMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "欢迎来到微博年度报告";
            ResumeLayout(false);
        }

        #endregion

        private Button btnopenReport;
        private Button btoCreateReport;
    }
}
