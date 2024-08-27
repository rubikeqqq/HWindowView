using HalconDotNet;
using System;
using System.Collections;

namespace HWindowView.Model
{
    /// <summary>
    /// This class creates and manages ROI objects. It responds to mouse device inputs using the
    /// methods mouseDownAction and mouseMoveAction. You don't have to know this class in detail
    /// when you build your own C# project. But you must consider a few things if you want to use
    /// interactive ROIs in your application: There is a quite close connection between the
    /// ROIController and the HWndCtrl class, which means that you must 'register' the ROIController
    /// with the HWndCtrl, so the HWndCtrl knows it has to forward user input (like mouse events) to
    /// the ROIController class. The visualization and manipulation of the ROI objects is done by
    /// the ROIController. This class provides special support for the matching applications by
    /// calculating a model region from the list of ROIs. For this, ROIs are added and subtracted
    /// according to their sign.
    /// </summary>
    public class ROIController
    {
        /// <summary>
        /// 激活的ROI
        /// </summary>
        private string _activeCol = "green";

        /// <summary>
        /// 激活的Handle
        /// </summary>
        private string _activeHdlCol = "red";

        /// <summary>
        /// 激活的roi的Index
        /// </summary>
        private int _activeROIidx;

        /// <summary>
        /// 当前鼠标的坐标
        /// </summary>
        private double _currX, _currY;

        /// <summary>
        /// 删除的roi的Index
        /// </summary>
        private int _deletedIdx;

        /// <summary>
        /// 是否可以编辑
        /// </summary>
        private bool _editModel = true;

        /// <summary>
        /// HWndCtrl对象,通过注入获取
        /// </summary>
        private HWndCtrl _hWndCtrl;

        /// <summary>
        /// 未激活
        /// </summary>
        private string _inactiveCol = "yellow";

        /// <summary>
        /// 通过positive或negative求和获得的region
        /// </summary>
        private HRegion _modelROI;

        /// <summary>
        /// 已创建的roi集合
        /// </summary>
        private ArrayList _roiList;

        /// <summary>
        /// 当前正在操作的ROI
        /// </summary>
        private ROI _roiMode;

        /// <summary>
        /// ROI的操作方式:positive negative
        /// </summary>
        private ROISignFlag _stateROI;

        /// <summary>
        /// Constructor
        /// </summary>
        protected internal ROIController( )
        {
            _stateROI = ROISignFlag.None;
            _roiList = new ArrayList( );
            _activeROIidx = -1;
            _modelROI = new HRegion( );
            NotifyRCObserver = new IconicDelegate( DummyI );
            _deletedIdx = -1;
            _currX = _currY = -1;
        }

        /// <summary>
        /// 激活的roi的Index
        /// </summary>
        public int ActiveROIIdx
        {
            get { return _activeROIidx; }
            set { _activeROIidx = value; }
        }

        /// <summary>
        /// 改变ROI操作的委托
        /// </summary>
        public IconicDelegate NotifyRCObserver { get; set; }

        /// <summary>
        /// roiList的数量
        /// </summary>
        public int ROICount
        {
            get { return this._roiList.Count; }
        }

        /// <summary>
        /// Calculates the ModelROI region for all objects contained in ROIList, by adding and
        /// subtracting the positive and negative ROI objects.
        /// </summary>
        public bool DefineModelROI( )
        {
            HRegion tmpAdd, tmpDiff, tmp;
            double row, col;

            if( _stateROI == ROISignFlag.None )
                return true;

            tmpAdd = new HRegion( );
            tmpDiff = new HRegion( );
            tmpAdd.GenEmptyRegion( );
            tmpDiff.GenEmptyRegion( );

            for( int i = 0 ; i < this.ROICount ; i++ )
            {
                switch( ( ( ROI )_roiList[ i ] ).GetOperatorFlag( ) )
                {
                    case ROISignFlag.Positive:
                        tmp = ( ( ROI )_roiList[ i ] ).GetRegion( );
                        tmpAdd = tmp.Union2( tmpAdd );
                        break;

                    case ROISignFlag.Negative:
                        tmp = ( ( ROI )_roiList[ i ] ).GetRegion( );
                        tmpDiff = tmp.Union2( tmpDiff );
                        break;

                    default:
                        break;
                }//end of switch
            }//end of for

            _modelROI = null;

            if( tmpAdd.AreaCenter( out row , out col ) > 0 )
            {
                tmp = tmpAdd.Difference( tmpDiff );
                if( tmp.AreaCenter( out row , out col ) > 0 )
                    _modelROI = tmp;
            }

            //in case the set of positiv and negative ROIs dissolve
            if( _modelROI == null || this.ROICount == 0 )
                return false;

            return true;
        }

