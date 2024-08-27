using HalconDotNet;
using System.Collections;

namespace HWindowView.Model
{
    /// <summary>
    /// 这是一个关联Graphical上下文到Halcon Hobject的辅助类，这个Graphical上下文用Hashtable来进行描述，
    /// hashtable中包含了一系列的Hobject的操作参数和他们对应的值，这些显示的参数在显示之前被调用
    /// </summary>
    public class HObjectEntry
    {
        /// <summary>
        /// 用一系列的参数存到hashlist中来对HObj进行设置
        /// </summary>
        public Hashtable GContext { get; set; }

        /// <summary>
        /// HALCON object
        /// </summary>
        public HObject HObj { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj"> HALCON object that is linked to the graphical context gc. </param>
        /// <param name="gc">
        /// Hashlist of graphical states that are applied before the object is displayed.
        /// </param>
        public HObjectEntry( HObject obj , Hashtable gc )
        {
            GContext = gc;
            HObj = obj;
        }

        /// <summary>
        /// Clears the entries of the class members Hobj and gContext
        /// </summary>
        public void Clear( )
        {
            GContext.Clear( );
            HObj.Dispose( );
        }
    }//end of class
}