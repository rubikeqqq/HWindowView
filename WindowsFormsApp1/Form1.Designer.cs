namespace WindowsFormsApp1
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
            this.button1 = new System.Windows.Forms.Button();
            this.uC_Window1 = new HWindowView.UC_Window();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(108, 87);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(149, 51);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // uC_Window1
            // 
            this.uC_Window1.BackColor = System.Drawing.Color.Transparent;
            this.uC_Window1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.uC_Window1.DrawModel = false;
            this.uC_Window1.EditModel = true;
            this.uC_Window1.Image = null;
            this.uC_Window1.Location = new System.Drawing.Point(352, 70);
            this.uC_Window1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.uC_Window1.Name = "uC_Window1";
            this.uC_Window1.Size = new System.Drawing.Size(393, 298);
            this.uC_Window1.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.uC_Window1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private HWindowView.UC_Window uC_Window1;
    }
}

