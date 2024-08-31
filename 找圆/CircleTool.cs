using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace 找圆
{
    internal class CircleTool
    {
        private HMetrologyModel metrology;

        public CircleTool()
        {
            metrology = new HMetrologyModel();
        }

        public CircleResult FindCircle( HImage image,
            double row,double col,double radius,
            CircleParam circleParam)
        {
            CircleResult result = new CircleResult();

            var index = metrology.AddMetrologyObjectCircleMeasure( row , col , radius ,
                circleParam.Len1 , circleParam.Len2 , circleParam.Sigma ,
                circleParam.Threshold , new HTuple( ) , new HTuple( ) );

            metrology.SetMetrologyObjectParam( index,"measure_length1" , circleParam.Len1 );
            metrology.SetMetrologyObjectParam( index,"measure_length2" , circleParam.Len2 );
            metrology.SetMetrologyObjectParam( index,"measure_transition" , circleParam.Transition );
            metrology.SetMetrologyObjectParam( index,"measure_select" , circleParam.Select);
            metrology.SetMetrologyObjectParam( index,"measure_threshold" , circleParam.Threshold );
            metrology.SetMetrologyObjectParam( index, "min_score" , circleParam.Score );
            metrology.SetMetrologyObjectParam( index,"num_measures" , circleParam.NumMeasure );

            metrology.ApplyMetrologyModel( image );

            var rows = metrology.GetMetrologyObjectResult(index,"all", "used_edges" , "row");
            var cols = metrology.GetMetrologyObjectResult( index , "all" , "used_edges" , "column" );

            result.CircleContour = metrology.GetMetrologyObjectResultContour( index , "all" , 1.5 );
            result.Points.GenCrossContourXld( rows , cols , 6 , 0.785398 );
            result.Rects = metrology.GetMetrologyObjectMeasures( index , "all" , out var _ , out var _ );

            return result;
        }
    }


    class CircleParam
    {
        public double Len1;
        public double Len2;
        public double Sigma;
        public double Threshold;
        public string Transition;
        public string Select;
        public int NumMeasure;
        public double Score;

        public CircleParam()
        {
            Len1 = 30;
            Len2 = 5;
            Sigma = 1;
            Threshold = 10;
            Transition = "all";
            Select = "first";
            NumMeasure = 18;
            Score = 0.7;
        }
    }


    class CircleResult
    {
        public HXLDCont Points;
        public HXLDCont Rects;
        public HXLDCont CircleContour;

        public CircleResult()
        {
            Points = new HXLDCont();
            Rects = new HXLDCont();
            CircleContour = new HXLDCont();
        }
    }
}
