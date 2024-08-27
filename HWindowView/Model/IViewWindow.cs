using HalconDotNet;
using System.Collections.Generic;

namespace HWindowView.Model
{
    internal interface IViewWindow
    {
        /// <summary>
        /// 显示所以的ROI
        /// </summary>
        /// <param name="rois"> roi集合 </param>
        void DisplayAllROIs( List<ROI> rois );

        /// <summary>
        /// 显示图像
        /// </summary>
        /// <param name="img"> image对象 </param>
        void DisplayImage( HObject img );

        /// <summary>
        /// 生成Point
        /// </summary>
        /// <param name="row"> row </param>
        /// <param name="col"> col </param>
        /// <param name="rois"> roi集合 </param>
        void GenPoint( double row , double col , ref List<ROI> rois );

        /// <summary>
        /// 生成Circle
        /// </summary>
        /// <param name="row"> row </param>
        /// <param name="col"> col </param>
        /// <param name="radius"> radius </param>
        /// <param name="rois"> roi集合 </param>
        void GenCircle( double row , double col , double radius , ref List<ROI> rois );

        /// <summary>
        /// 生成圆弧
        /// </summary>
        /// <param name="row"> row </param>
        /// <param name="col"> col </param>
        /// <param name="radius"> radius </param>
        /// <param name="startPhi"> startphi </param>
        /// <param name="extentPhi"> extentphi </param>
        /// <param name="direct"> direct </param>
        /// <param name="rois"> roi集合 </param>
        void GenCircularArc( double row , double col , double radius , double startPhi , double extentPhi , string direct , ref List<ROI> rois );

        /// <summary>
        /// 生成直线
        /// </summary>
        /// <param name="beginRow"> beginrow </param>
        /// <param name="beginCol"> begincol </param>
        /// <param name="endRow"> endrow </param>
        /// <param name="endCol"> endcol </param>
        /// <param name="rois"> roi集合 </param>
        void GenLine( double beginRow , double beginCol , double endRow , double endCol , ref List<ROI> rois );

        /// <summary>
        /// 生成rectangle1
        /// </summary>
        /// <param name="row1"> row1 </param>
        /// <param name="col1"> col1 </param>
        /// <param name="row2"> row2 </param>
        /// <param name="col2"> col2 </param>
        /// <param name="rois"> roi集合 </param>
        void GenRect1( double row1 , double col1 , double row2 , double col2 , ref List<ROI> rois );

        /// <summary>
        /// 生成rectangle2
        /// </summary>
        /// <param name="row"> row </param>
        /// <param name="col"> col </param>
        /// <param name="phi"> phi </param>
        /// <param name="length1"> length1 </param>
        /// <param name="length2"> length2 </param>
        /// <param name="rois"> roi集合 </param>
        void GenRect2( double row , double col , double phi , double length1 , double length2 , ref List<ROI> rois );

        /// <summary>
        /// 加载roi集合
        /// </summary>
        /// <param name="fileName"> 需要加载的文件 </param>
        /// <param name="rois"> roi集合 </param>
        void LoadROI( string fileName , out List<ROI> rois );

        /// <summary>
        /// 移除激活的ROI
        /// </summary>
        /// <param name="rois"> roi集合 </param>
        void RemoveActiveROI( ref List<ROI> rois );

        /// <summary>
        /// 重置图像
        /// </summary>
        void ResetWindowImage( );

        /// <summary>
        /// 保存roi集合到文件中
        /// </summary>
        /// <param name="rois"> roi集合 </param>
        /// <param name="fileNmae"> 文件 </param>
        void SaveROI( List<ROI> rois , string fileNmae );

        /// <summary>
        /// 选择roi
        /// </summary>
        /// <param name="index"> 选择的序号 </param>
        void SelectROI( int index );

        /// <summary>
        /// 选择roi
        /// </summary>
        /// <param name="rois"> roi集合 </param>
        /// <param name="index"> 选择的序号 </param>
        void SelectROI( List<ROI> rois , int index );

        /// <summary>
        /// 鼠标点击过程中选取的roi
        /// </summary>
        /// <param name="name"> roi的名称 </param>
        /// <param name="index"> roi在集合中的序号 </param>
        /// <returns> 激活的ROI的数据 </returns>
        List<HTuple> SmallestActiveROI( out string name , out int index );

        /// <summary>
        /// 鼠标点击过程中选取的roi
        /// </summary>
        /// <param name="data"> 选取的roi的数据 </param>
        /// <param name="index"> 选取的roi的序号 </param>
        /// <returns> 激活的ROI </returns>
        ROI SmallestActiveROI( out List<HTuple> data , out int index );
    }
}