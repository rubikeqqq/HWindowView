using System.Xml.Serialization;

namespace HWindowView.Config
{
    public class Line
    {
        private double _rowBegin;
        private double _columnBegin;
        private double _rowEnd;
        private double _columnEnd;

        private string color = "yellow";

        public Line( )
        {
        }

        /// <summary>
        /// Line的构造函数
        /// </summary>
        /// <param name="rowBegin"> rowBegin </param>
        /// <param name="columnBegin"> columnBegin </param>
        /// <param name="rowEnd"> rowEnd </param>
        /// <param name="columnEnd"> columnEnd </param>
        public Line( double rowBegin , double columnBegin , double rowEnd , double columnEnd )
        {
            this._rowBegin = rowBegin;
            this._columnBegin = columnBegin;
            this._rowEnd = rowEnd;
            this._columnEnd = columnEnd;
        }

        /// <summary>
        /// Line的RowBegin
        /// </summary>
        [XmlElement( ElementName = "RowBegin" )]
        public double RowBegin
        {
            get { return this._rowBegin; }
            set { this._rowBegin = value; }
        }

        /// <summary>
        /// Line的ColumnBegin
        /// </summary>
        [XmlElement( ElementName = "ColumnBegin" )]
        public double ColumnBegin
        {
            get { return this._columnBegin; }
            set { this._columnBegin = value; }
        }

        /// <summary>
        /// Line的RowEnd
        /// </summary>
        [XmlElement( ElementName = "RowEnd" )]
        public double RowEnd
        {
            get { return this._rowEnd; }
            set { this._rowEnd = value; }
        }

        /// <summary>
        /// Line的ColumnEnd
        /// </summary>
        [XmlElement( ElementName = "ColumnEnd" )]
        public double ColumnEnd
        {
            get { return this._columnEnd; }
            set { this._columnEnd = value; }
        }

        /// <summary>
        /// Line的Color
        /// </summary>
        [XmlElement( ElementName = "Color" )]
        public string Color
        {
            get { return this.color; }
            set { this.color = value; }
        }
    }
}