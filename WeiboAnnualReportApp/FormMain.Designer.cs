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
            label1 = new Label();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // btnopenReport
            // 
            btnopenReport.Location = new Point(33, 427);
            btnopenReport.Margin = new Padding(6, 5, 6, 5);
            btnopenReport.Name = "btnopenReport";
            btnopenReport.Size = new Size(174, 80);
            btnopenReport.TabIndex = 0;
            btnopenReport.Text = "打开年度报告";
            btnopenReport.UseVisualStyleBackColor = true;
            btnopenReport.Click += btnOpenReport_Click;
            // 
            // btoCreateReport
            // 
            btoCreateReport.Location = new Point(33, 263);
            btoCreateReport.Name = "btoCreateReport";
            btoCreateReport.Size = new Size(174, 82);
            btoCreateReport.TabIndex = 1;
            btoCreateReport.Text = "生成年度报告";
            btoCreateReport.UseVisualStyleBackColor = true;
            btoCreateReport.Click += btnCreateReport_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(33, 144);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(174, 34);
            textBox1.TabIndex = 2;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(228, 55);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 72;
            dataGridView1.Size = new Size(919, 569);
            dataGridView1.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(56, 92);
            label1.Name = "label1";
            label1.Size = new Size(139, 28);
            label1.TabIndex = 4;
            label1.Text = "请输入用户ID";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft YaHei UI", 14.1428576F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(578, 9);
            label2.Name = "label2";
            label2.Size = new Size(184, 43);
            label2.TabIndex = 5;
            label2.Text = "用户数据表";
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(13F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1159, 636);
            Controls.Add(label2);
            Controls.Add(label1);
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
        private Label label1;
        private Label label2;
    }
}
