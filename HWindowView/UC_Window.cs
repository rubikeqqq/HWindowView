using System;
using System.Windows.Forms;
using HalconDotNet;

namespace HWindowView
{
    public class UC_Window : UserControl
    {
        #region 私有变量定义.

        //窗体的控件
        public HWindowControl hWindowControl;

        //halcon窗体控件的句柄 this.mCtrl_HWindow.HalconWindow;
        private HWindow hv_window;

        //缩放时操作的图片  此处千万不要使用hv_image = new HImage(),/不然在生成控件dll的时候,会导致无法序列化
        private HImage ho_image;

        private ViewWindow _view;

        private bool _drawModel = false; //绘制模式下,不允许缩放和鼠标右键菜单
        private bool _editModel = true; //编辑模式，false时不允许编辑，不能移动ROI
        private string str_imgSize; //保存图像的信息
        private int hv_imageWidth,
            hv_imageHeight; //图片宽,高

        private ContextMenuStrip hv_MenuStrip; //右键菜单控件
        private ToolStripMenuItem saveImg_strip; //保存图像
        private ToolStripMenuItem barVisible_strip; //信息显示
        private ToolStripMenuItem saveWindow_strip; //保存截图
        private ToolStripMenuItem fit_strip; //自适应
        private ToolStripMenuItem cross_strip; //中心线
        #endregion 私有变量定义.

        /// <summary>
        /// 初始化控件
        /// </summary>
        public UC_Window( )
        {
            InitializeComponent( );

            _view = new ViewWindow( mCtrl_HWindow );

            try
            {
                //HALCON 窗体 赋值 给外部调用
                hWindowControl = mCtrl_HWindow;
                hv_window = mCtrl_HWindow.HalconWindow;
            }
            catch { }

            fit_strip = new ToolStripMenuItem( "适应窗口" );
            fit_strip.Click += new EventHandler( ( s , e ) => DispImageFit( mCtrl_HWindow ) );

            barVisible_strip = new ToolStripMenuItem( "显示StatusBar" );
            barVisible_strip.CheckOnClick = true;
            barVisible_strip.CheckedChanged += new EventHandler( barVisible_strip_CheckedChanged );
            m_CtrlHStatusLabelCtrl.Visible = false;
            mCtrl_HWindow.Height = this.Height;

            saveImg_strip = new ToolStripMenuItem( "保存原始图像" );
            saveImg_strip.Click += new EventHandler( ( s , e ) => SaveImage( ) );

            saveWindow_strip = new ToolStripMenuItem( "保存窗口缩略图" );
            saveWindow_strip.Click += new EventHandler( ( s , e ) => SaveWindowDump( ) );

            cross_strip = new ToolStripMenuItem( "显示中心线" );
            cross_strip.CheckOnClick = true;
            cross_strip.CheckedChanged += new EventHandler( Cross_CheckedChanged );

            hv_MenuStrip = new ContextMenuStrip( );
            hv_MenuStrip.Items.Add( fit_strip );
            hv_MenuStrip.Items.Add( barVisible_strip );
            hv_MenuStrip.Items.Add( cross_strip );
            hv_MenuStrip.Items.Add( saveImg_strip );
            hv_MenuStrip.Items.Add( saveWindow_strip );

            cross_strip.Enabled = false;
            barVisible_strip.Enabled = false;
            fit_strip.Enabled = false;
            saveImg_strip.Enabled = false;
            saveWindow_strip.Enabled = false;
            mCtrl_HWindow.ContextMenuStrip = hv_MenuStrip;
            mCtrl_HWindow.SizeChanged += new EventHandler( ( s , e ) => DispImageFit( mCtrl_HWindow ) );
        }

        /// <summary>
        /// ROIControl和HWndCtrl的处理类ViewWindow
        /// </summary>
        public ViewWindow ViewWindow
        {
            get { return _view; }
            set { _view = value; }
        }

        /// <summary>
        /// 绘制模式下,不允许缩放和鼠标右键菜单 为true时 不允许鼠标操作
        /// </summary>
        public bool DrawModel
        {
            get { return _drawModel; }
            set
            {
                //缩放控制
                _view.SetDrawModel( value );
                //绘制模式 不现实右键
                if( value == true )
                {
                    mCtrl_HWindow.ContextMenuStrip = null;
                }
                else
                {
                    //恢复
                    mCtrl_HWindow.ContextMenuStrip = hv_MenuStrip;
                }
                _drawModel = value;
            }
        }

