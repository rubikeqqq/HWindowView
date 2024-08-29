using System;
using System.ComponentModel;
using System.Reflection;
using HalconDotNet;

namespace 读码
{
    internal class CodeDetect
    {
        public HDataCode2D CodeHandle;


        public void CreateDataCode2DModel( string symbolType , CodeType codeType )
        {
            if( CodeHandle == null )
            {
                CodeHandle = new HDataCode2D( );
            }
            CodeHandle.ClearHandle( );

            var paramValue = EnumToStr( codeType );

            CodeHandle.CreateDataCode2dModel( symbolType , "default_parameters" , paramValue );

        }


        public HXLDCont FindDataCode2D( HImage image , out string codeString )
        {
            if( CodeHandle != null && CodeHandle.IsInitialized( ) )
            {
                var xld = CodeHandle.FindDataCode2d( image , new HTuple( ) , new HTuple( ) ,
                    out HTuple resultHandles , out HTuple decodedDataStrings );
                if( decodedDataStrings != null && decodedDataStrings.Length > 0 )
                {
                    codeString = decodedDataStrings[ 0 ].S;
                    return xld;
                }
            }
            codeString = null;
            return null;
        }



        public enum CodeType
        {
            [Description( "standard_recognition" )]
            标准,
            [Description( "enhanced_recognition" )]
            增强,
            [Description( "maximum_recognition" )]
            最强
        }

        string EnumToStr( Enum @enum )
        {
            if( @enum == null ) return null;
            Type type = @enum.GetType( );

            string des = @enum.ToString( );

            FieldInfo fieldInfo = type.GetField( des );


            if( fieldInfo.GetCustomAttribute<DescriptionAttribute>( ) != null )
            {
                des = fieldInfo.GetCustomAttribute<DescriptionAttribute>( ).Description;
            }

            return des;
        }

    }

}
