namespace 找圆
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) )
            {
                components.Dispose( );
            }
            base.Dispose( disposing );
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent( )
        {
            this.uC_Window1 = new HWindowView.UC_Window();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button4 = new System.Windows.Forms.Button();
            this.cbTransition = new System.Windows.Forms.ComboBox();
            this.cbSelect = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbSigma = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbScore = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbThreshold = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbLen2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbLen1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbNum = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.cbRect = new System.Windows.Forms.CheckBox();
            this.cbROI = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uC_Window1
            // 
            this.uC_Window1.BackColor = System.Drawing.Color.Transparent;
            this.uC_Window1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.uC_Window1.DrawModel = false;
            this.uC_Window1.EditModel = true;
            this.uC_Window1.Image = null;
            this.uC_Window1.Location = new System.Drawing.Point(11, 7);
            this.uC_Window1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.uC_Window1.Name = "uC_Window1";
            this.uC_Window1.Size = new System.Drawing.Size(700, 620);
            this.uC_Window1.TabIndex = 0;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(760, 87);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(148, 45);
            this.button3.TabIndex = 9;
            this.button3.Text = "划圆";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(947, 87);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(119, 45);
            this.button2.TabIndex = 8;
            this.button2.Text = "拟合圆";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbROI);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.cbTransition);
            this.groupBox1.Controls.Add(this.cbSelect);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.tbSigma);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tbScore);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tbThreshold);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tbLen2);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbLen1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbNum);
            this.groupBox1.Location = new System.Drawing.Point(751, 163);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(360, 462);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(100, 411);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(119, 45);
            this.button4.TabIndex = 18;
            this.button4.Text = "应用参数";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // cbTransition
            // 
            this.cbTransition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTransition.FormattingEnabled = true;
            this.cbTransition.Items.AddRange(new object[] {
            "all",
            "negative",
            "positive"});
            this.cbTransition.Location = new System.Drawing.Point(119, 315);
            this.cbTransition.Name = "cbTransition";
            this.cbTransition.Size = new System.Drawing.Size(121, 23);
            this.cbTransition.TabIndex = 17;
            // 
            // cbSelect
            // 
            this.cbSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSelect.FormattingEnabled = true;
            this.cbSelect.Items.AddRange(new object[] {
            "all",
            "first",
            "last"});
            this.cbSelect.Location = new System.Drawing.Point(119, 269);
            this.cbSelect.Name = "cbSelect";
            this.cbSelect.Size = new System.Drawing.Size(121, 23);
            this.cbSelect.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(32, 365);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 15);
            this.label8.TabIndex = 15;
            this.label8.Text = "Sigma";
            // 
            // tbSigma
            // 
            this.tbSigma.Location = new System.Drawing.Point(119, 359);
            this.tbSigma.Name = "tbSigma";
            this.tbSigma.Size = new System.Drawing.Size(100, 25);
            this.tbSigma.TabIndex = 14;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(32, 318);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 15);
            this.label7.TabIndex = 13;
            this.label7.Text = "查找方向";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(32, 272);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 15);
            this.label6.TabIndex = 11;
            this.label6.Text = "查找边";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(32, 221);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "查找分数";
            // 
            // tbScore
            // 
            this.tbScore.Location = new System.Drawing.Point(119, 215);
            this.tbScore.Name = "tbScore";
            this.tbScore.Size = new System.Drawing.Size(100, 25);
            this.tbScore.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 169);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "阈值";
            // 
            // tbThreshold
            // 
            this.tbThreshold.Location = new System.Drawing.Point(119, 163);
            this.tbThreshold.Name = "tbThreshold";
            this.tbThreshold.Size = new System.Drawing.Size(100, 25);
            this.tbThreshold.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "查找宽度";
            // 
            // tbLen2
            // 
            this.tbLen2.Location = new System.Drawing.Point(119, 121);
            this.tbLen2.Name = "tbLen2";
            this.tbLen2.Size = new System.Drawing.Size(100, 25);
            this.tbLen2.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "投影长度";
            // 
            // tbLen1
            // 
            this.tbLen1.Location = new System.Drawing.Point(119, 80);
            this.tbLen1.Name = "tbLen1";
            this.tbLen1.Size = new System.Drawing.Size(100, 25);
            this.tbLen1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "查找数量";
            // 
            // tbNum
            // 
            this.tbNum.Location = new System.Drawing.Point(119, 38);
            this.tbNum.Name = "tbNum";
            this.tbNum.Size = new System.Drawing.Size(100, 25);
            this.tbNum.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(760, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(148, 45);
            this.button1.TabIndex = 6;
            this.button1.Text = "图像文件";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbRect
            // 
            this.cbRect.AutoSize = true;
            this.cbRect.Location = new System.Drawing.Point(947, 26);
            this.cbRect.Name = "cbRect";
            this.cbRect.Size = new System.Drawing.Size(104, 19);
            this.cbRect.TabIndex = 10;
            this.cbRect.Text = "显示矩形框";
            this.cbRect.UseVisualStyleBackColor = true;
            this.cbRect.CheckedChanged += new System.EventHandler(this.cbRect_CheckedChanged);
            // 
            // cbROI
            // 
            this.cbROI.AutoSize = true;
            this.cbROI.Location = new System.Drawing.Point(235, 425);
            this.cbROI.Name = "cbROI";
            this.cbROI.Size = new System.Drawing.Size(83, 19);
            this.cbROI.TabIndex = 19;
            this.cbROI.Text = "显示ROI";
            this.cbROI.UseVisualStyleBackColor = true;
            this.cbROI.CheckedChanged += new System.EventHandler(this.cbROI_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1170, 636);
            this.Controls.Add(this.cbRect);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.uC_Window1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HWindowView.UC_Window uC_Window1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ComboBox cbTransition;
        private System.Windows.Forms.ComboBox cbSelect;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbSigma;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbScore;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbThreshold;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbLen2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbLen1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbNum;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox cbRect;
        private System.Windows.Forms.CheckBox cbROI;
    }
}

