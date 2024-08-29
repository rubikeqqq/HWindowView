using HalconDotNet;
using HWindowView.Config;
using System;
using System.Collections;
using System.Collections.Generic;

namespace HWindowView.Model
{
    public delegate void IconicDelegate( ROIControlEvent val );

    /// <summary>
    /// 这个类作为HALCON窗口HWindow的包装器类。HWndCtrl负责可视化。 您可以使用GUI组件输入或使用鼠标移动和缩放可见图像部分。
    /// 类HWndCtrl使用图形堆栈来管理用于显示的图标对象。
    /// 每个对象都链接到一个图形上下文，该上下文决定了对象的绘制方式。可以通过调用changeGraphicSettings()来更改上下文。
    /// 图形化的“模式”是由类GraphicsContext定义的，并且映射了HDevelop中提供的大多数dev_set_*操作符。
    /// </summary>
    public class HWndCtrl
    {
        #region Fields

        private const int MAXNUMOBJLIST = 2;
        private ViewMode _stateView;               //显示选项
        private HalconDotNet.HWindowControl _viewPort;          //HALCON显示控件
        private ROICludeMode _dispROI;             //是否显示ROI
        private ArrayList _hObjImageList;          //待显示的HObjectImage集合 不能超过MAXNUMOBJLIST,否则第一个会被移除
        private GraphicsContext _mGC;             //GraphicsContext对象
        private ROIController _roiManager;        //roi 操作对象

        private bool _drawModel = false;           //drawMode = 如果_drawMode为true的情况下,所有鼠标的操作都禁止

        private double _imgRow1;                   //控件左上角的图像坐标row
        private double _imgCol1;                   //控件左上角的图像坐标col
        private double _imgRow2;                   //控件右下角的图像坐标row
        private double _imgCol2;                   //控件右下角的图像坐标col

        private int _imageHeight;                 //相对控件来说的图像的高度,左上角为0
        private int _imageWidth;                  //相对控件来说的图像的宽度,左上角为0
        private int _initHeight;                  //图像的真实高度
        private int _initWidth;                   //图像的真实高度

        private bool _mousePressed = false;        //鼠标是否按下
        private bool _showCross = false;           //是否显示中心线
        private string _crossColor = "yellow";      //中心线的颜色

        private double _startX, _startY;           //纪录鼠标按下时的坐标
        private double _zoomWndFactor;             //图像和控件的比例

        // 图形对象的集合
        private List<HObjectWithColor> _hObjectList { get; set; } = new List<HObjectWithColor>( );

        // 文字对象的集合
        private List<HTextWithColor> _hTextList { get; set; } = new List<HTextWithColor>( );

        #endregion Fields

        /// <summary>
        /// 初始化图像尺寸,鼠标事件和图形上下文
        /// </summary>
        /// <param name="view"> halcon窗口控件 </param>
        protected internal HWndCtrl( HalconDotNet.HWindowControl view )
        {
            _viewPort = view;
            _stateView = ViewMode.View_None;

            _zoomWndFactor = ( double )_imageWidth / _viewPort.Width;

            _dispROI = ROICludeMode.Include_ROI;//1;

            _viewPort.HMouseUp += new HMouseEventHandler( MouseUp );
            _viewPort.HMouseDown += new HMouseEventHandler( MouseDown );
            _viewPort.HMouseWheel += new HMouseEventHandler( HMouseWheel );
            _viewPort.HMouseMove += new HMouseEventHandler( MouseMoved );

            // graphical stack
            _hObjImageList = new ArrayList( 20 );
            _mGC = new GraphicsContext( );
        }

