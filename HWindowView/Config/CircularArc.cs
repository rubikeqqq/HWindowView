using System.Xml.Serialization;

namespace HWindowView.Config
{
    public class CircularArc
    {
        private double _row;
        private double _column;
        private double _radius;
        private double _startPhi;
        private double _extentPhi;

        private string color = "yellow";

        private string _direct = "positive";

        public CircularArc( )
        {
        }

        /// <summary>
        /// CircularArc的构造函数
        /// </summary>
        /// <param name="row"> row </param>
        /// <param name="column"> column </param>
        /// <param name="radius"> radius </param>
        /// <param name="startPhi"> startPhi </param>
        /// <param name="extentPhi"> extentPhi </param>
        public CircularArc( double row , double column , double radius , double startPhi , double extentPhi )
        {
            this._row = row;
            this._column = column;
            this._radius = radius;
            this._startPhi = startPhi;
            this._extentPhi = extentPhi;
        }

        /// <summary>
        /// CircularArc的构造函数
        /// </summary>
        /// <param name="row"> row </param>
        /// <param name="column"> column </param>
        /// <param name="radius"> radius </param>
        /// <param name="startPhi"> startPhi </param>
        /// <param name="extentPhi"> extentPhi </param>
        /// <param name="direct"> direct </param>
        public CircularArc( double row , double column , double radius , double startPhi , double extentPhi , string direct )
        {
            this._row = row;
            this._column = column;
            this._radius = radius;
            this._startPhi = startPhi;
            this._extentPhi = extentPhi;
            this._direct = direct;
        }

        /// <summary>
        /// CircularArc的Row
        /// </summary>
        [XmlElement( ElementName = "Row" )]
        public double Row
        {
            get { return this._row; }
            set { this._row = value; }
        }

        /// <summary>
        /// CircularArc的Column
        /// </summary>
        [XmlElement( ElementName = "Column" )]
        public double Column
        {
            get { return this._column; }
            set { this._column = value; }
        }

        /// <summary>
        /// CircularArc的Radius
        /// </summary>
        [XmlElement( ElementName = "Radius" )]
        public double Radius
        {
            get { return this._radius; }
            set { this._radius = value; }
        }

        /// <summary>
        /// CircularArc的StartPhi
        /// </summary>
        [XmlElement( ElementName = "startPhi" )]
        public double StartPhi
        {
            get { return this._startPhi; }
            set { this._startPhi = value; }
        }

        /// <summary>
        /// CircularArc的ExtentPhi
        /// </summary>
        [XmlElement( ElementName = "ExtentPhi" )]
        public double ExtentPhi
        {
            get { return this._extentPhi; }
            set { this._extentPhi = value; }
        }

        /// <summary>
        /// CircularArc的Color
        /// </summary>
        [XmlElement( ElementName = "Color" )]
        public string Color
        {
            get { return this.color; }
            set { this.color = value; }
        }

        /// <summary>
        /// CircularArc的Direct
        /// </summary>
        [XmlElement( ElementName = "Direct" )]
        public string Direct
        {
            get { return this._direct; }
            set { this._direct = value; }
        }
    }
}