using HalconDotNet;
using System.Xml.Serialization;

namespace HWindowView.Model
{
    /// <summary>
    /// This class demonstrates one of the possible implementations for a circular ROI. ROICircle
    /// inherits from the base class ROI and implements (besides other auxiliary methods) all
    /// virtual methods defined in ROI.cs.
    /// </summary>
    public class ROIPoint : ROI
    {
        private double midR, midC;

        public ROIPoint( )
        {
            pNumHandles = 1;
            pActiveHandleIdx = 0;
        }

        // second handle
        public ROIPoint( double row , double col )
        {
            CreatePoint( row , col );
        }

        [XmlElement( ElementName = "Row" )]
        public double Row
        {
            get { return this.midR; }
            set { this.midR = value; }
        }

        [XmlElement( ElementName = "Column" )]
        public double Column
        {
            get { return this.midC; }
            set { this.midC = value; }
        }

        public override void CreatePoint( double row , double col )
        {
            base.CreatePoint( row , col );
            midR = row;
            midC = col;
        }

        /// <summary>
        /// Creates a new ROI instance at the mouse position
        /// </summary>
        public override void CreateROI( double midX , double midY )
        {
            midR = midY;
            midC = midX;
        }

        /// <summary>
        /// Paints the ROI into the supplied window
        /// </summary>
        /// <param name="window"> HALCON window </param>
        public override void Draw( HalconDotNet.HWindow window )
        {
            window.DispCross( midR , midC , 10 , 0 );
        }

        /// <summary>
        /// Returns the distance of the ROI handle being closest to the image point(x,y)
        /// </summary>
        public override double DistToClosestHandle( double x , double y )
        {
            double max = 10000;
            double[] val = new double[ pNumHandles ];

            val[ 0 ] = HMisc.DistancePp( y , x , midR , midC );

            for( int i = 0 ; i < pNumHandles ; i++ )
            {
                if( val[ i ] < max )
                {
                    max = val[ i ];
                    pActiveHandleIdx = i;
                }
            }// end of for
            return val[ pActiveHandleIdx ];
        }

        /// <summary>
        /// Paints the active handle of the ROI object into the supplied window
        /// </summary>
        public override void DisplayActive( HalconDotNet.HWindow window )
        {
            switch( pActiveHandleIdx )
            {
                //case 0:
                //    window.DispRectangle2(midR, midR, 0, 25, 25);
                //    break;
                case 1:
                    // window.DispRectangle2(midR, midC, 0, 25, 25);
                    window.DispCross( midR , midC , 10 , 0 );
                    break;
            }
        }

        /// <summary>
        /// Gets the HALCON region described by the ROI
        /// </summary>
        public override HRegion GetRegion( )
        {
            HRegion region = new HRegion( );
            region.GenRegionPoints( midR , midC );
            return region;
        }

        /// <summary>
        /// 获取roi的数据
        /// </summary>
        public override HTuple GetModelData( )
        {
            return new HTuple( new double[] { midR , midC } );
        }

        /// <summary>
        /// Recalculates the shape of the ROI. Translation is performed at the active handle of the
        /// ROI object for the image coordinate (x,y)
        /// </summary>
        public override void MoveByHandle( double newX , double newY )
        {
            double shiftX, shiftY;

            switch( pActiveHandleIdx )
            {
                case 0:

                    shiftY = midR - newY;
                    shiftX = midC - newX;

                    midR = newY;
                    midC = newX;
                    break;
            }
        }
    }//end of class
}