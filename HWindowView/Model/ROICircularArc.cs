﻿using HalconDotNet;
using System;
using System.Xml.Serialization;

namespace HWindowView.Model
{
    /// <summary>
    /// This class implements an ROI shaped as a circular arc. ROICircularArc inherits from the base
    /// class ROI and implements (besides other auxiliary methods) all virtual methods defined in
    /// ROI.cs.
    /// </summary>
    public class ROICircularArc : ROI
    {
        //handles
        private double midR, midC;

        // 0. handle: midpoint
        private double sizeR, sizeC;

        // 1. handle
        private double startR, startC;

        // 2. handle
        private double extentR, extentC;

        //model data to specify the arc
        private double radius;

        // 3. handle
        private double startPhi, extentPhi;

        // -2*PI <= x <= 2*PI
        // private string direct;
        //display attributes
        private HXLDCont contour;

        private HXLDCont arrowHandleXLD;

        private string circDir;

        private double TwoPI;

        private double PI;

        public ROICircularArc( )
        {
            pNumHandles = 4;         // midpoint handle + three handles on the arc
            pActiveHandleIdx = 0;
            contour = new HXLDCont( );
            circDir = "";

            TwoPI = 2 * Math.PI;
            PI = Math.PI;

            arrowHandleXLD = new HXLDCont( );
            arrowHandleXLD.GenEmptyObj( );
        }

        public ROICircularArc( double row , double col , double radius , double startPhi , double extentPhi )
        {
            CreateCircularArc( row , col , radius , startPhi , extentPhi , "positive" );
        }

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

        [XmlElement( ElementName = "Radius" )]
        public double Radius
        {
            get { return this.radius; }
            set { this.radius = value; }
        }

        [XmlElement( ElementName = "StartPhi" )]
        public double StartPhi
        {
            get { return this.startPhi; }
            set { this.startPhi = value; }
        }

        [XmlElement( ElementName = "ExtentPhi" )]
        public double ExtentPhi
        {
            get { return this.extentPhi; }
            set { this.extentPhi = value; }
        }

        public override void CreateCircularArc( double row , double col , double radius , double startPhi , double extentPhi , string direct )
        {
            base.CreateCircularArc( row , col , radius , startPhi , extentPhi , direct );
            midR = row;
            midC = col;

            this.radius = radius;

            sizeR = midR;
            sizeC = midC - radius;

            this.startPhi = startPhi;
            if( direct == "positive" )
            {
                this.extentPhi = extentPhi;
            }
            else
            {
                this.extentPhi = extentPhi;
            }
            // circDir = "positive";
            circDir = direct;
            DetermineArcHandles( );
            UpdateArrowHandle( );
        }

        /// <summary>
        /// Creates a new ROI instance at the mouse position
        /// </summary>
        public override void CreateROI( double midX , double midY )
        {
            midR = midY;
            midC = midX;

            radius = 100;

            sizeR = midR;
            sizeC = midC - radius;

            startPhi = PI * 0.25;
            extentPhi = PI * 1.5;
            circDir = "positive";

            DetermineArcHandles( );
            UpdateArrowHandle( );
        }

        /// <summary>
        /// Paints the ROI into the supplied window
        /// </summary>
        /// <param name="window"> HALCON window </param>
        public override void Draw( HalconDotNet.HWindow window )
        {
            contour.Dispose( );
            contour.GenCircleContourXld( midR , midC , radius , startPhi ,
                                        ( startPhi + extentPhi ) , circDir , 1.0 );
            window.DispObj( contour );
            window.DispRectangle2( sizeR , sizeC , 0 , pSize.Width , pSize.Height );
            window.DispRectangle2( midR , midC , 0 , pSize.Width , pSize.Height );
            window.DispRectangle2( startR , startC , startPhi , pSize.Width , pSize.Height );
            window.DispObj( arrowHandleXLD );
        }

        /// <summary>
        /// Returns the distance of the ROI handle being closest to the image point(x,y)
        /// </summary>
        public override double DistToClosestHandle( double x , double y )
        {
            double max = 10000;
            double[] val = new double[ pNumHandles ];

            val[ 0 ] = HMisc.DistancePp( y , x , midR , midC );       // midpoint
            val[ 1 ] = HMisc.DistancePp( y , x , sizeR , sizeC );     // border handle
            val[ 2 ] = HMisc.DistancePp( y , x , startR , startC );   // border handle
            val[ 3 ] = HMisc.DistancePp( y , x , extentR , extentC ); // border handle

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
                case 0:
                    window.DispRectangle2( midR , midC , 0 , pSize.Width , pSize.Height );
                    break;

                case 1:
                    window.DispRectangle2( sizeR , sizeC , 0 , pSize.Width , pSize.Height );
                    break;

                case 2:
                    window.DispRectangle2( startR , startC , startPhi , pSize.Width , pSize.Height );
                    break;

                case 3:
                    window.DispObj( arrowHandleXLD );
                    break;
            }
        }

