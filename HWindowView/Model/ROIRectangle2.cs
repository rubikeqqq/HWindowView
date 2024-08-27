﻿using HalconDotNet;
using System;
using System.Xml.Serialization;

namespace HWindowView.Model
{
    /// <summary>
    /// This class demonstrates one of the possible implementations for a (simple) rectangularly
    /// shaped ROI. To create this rectangle we use a center point (midR, midC), an orientation
    /// 'phi' and the half edge lengths 'length1' and 'length2', similar to the HALCON operator
    /// gen_rectangle2(). The class ROIRectangle2 inherits from the base class ROI and implements
    /// (besides other auxiliary methods) all virtual methods defined in ROI.cs.
    /// </summary>
    public class ROIRectangle2 : ROI
    {
        /// <summary>
        /// Half length of the rectangle side, perpendicular to phi
        /// </summary>
        private double length1;

        /// <summary>
        /// Half length of the rectangle side, in direction of phi
        /// </summary>
        private double length2;

        /// <summary>
        /// Row coordinate of midpoint of the rectangle
        /// </summary>
        private double midR;

        /// <summary>
        /// Column coordinate of midpoint of the rectangle
        /// </summary>
        private double midC;

        /// <summary>
        /// Orientation of rectangle defined in radians.
        /// </summary>
        private double phi;

        //auxiliary variables
        private HTuple rowsInit;

        private HTuple colsInit;

        private HTuple rows;

        private HTuple cols;

        private HHomMat2D hom2D, tmp;

        /// <summary>
        /// Constructor
        /// </summary>
        public ROIRectangle2( )
        {
            pNumHandles = 6; // 4 corners +  1 midpoint + 1 rotationpoint
            pActiveHandleIdx = 4;
        }

        public ROIRectangle2( double row , double col , double phi , double length1 , double length2 )
        {
            CreateRectangle2( row , col , phi , length1 , length2 );
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

        [XmlElement( ElementName = "Phi" )]
        public double Phi
        {
            get { return this.phi; }
            set { this.phi = value; }
        }

        [XmlElement( ElementName = "Length1" )]
        public double Lenth1
        {
            get { return this.length1; }
            set { this.length1 = value; }
        }

        [XmlElement( ElementName = "Length2" )]
        public double Lenth2
        {
            get { return this.length2; }
            set { this.length2 = value; }
        }

        public override void CreateRectangle2( double row , double col , double phi , double length1 , double length2 )
        {
            base.CreateRectangle2( row , col , phi , length1 , length2 );
            this.midR = row;
            this.midC = col;
            this.length1 = length1;
            this.length2 = length2;
            this.phi = phi;

            rowsInit = new HTuple( new double[] {-1.0, -1.0, 1.0,
                                                   1.0,  0.0, 0.0 } );
            colsInit = new HTuple( new double[] {-1.0, 1.0,  1.0,
                                                  -1.0, 0.0, 0.6 } );

            hom2D = new HHomMat2D( );
            tmp = new HHomMat2D( );

            UpdateHandlePos( );
        }

        /// <summary>
        /// Creates a new ROI instance at the mouse position
        /// </summary>
        /// <param name="midX"> x (=column) coordinate for interactive ROI </param>
        /// <param name="midY"> y (=row) coordinate for interactive ROI </param>
        public override void CreateROI( double midX , double midY )
        {
            midR = midY;
            midC = midX;

            length1 = 100;
            length2 = 50;

            phi = 0.0;

            rowsInit = new HTuple( new double[] {-1.0, -1.0, 1.0,
                                                   1.0,  0.0, 0.0 } );
            colsInit = new HTuple( new double[] {-1.0, 1.0,  1.0,
                                                  -1.0, 0.0, 0.6 } );

            hom2D = new HHomMat2D( );
            tmp = new HHomMat2D( );

            UpdateHandlePos( );
        }

        /// <summary>
        /// Paints the ROI into the supplied window
        /// </summary>
        /// <param name="window"> HALCON window </param>
        public override void Draw( HalconDotNet.HWindow window )
        {
            window.DispRectangle2( midR , midC , -phi , length1 , length2 );
            for( int i = 0 ; i < pNumHandles ; i++ )
                window.DispRectangle2( rows[ i ].D , cols[ i ].D , -phi , pSize.Width , pSize.Height );

            window.DispArrow( midR , midC , midR + ( Math.Sin( phi ) * length1 * 1.2 ) ,
                midC + ( Math.Cos( phi ) * length1 * 1.2 ) , 5.0 );
        }

        /// <summary>
        /// Returns the distance of the ROI handle being closest to the image point(x,y)
        /// </summary>
        /// <param name="x"> x (=column) coordinate </param>
        /// <param name="y"> y (=row) coordinate </param>
        /// <returns> Distance of the closest ROI handle. </returns>
        public override double DistToClosestHandle( double x , double y )
        {
            double max = 10000;
            double[] val = new double[ pNumHandles ];

            for( int i = 0 ; i < pNumHandles ; i++ )
                val[ i ] = HMisc.DistancePp( y , x , rows[ i ].D , cols[ i ].D );

            for( int i = 0 ; i < pNumHandles ; i++ )
            {
                if( val[ i ] < max )
                {
                    max = val[ i ];
                    pActiveHandleIdx = i;
                }
            }
            return val[ pActiveHandleIdx ];
        }

        /// <summary>
        /// Paints the active handle of the ROI object into the supplied window
        /// </summary>
        /// <param name="window"> HALCON window </param>
        public override void DisplayActive( HalconDotNet.HWindow window )
        {
            window.DispRectangle2( rows[ pActiveHandleIdx ].D ,
                                  cols[ pActiveHandleIdx ].D ,
                                  -phi , pSize.Width , pSize.Height );

            if( pActiveHandleIdx == 5 )
                window.DispArrow( midR , midC ,
                                 midR + ( Math.Sin( phi ) * length1 * 1.2 ) ,
                                 midC + ( Math.Cos( phi ) * length1 * 1.2 ) ,
                                 5.0 );
        }

        /// <summary>
        /// Gets the HALCON region described by the ROI
        /// </summary>
        public override HRegion GetRegion( )
        {
            HRegion region = new HRegion( );
            region.GenRectangle2( midR , midC , -phi , length1 , length2 );
            return region;
        }

        /// <summary>
        /// Gets the model information described by the interactive ROI
        /// </summary>
        public override HTuple GetModelData( )
        {
            return new HTuple( new double[] { midR , midC , phi , length1 , length2 } );
        }

        /// <summary>
        /// Recalculates the shape of the ROI instance. Translation is performed at the active
        /// handle of the ROI object for the image coordinate (x,y)
        /// </summary>
        /// <param name="newX"> x mouse coordinate </param>
        /// <param name="newY"> y mouse coordinate </param>
        public override void MoveByHandle( double newX , double newY )
        {
            double vX, vY, x = 0, y = 0;

            switch( pActiveHandleIdx )
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    tmp = hom2D.HomMat2dInvert( );
                    x = tmp.AffineTransPoint2d( newX , newY , out y );

                    length2 = Math.Abs( y );
                    length1 = Math.Abs( x );

                    CheckForRange( x , y );
                    break;

                case 4:
                    midC = newX;
                    midR = newY;
                    break;

                case 5:
                    vY = newY - rows[ 4 ].D;
                    vX = newX - cols[ 4 ].D;
                    phi = Math.Atan2( vY , vX );
                    break;
            }
            UpdateHandlePos( );
        }//end of method

