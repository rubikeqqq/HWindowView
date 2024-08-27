using System.Xml.Serialization;

namespace HWindowView.Config
{
    public class Point
    {
        private double _row;
        private double _column;

        private string color = "yellow";

        public Point( )
        {
        }

        /// <summary>
        /// Point的构造函数
        /// </summary>
        /// <param name="row"> row </param>
        /// <param name="column"> col </param>
        public Point( double row , double column )
        {
            this._row = row;
            this._column = column;
        }

        /// <summary>
        /// Point的Row
        /// </summary>
        [XmlElement( ElementName = "Row" )]
        public double Row
        {
            get { return this._row; }
            set { this._row = value; }
        }

        /// <summary>
        /// Point的Column
        /// </summary>
        [XmlElement( ElementName = "Column" )]
        public double Column
        {
            get { return this._column; }
            set { this._column = value; }
        }

        /// <summary>
        /// Point的Color
        /// </summary>
        [XmlElement( ElementName = "Color" )]
        public string Color
        {
            get { return this.color; }
            set { this.color = value; }
        }
    }
}