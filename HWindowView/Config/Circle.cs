using System.Xml.Serialization;

namespace HWindowView.Config
{
    public class Circle
    {
        private double _column;
        private double _radius;
        private double _row;
        private string color = "yellow";

        /// <summary>
        /// Circle的Color
        /// </summary>
        [XmlElement( ElementName = "Color" )]
        public string Color
        {
            get { return this.color; }
            set { this.color = value; }
        }

        /// <summary>
        /// Circle的Column
        /// </summary>
        [XmlElement( ElementName = "Column" )]
        public double Column
        {
            get { return this._column; }
            set { this._column = value; }
        }

        /// <summary>
        /// Circle的Radius
        /// </summary>
        [XmlElement( ElementName = "Radius" )]
        public double Radius
        {
            get { return this._radius; }
            set { this._radius = value; }
        }

        /// <summary>
        /// Circle的Row
        /// </summary>
        [XmlElement( ElementName = "Row" )]
        public double Row
        {
            get { return this._row; }
            set { this._row = value; }
        }

        public Circle( )
        {
        }

        /// <summary>
        /// Circle构造函数
        /// </summary>
        /// <param name="row"> row </param>
        /// <param name="column"> col </param>
        /// <param name="radius"> radius </param>
        public Circle( double row , double column , double radius )
        {
            this._row = row;
            this._column = column;
            this._radius = radius;
        }
    }
}