        /// <summary>
        /// 在指定位置生成Circle
        /// </summary>
        /// <param name="color"> 颜色 </param>
        /// <param name="row"> row </param>
        /// <param name="col"> col </param>
        /// <param name="radius"> radius </param>
        /// <param name="active"> 是否激活ROI(默认激活) </param>
        public void DisplayCircle( string color , double row , double col , double radius , bool active = true )
        {
            SetROIShape( new ROICircle( ) );

            if( _roiMode != null )			 //either a new ROI object is created
            {
                _roiMode.CreateCircle( row , col , radius );
                _roiMode.Type = _roiMode.GetType( ).Name;
                _roiMode.Color = color;
                _roiList.Add( _roiMode );
                _roiMode = null;
                if( active )
                    _activeROIidx = this.ROICount - 1;
                _hWndCtrl.Repaint( );

                NotifyRCObserver( ROIControlEvent.Created_ROI );
            }
        }

        /// <summary>
        /// 在指定位置显示CircularArc
        /// </summary>
        /// <param name="color"> 颜色 </param>
        /// <param name="row"> row </param>
        /// <param name="col"> col </param>
        /// <param name="radius"> radius </param>
        /// <param name="startPhi"> startPhi </param>
        /// <param name="extentPhi"> extentPhi </param>
        /// <param name="direct"> direct </param>
        /// <param name="active"> 是否激活ROI(默认激活) </param>
        public void DisplayCircularArc( string color , double row , double col , double radius ,
            double startPhi , double extentPhi , string direct , bool active = true )
        {
            SetROIShape( new ROICircularArc( ) );

            if( _roiMode != null )			 //either a new ROI object is created
            {
                _roiMode.CreateCircularArc( row , col , radius , startPhi , extentPhi , direct );
                _roiMode.Type = _roiMode.GetType( ).Name;
                _roiMode.Color = color;
                _roiList.Add( _roiMode );
                _roiMode = null;
                if( active )
                    _activeROIidx = this.ROICount - 1;
                _hWndCtrl.Repaint( );

                NotifyRCObserver( ROIControlEvent.Created_ROI );
            }
        }

        /// <summary>
        /// 在指定位置显示CircularArc,默认Direct = positive
        /// </summary>
        /// <param name="color"> color </param>
        /// <param name="row"> row </param>
        /// <param name="col"> col </param>
        /// <param name="radius"> radius </param>
        /// <param name="startPhi"> startPhi </param>
        /// <param name="extentPhi"> extentPhi </param>
        /// <param name="active"> 是否激活ROI(默认激活) </param>
        public void DisplayCircularArc( string color , double row , double col , double radius , double startPhi , double extentPhi , bool active = true )
        {
            SetROIShape( new ROICircularArc( ) );

            if( _roiMode != null )			 //either a new ROI object is created
            {
                _roiMode.CreateCircularArc( row , col , radius , startPhi , extentPhi , "positive" );
                _roiMode.Type = _roiMode.GetType( ).Name;
                _roiMode.Color = color;
                _roiList.Add( _roiMode );
                _roiMode = null;
                if( active )
                    _activeROIidx = this.ROICount - 1;
                _hWndCtrl.Repaint( );

                NotifyRCObserver( ROIControlEvent.Created_ROI );
            }
        }

