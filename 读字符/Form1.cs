using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;

namespace 读字符
{
    public partial class Form1 : Form
    {
        public Form1( )
        {
            InitializeComponent( );
        }

        HImage image = new HImage( );
        OcrDetect OcrDetect = new OcrDetect( "Document_0-9A-Z_NoRej.omc" );

        private void button1_Click( object sender , EventArgs e )
        {

            //            mean_image( Image , Img_m1 , 3 , 3 )
            //mean_image( Img_m1 , Img_m2 , 17 , 17 )
            //dyn_threshold( Img_m1 , Img_m2 , Reg_th , 5 , 'light' )
            //connection( Reg_th , Reg_con )
            //select_shape( Reg_con , Reg_sel , [ 'height' , 'width' ] , 'and' , [ 50 , 15 ] , [ 70 , 45 ] )
            //sort_region( Reg_sel , Reg_sort , 'character' , 'true' , 'row' )
            //dilation_circle( Reg_sort , Reg_dil , 1.5 )
            //invert_image( Img_m1 , Img_inv )

            HOperatorSet.MeanImage( image , out var image1 , 3 , 3 );
            HOperatorSet.MeanImage( image1 , out var image2 , 17 , 17 );
            HOperatorSet.DynThreshold( image1 , image2 , out var reg_th , 5 , "light" );
            HOperatorSet.Connection( reg_th , out var imageCon );
            HOperatorSet.SelectShape( imageCon , out var RegionSel , new string[] { "height" , "width" } ,
                "and" , new double[] { 50 , 15 } , new double[] { 70 , 45 } );
            HOperatorSet.SortRegion( RegionSel , out var RegionSort , "character" , "true" , "row" );
            HOperatorSet.DilationCircle( RegionSort , out var RegionDil , 1.5 );
            HOperatorSet.InvertImage( image1 , out HObject ImageInv );

            HImage imgInv = new HImage(ImageInv );
            HRegion regionDil = new HRegion( RegionDil );
            OcrDetect.Detect( imgInv , regionDil );

            var x = OcrDetect.CharacterValues;

            StringBuilder res = new StringBuilder();

            foreach( var item in x )
            {
                res.Append( item.value );
            }
            uC_Window1.DispText( res.ToString( ) , "green" );
        }

        private void Form1_Load( object sender , EventArgs e )
        {
            image.ReadImage( "字符识别.bmp" );
            uC_Window1.HobjectToHimage( image );
        }
    }
}
