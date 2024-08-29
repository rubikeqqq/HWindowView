namespace 读码
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
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.uC_Window1 = new HWindowView.UC_Window();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(968, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(234, 45);
            this.button1.TabIndex = 1;
            this.button1.Text = "图像文件";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1012, 300);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(131, 45);
            this.button2.TabIndex = 2;
            this.button2.Text = "ReadCode";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "标准",
            "增强",
            "最强"});
            this.comboBox1.Location = new System.Drawing.Point(1019, 414);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 23);
            this.comboBox1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(958, 417);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "模式";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(958, 381);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "码类型";
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "Data Matrix ECC 200",
            "QR Code",
            "PDF417",
            "Micro QR Code",
            "Aztec Code",
            "GS1 Aztec Code",
            "GS1 DataMatrix",
            "GS1 QR Code"});
            this.comboBox2.Location = new System.Drawing.Point(1019, 378);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 23);
            this.comboBox2.TabIndex = 5;
            // 
            // uC_Window1
            // 
            this.uC_Window1.BackColor = System.Drawing.Color.Transparent;
            this.uC_Window1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.uC_Window1.DrawModel = false;
            this.uC_Window1.EditModel = true;
            this.uC_Window1.Image = null;
            this.uC_Window1.Location = new System.Drawing.Point(14, 16);
            this.uC_Window1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.uC_Window1.Name = "uC_Window1";
            this.uC_Window1.Size = new System.Drawing.Size(925, 821);
            this.uC_Window1.TabIndex = 0;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(968, 86);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(234, 45);
            this.button3.TabIndex = 7;
            this.button3.Text = "文件夹";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(975, 157);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(100, 31);
            this.button4.TabIndex = 8;
            this.button4.Text = "上一张";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(1102, 157);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(100, 31);
            this.button5.TabIndex = 9;
            this.button5.Text = "下一张";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1214, 849);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.uC_Window1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HWindowView.UC_Window uC_Window1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
    }
}