        /// <summary>
        /// 在指定位置显示Line
        /// </summary>
        /// <param name="color"> 颜色 </param>
        /// <param name="beginRow"> beginRow </param>
        /// <param name="beginCol"> beginCol </param>
        /// <param name="endRow"> endRow </param>
        /// <param name="endCol"> endCol </param>
        /// <param name="active"> 是否需要激活此ROI(默认激活) </param>
        public void DisplayLine( string color , double beginRow , double beginCol , double endRow , double endCol , bool active = true )
        {
            this.SetROIShape( new ROILine( ) );

            if( _roiMode != null )			 //either a new ROI object is created
            {
                _roiMode.CreateLine( beginRow , beginCol , endRow , endCol );
                _roiMode.Type = _roiMode.GetType( ).Name;
                _roiMode.Color = color;
                _roiList.Add( _roiMode );
                _roiMode = null;
                if( active )
                    _activeROIidx = this.ROICount - 1;
                _hWndCtrl.Repaint( );

                NotifyRCObserver( ROIControlEvent.Created_ROI );
            }
        }

        /// <summary>
        /// 在指定位置显示Point
        /// </summary>
        /// <param name="color"> 颜色 </param>
        /// <param name="row"> row </param>
        /// <param name="col"> col </param>
        /// <param name="active"> 是否激活ROI(默认激活) </param>
        public void DisplayPoint( string color , double row , double col , bool active = true )
        {
            SetROIShape( new ROIPoint( ) );

            if( _roiMode != null )			 //either a new ROI object is created
            {
                _roiMode.CreatePoint( row , col );
                _roiMode.Type = _roiMode.GetType( ).Name;
                _roiMode.Color = color;
                _roiList.Add( _roiMode );
                _roiMode = null;
                if( active )
                    _activeROIidx = this.ROICount - 1;
                _hWndCtrl.Repaint( );

                NotifyRCObserver( ROIControlEvent.Created_ROI );
            }
        }

        /// <summary>
        /// 在指定位置显示Rectangle1
        /// </summary>
        /// <param name="color"> 颜色 </param>
        /// <param name="row1"> row1 </param>
        /// <param name="col1"> col1 </param>
        /// <param name="row2"> row2 </param>
        /// <param name="col2"> col2 </param>
        /// <param name="active"> 是否激活ROI(默认激活) </param>
        public void DisplayRect1( string color , double row1 , double col1 , double row2 , double col2 , bool active = true )
        {
            SetROIShape( new ROIRectangle1( ) );

            if( _roiMode != null )			 //either a new ROI object is created
            {
                _roiMode.CreateRectangle1( row1 , col1 , row2 , col2 );
                _roiMode.Type = _roiMode.GetType( ).Name;
                _roiMode.Color = color;
                _roiList.Add( _roiMode );
                _roiMode = null;
                if( active )
                    _activeROIidx = this.ROICount - 1;
                _hWndCtrl.Repaint( );

                NotifyRCObserver( ROIControlEvent.Created_ROI );
            }
        }

        /// <summary>
        /// 在指定位置显示Rectangle2
        /// </summary>
        /// <param name="color"> 颜色 </param>
        /// <param name="row"> row </param>
        /// <param name="col"> row </param>
        /// <param name="phi"> phi </param>
        /// <param name="length1"> length1 </param>
        /// <param name="length2"> length2 </param>
        /// <param name="active"> 是否激活ROI(默认激活) </param>
        public void DisplayRect2( string color , double row , double col , double phi , double length1 , double length2 , bool active = true )
        {
            SetROIShape( new ROIRectangle2( ) );

            if( _roiMode != null )			 //either a new ROI object is created
            {
                _roiMode.CreateRectangle2( row , col , phi , length1 , length2 );
                _roiMode.Type = _roiMode.GetType( ).Name;
                _roiMode.Color = color;
                _roiList.Add( _roiMode );
                _roiMode = null;
                if( active )
                    _activeROIidx = this.ROICount - 1;
                _hWndCtrl.Repaint( );

                NotifyRCObserver( ROIControlEvent.Created_ROI );
            }
        }

        public void DummyI( ROIControlEvent v )
        {
        }

