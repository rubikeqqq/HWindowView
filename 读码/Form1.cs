using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using HalconDotNet;

namespace 读码
{
    public partial class Form1 : Form
    {
        public Form1( )
        {
            InitializeComponent( );
        }

        private HImage image = new HImage( );

        private void button1_Click( object sender , EventArgs e )
        {
            OpenFileDialog ofd = new OpenFileDialog( );
            ofd.Filter = "*.bmp;*.jpg;*.png|*.bmp;*.jpg;*.png";
            if( ofd.ShowDialog( ) == DialogResult.OK )
            {
                image.ReadImage( ofd.FileName );

                uC_Window1.HobjectToHimage( image );
            }

        }
        CodeDetect codeDetect = new CodeDetect( );
        private void button2_Click( object sender , EventArgs e )
        {

            CodeDt( );

        }

        void CodeDt( )
        {
            uC_Window1.ClearObj( );



            string symbolType = comboBox2.Text;
            string codeType = comboBox1.Text;

            if( Enum.TryParse<CodeDetect.CodeType>( codeType , out var res ) )
            {
                codeDetect.CreateDataCode2DModel( symbolType , res );


                var xld = codeDetect.FindDataCode2D( image , out string codeString );

                if( codeString != null )
                {
                    uC_Window1.DispObj( xld , "green" );
                    uC_Window1.DispText( codeString , "blue" );
                }

            }
        }

        List<FileInfo> files;
        int index = 0;

        private void button3_Click( object sender , EventArgs e )
        {
            if( files != null ) files.Clear( );
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog( );
            if( folderBrowserDialog.ShowDialog( ) == DialogResult.OK )
            {
                string dir = folderBrowserDialog.SelectedPath;
                DirectoryInfo dirInfo = new DirectoryInfo( dir );

                files = dirInfo.GetFiles( "*.png" ).
                    Concat( dirInfo.GetFiles( "*.bmp" ) ).
                    Concat( dirInfo.GetFiles( "*.jpg" ) ).ToList( );


                if( files != null && files.Count > 0 )
                {
                    image.ReadImage( files[ 0 ].FullName );
                    uC_Window1.HobjectToHimage( image );
                }
                index = 0;
            }

        }

        private void button4_Click( object sender , EventArgs e )
        {
            if( files == null || files.Count == 0 ) return;
            if( files.Count > 0 )
            {
                if( index == 0 )
                {
                    index = files.Count - 1;
                }
                index--;
                image.ReadImage( files[ index ].FullName );
                uC_Window1.HobjectToHimage( image );
                CodeDt( );
            }
        }

        private void button5_Click( object sender , EventArgs e )
        {
            if( files == null || files.Count == 0 ) return;
            if( files.Count > 0 )
            {
                if( index == files.Count - 1 )
                {
                    index = 0;
                }
                index++;
                image.ReadImage( files[ index ].FullName );
                uC_Window1.HobjectToHimage( image );
                CodeDt( );
            }
        }
    }
}
