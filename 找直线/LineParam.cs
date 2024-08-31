using System.Reflection;
using HalconDotNet;

namespace 找直线
{
    internal class LineParam
    {
        public int MeasureNum;
        public double MeasureLen1;
        public double MeasureLen2;
        public double MeasureSigma;
        public double MeasureThreshold;
        public double MeasureScore;
        public string MeasureSelect;
        public string MeasureTransition;

        public LineParam()
        {
            MeasureNum = 10;
            MeasureLen1 = 30;
            MeasureLen2 = 5;
            MeasureThreshold = 10;
            MeasureScore = 0.7;
            MeasureSelect = "first";
            MeasureTransition = "all";
            MeasureSigma = 1;
        }

    }

    class LineResult
    {
        public HXLDCont Rectangles;
        public HXLDCont Points;
        public HXLDCont Line;

        public LineResult()
        {
            Rectangles = new HXLDCont();
            Points = new HXLDCont();
            Line = new HXLDCont();
        }
    }



    class LineTool
    {
        private HMetrologyModel _metrologyHandle;


        public LineTool()
        {
            //1、创建测量模块
            _metrologyHandle = new HMetrologyModel( );
        }

        public LineResult FindLine(HImage image,
            double rowBegin,double colBegin,
            double  rowEnd,double colEnd,
            LineParam lineParam)
        {

            LineResult result = new LineResult();

            image.GetImageSize( out HTuple width , out var height );
            _metrologyHandle.SetMetrologyModelImageSize(width, height);

            //2、添加直线的测量
            var index = _metrologyHandle.AddMetrologyObjectLineMeasure(
                rowBegin , colBegin , rowEnd , colEnd ,
                lineParam.MeasureLen1 , lineParam.MeasureLen2 ,
                lineParam.MeasureSigma , lineParam.MeasureThreshold,
                new HTuple(),new HTuple() );

            //3、查询模型轮廓
            _metrologyHandle.GetMetrologyObjectModelContour( "all" , 1.5 );

            //4、把测量的位置和模板的位置关联起来
            _metrologyHandle.SetMetrologyObjectParam( index , "measure_length1" , lineParam.MeasureLen1 );
            _metrologyHandle.SetMetrologyObjectParam( index , "measure_length2" , lineParam.MeasureLen2 );
            _metrologyHandle.SetMetrologyObjectParam( index , "measure_select" , lineParam.MeasureSelect );
            _metrologyHandle.SetMetrologyObjectParam( index , "measure_transition" , lineParam.MeasureTransition );
            _metrologyHandle.SetMetrologyObjectParam( index , "measure_threshold" , lineParam.MeasureThreshold );
            _metrologyHandle.SetMetrologyObjectParam( index , "min_score" , lineParam.MeasureScore );
            _metrologyHandle.SetMetrologyObjectParam( index , "measure_sigma" , lineParam.MeasureSigma );
            _metrologyHandle.SetMetrologyObjectParam( index , "num_measures" , lineParam.MeasureNum );

            //5、应用
            _metrologyHandle.ApplyMetrologyModel( image );

            //6、获取结果
            result.Rectangles = _metrologyHandle.GetMetrologyObjectMeasures( index, "all" , out HTuple row , out var col );

            //7、分别获取行和列的坐标
            var rows = _metrologyHandle.GetMetrologyObjectResult( index , "all" , "used_edges" , "row" );
            var cols = _metrologyHandle.GetMetrologyObjectResult( index , "all" , "used_edges" , "column" );

            //8、交点
            result.Points.GenCrossContourXld(rows, cols ,6,0.785398);

            //结果线
            result.Line = _metrologyHandle.GetMetrologyObjectResultContour( index , "all" , 1.5 );

            return result;
        }

    }
}