        /// <summary>
        /// 获取激活的roi
        /// </summary>
        public ROI GetActiveROI( )
        {
            try
            {
                if( _activeROIidx != -1 )
                    return ( ( ROI )_roiList[ _activeROIidx ] );
                return null;
            }
            catch( Exception )
            {
                return null;
            }
        }

        /// <summary>
        /// 获取激活的roi的Index
        /// </summary>
        /// <returns> </returns>
        public int GetActiveROIIdx( )
        {
            return _activeROIidx;
        }

        /// <summary>
        /// 获取删除的roi的Index
        /// </summary>
        /// <returns> </returns>
        public int GetDelROIIdx( )
        {
            return _deletedIdx;
        }

        /// <summary>
        /// 获取计算后的Region(positive/negative)
        /// </summary>
        public HRegion GetModelRegion( )
        {
            return _modelROI;
        }

        /// <summary>
        /// 鼠标按下事件的反应:获取按下时取得的ROI
        /// </summary>
        /// <param name="imgX"> x coordinate of mouse event </param>
        /// <param name="imgY"> y coordinate of mouse event </param>
        /// <returns> </returns>
        public int MouseDownAction( double imgX , double imgY )
        {
            int idxROI = -1;
            double max = 10000, dist = 0;
            double epsilon = 35.0;          //maximal shortest distance to one of
                                            //the handles

            if( _roiMode != null )             //either a new ROI object is created
            {
                _roiMode.CreateROI( imgX , imgY );
                _roiList.Add( _roiMode );
                _roiMode = null;
                _activeROIidx = this.ROICount - 1;
                _hWndCtrl.Repaint( );

                NotifyRCObserver( ROIControlEvent.Created_ROI );
            }
            else if( this.ROICount > 0 )     // ... or an existing one is manipulated
            {
                _activeROIidx = -1;

                for( int i = 0 ; i < this.ROICount ; i++ )
                {
                    dist = ( ( ROI )_roiList[ i ] ).DistToClosestHandle( imgX , imgY );
                    if( ( dist < max ) && ( dist < epsilon ) )
                    {
                        max = dist;
                        idxROI = i;
                    }
                }//end of for

                if( idxROI >= 0 )
                {
                    _activeROIidx = idxROI;
                    NotifyRCObserver( ROIControlEvent.Activated_ROI );
                }

                _hWndCtrl.Repaint( );
            }
            return _activeROIidx;
        }

        /// <summary>
        /// 鼠标按下且移动时发生 -- 获取被激活的Handle
        /// </summary>
        /// <param name="newX"> x coordinate of mouse event </param>
        /// <param name="newY"> y coordinate of mouse event </param>
        public void MouseMoveAction( double newX , double newY )
        {
            try
            {
                // 如果编辑模式为false时 直接return 就不能编辑ROI了
                if( _editModel == false ) return;
                if( ( newX == _currX ) && ( newY == _currY ) )
                    return;

                ( ( ROI )_roiList[ _activeROIidx ] ).MoveByHandle( newX , newY );
                _hWndCtrl.Repaint( );
                _currX = newX;
                _currY = newY;
                NotifyRCObserver( ROIControlEvent.Moving_ROI );
            }
            catch( Exception )
            {
                //没有显示roi的时候 移动鼠标会报错
            }
        }

        /// <summary>
        /// Paints all objects from the ROIList into the HALCON window
        /// </summary>
        /// <param name="window"> HALCON window </param>
        public void PaintData( HWindow window )
        {
            window.SetDraw( "margin" );
            window.SetLineWidth( 1 );

            if( this.ROICount > 0 )
            {
                //
                //window.SetColor(inactiveCol);

                window.SetDraw( "margin" );

                for( int i = 0 ; i < this.ROICount ; i++ )
                {
                    window.SetColor( ( ( ROI )_roiList[ i ] ).Color );
                    window.SetLineStyle( ( ( ROI )_roiList[ i ] ).FlagLineStyle );
                    ( ( ROI )_roiList[ i ] ).Draw( window );
                }

                if( _activeROIidx != -1 )
                {
                    window.SetColor( _activeCol );
                    window.SetLineStyle( ( ( ROI )_roiList[ _activeROIidx ] ).FlagLineStyle );
                    ( ( ROI )_roiList[ _activeROIidx ] ).Draw( window );

                    window.SetColor( _activeHdlCol );
                    ( ( ROI )_roiList[ _activeROIidx ] ).DisplayActive( window );
                }
            }
        }