        /// <summary>
        /// Recalculates the shape of the ROI. Translation is performed at the active handle of the
        /// ROI object for the image coordinate (x,y)
        /// </summary>
        public override void MoveByHandle( double newX , double newY )
        {
            HTuple distance;
            double dirX, dirY, prior, next, valMax, valMin;

            switch( pActiveHandleIdx )
            {
                case 0: // midpoint
                    dirY = midR - newY;
                    dirX = midC - newX;

                    midR = newY;
                    midC = newX;

                    sizeR -= dirY;
                    sizeC -= dirX;

                    DetermineArcHandles( );
                    break;

                case 1: // handle at circle border
                    sizeR = newY;
                    sizeC = newX;

                    HOperatorSet.DistancePp( new HTuple( sizeR ) , new HTuple( sizeC ) ,
                                            new HTuple( midR ) , new HTuple( midC ) , out distance );
                    radius = distance[ 0 ].D;
                    DetermineArcHandles( );
                    break;

                case 2: // start handle for arc
                    dirY = newY - midR;
                    dirX = newX - midC;

                    startPhi = Math.Atan2( -dirY , dirX );

                    if( startPhi < 0 )
                        startPhi = PI + ( startPhi + PI );

                    SetStartHandle( );
                    prior = extentPhi;
                    extentPhi = HMisc.AngleLl( midR , midC , startR , startC , midR , midC , extentR , extentC );

                    if( extentPhi < 0 && prior > PI * 0.8 )
                        extentPhi = ( PI + extentPhi ) + PI;
                    else if( extentPhi > 0 && prior < -PI * 0.7 )
                        extentPhi = -PI - ( PI - extentPhi );

                    break;

                case 3: // end handle for arc
                    dirY = newY - midR;
                    dirX = newX - midC;

                    prior = extentPhi;
                    next = Math.Atan2( -dirY , dirX );

                    if( next < 0 )
                        next = PI + ( next + PI );

                    if( circDir == "positive" && startPhi >= next )
                        extentPhi = ( next + TwoPI ) - startPhi;
                    else if( circDir == "positive" && next > startPhi )
                        extentPhi = next - startPhi;
                    else if( circDir == "negative" && startPhi >= next )
                        extentPhi = -1.0 * ( startPhi - next );
                    else if( circDir == "negative" && next > startPhi )
                        extentPhi = -1.0 * ( startPhi + TwoPI - next );

                    valMax = Math.Max( Math.Abs( prior ) , Math.Abs( extentPhi ) );
                    valMin = Math.Min( Math.Abs( prior ) , Math.Abs( extentPhi ) );

                    if( ( valMax - valMin ) >= PI )
                        extentPhi = ( circDir == "positive" ) ? -1.0 * valMin : valMin;

                    SetExtentHandle( );
                    break;
            }

            circDir = ( extentPhi < 0 ) ? "negative" : "positive";
            UpdateArrowHandle( );
        }

        /// <summary>
        /// Gets the HALCON region described by the ROI
        /// </summary>
        public override HRegion GetRegion( )
        {
            HRegion region;
            contour.Dispose( );
            contour.GenCircleContourXld( midR , midC , radius , startPhi , ( startPhi + extentPhi ) , circDir , 1.0 );
            region = new HRegion( contour );
            return region;
        }

        /// <summary>
        /// Gets the model information described by the ROI
        /// </summary>
        public override HTuple GetModelData( )
        {
            return new HTuple( new HTuple[] { midR , midC , radius , startPhi , extentPhi , circDir } );
        }

        /// <summary>
        /// Auxiliary method to determine the positions of the second and third handle.
        /// </summary>
        private void DetermineArcHandles( )
        {
            SetStartHandle( );
            SetExtentHandle( );
        }

        /// <summary>
        /// Auxiliary method to recalculate the start handle for the arc
        /// </summary>
        private void SetStartHandle( )
        {
            startR = midR - radius * Math.Sin( startPhi );
            startC = midC + radius * Math.Cos( startPhi );
        }

        /// <summary>
        /// Auxiliary method to recalculate the extent handle for the arc
        /// </summary>
        private void SetExtentHandle( )
        {
            extentR = midR - radius * Math.Sin( startPhi + extentPhi );
            extentC = midC + radius * Math.Cos( startPhi + extentPhi );
        }

        /// <summary>
        /// Auxiliary method to display an arrow at the extent arc position
        /// </summary>
        private void UpdateArrowHandle( )
        {
            double row1, col1, row2, col2;
            double rowP1, colP1, rowP2, colP2;
            double length, dr, dc, halfHW, sign, angleRad;
            double headLength = 15;
            double headWidth = 15;

            arrowHandleXLD.Dispose( );
            arrowHandleXLD.GenEmptyObj( );

            row2 = extentR;
            col2 = extentC;
            angleRad = ( startPhi + extentPhi ) + Math.PI * 0.5;

            sign = ( circDir == "negative" ) ? -1.0 : 1.0;
            row1 = row2 + sign * Math.Sin( angleRad ) * 20;
            col1 = col2 - sign * Math.Cos( angleRad ) * 20;

            length = HMisc.DistancePp( row1 , col1 , row2 , col2 );
            if( length == 0 )
                length = -1;

            dr = ( row2 - row1 ) / length;
            dc = ( col2 - col1 ) / length;

            halfHW = headWidth / 2.0;
            rowP1 = row1 + ( length - headLength ) * dr + halfHW * dc;
            rowP2 = row1 + ( length - headLength ) * dr - halfHW * dc;
            colP1 = col1 + ( length - headLength ) * dc - halfHW * dr;
            colP2 = col1 + ( length - headLength ) * dc + halfHW * dr;

            if( length == -1 )
                arrowHandleXLD.GenContourPolygonXld( row1 , col1 );
            else
                arrowHandleXLD.GenContourPolygonXld( new HTuple( new double[] { row1 , row2 , rowP1 , row2 , rowP2 , row2 } ) ,
                    new HTuple( new double[] { col1 , col2 , colP1 , col2 , colP2 , col2 } ) );
        }
    }//end of class
}