        /// <summary>
        /// Auxiliary method to recalculate the contour points of the rectangle by transforming the
        /// initial row and column coordinates (rowsInit, colsInit) by the updated homography hom2D
        /// </summary>
        private void UpdateHandlePos( )
        {
            hom2D.HomMat2dIdentity( );
            hom2D = hom2D.HomMat2dTranslate( midC , midR );
            hom2D = hom2D.HomMat2dRotateLocal( phi );
            tmp = hom2D.HomMat2dScaleLocal( length1 , length2 );
            cols = tmp.AffineTransPoint2d( colsInit , rowsInit , out rows );
        }

        /* This auxiliary method checks the half lengths
		 * (length1, length2) using the coordinates (x,y) of the four
		 * rectangle corners (handles 0 to 3) to avoid 'bending' of
		 * the rectangular ROI at its midpoint, when it comes to a
		 * 'collapse' of the rectangle for length1=length2=0.
		 * */

        private void CheckForRange( double x , double y )
        {
            switch( pActiveHandleIdx )
            {
                case 0:
                    if( ( x < 0 ) && ( y < 0 ) )
                        return;
                    if( x >= 0 ) length1 = 0.01;
                    if( y >= 0 ) length2 = 0.01;
                    break;

                case 1:
                    if( ( x > 0 ) && ( y < 0 ) )
                        return;
                    if( x <= 0 ) length1 = 0.01;
                    if( y >= 0 ) length2 = 0.01;
                    break;

                case 2:
                    if( ( x > 0 ) && ( y > 0 ) )
                        return;
                    if( x <= 0 ) length1 = 0.01;
                    if( y <= 0 ) length2 = 0.01;
                    break;

                case 3:
                    if( ( x < 0 ) && ( y > 0 ) )
                        return;
                    if( x >= 0 ) length1 = 0.01;
                    if( y <= 0 ) length2 = 0.01;
                    break;

                default:
                    break;
            }
        }
    }//end of class
}