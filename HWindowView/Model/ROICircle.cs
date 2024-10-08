﻿using HalconDotNet;
using System;
using System.Xml.Serialization;

namespace HWindowView.Model
{
    /// <summary>
    /// This class demonstrates one of the possible implementations for a circular ROI. ROICircle
    /// inherits from the base class ROI and implements (besides other auxiliary methods) all
    /// virtual methods defined in ROI.cs.
    /// </summary>
    public class ROICircle : ROI
    {
        // first handle
        private double midR, midC;

        private double radius;

        private double row1, col1;

        [XmlElement( ElementName = "Column" )]
        public double Column
        {
            get { return this.midC; }
            set { this.midC = value; }
        }

        [XmlElement( ElementName = "Radius" )]
        public double Radius
        {
            get { return this.radius; }
            set { this.radius = value; }
        }

        [XmlElement( ElementName = "Row" )]
        public double Row
        {
            get { return this.midR; }
            set { this.midR = value; }
        }

        public ROICircle( )
        {
            pNumHandles = 2; // one at corner of circle + midpoint
            pActiveHandleIdx = 1;
        }

        // second handle
        public ROICircle( double row , double col , double radius )
        {
            CreateCircle( row , col , radius );
        }

        public override void CreateCircle( double row , double col , double radius )
        {
            base.CreateCircle( row , col , radius );
            midR = row;
            midC = col;

            this.radius = radius;

            row1 = midR;
            col1 = midC + radius;
        }

        /// <summary>
        /// Creates a new ROI instance at the mouse position
        /// </summary>
        public override void CreateROI( double midX , double midY )
        {
            midR = midY;
            midC = midX;

            radius = 100;

            row1 = midR;
            col1 = midC + radius;
        }

        /// <summary>
        /// Paints the active handle of the ROI object into the supplied window
        /// </summary>
        public override void DisplayActive( HalconDotNet.HWindow window )
        {
            switch( pActiveHandleIdx )
            {
                case 0:
                    window.DispRectangle2( row1 , col1 , 0 , pSize.Width , pSize.Height );
                    break;

                case 1:
                    window.DispRectangle2( midR , midC , 0 , pSize.Width , pSize.Height );
                    break;
            }
        }

        /// <summary>
        /// Returns the distance of the ROI handle being closest to the image point(x,y)
        /// </summary>
        public override double DistToClosestHandle( double x , double y )
        {
            double max = 10000;
            double[] val = new double[ pNumHandles ];

            val[ 0 ] = HMisc.DistancePp( y , x , row1 , col1 ); // border handle
            val[ 1 ] = HMisc.DistancePp( y , x , midR , midC ); // midpoint

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
        /// Paints the ROI into the supplied window
        /// </summary>
        /// <param name="window"> HALCON window </param>
        public override void Draw( HalconDotNet.HWindow window )
        {
            window.DispCircle( midR , midC , radius );
            window.DispRectangle2( row1 , col1 , 0 , pSize.Width , pSize.Height );
            window.DispRectangle2( midR , midC , 0 , pSize.Width , pSize.Height );
        }

        public override double GetDistanceFromStartPoint( double row , double col )
        {
            double sRow = midR; // assumption: we have an angle starting at 0.0
            double sCol = midC + 1 * radius;

            double angle = HMisc.AngleLl( midR , midC , sRow , sCol , midR , midC , row , col );

            if( angle < 0 )
                angle += 2 * Math.PI;

            return ( radius * angle );
        }

        /// <summary>
        /// Gets the model information described by the ROI
        /// </summary>
        public override HTuple GetModelData( )
        {
            return new HTuple( new double[] { midR , midC , radius } );
        }

        /// <summary>
        /// Gets the HALCON region described by the ROI
        /// </summary>
        public override HRegion GetRegion( )
        {
            HRegion region = new HRegion( );
            region.GenCircle( midR , midC , radius );
            return region;
        }

        /// <summary>
        /// Recalculates the shape of the ROI. Translation is performed at the active handle of the
        /// ROI object for the image coordinate (x,y)
        /// </summary>
        public override void MoveByHandle( double newX , double newY )
        {
            HTuple distance;
            double shiftX, shiftY;

            switch( pActiveHandleIdx )
            {
                case 0: // handle at circle border

                    row1 = newY;
                    col1 = newX;
                    HOperatorSet.DistancePp( new HTuple( row1 ) , new HTuple( col1 ) ,
                                            new HTuple( midR ) , new HTuple( midC ) ,
                                            out distance );

                    radius = distance[ 0 ].D;
                    break;

                case 1: // midpoint

                    shiftY = midR - newY;
                    shiftX = midC - newX;

                    midR = newY;
                    midC = newX;

                    row1 -= shiftY;
                    col1 -= shiftX;
                    break;
            }
        }
    }//end of class
}