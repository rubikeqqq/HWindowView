using HalconDotNet;
using HWindowView.Model;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace HWindowView
{
    /// <summary>
    /// 处理ROI和图像处理的类
    /// </summary>
    public class ViewWindow : IViewWindow
    {
        private ROIController _roiController;
        private HWndCtrl _hWndControl;

        /// <summary>
        /// ViewWindow构造函数 HWndCtrl实例化 roiController实例化 将roiController注册到hWndCtrl中
        /// </summary>
        /// <param name="window"> </param>
        public ViewWindow( HWindowControl window )
        {
            _hWndControl = new HWndCtrl( window );
            _roiController = new ROIController( );
            _hWndControl.SetROIController( _roiController );
            _hWndControl.SetViewState( ViewMode.View_None );
        }

        /// <summary>
        /// 清空所有显示内容
        /// </summary>
        public void ClearWindow( )
        {
            //清空显示image
            _hWndControl.ClearImageList( );
            //清空hobjectList
            _hWndControl.ClearHObjectList( );
            //清空roi
            _roiController.Reset( );
        }

        public void ClearHObject( )
        {
            //清空hobjectList
            _hWndControl.ClearHObjectList( );
        }

        /// <summary>
        /// 显示图像
        /// </summary>
        /// <param name="img"> 图像对象 </param>
        public void DisplayImage( HObject img )
        {
            //添加背景图片
            _hWndControl.AddImageShow( img );
            //清空roi容器
            _roiController.Reset( );
            //显示图片
            _hWndControl.Repaint( );
        }

        /// <summary>
        /// 获取当前窗口显示的roi数量
        /// </summary>
        /// <returns> ROI的数量 </returns>
        public int GetROICount( )
        {
            return _roiController.ROICount;
        }

        /// <summary>
        /// 是否开启图像缩放
        /// </summary>
        /// <param name="flag"> 当flag为true时,鼠标禁止操作 </param>
        public void SetDrawModel( bool flag )
        {
            _hWndControl.SetDrawModel( flag );
        }

        /// <summary>
        /// 设置中心线
        /// </summary>
        /// <param name="flag"> 是否显示中心线 </param>
        /// <param name="color"> 中心线的颜色 (默认yellow) </param>
        public void SetCross( bool flag , string color = "green" )
        {
            _hWndControl.SetCross( flag , color );
        }

        /// <summary>
        /// 是否开启ROI编辑
        /// </summary>
        /// <param name="flag"> true时可以编辑ROI </param>
        public void SetEditModel( bool flag )
        {
            _roiController.SetEditModel( flag );
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        public void Repaint( )
        {
            _hWndControl.Repaint( );
        }

        /// <summary>
        /// 将image适应图像控件
        /// </summary>
        public void ResetWindowImage( )
        {
            _hWndControl.ResetWindow( );
            _hWndControl.Repaint( );
        }

        /// <summary>
        /// 鼠标离开
        /// </summary>
        public void Mouseleave( )
        {
            _hWndControl.RaiseMouseUp( );
        }

        /// <summary>
        /// 设置是否显示ROI
        /// </summary>
        /// <param name="mode"> </param>
        public void SetDispLevel( ROICludeMode mode )
        {
            _hWndControl.SetDispLevel( mode );
        }

        /// <summary>
        /// 设置ROI的计算方法
        /// </summary>
        /// <param name="roiSignFlag"> positive / negative </param>
        public void SetROISign( ROISignFlag roiSignFlag )
        {
            _roiController.SetROISign( roiSignFlag );
        }

        /// <summary>
        /// 获取所有的ROI
        /// </summary>
        /// <returns> </returns>
        public ArrayList GetROIList( )
        {
            return _roiController.GetROIList( );
        }

        /// <summary>
        /// 生成Rect1
        /// </summary>
        /// <param name="row1"> row1 </param>
        /// <param name="col1"> col1 </param>
        /// <param name="row2"> row2 </param>
        /// <param name="col2"> col2 </param>
        /// <param name="rois"> roi集合对象 </param>
        public void GenRect1( double row1 , double col1 , double row2 , double col2 , ref List<ROI> rois )
        {
            _roiController.GenRect1( row1 , col1 , row2 , col2 , ref rois );
        }

        /// <summary>
        /// 生成Rect2
        /// </summary>
        /// <param name="row"> row </param>
        /// <param name="col"> col </param>
        /// <param name="phi"> phi </param>
        /// <param name="length1"> length1 </param>
        /// <param name="length2"> length2 </param>
        /// <param name="rois"> roi集合对象 </param>
        public void GenRect2( double row , double col , double phi , double length1 , double length2 , ref List<ROI> rois )
        {
            _roiController.GenRect2( row , col , phi , length1 , length2 , ref rois );
        }

        /// <summary>
        /// 生成Circle
        /// </summary>
        /// <param name="row"> row </param>
        /// <param name="col"> col </param>
        /// <param name="radius"> radius </param>
        /// <param name="rois"> roi集合对象 </param>
        public void GenCircle( double row , double col , double radius , ref List<ROI> rois )
        {
            _roiController.GenCircle( row , col , radius , ref rois );
        }

        /// <summary>
        /// 生成Point
        /// </summary>
        /// <param name="row"> row </param>
        /// <param name="col"> col </param>
        /// <param name="rois"> roi集合对象 </param>
        public void GenPoint( double row , double col , ref List<ROI> rois )
        {
            _roiController.GenPoint( row , col , ref rois );
        }

        /// <summary>
        /// 生成CircularArc
        /// </summary>
        /// <param name="row"> row </param>
        /// <param name="col"> col </param>
        /// <param name="radius"> radius </param>
        /// <param name="startPhi"> startphi </param>
        /// <param name="extentPhi"> extentphi </param>
        /// <param name="direct"> direct方向 </param>
        /// <param name="rois"> roi集合对象 </param>
        public void GenCircularArc( double row , double col , double radius , double startPhi , double extentPhi , string direct , ref List<ROI> rois )
        {
            _roiController.GenCircularArc( row , col , radius , startPhi , extentPhi , direct , ref rois );
        }

        /// <summary>
        /// 生成Line
        /// </summary>
        /// <param name="beginRow"> 左上row </param>
        /// <param name="beginCol"> 左上col </param>
        /// <param name="endRow"> 右下row </param>
        /// <param name="endCol"> 右下col </param>
        /// <param name="rois"> roi集合对象 </param>
        public void GenLine( double beginRow , double beginCol , double endRow , double endCol , ref List<ROI> rois )
        {
            _roiController.GenLine( beginRow , beginCol , endRow , endCol , ref rois );
        }

        /// <summary>
        /// 获取被激活的ROI的类型名称
        /// </summary>
        /// <param name="name"> roi的类型名称 </param>
        /// <param name="index"> roi的序号 </param>
        /// <returns> roi的数据 </returns>
        public List<HTuple> SmallestActiveROI( out string name , out int index )
        {
            List<HTuple> result = _roiController.SmallestActiveROI( out name , out index );
            return result;
        }

        /// <summary>
        /// 获取被激活的ROI
        /// </summary>
        /// <param name="data"> ROI的数据 </param>
        /// <param name="index"> ROI的序号 </param>
        /// <returns> 激活的ROI对象 </returns>
        public ROI SmallestActiveROI( out List<HTuple> data , out int index )
        {
            ROI roi = _roiController.SmallestActiveROI( out data , out index );
            return roi;
        }

        /// <summary>
        /// 通过Index激活ROI
        /// </summary>
        /// <param name="index"> ROI的Index </param>
        public void SelectROI( int index )
        {
            _roiController.SelectROI( index );
        }

        /// <summary>
        /// 清除ROIControl中的所有ROI,然后只显示此一个ROI
        /// </summary>
        /// <param name="rois"> roi对象集合 </param>
        /// <param name="index"> 选取的index </param>
        public void SelectROI( List<ROI> rois , int index )
        {
            if( ( rois.Count > index ) && ( index >= 0 ) )
            {
                _roiController.Reset( );
                _hWndControl.Repaint( );

                HTuple m_roiData = null;
                m_roiData = rois[ index ].GetModelData( );

                switch( rois[ index ].Type )
                {
                    case "ROIRectangle1":

                        if( m_roiData != null )
                        {
                            _roiController.DisplayRect1( rois[ index ].Color , m_roiData[ 0 ].D , m_roiData[ 1 ].D , m_roiData[ 2 ].D , m_roiData[ 3 ].D );
                        }
                        break;

                    case "ROIRectangle2":

                        if( m_roiData != null )
                        {
                            _roiController.DisplayRect2( rois[ index ].Color , m_roiData[ 0 ].D , m_roiData[ 1 ].D , m_roiData[ 2 ].D , m_roiData[ 3 ].D , m_roiData[ 4 ].D );
                        }
                        break;

                    case "ROICircle":

                        if( m_roiData != null )
                        {
                            _roiController.DisplayCircle( rois[ index ].Color , m_roiData[ 0 ].D , m_roiData[ 1 ].D , m_roiData[ 2 ].D );
                        }
                        break;

                    case "ROIPoint":

                        if( m_roiData != null )
                        {
                            _roiController.DisplayPoint( rois[ index ].Color , m_roiData[ 0 ].D , m_roiData[ 1 ].D );
                        }
                        break;

                    case "ROICircularArc":

                        if( m_roiData != null )
                        {
                            _roiController.DisplayCircularArc( rois[ index ].Color , m_roiData[ 0 ].D , m_roiData[ 1 ].D , m_roiData[ 2 ].D , m_roiData[ 3 ].D , m_roiData[ 4 ].D , m_roiData[ 5 ].S );
                        }
                        break;

                    case "ROILine":

                        if( m_roiData != null )
                        {
                            _roiController.DisplayLine( rois[ index ].Color , m_roiData[ 0 ].D , m_roiData[ 1 ].D , m_roiData[ 2 ].D , m_roiData[ 3 ].D );
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 显示所有的roi
        /// </summary>
        /// <param name="rois"> roi对象集合 </param>
        public void DisplayAllROIs( List<ROI> rois )
        {
            if( rois == null )
            {
                return;
            }
            foreach( var roi in rois )
            {
                HTuple m_roiData = null;
                m_roiData = roi.GetModelData( );

                switch( roi.Type )
                {
                    case "ROIRectangle1":

                        if( m_roiData != null )
                        {
                            _roiController.DisplayRect1( roi.Color , m_roiData[ 0 ].D ,
                                m_roiData[ 1 ].D , m_roiData[ 2 ].D , m_roiData[ 3 ].D , active: false );
                        }
                        break;

                    case "ROIRectangle2":

                        if( m_roiData != null )
                        {
                            _roiController.DisplayRect2( roi.Color , m_roiData[ 0 ].D , m_roiData[ 1 ].D ,
                                m_roiData[ 2 ].D , m_roiData[ 3 ].D , m_roiData[ 4 ].D , active: false );
                        }
                        break;

                    case "ROICircle":

                        if( m_roiData != null )
                        {
                            _roiController.DisplayCircle( roi.Color , m_roiData[ 0 ].D ,
                                m_roiData[ 1 ].D , m_roiData[ 2 ].D , active: false );
                        }
                        break;

                    case "ROIPoint":

                        if( m_roiData != null )
                        {
                            _roiController.DisplayPoint( roi.Color , m_roiData[ 0 ].D , m_roiData[ 1 ].D , active: false );
                        }
                        break;

                    case "ROICircularArc":

                        if( m_roiData != null )
                        {
                            _roiController.DisplayCircularArc( roi.Color , m_roiData[ 0 ].D , m_roiData[ 1 ].D ,
                                m_roiData[ 2 ].D , m_roiData[ 3 ].D , m_roiData[ 4 ].D , m_roiData[ 5 ].S , active: false );
                        }
                        break;

                    case "ROILine":

                        if( m_roiData != null )
                        {
                            _roiController.DisplayLine( roi.Color , m_roiData[ 0 ].D , m_roiData[ 1 ].D ,
                                m_roiData[ 2 ].D , m_roiData[ 3 ].D , active: false );
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 移除被激活的roi
        /// </summary>
        /// <param name="rois"> roi对象集合 </param>
        public void RemoveActiveROI( ref List<ROI> rois )
        {
            _roiController.RemoveActiveROI( ref rois );
        }

        /// <summary>
        /// 手动设置index激活roi
        /// </summary>
        /// <param name="index"> rois的index </param>
        public void SetActiveROI( int index )
        {
            _roiController.ActiveROIIdx = index;
        }

        /// <summary>
        /// Xml序列化保存ROI
        /// </summary>
        /// <param name="rois"> roi对象集合 </param>
        /// <param name="fileNmae"> 保存的文件路径 </param>
        public void SaveROI( List<ROI> rois , string fileNmae )
        {
            List<ROIData> m_RoiData = new List<ROIData>( );
            for( int i = 0 ; i < rois.Count ; i++ )
            {
                m_RoiData.Add( new ROIData( i , rois[ i ] ) );
            }

            Config.SerializeHelper.Save( m_RoiData , fileNmae );
        }

        /// <summary>
        /// 加载被序列化的ROI
        /// </summary>
        /// <param name="fileName"> 读取的文件路径 </param>
        /// <param name="rois"> roi对象集合 </param>
        public void LoadROI( string fileName , out List<ROI> rois )
        {
            rois = new List<ROI>( );
            List<ROIData> m_RoiData = new List<ROIData>( );
            m_RoiData = ( List<ROIData> )Config.SerializeHelper.Load( m_RoiData.GetType( ) , fileName );

            for( int i = 0 ; i < m_RoiData.Count ; i++ )
            {
                switch( m_RoiData[ i ].Name )
                {
                    case "Rectangle1":
                        _roiController.GenRect1( m_RoiData[ i ].Rectangle1.Row1 , m_RoiData[ i ].Rectangle1.Column1 ,
                            m_RoiData[ i ].Rectangle1.Row2 , m_RoiData[ i ].Rectangle1.Column2 , ref rois );
                        rois.Last( ).Color = m_RoiData[ i ].Rectangle1.Color;
                        break;

                    case "Rectangle2":
                        _roiController.GenRect2( m_RoiData[ i ].Rectangle2.Row , m_RoiData[ i ].Rectangle2.Column ,
                            m_RoiData[ i ].Rectangle2.Phi , m_RoiData[ i ].Rectangle2.Lenth1 , m_RoiData[ i ].Rectangle2.Lenth2 , ref rois );
                        rois.Last( ).Color = m_RoiData[ i ].Rectangle2.Color;
                        break;

                    case "Circle":
                        _roiController.GenCircle( m_RoiData[ i ].Circle.Row , m_RoiData[ i ].Circle.Column , m_RoiData[ i ].Circle.Radius , ref rois );
                        rois.Last( ).Color = m_RoiData[ i ].Circle.Color;
                        break;

                    case "Point":
                        _roiController.GenPoint( m_RoiData[ i ].Point.Row , m_RoiData[ i ].Point.Column , ref rois );
                        rois.Last( ).Color = m_RoiData[ i ].Point.Color;
                        break;

                    case "CircularArc":
                        _roiController.GenCircularArc( m_RoiData[ i ].CircularArc.Row , m_RoiData[ i ].CircularArc.Column , m_RoiData[ i ].CircularArc.Radius , m_RoiData[ i ].CircularArc.StartPhi , m_RoiData[ i ].CircularArc.ExtentPhi , m_RoiData[ i ].CircularArc.Direct , ref rois );
                        rois.Last( ).Color = m_RoiData[ i ].CircularArc.Color;
                        break;

                    case "Line":
                        _roiController.GenLine( m_RoiData[ i ].Line.RowBegin , m_RoiData[ i ].Line.ColumnBegin ,
                            m_RoiData[ i ].Line.RowEnd , m_RoiData[ i ].Line.ColumnEnd , ref rois );
                        rois.Last( ).Color = m_RoiData[ i ].Line.Color;
                        break;

                    default:
                        break;
                }
            }
        }

        #region 专门用于 显示region 和xld的方法

        /// <summary>
        /// 显示带颜色的hobject
        /// </summary>
        /// <param name="obj"> hobject对象 </param>
        /// <param name="color"> hobject颜色 </param>
        public void DisplayHobject( HObject obj , string color )
        {
            _hWndControl.DispObj( obj , color );
        }

        /// <summary>
        /// 显示默认红色的hobject
        /// </summary>
        /// <param name="obj"> hobject对象 </param>
        public void DisplayHobject( HObject obj )
        {
            _hWndControl.DispObj( obj , null );
        }

        #endregion 专门用于 显示region 和xld的方法

        /// <summary>
        /// 显示text
        /// </summary>
        /// <param name="htext"> 文字对象 </param>
        /// <param name="coordSystem"> 显示相对于窗体的信息 </param>
        /// <param name="color"> 文字颜色 </param>
        /// <param name="row"> 文字row坐标 </param>
        /// <param name="col"> 文字col坐标 </param>
        /// <param name="hv_Size"> 文字大小 </param>
        /// <param name="hv_Font"> 文字字体 </param>
        /// <param name="hv_Bold"> 文字加粗 </param>
        /// <param name="hv_Slant"> 文字倾斜 </param>
        public void DisplayHtuple( HTuple htext , HTuple coordSystem , HTuple color , HTuple row , HTuple col , int hv_Size , HTuple hv_Font , HTuple hv_Bold , HTuple hv_Slant )
        {
            _hWndControl.DispText( htext , coordSystem , row , col , color , hv_Size , hv_Font , hv_Bold , hv_Slant );
        }

        public void DispText( HTuple text , HTuple color )
        {
            _hWndControl.DispText( text , "window" , "top" , "left" , color , 40 , "mono" , "true" , "false" );
        }
    }
}