        /// <summary>
        /// 移除激活的ROI
        /// </summary>
        public void RemoveActiveROI( )
        {
            if( _activeROIidx != -1 )
            {
                _roiList.RemoveAt( _activeROIidx );
                _deletedIdx = _activeROIidx;
                _activeROIidx = -1;
                ROIRepaint( );
                NotifyRCObserver( ROIControlEvent.Deleted_Active_ROI );
            }
        }

        /// <summary>
        /// 清空所有ROI对象
        /// </summary>
        public void Reset( )
        {
            _roiList.Clear( );
            _activeROIidx = -1;
            _modelROI = null;
            _roiMode = null;
            NotifyRCObserver( ROIControlEvent.Deleted_All_ROIs );
        }

        /// <summary>
        /// 删除种子roi
        /// </summary>
        public void ResetROI( )
        {
            _activeROIidx = -1;
            _roiMode = null;
        }

        /// <summary>
        /// 激活roi
        /// </summary>
        /// <param name="active"> rois的Index </param>
        public void SetActiveROIIdx( int active )
        {
            _activeROIidx = active;
        }

        /// <summary>
        /// 定义ROI的颜色
        /// </summary>
        /// <param name="aColor"> 激活ROI的颜色 </param>
        /// <param name="inaColor"> 未激活的ROI的颜色 </param>
        /// <param name="aHdlColor"> 激活的ROI的Handle的颜色 </param>
        public void SetDrawColor( string aColor , string aHdlColor , string inaColor )
        {
            if( aColor != "" )
                _activeCol = aColor;
            if( aHdlColor != "" )
                _activeHdlCol = aHdlColor;
            if( inaColor != "" )
                _inactiveCol = inaColor;
        }

        /// <summary>
        /// 设置编辑模式
        /// </summary>
        /// <param name="flag"> </param>
        public void SetEditModel( bool flag )
        {
            _editModel = flag;
        }

        /// <summary>
        /// 注册HWndCtrl到ROIController中
        /// </summary>
        public void SetViewController( HWndCtrl view )
        {
            _hWndCtrl = view;
        }

        /// <summary>
        /// 获取所有的rois
        /// </summary>
        internal ArrayList GetROIList( )
        {
            return _roiList;
        }

        /// <summary>
        /// 设置ROI的计算模式(positive / negative)
        /// </summary>
        protected internal void SetROISign( ROISignFlag mode )
        {
            _stateROI = mode;

            if( _activeROIidx != -1 )
            {
                ( ( ROI )_roiList[ _activeROIidx ] ).SetOperatorFlag( _stateROI );
                _hWndCtrl.Repaint( );
                NotifyRCObserver( ROIControlEvent.Changed_ROI_Sign );
            }
        }

        /// <summary>
        /// 在指定位置生成ROI--Circle
        /// </summary>
        /// <param name="row"> </param>
        /// <param name="col"> </param>
        /// <param name="radius"> </param>
        /// <param name="rois"> </param>
        protected internal void GenCircle( double row , double col , double radius , ref System.Collections.Generic.List<ROI> rois )
        {
            SetROIShape( new ROICircle( ) );

            if( rois == null )
            {
                rois = new System.Collections.Generic.List<ROI>( );
            }

            if( _roiMode != null )			 //either a new ROI object is created
            {
                _roiMode.CreateCircle( row , col , radius );
                _roiMode.Type = _roiMode.GetType( ).Name;
                rois.Add( _roiMode );
                _roiList.Add( _roiMode );
                _roiMode = null;
                _activeROIidx = this.ROICount - 1;
                _hWndCtrl.Repaint( );

                NotifyRCObserver( ROIControlEvent.Created_ROI );
            }
        }

