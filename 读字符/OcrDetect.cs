using System.Collections.Generic;
using HalconDotNet;

namespace 读字符
{
    internal class OcrDetect
    {
        HOCRMlp HOCRMlp;

        public List<CharacterValue> CharacterValues = new List<CharacterValue>( );

        public OcrDetect( string orc )
        {
            HOCRMlp = new HOCRMlp( );
            HOCRMlp.ReadOcrClassMlp( orc );
        }

        public void Detect( HImage image , HRegion hRegion )
        {
            if( HOCRMlp.IsInitialized( ) )
            {
                var c = HOCRMlp.DoOcrMultiClassMlp( hRegion , image , out HTuple confidence );

                for( int i = 0 ; i < c.TupleLength( ) ; i++ )
                {
                    CharacterValue cValue = new CharacterValue( );
                    cValue.value = c[ i ];
                    cValue.score = confidence[ i ];
                    CharacterValues.Add( cValue );
                }
            }
        }
    }

    class CharacterValue
    {
        public string value;
        public double score;
    }
}
