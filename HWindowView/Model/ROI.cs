using HalconDotNet;
using System.Drawing;

namespace HWindowView.Model
{
    /// <summary>
    /// 这是一个处理ROI的基类 需要有继承类来重写内部方法给ROIController提供必要的信息
    /// </summary>
    public class ROI
    {
        protected int pActiveHandleIdx;                            // 子类的激活的Index
        protected int pNumHandles;                                 // 子类中可以操作的Handle的数量
        protected ROISignFlag pROISignFlag;                        // positive/negative
        protected Size pSize = new Size( 8 , 8 );                     // 选中框的大小

        private HTuple _flagLineStyle;                             // 线的类型

        /// <summary>
        /// ROI的颜色
        /// </summary>
        public string Color
        {
            get; set;
        } = "yellow";

        /// <summary>
        /// 线条的类型
        /// </summary>
        public HTuple FlagLineStyle
        {
            get { return _flagLineStyle; }
        }

        /// <summary>
        /// ROI的Type
        /// </summary>
        public string Type
        {
            get; set;
        }

        public ROI( )
        { }

        /// <summary>
        /// 创建Circle
        /// </summary>
        /// <param name="row"> row </param>
        /// <param name="col"> col </param>
        /// <param name="radius"> radius </param>
        public virtual void CreateCircle( double row , double col , double radius )
        { }

        /// <summary>
        /// 创建圆弧
        /// </summary>
        /// <param name="row"> row </param>
        /// <param name="col"> col </param>
        /// <param name="radius"> radius </param>
        /// <param name="startPhi"> startphi </param>
        /// <param name="extentPhi"> extentphi </param>
        /// <param name="direct"> direct </param>
        public virtual void CreateCircularArc( double row , double col , double radius , double startPhi , double extentPhi , string direct )
        { }

        /// <summary>
        /// 创建直线
        /// </summary>
        /// <param name="beginRow"> row1 </param>
        /// <param name="beginCol"> col1 </param>
        /// <param name="endRow"> row2 </param>
        /// <param name="endCol"> col2 </param>
        public virtual void CreateLine( double beginRow , double beginCol , double endRow , double endCol )
        { }

        /// <summary>
        /// 创建Point
        /// </summary>
        /// <param name="row"> row </param>
        /// <param name="col"> col </param>
        public virtual void CreatePoint( double row , double col )
        { }

        /// <summary>
        /// 创建Rect1
        /// </summary>
        /// <param name="row1"> row1 </param>
        /// <param name="col1"> col1 </param>
        /// <param name="row2"> row2 </param>
        /// <param name="col2"> col2 </param>
        public virtual void CreateRectangle1( double row1 , double col1 , double row2 , double col2 )
        { }

        /// <summary>
        /// 创建Rect2
        /// </summary>
        /// <param name="row"> row </param>
        /// <param name="col"> col </param>
        /// <param name="phi"> phi </param>
        /// <param name="length1"> length1 </param>
        /// <param name="length2"> length2 </param>
        public virtual void CreateRectangle2( double row , double col , double phi , double length1 , double length2 )
        { }

        /// <summary>
        /// 在鼠标点的位置创建ROI
        /// </summary>
        /// <param name="midX"> x (=column) coordinate for ROI </param>
        /// <param name="midY"> y (=row) coordinate for ROI </param>
        public virtual void CreateROI( double midX , double midY )
        { }

        /// <summary>
        /// 在窗体绘制激活的Handle
        /// </summary>
        /// <param name="window"> HALCON window </param>
        public virtual void DisplayActive( HalconDotNet.HWindow window )
        { }

        /// <summary>
        /// 返回距离鼠标最近的距离
        /// </summary>
        /// <param name="x"> x (=column) coordinate </param>
        /// <param name="y"> y (=row) coordinate </param>
        /// <returns> Distance of the closest ROI handle. </returns>
        public virtual double DistToClosestHandle( double x , double y )
        {
            return 0.0;
        }

        /// <summary>
        /// 绘制ROI到窗体界面
        /// </summary>
        /// <param name="window"> HALCON window </param>
        public virtual void Draw( HalconDotNet.HWindow window )
        { }

        /// <summary>
        /// 获取当前ROI的被激活的Handle
        /// </summary>
        /// <returns> Index of the active handle (from the handle list) </returns>
        public int GetActHandleIdx( )
        {
            return pActiveHandleIdx;
        }

        /// <summary>
        /// 计算现在坐标和原来坐标的距离
        /// </summary>
        /// <param name="row"> row </param>
        /// <param name="col"> col </param>
        /// <returns> </returns>
        public virtual double GetDistanceFromStartPoint( double row , double col )
        {
            return 0.0;
        }

        /// <summary>
        /// 获取ROI的数据
        /// </summary>
        public virtual HTuple GetModelData( )
        {
            return null;
        }

        /// <summary>
        /// 获取handle的数量
        /// </summary>
        /// <returns> Number of handles </returns>
        public int GetNumHandles( )
        {
            return pNumHandles;
        }

        /// <summary>
        /// 获取当前ROI的Sign(positive/negative)
        /// </summary>
        public ROISignFlag GetOperatorFlag( )
        {
            return pROISignFlag;
        }

        /// <summary>
        /// 获取ROI的Region
        /// </summary>
        public virtual HRegion GetRegion( )
        {
            return null;
        }

        /// <summary>
        /// 鼠标移动时,重新计算roi的数据
        /// </summary>
        /// <param name="x"> x (=column) coordinate </param>
        /// <param name="y"> y (=row) coordinate </param>
        public virtual void MoveByHandle( double x , double y )
        { }

        /// <summary>
        /// 设置当前ROI的Sign(positive/negative)
        /// </summary>
        /// <param name="flag"> Sign of ROI object </param>
        public void SetOperatorFlag( ROISignFlag mode )
        {
            pROISignFlag = mode;

            switch( pROISignFlag )
            {
                case ROISignFlag.Positive:
                    _flagLineStyle = new HTuple( );
                    break;

                case ROISignFlag.Negative:
                    _flagLineStyle = new HTuple( new int[] { 2 , 2 } );
                    break;

                default:
                    _flagLineStyle = new HTuple( );
                    break;
            }
        }
    }//end of class
}