        /// <summary>
        /// 绘制的图形是否可以编辑 True为允许编辑
        /// </summary>
        public bool EditModel
        {
            get { return _editModel; }
            set
            {
                _view.SetEditModel( value );
                _editModel = value;
            }
        }

        /// <summary>
        /// image赋值并显示
        /// </summary>
        public HImage Image
        {
            get { return ho_image; }
            set
            {
                if( value != null )
                {
                    if( ho_image != null )
                    {
                        ho_image.Dispose( );
                    }

                    ho_image = value;
                    ho_image.GetImageSize( out hv_imageWidth , out hv_imageHeight );
                    str_imgSize = String.Format( "图像大小:({0},{1})" , hv_imageWidth , hv_imageHeight );

                    try
                    {
                        barVisible_strip.Enabled = true;
                        fit_strip.Enabled = true;
                        saveImg_strip.Enabled = true;
                        saveWindow_strip.Enabled = true;
                        cross_strip.Enabled = true;
                    }
                    catch( Exception ) { }
                    _view.DisplayImage( ho_image );
                }
            }
        }

        /// <summary>
        /// 清空窗体显示
        /// </summary>
        public void ClearWindow( )
        {
            try
            {
                this.Invoke(
                    new Action( ( ) =>
                    {
                        m_CtrlHStatusLabelCtrl.Visible = false;
                        barVisible_strip.Enabled = false;
                        fit_strip.Enabled = false;
                        saveImg_strip.Enabled = false;
                        saveWindow_strip.Enabled = false;
                        cross_strip.Enabled = false;

                        mCtrl_HWindow.HalconWindow.ClearWindow( );
                        _view.ClearWindow( );
                    } )
                );
            }
            catch( Exception ex )
            {
                throw ex;
            }
        }

        public void ClearObj( )
        {
            _view.ClearHObject( );
        }

        /// <summary>
        /// 返回 HALCON 控件
        /// </summary>
        /// <returns> HALCON 控件 </returns>
        public HWindowControl GetHWindowControl( )
        {
            return mCtrl_HWindow;
        }

        /// <summary>
        /// 添加图片并显示
        /// </summary>
        /// <param name="hobject"> 传递Hobject,必须为图像 </param>
        public void HobjectToHimage( HObject hobject )
        {
            if( hobject == null || !hobject.IsInitialized( ) )
            {
                ClearWindow( );
                return;
            }

            Image = new HImage( hobject );
        }

        #region 事件处理器

        /// <summary>
        /// 状态条 显示/隐藏 CheckedChanged事件
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void barVisible_strip_CheckedChanged( object sender , EventArgs e )
        {
            ToolStripMenuItem strip = sender as ToolStripMenuItem;

            SuspendLayout( );

            if( strip.Checked )
            {
                m_CtrlHStatusLabelCtrl.Visible = true;
                mCtrl_HWindow.HMouseMove += HWindowControl_HMouseMove;
            }
            else
            {
                m_CtrlHStatusLabelCtrl.Visible = false;
                mCtrl_HWindow.HMouseMove -= HWindowControl_HMouseMove;
            }

            ResumeLayout( false );
            PerformLayout( );
        }

        /// <summary>
        /// 图片适应大小显示在窗体
        /// </summary>
        /// <param name="hw_Ctrl"> halcon窗体控件 </param>
        private void DispImageFit( HWindowControl hw_Ctrl )
        {
            try
            {
                _view.ResetWindowImage( );
            }
            catch( Exception ) { }
        }