        protected internal void GenCircularArc( double row , double col , double radius , double startPhi , double extentPhi , string direct , ref System.Collections.Generic.List<ROI> rois )
        {
            SetROIShape( new ROICircularArc( ) );

            if( rois == null )
            {
                rois = new System.Collections.Generic.List<ROI>( );
            }

            if( _roiMode != null )			 //either a new ROI object is created
            {
                _roiMode.CreateCircularArc( row , col , radius , startPhi , extentPhi , direct );
                _roiMode.Type = _roiMode.GetType( ).Name;
                rois.Add( _roiMode );
                _roiList.Add( _roiMode );
                _roiMode = null;
                _activeROIidx = this.ROICount - 1;
                _hWndCtrl.Repaint( );

                NotifyRCObserver( ROIControlEvent.Created_ROI );
            }
        }

        /// <summary>
        /// 在指定位置生成ROI--Line
        /// </summary>
        /// <param name="beginRow"> </param>
        /// <param name="beginCol"> </param>
        /// <param name="endRow"> </param>
        /// <param name="endCol"> </param>
        /// <param name="rois"> </param>
        protected internal void GenLine( double beginRow , double beginCol , double endRow , double endCol , ref System.Collections.Generic.List<ROI> rois )
        {
            this.SetROIShape( new ROILine( ) );

            if( rois == null )
            {
                rois = new System.Collections.Generic.List<ROI>( );
            }

            if( _roiMode != null )			 //either a new ROI object is created
            {
                _roiMode.CreateLine( beginRow , beginCol , endRow , endCol );
                _roiMode.Type = _roiMode.GetType( ).Name;
                rois.Add( _roiMode );
                _roiList.Add( _roiMode );
                _roiMode = null;
                _activeROIidx = this.ROICount - 1;
                _hWndCtrl.Repaint( );

                NotifyRCObserver( ROIControlEvent.Created_ROI );
            }
        }

        protected internal void GenPoint( double row , double col , ref System.Collections.Generic.List<ROI> rois )
        {
            SetROIShape( new ROIPoint( ) );

            if( rois == null )
            {
                rois = new System.Collections.Generic.List<ROI>( );
            }

            if( _roiMode != null )			 //either a new ROI object is created
            {
                _roiMode.CreatePoint( row , col );
                _roiMode.Type = _roiMode.GetType( ).Name;
                rois.Add( _roiMode );
                _roiList.Add( _roiMode );
                _roiMode = null;
                _activeROIidx = this.ROICount - 1;
                _hWndCtrl.Repaint( );

                NotifyRCObserver( ROIControlEvent.Created_ROI );
            }
        }

        /// <summary>
        /// 在指定位置生成ROI--Rectangle1
        /// </summary>
        /// <param name="row1"> </param>
        /// <param name="col1"> </param>
        /// <param name="row2"> </param>
        /// <param name="col2"> </param>
        /// <param name="rois"> </param>
        protected internal void GenRect1( double row1 , double col1 , double row2 , double col2 , ref System.Collections.Generic.List<ROI> rois )
        {
            SetROIShape( new ROIRectangle1( ) );

            if( rois == null )
            {
                rois = new System.Collections.Generic.List<ROI>( );
            }

            if( _roiMode != null )			 //either a new ROI object is created
            {
                _roiMode.CreateRectangle1( row1 , col1 , row2 , col2 );
                _roiMode.Type = _roiMode.GetType( ).Name;
                rois.Add( _roiMode );
                _roiList.Add( _roiMode );
                _roiMode = null;
                _activeROIidx = this.ROICount - 1;
                _hWndCtrl.Repaint( );

                NotifyRCObserver( ROIControlEvent.Created_ROI );
            }
        }

        /// <summary>
        /// 在指定位置生成ROI--Rectangle2
        /// </summary>
        /// <param name="row"> </param>
        /// <param name="col"> </param>
        /// <param name="phi"> </param>
        /// <param name="length1"> </param>
        /// <param name="length2"> </param>
        /// <param name="rois"> </param>
        protected internal void GenRect2( double row , double col , double phi , double length1 , double length2 , ref System.Collections.Generic.List<ROI> rois )
        {
            SetROIShape( new ROIRectangle2( ) );

            if( rois == null )
            {
                rois = new System.Collections.Generic.List<ROI>( );
            }

            if( _roiMode != null )			 //either a new ROI object is created
            {
                _roiMode.CreateRectangle2( row , col , phi , length1 , length2 );
                _roiMode.Type = _roiMode.GetType( ).Name;
                rois.Add( _roiMode );
                _roiList.Add( _roiMode );
                _roiMode = null;
                _activeROIidx = this.ROICount - 1;
                _hWndCtrl.Repaint( );

                NotifyRCObserver( ROIControlEvent.Created_ROI );
            }
        }