        /// <summary>
        /// 添加图像对象
        /// </summary>
        /// <param name="obj"> 图像对象 </param>
        public void AddIconicVar( HObject img )
        {
            //先把HObjImageList给全部释放了,源代码 会出现内存泄漏问题
            //先把HObjImageList给全部释放了,源代码 会出现内存泄漏问题
            //先把HObjImageList给全部释放了,源代码 会出现内存泄漏问题
            for( int i = 0 ; i < _hObjImageList.Count ; i++ )
            {
                ( ( HObjectEntry )_hObjImageList[ i ] ).Clear( );
            }

            HObjectEntry entry;

            if( img == null )
                return;

            HOperatorSet.GetObjClass( img , out var classValue );
            if( !classValue.S.Equals( "image" ) )
            {
                return;
            }

            HImage obj = new HImage( img );

            if( obj is HImage )
            {
                int h, w, area;

                area = obj.GetDomain( ).AreaCenter( out double _ , out double _ );
                obj.GetImagePointer1( out var _ , out w , out h );

                // 判断是否是完整图像
                if( area == ( w * h ) )
                {
                    ClearImageList( );

                    if( ( h != _initHeight ) || ( w != _initWidth ) )//这里判定是否要重绘，如果图像大小没有改变就不需要
                    {
                        _imageHeight = h;
                        _initHeight = h;
                        _imageWidth = w;
                        _initWidth = w;

                        _zoomWndFactor = ( double )_imageWidth / _viewPort.Width;

                        // 比较图像的宽高和控件的宽高的比例
                        double ratioWidth = ( 1.0 ) * _imageWidth / _viewPort.Width;       //图像的宽和控件宽的比例
                        double ratioHeight = ( 1.0 ) * _imageHeight / _viewPort.Height;    //图像的高和控件高的比例
                        HTuple row1, column1, row2, column2;

                        if( ratioWidth >= ratioHeight )
                        {
                            // 宽度的比例超过高度的比例 以col = 0计算
                            row1 = -( 1.0 ) * ( ( _viewPort.Height * ratioWidth ) - _imageHeight ) / 2;
                            column1 = 0;
                            row2 = row1 + _viewPort.Height * ratioWidth;
                            column2 = column1 + _viewPort.Width * ratioWidth;
                        }
                        else
                        {
                            // 高度的比例超过宽度的比例
                            row1 = 0;
                            column1 = -( 1.0 ) * ( ( _viewPort.Width * ratioHeight ) - _imageWidth ) / 2;
                            row2 = row1 + _viewPort.Height * ratioHeight;
                            column2 = column1 + _viewPort.Width * ratioHeight;
                        }
                        SetImagePart( ( int )row1.D , ( int )column1.D , ( int )row2.D , ( int )column2.D );
                        HOperatorSet.SetPart( _viewPort.HalconWindow , row1 , column1 , row2 , column2 );

                        //以上是同比例缩放显示窗口

                        // setImagePart(0, 0, h, w);
                    }
                }//if
            }//if

            entry = new HObjectEntry( obj , _mGC.CopyContextList( ) );

            _hObjImageList.Add( entry );

            //每当传入背景图的时候 都清空HObjectList
            ClearHObjectList( );

            if( _hObjImageList.Count > MAXNUMOBJLIST )
            {
                //需要自己手动释放
                ( ( HObjectEntry )_hObjImageList[ 0 ] ).Clear( );
                _hObjImageList.RemoveAt( 0 );
            }
        }

