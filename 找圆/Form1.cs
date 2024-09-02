using System;
using System.Collections;
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

        Dictionary<string , HXLDCont> hXLDConts = new Dictionary<string , HXLDCont>( );

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

            OpenFileDialog openFileDialog = new OpenFileDialog( );
            openFileDialog.Filter = "图像文件(*.jpg,*.bmp,*.png,*.jpeg)|*.jpg;*.bmp;*.png;*.jpeg";
            if( openFileDialog.ShowDialog( ) == DialogResult.OK )
            {
                image.ReadImage( openFileDialog.FileName );

                uC_Window1.HobjectToHimage( image );
            }

            //image.ReadImage( "齿轮.bmp" );

            //uC_Window1.HobjectToHimage( image );
        }

        List<ROI> rois = new List<ROI> { };

        private void button3_Click( object sender , EventArgs e )
        {
            uC_Window1.ViewWindow.GenCircle( 100 , 100 , 50 , ref rois );
            rois.Last( ).Color = "blue";
        }

        private void cbRect_CheckedChanged( object sender , EventArgs e )
        {
            ChangeRect( );
        }

        private void cbROI_CheckedChanged( object sender , EventArgs e )
        {
            ChangeROI( );
        }

        void ChangeROI( )
        {
            if( cbROI.Checked )
            {
                uC_Window1.ViewWindow.SetDispLevel( HWindowView.Model.ROICludeMode.Include_ROI );
            }
            else
            {
                uC_Window1.ViewWindow.SetDispLevel( HWindowView.Model.ROICludeMode.Exclude_ROI );
            }
        }

        void ChangeRect( )
        {
            uC_Window1.ClearObj( );
            if( cbRect.Checked )
            {
                hXLDConts.Add( "rects" , circleResult.Rects );
            }
            else
            {
                if( hXLDConts.ContainsKey( "rects" ) )
                    hXLDConts.Remove( "rects" );
            }

            foreach( var item in hXLDConts )
            {
                switch( item.Key )
                {
                    case "points":
                        uC_Window1.DispObj( item.Value , "red" );
                        break;
                    case "rects":
                        uC_Window1.DispObj( item.Value , "blue" );
                        break;
                    default:
                        uC_Window1.DispObj( item.Value , "green" );
                        break;
                }


            }
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



            circleResult = tool.FindCircle( image , row , col , radius , circleParam );

            hXLDConts.Clear( );

            hXLDConts.Add( "centerPoint" , circleResult.CenterPoint );
            hXLDConts.Add( "points" , circleResult.Points );
            hXLDConts.Add( "circle" , circleResult.CircleContour );
            //uC_Window1.DispObj( circleResult.Rects , "blue" );
            uC_Window1.DispObj( circleResult.Points , "red" );
            uC_Window1.DispObj( circleResult.CircleContour , "green" );
            uC_Window1.DispObj( circleResult.CenterPoint , "green" );
            ChangeROI( );
            ChangeRect( );
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