        protected internal void MoveWindowImage( )
        {
            this._hWndCtrl.SetViewState( ViewMode.View_Move );
        }

        protected internal void NoneWindowImage( )
        {
            this._hWndCtrl.SetViewState( ViewMode.View_None );
        }

        /// <summary>
        /// 删除当前选中ROI
        /// </summary>
        /// <param name="rois"> </param>
        protected internal void RemoveActiveROI( ref System.Collections.Generic.List<ROI> rois )
        {
            int activeROIIdx = GetActiveROIIdx( );
            if( activeROIIdx > -1 )
            {
                RemoveActiveROI( );
                rois.RemoveAt( activeROIIdx );
            }
        }

        /// <summary>
        /// 复位窗口显示
        /// </summary>
        protected internal void ROIRepaint( )
        {
            this._hWndCtrl.Repaint( );
        }

        /// <summary>
        /// 根据index激活ROI
        /// </summary>
        /// <param name="index"> 选择的index </param>
        protected internal void SelectROI( int index )
        {
            this._activeROIidx = index;
            this.NotifyRCObserver( ROIControlEvent.Activated_ROI );
            ROIRepaint( );
        }

        /// <summary>
        /// 获取被激活的ROI的数据,名称和Index
        /// </summary>
        /// <returns> 激活的ROI的数据 </returns>
        protected internal System.Collections.Generic.List<HTuple> SmallestActiveROI( out string name , out int index )
        {
            name = "";
            int activeROIIdx = this.GetActiveROIIdx( );
            index = activeROIIdx;
            if( activeROIIdx > -1 )
            {
                ROI region = this.GetActiveROI( );
                Type type = region.GetType( );
                name = type.Name;

                HTuple smallest = region.GetModelData( );
                System.Collections.Generic.List<HTuple> resual = new System.Collections.Generic.List<HTuple>( );
                for( int i = 0 ; i < smallest.Length ; i++ )
                {
                    if( i < 5 )
                    {
                        resual.Add( smallest[ i ].D );
                    }
                    else
                    {
                        resual.Add( smallest[ i ].S );
                    }
                }

                return resual;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取被激活的ROI 以及ROI的数据和Index
        /// </summary>
        /// <param name="data"> 激活的ROI的数据 </param>
        /// <param name="index"> 激活的ROI的Index </param>
        /// <returns> 激活的ROI </returns>
        protected internal ROI SmallestActiveROI( out System.Collections.Generic.List<HTuple> data , out int index )
        {
            try
            {
                int activeROIIdx = GetActiveROIIdx( );
                index = activeROIIdx;
                data = new System.Collections.Generic.List<HTuple>( );

                if( activeROIIdx > -1 )
                {
                    ROI region = GetActiveROI( );
                    Type type = region.GetType( );

                    HTuple smallest = region.GetModelData( );

                    for( int i = 0 ; i < smallest.Length ; i++ )
                    {
                        if( i < 5 )
                        {
                            data.Add( smallest[ i ].D );
                        }
                        else
                        {
                            data.Add( smallest[ i ].S );
                        }
                    }

                    return region;
                }
                else
                {
                    return null;
                }
            }
            catch( Exception )
            {
                data = null;
                index = 0;
                return null;
            }
        }

        /// <summary>
        /// 创建一个新的ROI
        /// </summary>
        /// <param name="r"> 'Seed' ROI object forwarded by the application forms class. </param>
        protected internal void SetROIShape( ROI r )
        {
            _roiMode = r;
            _roiMode.SetOperatorFlag( _stateROI );
        }
    }//end of class
}