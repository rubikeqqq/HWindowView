using HalconDotNet;
using System.Xml.Serialization;

namespace HWindowView.Model
{
    public class ROIData
    {
        private int _id;
        private string _name;
        private Config.Rectangle1 _rectangle1;
        private Config.Rectangle2 _rectangle2;
        private Config.Circle _circle;
        private Config.CircularArc _circularArc;
        private Config.Line _line;
        private Config.Point _point;

        protected internal ROIData( )
        {
        }

        protected internal ROIData( int id , ROI roi )
        {
            this._id = id;
            HTuple m_roiData = null;

            m_roiData = roi.GetModelData( );

            switch( roi.Type )
            {
                case "ROIRectangle1":
                    this._name = "Rectangle1";
                    if( m_roiData != null )
                    {
                        this._rectangle1 = new Config.Rectangle1( m_roiData[ 0 ].D , m_roiData[ 1 ].D , m_roiData[ 2 ].D , m_roiData[ 3 ].D );
                        this._rectangle1.Color = roi.Color;
                    }
                    break;

                case "ROIRectangle2":
                    this._name = "Rectangle2";
                    if( m_roiData != null )
                    {
                        this._rectangle2 = new Config.Rectangle2( m_roiData[ 0 ].D , m_roiData[ 1 ].D , m_roiData[ 2 ].D , m_roiData[ 3 ].D , m_roiData[ 4 ].D );
                        this._rectangle2.Color = roi.Color;
                    }
                    break;

                case "ROICircle":
                    this._name = "Circle";
                    if( m_roiData != null )
                    {
                        this._circle = new Config.Circle( m_roiData[ 0 ].D , m_roiData[ 1 ].D , m_roiData[ 2 ].D );
                        this._circle.Color = roi.Color;
                    }
                    break;

                case "ROIPoint":
                    this._name = "Point";
                    if( m_roiData != null )
                    {
                        this._point = new Config.Point( m_roiData[ 0 ].D , m_roiData[ 1 ].D );
                        this._point.Color = roi.Color;
                    }
                    break;

                case "ROICircularArc":
                    this._name = "CircularArc";
                    if( m_roiData != null )
                    {
                        this._circularArc = new Config.CircularArc( m_roiData[ 0 ].D , m_roiData[ 1 ].D , m_roiData[ 2 ].D , m_roiData[ 3 ].D , m_roiData[ 4 ].D , m_roiData[ 5 ].S );
                        this._circularArc.Color = roi.Color;
                    }
                    break;

                case "ROILine":
                    this._name = "Line";
                    if( m_roiData != null )
                    {
                        this._line = new Config.Line( m_roiData[ 0 ].D , m_roiData[ 1 ].D , m_roiData[ 2 ].D , m_roiData[ 3 ].D );
                        this._line.Color = roi.Color;
                    }
                    break;

                default:
                    break;
            }
        }

        protected internal ROIData( int id , Config.Rectangle1 rectangle1 )
        {
            this._id = id;
            this._name = "Rectangle1";
            this._rectangle1 = rectangle1;
        }

        protected internal ROIData( int id , Config.Rectangle2 rectangle2 )
        {
            this._id = id;
            this._name = "Rectangle2";
            this._rectangle2 = rectangle2;
        }

        protected internal ROIData( int id , Config.Circle circle )
        {
            this._id = id;
            this._name = "Circle";
            this._circle = circle;
        }

        protected internal ROIData( int id , Config.Line line )
        {
            this._id = id;
            this._name = "Line";
            this._line = line;
        }

        protected internal ROIData( int id , Config.CircularArc arc )
        {
            this._id = id;
            this._name = "CircularArc";
            this._circularArc = arc;
        }

        protected internal ROIData( int id , Config.Point point )
        {
            this._id = id;
            this._name = "Point";
            this._point = point;
        }

        [XmlElement( ElementName = "ID" )]
        public int ID
        {
            get { return this._id; }
            set { this._id = value; }
        }

        [XmlElement( ElementName = "Name" )]
        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }

        [XmlElement( ElementName = "Rectangle1" )]
        public Config.Rectangle1 Rectangle1
        {
            get { return this._rectangle1; }
            set { this._rectangle1 = value; }
        }

        [XmlElement( ElementName = "Rectangle2" )]
        public Config.Rectangle2 Rectangle2
        {
            get { return this._rectangle2; }
            set { this._rectangle2 = value; }
        }

        [XmlElement( ElementName = "Circle" )]
        public Config.Circle Circle
        {
            get { return this._circle; }
            set { this._circle = value; }
        }

        [XmlElement( ElementName = "Point" )]
        public Config.Point Point
        {
            get { return this._point; }
            set { this._point = value; }
        }

        [XmlElement( ElementName = "CircularArc" )]
        public Config.CircularArc CircularArc
        {
            get { return this._circularArc; }
            set { this._circularArc = value; }
        }

        [XmlElement( ElementName = "Line" )]
        public Config.Line Line
        {
            get { return this._line; }
            set { this._line = value; }
        }
    }
}