        /// <summary>
        /// Changes the current graphical context by setting the specified mode (constant starting
        /// by GC_*) to the specified value.
        /// </summary>
        /// <param name="mode">
        /// Constant that is provided by the class GraphicsContext and describes the mode that has
        /// to be changed, e.g., GraphicsContext.GC_COLOR
        /// </param>
        /// <param name="val">
        /// Value, provided as a string, the mode is to be changed to, e.g., "blue"
        /// </param>
        public void ChangeGraphicSettings( GCSetType mode , string val )
        {
            switch( mode )
            {
                case GCSetType.GC_COLOR:
                    _mGC.SetColorAttribute( val );
                    break;

                case GCSetType.GC_DRAWMODE:
                    _mGC.SetDrawModeAttribute( val );
                    break;

                case GCSetType.GC_LUT:
                    _mGC.SetLutAttribute( val );
                    break;

                case GCSetType.GC_PAINT:
                    _mGC.SetPaintAttribute( val );
                    break;

                case GCSetType.GC_SHAPE:
                    _mGC.SetShapeAttribute( val );
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Changes the current graphical context by setting the specified mode (constant starting
        /// by GC_*) to the specified value.
        /// </summary>
        /// <param name="mode">
        /// Constant that is provided by the class GraphicsContext and describes the mode that has
        /// to be changed, e.g., GraphicsContext.GC_LINEWIDTH
        /// </param>
        /// <param name="val">
        /// Value, provided as an integer, the mode is to be changed to, e.g., 5
        /// </param>
        public void ChangeGraphicSettings( GCSetType mode , int val )
        {
            switch( mode )
            {
                case GCSetType.GC_COLORED:
                    _mGC.SetColoredAttribute( val );
                    break;

                case GCSetType.GC_LINEWIDTH:
                    _mGC.SetLineWidthAttribute( val );
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Changes the current graphical context by setting the specified mode (constant starting
        /// by GC_*) to the specified value.
        /// </summary>
        /// <param name="mode">
        /// Constant that is provided by the class GraphicsContext and describes the mode that has
        /// to be changed, e.g., GraphicsContext.GC_LINESTYLE
        /// </param>
        /// <param name="val">
        /// Value, provided as an HTuple instance, the mode is to be changed to, e.g., new
        /// HTuple(new int[]{2,2})
        /// </param>
        public void ChangeGraphicSettings( GCSetType mode , HTuple val )
        {
            switch( mode )
            {
                case GCSetType.GC_LINESTYLE:
                    _mGC.SetLineStyleAttribute( val );
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Clears all entries from the graphical context list
        /// </summary>
        public void ClearGraphicContext( )
        {
            _mGC.ClearGraphicalSettings( );
        }

        /// <summary>
        /// 清除图像列表
        /// </summary>
        public void ClearImageList( )
        {
            _hObjImageList.Clear( );
        }

        /// <summary>
        /// Returns a clone of the graphical context list (hashtable)
        /// </summary>
        public Hashtable GetGraphicContext( )
        {
            return _mGC.CopyContextList( );
        }

        /// <summary>
        /// Returns the number of items on the graphics stack
        /// </summary>
        public int GetListCount( )
        {
            return _hObjImageList.Count;
        }

        /// <summary>
        /// 鼠标抬起时
        /// </summary>
        internal void RaiseMouseUp( )
        {
            _mousePressed = false;

            if( ( _roiManager != null ) && ( _roiManager.ActiveROIIdx != -1 )
                && ( _dispROI == ROICludeMode.Include_ROI ) )
            {
                _roiManager.NotifyRCObserver( ROIControlEvent.Update_ROI );
            }
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        protected internal void Repaint( )
        {
            Repaint( _viewPort.HalconWindow );
        }

        /// <summary>
        /// 图像是否允许缩放和拖动
        /// </summary>
        /// <param name="flag"> true时不能进行鼠标操作 </param>
        protected internal void SetDrawModel( bool flag )
        {
            _drawModel = flag;
        }

        /// <summary>
        /// 是否显示中心线
        /// </summary>
        /// <param name="flag"> </param>
        protected internal void SetCross( bool flag , string color )
        {
            _showCross = flag;
            _crossColor = color;
        }

        /// <summary>
        /// Sets the view mode for mouse events in the HALCON window (zoom, move, magnify or none).
        /// </summary>
        /// <param name="mode"> One of the MODE_VIEW_* constants </param>
        protected internal void SetViewState( ViewMode mode )
        {
            _stateView = mode;

            if( _roiManager != null )
                _roiManager.ResetROI( );
        }

        /// <summary>
        /// 设置ROI是否显示
        /// </summary>
        protected internal void SetDispLevel( ROICludeMode mode )
        {
            _dispROI = mode;
            Repaint( );
        }

        /// <summary>
        /// 注册ROIController到窗口
        /// </summary>
        /// <param name="rC"> ROIController实例 </param>
        protected internal void SetROIController( ROIController rC )
        {
            _roiManager = rC;
            rC.SetViewController( this );
            this.SetViewState( ViewMode.View_None );
        }

        /// <summary>
        /// 添加显示的图像
        /// </summary>
        /// <param name="image"> </param>
        protected internal void AddImageShow( HObject image )
        {
            AddIconicVar( image );
        }

        /// <summary>
        /// 重置所有参数
        /// </summary>
        protected internal void ResetROI( )
        {
            //_imgRow1 = 0;
            //_imgCol1 = 0;
            //_imgRow2 = _imageHeight;
            //_imgCol2 = _imageWidth;
            //LogHelper.Info($"imgRow1,imgCol1,imgRow2,imgCol2:{_imgRow1},{_imgCol1},{_imgRow2},{_imgCol2}");

            //_zoomWndFactor = (double)_imageWidth / _viewPort.Width;

            //System.Drawing.Rectangle rect = _viewPort.ImagePart;
            //rect.X = (int)_imgCol1;
            //rect.Y = (int)_imgRow1;
            //rect.Width = (int)_imageWidth;
            //rect.Height = (int)_imageHeight;
            //_viewPort.ImagePart = rect;
            //LogHelper.Info($"SetImagePart:{rect.X},{rect.Y},{rect.Width},{rect.Y}");

            if( _roiManager != null )
                _roiManager.Reset( );
        }

        /// <summary>
        /// 重置图像大小
        /// </summary>
        protected internal void ResetWindow( )
        {
            _zoomWndFactor = ( double )_imageWidth / _viewPort.Width;

            double ratioWidth = ( 1.0 ) * _initWidth / _viewPort.Width;
            double ratioHeight = ( 1.0 ) * _initHeight / _viewPort.Height;
            HTuple row1, column1, row2, column2;
            if( ratioWidth >= ratioHeight )
            {
                row1 = -( 1.0 ) * ( ( _viewPort.Height * ratioWidth ) - _initHeight ) / 2;
                column1 = 0;
                row2 = row1 + _viewPort.Height * ratioWidth;
                column2 = column1 + _viewPort.Width * ratioWidth;
            }
            else
            {
                row1 = 0;
                column1 = -( 1.0 ) * ( ( _viewPort.Width * ratioHeight ) - _initWidth ) / 2;
                row2 = row1 + _viewPort.Height * ratioHeight;
                column2 = column1 + _viewPort.Width * ratioHeight;
            }
            // 设置图像的坐标传入到_imgRow1,_imgCol1,_imgRow2,_imgCol2
            SetImagePart( ( int )row1.D , ( int )column1.D , ( int )row2.D , ( int )column2.D );
            HOperatorSet.SetPart( _viewPort.HalconWindow , row1 , column1 , row2 , column2 );
            ////以上是同比例缩放显示窗口
        }

        /// <summary>
        /// Repaints the HALCON window 'window'
        /// </summary>
        private void Repaint( HWindow window )
        {
            try
            {
                int count = _hObjImageList.Count;
                HObjectEntry entry;

                HSystem.SetSystem( "flush_graphic" , "false" );
                window.ClearWindow( );
                _mGC.ClearStateSettings( );

                //显示图片
                for( int i = 0 ; i < count ; i++ )
                {
                    entry = ( ( HObjectEntry )_hObjImageList[ i ] );
                    _mGC.ApplyContext( window , entry.GContext );
                    window.DispObj( entry.HObj );
                }

                // 显示中心线
                ShowMidCross( );

                // 显示region 包括查找到的模板contour等等
                ShowHObjectList( );

                // 显示绘制的roi
                if( _roiManager != null && ( _dispROI == ROICludeMode.Include_ROI ) )
                    _roiManager.PaintData( window );

                HSystem.SetSystem( "flush_graphic" , "true" );

                //注释了下面语句,会导致窗口无法实现缩放和拖动
                //window.SetColor("black");
                //window.DispLine(-100.0, -100.0, -101.0, -101.0);
            }
            catch( Exception )
            {
            }
        }

        /// <summary>
        /// 图像显示中心线
        /// </summary>
        private void ShowMidCross( )
        {
            if( _showCross )
            {
                HOperatorSet.SetColor( _viewPort.HalconWindow , _crossColor );
                double CrossCol = _initWidth / 2.0, CrossRow = _initHeight / 2.0;

                //竖线
                _viewPort.HalconWindow.DispPolygon( new HTuple( 0 , CrossRow - 50 ) , new HTuple( CrossCol , CrossCol ) );
                _viewPort.HalconWindow.DispPolygon( new HTuple( CrossRow + 50 , _initHeight ) , new HTuple( CrossCol , CrossCol ) );

                //中心点
                _viewPort.HalconWindow.DispPolygon( new HTuple( CrossRow - 2 , CrossRow + 2 ) , new HTuple( CrossCol , CrossCol ) );
                _viewPort.HalconWindow.DispPolygon( new HTuple( CrossRow , CrossRow ) , new HTuple( CrossCol - 2 , CrossCol + 2 ) );

                //横线

                _viewPort.HalconWindow.DispPolygon( new HTuple( CrossRow , CrossRow ) , new HTuple( 0 , CrossCol - 50 ) );
                _viewPort.HalconWindow.DispPolygon( new HTuple( CrossRow , CrossRow ) , new HTuple( CrossCol + 50 , _initWidth ) );
            }
        }

        /// <summary>
        /// 鼠标滚动事件
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void HMouseWheel( object sender , HMouseEventArgs e )
        {
            //关闭缩放
            if( _drawModel )
            {
                return;
            }

            double scale;

            if( e.Delta > 0 )
                scale = 0.9;
            else
                scale = 1 / 0.9;

            ZoomImage( e.X , e.Y , scale );
        }

        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void MouseDown( object sender , HalconDotNet.HMouseEventArgs e )
        {
            //关闭缩放事件
            if( _drawModel )
            {
                return;
            }

            _stateView = ViewMode.View_Move;
            _mousePressed = true;
            int activeROIidx = -1;

            // 当选中的是ROI时--改变ROI大小形状
            if( _roiManager != null && ( _dispROI == ROICludeMode.Include_ROI ) )
            {
                activeROIidx = _roiManager.MouseDownAction( e.X , e.Y );
            }
            // 当没有选中ROI时--移动图像
            if( activeROIidx == -1 )
            {
                switch( _stateView )
                {
                    case ViewMode.View_Move:
                        _startX = e.X;
                        _startY = e.Y;
                        break;

                    case ViewMode.View_None:
                        break;

                    default:
                        break;
                }
            }
            //end of if
        }

        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void MouseMoved( object sender , HMouseEventArgs e )
        {
            //关闭缩放事件
            if( _drawModel )
            {
                return;
            }

            double motionX, motionY;

            //如果鼠标没有按下时 直接返回
            //在HWindowCtrl中 处理灰度值的显示功能
            if( !_mousePressed )
                return;

            if( _roiManager != null && ( _roiManager.ActiveROIIdx != -1 ) && ( _dispROI == ROICludeMode.Include_ROI ) )
            {
                // 移动ROI时
                _roiManager.MouseMoveAction( e.X , e.Y );
            }
            else if( _stateView == ViewMode.View_Move )
            {
                // 移动图像时
                motionX = ( ( e.X - _startX ) );
                motionY = ( ( e.Y - _startY ) );

                if( ( ( int )motionX != 0 ) || ( ( int )motionY != 0 ) )
                {
                    MoveImage( motionX , motionY );
                    _startX = e.X - motionX;
                    _startY = e.Y - motionY;
                }
            }
        }

        /// <summary>
        /// 鼠标抬起事件
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void MouseUp( object sender , HalconDotNet.HMouseEventArgs e )
        {
            //关闭缩放事件
            if( _drawModel )
            {
                return;
            }

            _mousePressed = false;

            if( _roiManager != null
                && ( _roiManager.ActiveROIIdx != -1 )
                && ( _dispROI == ROICludeMode.Include_ROI ) )
            {
                _roiManager.NotifyRCObserver( ROIControlEvent.Update_ROI );
            }
        }

        /// <summary>
        /// 移动图像
        /// </summary>
        /// <param name="motionX"> 移动的x </param>
        /// <param name="motionY"> 移动的y </param>
        private void MoveImage( double motionX , double motionY )
        {
            _imgRow1 += -motionY;
            _imgRow2 += -motionY;

            _imgCol1 += -motionX;
            _imgCol2 += -motionX;

            // 将viewPort的ImagePart的height和width赋值给rect
            System.Drawing.Rectangle rect = _viewPort.ImagePart;
            rect.X = ( int )Math.Round( _imgCol1 );
            rect.Y = ( int )Math.Round( _imgRow1 );
            _viewPort.ImagePart = rect;

            Repaint( );
        }

        /// <summary>
        /// 设置控件的左上和右下的角对应的图像的坐标
        /// </summary>
        /// <param name="r1"> 左上角-图像row的坐标 </param>
        /// <param name="c1"> 左上角-图像col的坐标 </param>
        /// <param name="r2"> 右下角-图像row的坐标 </param>
        /// <param name="c2"> 右下角-图像col的坐标 </param>
        private void SetImagePart( int r1 , int c1 , int r2 , int c2 )
        {
            _imgRow1 = r1;
            _imgCol1 = c1;
            _imgRow2 = _imageHeight = r2;
            _imgCol2 = _imageWidth = c2;

            System.Drawing.Rectangle rect = _viewPort.ImagePart;
            rect.X = ( int )_imgCol1;
            rect.Y = ( int )_imgRow1;
            rect.Height = ( int )_imageHeight;
            rect.Width = ( int )_imageWidth;
            _viewPort.ImagePart = rect;
        }

        /// <summary>
        /// 根据当前的鼠标的x,y坐标和缩放比例进行图像缩放
        /// </summary>
        /// <param name="x"> 当前的x坐标 </param>
        /// <param name="y"> 当前的y坐标 </param>
        /// <param name="scale"> 缩放比例 </param>
        private void ZoomImage( double x , double y , double scale )
        {
            //关闭缩放事件
            if( _drawModel )
            {
                return;
            }

            double lengthC, lengthR;
            double percentC, percentR;
            int lenC, lenR;

            percentC = ( x - _imgCol1 ) / ( _imgCol2 - _imgCol1 );
            percentR = ( y - _imgRow1 ) / ( _imgRow2 - _imgRow1 );

            lengthC = ( _imgCol2 - _imgCol1 ) * scale;
            lengthR = ( _imgRow2 - _imgRow1 ) * scale;

            _imgCol1 = x - lengthC * percentC;
            _imgCol2 = x + lengthC * ( 1 - percentC );

            _imgRow1 = y - lengthR * percentR;
            _imgRow2 = y + lengthR * ( 1 - percentR );

            lenC = ( int )Math.Round( lengthC );
            lenR = ( int )Math.Round( lengthR );

            System.Drawing.Rectangle rect = _viewPort.ImagePart;
            rect.X = ( int )Math.Round( _imgCol1 );
            rect.Y = ( int )Math.Round( _imgRow1 );
            rect.Width = ( lenC > 0 ) ? lenC : 1;
            rect.Height = ( lenR > 0 ) ? lenR : 1;
            _viewPort.ImagePart = rect;

            double _zoomWndFactor = 1;
            _zoomWndFactor = scale * this._zoomWndFactor;

            if( this._zoomWndFactor < 0.001 && _zoomWndFactor < this._zoomWndFactor )
            {
                //超过一定缩放比例就不在缩放
                ResetWindow( );
                return;
            }
            if( this._zoomWndFactor > 3200 && _zoomWndFactor > this._zoomWndFactor )
            {
                //超过一定缩放比例就不在缩放
                ResetWindow( );
                return;
            }
            this._zoomWndFactor = _zoomWndFactor;

            Repaint( );
        }

        #region 再次显示region和 xld

        /// <summary>
        /// 清除所有的HObj对象和文字对象
        /// </summary>
        public void ClearHObjectList( )
        {
            foreach( HObjectWithColor hObjectWithColor in _hObjectList )
            {
                hObjectWithColor.HObject.Dispose( );
            }

            foreach( HTextWithColor hTupleWithColor in _hTextList )
            {
                hTupleWithColor.HText.Dispose( );
            }

            _hObjectList.Clear( );
            _hTextList.Clear( );

            Repaint( );
        }

        /// <summary>
        /// 默认红颜色显示
        /// </summary>
        /// <param name="hObj"> 传入的region.xld,image </param>
        public void DispObj( HObject hObj )
        {
            DispObj( hObj , null );
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
                //显示指定的颜色
                if( color != null )
                {
                    HOperatorSet.SetColor( _viewPort.HalconWindow , color );
                }
                else
                {
                    HOperatorSet.SetColor( _viewPort.HalconWindow , "red" );
                }

                if( hObj != null && hObj.IsInitialized( ) )
                {
                    //这就是重新开辟内存
                    HObject temp = new HObject( hObj );

                    _hObjectList.Add( new HObjectWithColor( temp , color ) );

                    _viewPort.HalconWindow.DispObj( temp );
                }
                //恢复默认的红色
                HOperatorSet.SetColor( _viewPort.HalconWindow , "red" );
            }
        }

        /// <summary>
        /// 显示文字信息
        /// </summary>
        /// <param name="hText"> </param>
        /// <param name="coordSystem"> </param>
        /// <param name="row"> </param>
        /// <param name="col"> </param>
        /// <param name="color"> </param>
        /// <param name="hv_Size"> </param>
        /// <param name="hv_Font"> </param>
        /// <param name="hv_Bold"> </param>
        /// <param name="hv_Slant"> </param>
        public void DispText( HTuple hText , HTuple coordSystem , HTuple row , HTuple col , HTuple color , int hv_Size , HTuple hv_Font , HTuple hv_Bold , HTuple hv_Slant )
        {
            lock( this )
            {
                if( hText != null )
                {
                    HTuple temp = new HTuple( hText );

                    _hTextList.Add( new HTextWithColor( temp , color , row , col , coordSystem , hv_Size , hv_Font , hv_Bold , hv_Slant ) );

                    set_display_font( _viewPort.HalconWindow , hv_Size , hv_Font , hv_Bold , hv_Slant );

                    _viewPort.HalconWindow.DispText( hText , coordSystem , row , col , color , "box" , "false" );
                    temp?.Dispose( );
                }
            }
        }

        private void set_display_font( HTuple hv_WindowHandle , HTuple hv_Size , HTuple hv_Font ,
            HTuple hv_Bold , HTuple hv_Slant )
        {
            // Local iconic variables

            // Local control variables

            HTuple hv_OS = new HTuple( ), hv_Fonts = new HTuple( );
            HTuple hv_Style = new HTuple( ), hv_Exception = new HTuple( );
            HTuple hv_AvailableFonts = new HTuple( ), hv_Fdx = new HTuple( );
            HTuple hv_Indices = new HTuple( );
            HTuple hv_Font_COPY_INP_TMP = new HTuple( hv_Font );
            HTuple hv_Size_COPY_INP_TMP = new HTuple( hv_Size );

            // Initialize local and output iconic variables
            try
            {
                //This procedure sets the text font of the current window with
                //the specified attributes.
                //
                //Input parameters:
                //WindowHandle: The graphics window for which the font will be set
                //Size: The font size. If Size=-1, the default of 16 is used.
                //Bold: If set to 'true', a bold font is used
                //Slant: If set to 'true', a slanted font is used
                //
                hv_OS.Dispose( );
                HOperatorSet.GetSystem( "operating_system" , out hv_OS );
                if( ( int )( ( new HTuple( hv_Size_COPY_INP_TMP.TupleEqual( new HTuple( ) ) ) ).TupleOr(
                    new HTuple( hv_Size_COPY_INP_TMP.TupleEqual( -1 ) ) ) ) != 0 )
                {
                    hv_Size_COPY_INP_TMP.Dispose( );
                    hv_Size_COPY_INP_TMP = 16;
                }
                if( ( int )( new HTuple( ( ( hv_OS.TupleSubstr( 0 , 2 ) ) ).TupleEqual( "Win" ) ) ) != 0 )
                {
                    //Restore previous behaviour
                    using( HDevDisposeHelper dh = new HDevDisposeHelper( ) )
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_Size = ( ( 1.13677 * hv_Size_COPY_INP_TMP ) ).TupleInt( )
                                ;
                            hv_Size_COPY_INP_TMP.Dispose( );
                            hv_Size_COPY_INP_TMP = ExpTmpLocalVar_Size;
                        }
                    }
                }
                else
                {
                    using( HDevDisposeHelper dh = new HDevDisposeHelper( ) )
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_Size = hv_Size_COPY_INP_TMP.TupleInt( )
                                ;
                            hv_Size_COPY_INP_TMP.Dispose( );
                            hv_Size_COPY_INP_TMP = ExpTmpLocalVar_Size;
                        }
                    }
                }
                if( ( int )( new HTuple( hv_Font_COPY_INP_TMP.TupleEqual( "Courier" ) ) ) != 0 )
                {
                    hv_Fonts.Dispose( );
                    hv_Fonts = new HTuple( );
                    hv_Fonts[ 0 ] = "Courier";
                    hv_Fonts[ 1 ] = "Courier 10 Pitch";
                    hv_Fonts[ 2 ] = "Courier New";
                    hv_Fonts[ 3 ] = "CourierNew";
                    hv_Fonts[ 4 ] = "Liberation Mono";
                }
                else if( ( int )( new HTuple( hv_Font_COPY_INP_TMP.TupleEqual( "mono" ) ) ) != 0 )
                {
                    hv_Fonts.Dispose( );
                    hv_Fonts = new HTuple( );
                    hv_Fonts[ 0 ] = "Consolas";
                    hv_Fonts[ 1 ] = "Menlo";
                    hv_Fonts[ 2 ] = "Courier";
                    hv_Fonts[ 3 ] = "Courier 10 Pitch";
                    hv_Fonts[ 4 ] = "FreeMono";
                    hv_Fonts[ 5 ] = "Liberation Mono";
                }
                else if( ( int )( new HTuple( hv_Font_COPY_INP_TMP.TupleEqual( "sans" ) ) ) != 0 )
                {
                    hv_Fonts.Dispose( );
                    hv_Fonts = new HTuple( );
                    hv_Fonts[ 0 ] = "Luxi Sans";
                    hv_Fonts[ 1 ] = "DejaVu Sans";
                    hv_Fonts[ 2 ] = "FreeSans";
                    hv_Fonts[ 3 ] = "Arial";
                    hv_Fonts[ 4 ] = "Liberation Sans";
                }
                else if( ( int )( new HTuple( hv_Font_COPY_INP_TMP.TupleEqual( "serif" ) ) ) != 0 )
                {
                    hv_Fonts.Dispose( );
                    hv_Fonts = new HTuple( );
                    hv_Fonts[ 0 ] = "Times New Roman";
                    hv_Fonts[ 1 ] = "Luxi Serif";
                    hv_Fonts[ 2 ] = "DejaVu Serif";
                    hv_Fonts[ 3 ] = "FreeSerif";
                    hv_Fonts[ 4 ] = "Utopia";
                    hv_Fonts[ 5 ] = "Liberation Serif";
                }
                else
                {
                    hv_Fonts.Dispose( );
                    hv_Fonts = new HTuple( hv_Font_COPY_INP_TMP );
                }
                hv_Style.Dispose( );
                hv_Style = "";
                if( ( int )( new HTuple( hv_Bold.TupleEqual( "true" ) ) ) != 0 )
                {
                    using( HDevDisposeHelper dh = new HDevDisposeHelper( ) )
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_Style = hv_Style + "Bold";
                            hv_Style.Dispose( );
                            hv_Style = ExpTmpLocalVar_Style;
                        }
                    }
                }
                else if( ( int )( new HTuple( hv_Bold.TupleNotEqual( "false" ) ) ) != 0 )
                {
                    hv_Exception.Dispose( );
                    hv_Exception = "Wrong value of control parameter Bold";
                    throw new HalconException( hv_Exception );
                }
                if( ( int )( new HTuple( hv_Slant.TupleEqual( "true" ) ) ) != 0 )
                {
                    using( HDevDisposeHelper dh = new HDevDisposeHelper( ) )
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_Style = hv_Style + "Italic";
                            hv_Style.Dispose( );
                            hv_Style = ExpTmpLocalVar_Style;
                        }
                    }
                }
                else if( ( int )( new HTuple( hv_Slant.TupleNotEqual( "false" ) ) ) != 0 )
                {
                    hv_Exception.Dispose( );
                    hv_Exception = "Wrong value of control parameter Slant";
                    throw new HalconException( hv_Exception );
                }
                if( ( int )( new HTuple( hv_Style.TupleEqual( "" ) ) ) != 0 )
                {
                    hv_Style.Dispose( );
                    hv_Style = "Normal";
                }
                hv_AvailableFonts.Dispose( );
                HOperatorSet.QueryFont( hv_WindowHandle , out hv_AvailableFonts );
                hv_Font_COPY_INP_TMP.Dispose( );
                hv_Font_COPY_INP_TMP = "";
                for( hv_Fdx = 0 ; ( int )hv_Fdx <= ( int )( ( new HTuple( hv_Fonts.TupleLength( ) ) ) - 1 ) ; hv_Fdx = ( int )hv_Fdx + 1 )
                {
                    hv_Indices.Dispose( );
                    using( HDevDisposeHelper dh = new HDevDisposeHelper( ) )
                    {
                        hv_Indices = hv_AvailableFonts.TupleFind(
                            hv_Fonts.TupleSelect( hv_Fdx ) );
                    }
                    if( ( int )( new HTuple( ( new HTuple( hv_Indices.TupleLength( ) ) ).TupleGreater(
                        0 ) ) ) != 0 )
                    {
                        if( ( int )( new HTuple( ( ( hv_Indices.TupleSelect( 0 ) ) ).TupleGreaterEqual( 0 ) ) ) != 0 )
                        {
                            hv_Font_COPY_INP_TMP.Dispose( );
                            using( HDevDisposeHelper dh = new HDevDisposeHelper( ) )
                            {
                                hv_Font_COPY_INP_TMP = hv_Fonts.TupleSelect(
                                    hv_Fdx );
                            }
                            break;
                        }
                    }
                }
                if( ( int )( new HTuple( hv_Font_COPY_INP_TMP.TupleEqual( "" ) ) ) != 0 )
                {
                    throw new HalconException( "Wrong value of control parameter Font" );
                }
                using( HDevDisposeHelper dh = new HDevDisposeHelper( ) )
                {
                    {
                        HTuple
                          ExpTmpLocalVar_Font = ( ( ( hv_Font_COPY_INP_TMP + "-" ) + hv_Style ) + "-" ) + hv_Size_COPY_INP_TMP;
                        hv_Font_COPY_INP_TMP.Dispose( );
                        hv_Font_COPY_INP_TMP = ExpTmpLocalVar_Font;
                    }
                }
                HOperatorSet.SetFont( hv_WindowHandle , hv_Font_COPY_INP_TMP );

                hv_Font_COPY_INP_TMP.Dispose( );
                hv_Size_COPY_INP_TMP.Dispose( );
                hv_OS.Dispose( );
                hv_Fonts.Dispose( );
                hv_Style.Dispose( );
                hv_Exception.Dispose( );
                hv_AvailableFonts.Dispose( );
                hv_Fdx.Dispose( );
                hv_Indices.Dispose( );

                return;
            }
            catch( HalconException HDevExpDefaultException )
            {
                hv_Font_COPY_INP_TMP.Dispose( );
                hv_Size_COPY_INP_TMP.Dispose( );
                hv_OS.Dispose( );
                hv_Fonts.Dispose( );
                hv_Style.Dispose( );
                hv_Exception.Dispose( );
                hv_AvailableFonts.Dispose( );
                hv_Fdx.Dispose( );
                hv_Indices.Dispose( );

                throw HDevExpDefaultException;
            }
        }

