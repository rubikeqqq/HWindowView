using HalconDotNet;

namespace HWindowView.Config
{
    /// <summary>
    /// 显示xld和region 带有颜色
    /// </summary>
    internal class HObjectWithColor
    {
        private HObject hObject;
        private string color;

        public HObjectWithColor( HObject _hbj , string _color )
        {
            hObject = _hbj;
            color = _color;
        }

        public HObject HObject
        {
            get { return hObject; }
            set { hObject = value; }
        }

        public string Color
        {
            get { return color; }
            set { color = value; }
        }
    }

    /// <summary>
    /// 带颜色和坐标数据的文字信息
    /// </summary>
    internal class HTextWithColor
    {
        private HTuple hText;
        private HTuple color;
        private HTuple row;
        private HTuple col;
        private HTuple coordSystem;

        public HTextWithColor( HTuple _hText , string _color , HTuple _row , HTuple _col , HTuple _coordSystem , int size , string font , HTuple bold , HTuple slant )
        {
            hText = _hText;
            color = _color;
            row = _row;
            col = _col;
            coordSystem = _coordSystem;
            this.Size = size;
            this.Font = font;
            this.Bold = bold;
            this.Slant = slant;
        }

        public int Size { get; set; }
        public string Font { get; set; }
        public HTuple Bold { get; set; }
        public HTuple Slant { get; set; }

        /// <summary>
        /// 文字信息
        /// </summary>
        public HTuple HText
        {
            get { return hText; }
            set { hText = value; }
        }

        /// <summary>
        /// 文字坐标row
        /// </summary>
        public HTuple Row
        {
            get { return row; }
            set { row = value; }
        }

        /// <summary>
        /// 文字坐标col
        /// </summary>
        public HTuple Col
        {
            get { return col; }
            set { col = value; }
        }

        /// <summary>
        /// 文字颜色
        /// </summary>
        public HTuple Color
        {
            get { return color; }
            set { color = value; }
        }

        /// <summary>
        /// 文字相对于窗口
        /// </summary>
        public HTuple CoordSystem
        {
            get { return coordSystem; }
            set { coordSystem = value; }
        }
    }
}