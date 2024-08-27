
namespace 模板匹配
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
            if(disposing && (components != null))
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
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // uC_Window1
            // 
            this.uC_Window1.BackColor = System.Drawing.Color.Transparent;
            this.uC_Window1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.uC_Window1.DrawModel = false;
            this.uC_Window1.EditModel = true;
            this.uC_Window1.Image = null;
            this.uC_Window1.Location = new System.Drawing.Point(20, 17);
            this.uC_Window1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.uC_Window1.Name = "uC_Window1";
            this.uC_Window1.Size = new System.Drawing.Size(954, 869);
            this.uC_Window1.TabIndex = 0;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(1030, 501);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(138, 51);
            this.button4.TabIndex = 8;
            this.button4.Text = "测试";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1030, 405);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(138, 51);
            this.button3.TabIndex = 7;
            this.button3.Text = "查找模板";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1030, 299);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(138, 51);
            this.button2.TabIndex = 6;
            this.button2.Text = "创建模板";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1030, 216);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(138, 51);
            this.button1.TabIndex = 5;
            this.button1.Text = "读取图像";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1222, 898);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.uC_Window1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private HWindowView.UC_Window uC_Window1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
    }
}