        /// <summary>
        /// 鼠标在空间窗体里滑动,显示鼠标所在位置的灰度值
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void HWindowControl_HMouseMove( object sender , HMouseEventArgs e )
        {
            if( ho_image != null )
            {
                try
                {
                    int button_state;
                    double positionX,
                        positionY;
                    string str_value;
                    string str_position;
                    bool _isXOut = true,
                        _isYOut = true;
                    HTuple channel_count;
                    HOperatorSet.CountChannels( ho_image , out channel_count );

                    hv_window.GetMpositionSubPix( out positionY , out positionX , out button_state );

                    str_position = String.Format(
                        "X: {0:0000.0}, Y: {1:0000.0}" ,
                        positionX ,
                        positionY
                    );

                    _isXOut = ( positionX < 0 || positionX >= hv_imageWidth );
                    _isYOut = ( positionY < 0 || positionY >= hv_imageHeight );

                    if( !_isXOut && !_isYOut )
                    {
                        if( ( int )channel_count == 1 )
                        {
                            double grayVal;
                            grayVal = ho_image.GetGrayval( ( int )positionY , ( int )positionX );
                            str_value = String.Format( "Val: {0:000.0}" , grayVal );
                        }
                        else if( ( int )channel_count == 3 )
                        {
                            double grayValRed,
                                grayValGreen,
                                grayValBlue;

                            HImage _RedChannel,
                                _GreenChannel,
                                _BlueChannel;

                            _RedChannel = ho_image.AccessChannel( 1 );
                            _GreenChannel = ho_image.AccessChannel( 2 );
                            _BlueChannel = ho_image.AccessChannel( 3 );

                            grayValRed = _RedChannel.GetGrayval( ( int )positionY , ( int )positionX );
                            grayValGreen = _GreenChannel.GetGrayval( ( int )positionY , ( int )positionX );
                            grayValBlue = _BlueChannel.GetGrayval( ( int )positionY , ( int )positionX );

                            _RedChannel.Dispose( );
                            _GreenChannel.Dispose( );
                            _BlueChannel.Dispose( );

                            str_value = String.Format(
                                "Val: ({0:000.0}, {1:000.0}, {2:000.0})" ,
                                grayValRed ,
                                grayValGreen ,
                                grayValBlue
                            );
                        }
                        else
                        {
                            str_value = "";
                        }
                        m_CtrlHStatusLabelCtrl.Text =
                            str_imgSize + "    " + str_position + "    " + str_value;
                    }
                }
                catch( Exception )
                {
                    //不处理
                }
            }
        }

        /// <summary>
        /// 鼠标离开事件
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void mCtrl_HWindow_MouseLeave( object sender , EventArgs e )
        {
            //避免鼠标离开窗口,再返回的时候,图表随着鼠标移动
            _view.Mouseleave( );
        }

        /// <summary>
        /// 保存原始图片到本地
        /// </summary>
        private void SaveImage( )
        {
            SaveFileDialog sfd = new SaveFileDialog( );
            sfd.Filter = "BMP图像|*.bmp|所有文件|*.*";

            if( sfd.ShowDialog( ) == DialogResult.OK )
            {
                if( String.IsNullOrEmpty( sfd.FileName ) )
                {
                    return;
                }

                HOperatorSet.WriteImage( ho_image , "bmp" , 0 , sfd.FileName );
            }
        }

        /// <summary>
        /// 保存窗体截图到本地
        /// </summary>
        private void SaveWindowDump( )
        {
            SaveFileDialog sfd = new SaveFileDialog( );
            sfd.Filter = "PNG图像|*.png|所有文件|*.*";

            if( sfd.ShowDialog( ) == DialogResult.OK )
            {
                if( String.IsNullOrEmpty( sfd.FileName ) )
                    return;

                //截取窗口图
                HOperatorSet.DumpWindow( hWindowControl.HalconWindow , "png best" , sfd.FileName );
            }
        }

        /// <summary>
        /// 显示中心线
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void Cross_CheckedChanged( object sender , EventArgs e )
        {
            ToolStripMenuItem strip = sender as ToolStripMenuItem;
            if( strip.Checked )
            {
                _view.SetCross( true );
            }
            else
            {
                _view.SetCross( false );
            }
            _view.Repaint( );
        }

        #endregion 事件处理器

        #region 缩放后,再次显示传入的HObject

        /// <summary>
        /// 默认红颜色显示
        /// </summary>
        /// <param name="hObj"> 传入的region.xld,image </param>
        public void DispObj( HObject hObj )
        {
            lock( this )
            {
                _view.DisplayHobject( hObj , null );
            }
        }

        /// <summary>
        /// 重新开辟内存保存 防止被传入的HObject在其他地方dispose后,不能重现
        /// </summary>
        /// <param name="hObj"> 传入的region.xld,image </param>
        /// <param name="color"> 颜色 </param>
        public void DispObj( HObject hObj , string color )
        {
            lock( this )
            {
                _view.DisplayHobject( hObj , color );
            }
        }

