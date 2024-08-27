using System.Xml.Serialization;

namespace HWindowView.Config
{
    public class Rectangle1
    {
        private double _row1;
        private double _column1;
        private double _row2;
        private double _column2;

        private string color = "yellow";

        public Rectangle1( )
        {
        }

        /// <summary>
        /// Rect1的构造函数
        /// </summary>
        /// <param name="row1"> row1 </param>
        /// <param name="column1"> column1 </param>
        /// <param name="row2"> row2 </param>
        /// <param name="column2"> column2 </param>
        public Rectangle1( double row1 , double column1 , double row2 , double column2 )
        {
            this._row1 = row1;
            this._column1 = column1;
            this._row2 = row2;
            this._column2 = column2;
        }

        /// <summary>
        /// Rect1的Row1
        /// </summary>
        [XmlElement( ElementName = "Row1" )]
        public double Row1
        {
            get { return this._row1; }
            set { this._row1 = value; }
        }

        /// <summary>
        /// Rect1的Column1
        /// </summary>
        [XmlElement( ElementName = "Column1" )]
        public double Column1
        {
            get { return this._column1; }
            set { this._column1 = value; }
        }

        /// <summary>
        /// Rect1的Row2
        /// </summary>
        [XmlElement( ElementName = "Row2" )]
        public double Row2
        {
            get { return this._row2; }
            set { this._row2 = value; }
        }

        /// <summary>
        /// Rect1的Column2
        /// </summary>
        [XmlElement( ElementName = "Column2" )]
        public double Column2
        {
            get { return this._column2; }
            set { this._column2 = value; }
        }

        /// <summary>
        /// Rect1的Color
        /// </summary>
        [XmlElement( ElementName = "Color" )]
        public string Color
        {
            get { return this.color; }
            set { this.color = value; }
        }
    }
}