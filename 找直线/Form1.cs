using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HalconDotNet;

namespace 找直线
{
    public partial class Form1 : Form
    {
        public Form1( )
        {
            InitializeComponent( );
            Init( );
        }


        HImage image = new HImage( );

        LineParam lineParam = new LineParam( );
        LineResult lineResult = new LineResult( );
        LineTool tool = new LineTool( );

        private void button1_Click( object sender , EventArgs e )
        {
            //OpenFileDialog openFileDialog = new OpenFileDialog( );
            //openFileDialog.Filter = "图像文件(*.jpg,*.bmp,*.png,*.jpeg)|*.jpg;*.bmp;*.png;*.jpeg";
            //if( openFileDialog.ShowDialog( ) == DialogResult.OK )
            //{
            //    image.ReadImage( openFileDialog.FileName );

            //    uC_Window1.HobjectToHimage( image );
            //}

            image.ReadImage( "直线.png" );

            uC_Window1.HobjectToHimage( image );
        }

        void Init( )
        {
            tbLen1.Text = lineParam.MeasureLen1.ToString( );
            tbLen2.Text = lineParam.MeasureLen2.ToString( );
            tbNum.Text = lineParam.MeasureNum.ToString( );
            tbScore.Text = lineParam.MeasureScore.ToString( );
            tbSigma.Text = lineParam.MeasureSigma.ToString( );
            tbThreshold.Text = lineParam.MeasureThreshold.ToString( );
            cbSelect.SelectedItem = lineParam.MeasureSelect.ToString( );
            cbTransition.SelectedItem = lineParam.MeasureTransition.ToString( );
        }

        double rowBegin, colBegin;
        double rowEnd, colEnd;

        private void button4_Click( object sender , EventArgs e )
        {
            lineParam.MeasureLen1 = Convert.ToDouble( tbLen1.Text );
            lineParam.MeasureLen2 = Convert.ToDouble( tbLen2.Text );
            lineParam.MeasureNum = Convert.ToInt32( tbNum.Text );
            lineParam.MeasureScore = Convert.ToDouble( tbScore.Text );
            lineParam.MeasureSigma = Convert.ToDouble( tbSigma.Text );
            lineParam.MeasureThreshold = Convert.ToDouble( tbThreshold.Text );
            lineParam.MeasureSelect = cbSelect.SelectedItem.ToString( );
            lineParam.MeasureTransition = cbTransition.SelectedItem.ToString( );
        }

        private void button2_Click( object sender , EventArgs e )
        {
            uC_Window1.ClearObj( );

            if( rois.Count > 0 )
            {
                rowBegin = rois[ 0 ].GetModelData( )[ 0 ];
                colBegin = rois[ 0 ].GetModelData( )[ 1 ];
                rowEnd = rois[ 0 ].GetModelData( )[ 2 ];
                colEnd = rois[ 0 ].GetModelData( )[ 3 ];
            }

            uC_Window1.ViewWindow.SetDispLevel(HWindowView.Model.ROICludeMode.Exclude_ROI );

            lineResult = tool.FindLine( image , rowBegin , colBegin , rowEnd , colEnd , lineParam );

            uC_Window1.DispObj( lineResult.Rectangles , "blue" );
            uC_Window1.DispObj( lineResult.Points , "red" );
            uC_Window1.DispObj( lineResult.Line , "green" );

        }

        List<HWindowView.Model.ROI> rois = new List<HWindowView.Model.ROI>( );

        //画线
        private void button3_Click( object sender , EventArgs e )
        {
            //HOperatorSet.DrawLine(uC_Window1.GetHWindowControl().HalconWindow,out rowBegin,out colBegin,out rowEnd,out ColEnd);

            //uC_Window1.GetHWindowControl( ).HalconWindow.DrawLine( out rowBegin , out colBegin , out rowEnd , out ColEnd );


            uC_Window1.ViewWindow.GenLine( 50 , 10 , 50 , 200 , ref rois );
            rois.Last( ).Color = "blue";
            uC_Window1.DrawModel = false;

        }
    }
}
