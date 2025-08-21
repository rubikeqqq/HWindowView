using System;
using System.Windows.Forms;
using HalconDotNet;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1( )
        {
            InitializeComponent( );
        }



        private void button1_Click( object sender , EventArgs e )
        {
            HImage image1;  //未赋值的局部变量。未引用任何对象实例，不能调用任何halcon函数，否则编译失败
            HImage image2 = null; //赋值的局部变量，未引用任何对象实例。可以调用halcon函数，且编译成功。但运行会失败，因为未引用任何对象实例
            HImage image3 = new HImage( ); //引用了对象实例，但未初始化，Key = 0x000000000000
            HImage image4 = new HImage( "1.bmp" ); //引用了对象实例，并且初始化，Key = 0x0003212313
            //if( image1.IsInitialized( ) )
            //{
            //    int i = 0;
            //}
            //if( image2.IsInitialized( ) )
            //{
            //    int i = 0;
            //}
            if( image3.IsInitialized( ) )
            {
                int i = 0;
            }
            if( image4.IsInitialized( ) )
            {
                uC_Window1.HobjectToHimage( image4 );
            }

            //HObject类型的变量可以使用GenEmptyObj函数初始化
            //HRegion类型的变量可以使用GenEmptyRegion函数初始化
        }
    }
}