        public void DispText(
            HTuple htext ,
            HTuple coordSystem ,
            HTuple color ,
            HTuple row ,
            HTuple col ,
            int size ,
            string font ,
            HTuple bold ,
            HTuple slant
        )
        {
            lock( this )
            {
                _view.DisplayHtuple( htext , coordSystem , color , row , col , size , font , bold , slant );
            }
        }

        public void DispText( HTuple htext , HTuple color )
        {
            lock( this )
            {
                _view.DispText( htext , color );
            }
        }

        #endregion 缩放后,再次显示传入的HObject

        #region Auto Generate

        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label m_CtrlHStatusLabelCtrl;

        private HalconDotNet.HWindowControl mCtrl_HWindow;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing"> 如果应释放托管资源，为 true；否则为 false。 </param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) )
            {
                components.Dispose( );
                hv_MenuStrip.Dispose( );

                mCtrl_HWindow.HMouseMove -= HWindowControl_HMouseMove;
            }
            if( disposing && ho_image != null )
            {
                ho_image.Dispose( );
            }
            if( disposing && hv_window != null )
            {
                hv_window.Dispose( );
            }
            base.Dispose( disposing );
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent( )
        {
            this.m_CtrlHStatusLabelCtrl = new System.Windows.Forms.Label( );
            this.mCtrl_HWindow = new HalconDotNet.HWindowControl( );
            this.SuspendLayout( );
            // 
            // m_CtrlHStatusLabelCtrl
            // 
            this.m_CtrlHStatusLabelCtrl.BackColor = System.Drawing.Color.Black;
            this.m_CtrlHStatusLabelCtrl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.m_CtrlHStatusLabelCtrl.Font = new System.Drawing.Font( "微软雅黑" , 9F , System.Drawing.FontStyle.Bold , System.Drawing.GraphicsUnit.Point , ( ( byte )( 134 ) ) );
            this.m_CtrlHStatusLabelCtrl.ForeColor = System.Drawing.Color.PaleTurquoise;
            this.m_CtrlHStatusLabelCtrl.Location = new System.Drawing.Point( 0 , 358 );
            this.m_CtrlHStatusLabelCtrl.Margin = new System.Windows.Forms.Padding( 4 , 3 , 4 , 3 );
            this.m_CtrlHStatusLabelCtrl.Name = "m_CtrlHStatusLabelCtrl";
            this.m_CtrlHStatusLabelCtrl.Size = new System.Drawing.Size( 526 , 22 );
            this.m_CtrlHStatusLabelCtrl.TabIndex = 1;
            this.m_CtrlHStatusLabelCtrl.Text = "(-,-)";
            // 
            // mCtrl_HWindow
            // 
            this.mCtrl_HWindow.BackColor = System.Drawing.Color.Black;
            this.mCtrl_HWindow.BorderColor = System.Drawing.Color.Black;
            this.mCtrl_HWindow.Cursor = System.Windows.Forms.Cursors.Default;
            this.mCtrl_HWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mCtrl_HWindow.ImagePart = new System.Drawing.Rectangle( 0 , 0 , 640 , 480 );
            this.mCtrl_HWindow.Location = new System.Drawing.Point( 0 , 0 );
            this.mCtrl_HWindow.Margin = new System.Windows.Forms.Padding( 0 );
            this.mCtrl_HWindow.Name = "mCtrl_HWindow";
            this.mCtrl_HWindow.Size = new System.Drawing.Size( 526 , 380 );
            this.mCtrl_HWindow.TabIndex = 0;
            this.mCtrl_HWindow.WindowSize = new System.Drawing.Size( 526 , 380 );
            this.mCtrl_HWindow.MouseLeave += new System.EventHandler( this.mCtrl_HWindow_MouseLeave );
            // 
            // UC_Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 8F , 15F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add( this.m_CtrlHStatusLabelCtrl );
            this.Controls.Add( this.mCtrl_HWindow );
            this.Margin = new System.Windows.Forms.Padding( 4 , 3 , 4 , 3 );
            this.Name = "UC_Window";
            this.Size = new System.Drawing.Size( 526 , 380 );
            this.ResumeLayout( false );

        }

        #endregion 组件设计器生成的代码

        #endregion Auto Generate
    }
}
