namespace HWindowView.Model
{
    /// <summary>
    /// GraphicsContext的设置选项
    /// </summary>
    public enum GCSetType
    {
        GC_COLOR = 10,
        GC_COLORED,
        GC_LINEWIDTH,
        GC_DRAWMODE,
        GC_SHAPE,
        GC_LUT,
        GC_PAINT,
        GC_LINESTYLE
    }

    /// <summary>
    /// ROI显示模式(是否显示)
    /// </summary>
    public enum ROICludeMode
    {
        /// <summary>
        /// 包括roi显示
        /// </summary>
        Include_ROI = 1,

        /// <summary>
        /// 不包括roi显示
        /// </summary>
        Exclude_ROI,
    }

    /// <summary>
    /// ROI操作flag(给delegate委托调用)
    /// </summary>
    public enum ROIControlEvent
    {
        /// <summary>
        /// Describe an update of model region
        /// </summary>
        Update_ROI = 50,

        /// <summary>
        /// Change the sign of model region
        /// </summary>
        Changed_ROI_Sign,

        /// <summary>
        /// Moving the model region
        /// </summary>
        Moving_ROI,

        /// <summary>
        /// Delete the active region
        /// </summary>
        Deleted_Active_ROI,

        /// <summary>
        /// Delete all regions
        /// </summary>
        Deleted_All_ROIs,

        /// <summary>
        /// Active the region
        /// </summary>
        Activated_ROI,

        /// <summary>
        /// Created region
        /// </summary>
        Created_ROI,

        /// <summary>
        /// Describes delegate message to signal error when defining a graphical context
        /// </summary>
        Err_Defining_GC,

        /// <summary>
        /// Describes delegate message to signal error when reading an image from file
        /// </summary>
        Err_Defining_IMG,
    }

    /// <summary>
    /// ROI计算模式(positive/negative)
    /// </summary>
    public enum ROISignFlag
    {
        /// <summary>
        /// Setting the ROI mode:positive ROI Sign
        /// </summary>
        Positive = 21,

        /// <summary>
        /// Setting the ROI mode:negative ROI Sign
        /// </summary>
        Negative,

        /// <summary>
        /// Setting the ROI mode:none ROI Sign
        /// </summary>
        None,
    }

    /// <summary>
    /// 鼠标显示操作
    /// </summary>
    public enum ViewMode
    {
        /// <summary>
        /// 鼠标没有任何操作
        /// </summary>
        View_None = 10,

        /// <summary>
        /// 鼠标滚动
        /// </summary>
        View_Zoom,

        /// <summary>
        /// 鼠标移动
        /// </summary>
        View_Move,
    }
}