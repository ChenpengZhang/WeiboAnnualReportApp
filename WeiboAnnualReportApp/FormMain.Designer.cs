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
            textBox1 = new TextBox();
            dataGridView1 = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // btnopenReport
            // 
            btnopenReport.Location = new Point(33, 436);
            btnopenReport.Margin = new Padding(6, 5, 6, 5);
            btnopenReport.Name = "btnopenReport";
            btnopenReport.Size = new Size(214, 80);
            btnopenReport.TabIndex = 0;
            btnopenReport.Text = "打开年度报告";
            btnopenReport.UseVisualStyleBackColor = true;
            btnopenReport.Click += btnOpenReport_Click;
            // 
            // btoCreateReport
            // 
            btoCreateReport.Location = new Point(33, 306);
            btoCreateReport.Name = "btoCreateReport";
            btoCreateReport.Size = new Size(214, 82);
            btoCreateReport.TabIndex = 1;
            btoCreateReport.Text = "生成年度报告";
            btoCreateReport.UseVisualStyleBackColor = true;
            btoCreateReport.Click += btnCreateReport_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(33, 198);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(214, 34);
            textBox1.TabIndex = 2;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(282, 12);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 72;
            dataGridView1.Size = new Size(865, 612);
            dataGridView1.TabIndex = 3;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(13F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1159, 636);
            Controls.Add(textBox1);
            Controls.Add(btoCreateReport);
            Controls.Add(btnopenReport);
            Controls.Add(dataGridView1);
            Margin = new Padding(6, 5, 6, 5);
            Name = "FormMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "欢迎来到微博年度报告";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnopenReport;
        private Button btoCreateReport;
        private TextBox textBox1;
        private DataGridView dataGridView1;
    }
}
