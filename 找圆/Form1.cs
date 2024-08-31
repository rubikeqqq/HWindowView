using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using HalconDotNet;
using HWindowView.Model;

namespace 找圆
{
    public partial class Form1 : Form
    {
        public Form1( )
        {
            InitializeComponent( );
            Init( );
        }

        HImage image = new HImage( );
        CircleParam circleParam = new CircleParam( );
        CircleResult circleResult = new CircleResult( );
        CircleTool tool = new CircleTool( );

        void Init( )
        {
            tbLen1.Text = circleParam.Len1.ToString( );
            tbLen2.Text = circleParam.Len2.ToString( );
            tbNum.Text = circleParam.NumMeasure.ToString( );
            tbScore.Text = circleParam.Score.ToString( );
            tbSigma.Text = circleParam.Sigma.ToString( );
            tbThreshold.Text = circleParam.Threshold.ToString( );
            cbSelect.SelectedItem = circleParam.Select.ToString( );
            cbTransition.SelectedItem = circleParam.Transition.ToString( );
        }

        double row, col, radius;

        private void button1_Click( object sender , EventArgs e )
        {
            image.ReadImage( "齿轮.bmp" );

            uC_Window1.HobjectToHimage( image );
        }

        List<ROI> rois = new List<ROI> { };

        private void button3_Click( object sender , EventArgs e )
        {
            uC_Window1.ViewWindow.GenCircle( 100 , 100 , 50 ,ref rois );
            rois.Last( ).Color = "blue";
        }

        private void button2_Click( object sender , EventArgs e )
        {
            uC_Window1.ClearObj( );

            if( rois.Count > 0 )
            {
                row = rois[ 0 ].GetModelData( )[ 0 ];
                col = rois[ 0 ].GetModelData( )[ 1 ];
                radius = rois[ 0 ].GetModelData( )[ 2 ];
            }

            uC_Window1.ViewWindow.SetDispLevel( HWindowView.Model.ROICludeMode.Exclude_ROI );

            circleResult = tool.FindCircle( image , row , col  , radius , circleParam );

            uC_Window1.DispObj( circleResult.Rects , "blue" );
            uC_Window1.DispObj( circleResult.Points , "red" );
            uC_Window1.DispObj( circleResult.CircleContour , "green" );
        }

        

        private void button4_Click( object sender , EventArgs e )
        {
            circleParam.Len1 = Convert.ToDouble( tbLen1.Text );
            circleParam.Len2 = Convert.ToDouble( tbLen2.Text );
            circleParam.NumMeasure = Convert.ToInt32( tbNum.Text );
            circleParam.Score = Convert.ToDouble( tbScore.Text );
            circleParam.Sigma = Convert.ToDouble( tbSigma.Text );
            circleParam.Threshold = Convert.ToDouble( tbThreshold.Text );
            circleParam.Select = cbSelect.SelectedItem.ToString( );
            circleParam.Transition = cbTransition.SelectedItem.ToString( );
        }
    }
}