        /// <summary>
        /// 将hObjectList中的HObject,按照先后顺序显示出来
        /// </summary>
        private void ShowHObjectList( )
        {
            try
            {
                foreach( HObjectWithColor hObjectWithColor in _hObjectList )
                {
                    if( hObjectWithColor.Color != null )
                    {
                        HOperatorSet.SetColor( _viewPort.HalconWindow , hObjectWithColor.Color );
                    }
                    else
                    {
                        HOperatorSet.SetColor( _viewPort.HalconWindow , "red" );
                    }
                    if( hObjectWithColor != null && hObjectWithColor.HObject.IsInitialized( ) )
                    {
                        _viewPort.HalconWindow.DispObj( hObjectWithColor.HObject );

                        //恢复默认的红色
                        HOperatorSet.SetColor( _viewPort.HalconWindow , "red" );
                    }
                }

                foreach( HTextWithColor hTupleWithColor in _hTextList )
                {
                    if( hTupleWithColor.HText != null )
                    {
                        set_display_font( _viewPort.HalconWindow , hTupleWithColor.Size , hTupleWithColor.Font , hTupleWithColor.Bold , hTupleWithColor.Slant );

                        _viewPort.HalconWindow.DispText( hTupleWithColor.HText , hTupleWithColor.CoordSystem , hTupleWithColor.Row , hTupleWithColor.Col , hTupleWithColor.Color , "box" , "false" );
                    }
                }
            }
            catch( Exception )
            {
                //有时候hobj被dispose了,但是其本身不为null,此时则报错. 已经使用IsInitialized解决了
            }
        }

        #endregion 再次显示region和 xld
    }//end of class
}