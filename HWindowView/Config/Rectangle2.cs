using System.Xml.Serialization;

namespace HWindowView.Config
{
    public class Rectangle2
    {
        private double _row;
        private double _column;
        private double _phi;
        private double _lenth1;
        private double _lenth2;

        private string color = "yellow";

        public Rectangle2( )
        {
        }

        /// <summary>
        /// Rect2的构造函数
        /// </summary>
        /// <param name="row"> row </param>
        /// <param name="column"> column </param>
        /// <param name="phi"> phi </param>
        /// <param name="lenth1"> length1 </param>
        /// <param name="lenth2"> length2 </param>
        public Rectangle2( double row , double column , double phi , double lenth1 , double lenth2 )
        {
            this._row = row;
            this._column = column;
            this._phi = phi;
            this._lenth1 = lenth1;
            this._lenth2 = lenth2;
        }

        /// <summary>
        /// Rect2的Row
        /// </summary>
        [XmlElement( ElementName = "Row" )]
        public double Row
        {
            get { return this._row; }
            set { this._row = value; }
        }

        /// <summary>
        /// Rect2的Column
        /// </summary>
        [XmlElement( ElementName = "Column" )]
        public double Column
        {
            get { return this._column; }
            set { this._column = value; }
        }

        /// <summary>
        /// Rect2的Phi
        /// </summary>
        [XmlElement( ElementName = "Phi" )]
        public double Phi
        {
            get { return this._phi; }
            set { this._phi = value; }
        }

        /// <summary>
        /// Rect2的Length1
        /// </summary>
        [XmlElement( ElementName = "Lenth1" )]
        public double Lenth1
        {
            get { return this._lenth1; }
            set { this._lenth1 = value; }
        }

        /// <summary>
        /// Rect2的Length2
        /// </summary>
        [XmlElement( ElementName = "Lenth2" )]
        public double Lenth2
        {
            get { return this._lenth2; }
            set { this._lenth2 = value; }
        }

        /// <summary>
        /// Rect2的Color
        /// </summary>
        [XmlElement( ElementName = "Color" )]
        public string Color
        {
            get { return this.color; }
            set { this.color = value; }
        }
    }
}