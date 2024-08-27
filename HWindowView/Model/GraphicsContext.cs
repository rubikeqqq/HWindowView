using HalconDotNet;
using System.Collections;

namespace HWindowView.Model
{
    public delegate void GCDelegate( string val );

    /// <summary>
    /// This class contains the graphical context of an HALCON object. The set of graphical modes is
    /// defined by the hashlist 'graphicalSettings'. If the list is empty, then there is no
    /// difference to the graphical setting defined by the system by default. Otherwise, the
    /// provided HALCON window is adjusted according to the entries of the supplied graphical
    /// context (when calling applyContext())
    /// </summary>
    public class GraphicsContext
    {
        /// <summary>
        /// Hashlist containing entries for graphical modes (defined by GC_*), which is then linked
        /// to some HALCON object to describe its graphical context.
        /// </summary>
        private Hashtable _graphicalSettings;

        /// <summary>
        /// Backup of the last graphical context applied to the window.
        /// </summary>
        private Hashtable _stateOfSettings;

        private IEnumerator iterator;

        /// <summary>
        /// Option to delegate messages from the graphical context to some observer class
        /// </summary>
        public GCDelegate GcNotificationDel { get; set; }

        /// <summary>
        /// Creates a graphical context with no initial graphical modes
        /// </summary>
        public GraphicsContext( )
        {
            _graphicalSettings = new Hashtable( 10 , 0.2f );
            GcNotificationDel = new GCDelegate( Dummy );
            _stateOfSettings = new Hashtable( 10 , 0.2f );
        }

        /// <summary>
        /// Creates an instance of the graphical context with the modes defined in the hashtable
        /// 'settings'
        /// </summary>
        /// <param name="settings"> List of modes, which describes the graphical context </param>
        public GraphicsContext( Hashtable settings )
        {
            _graphicalSettings = settings;
            GcNotificationDel = new GCDelegate( Dummy );
            _stateOfSettings = new Hashtable( 10 , 0.2f );
        }

        /// <summary>
        /// 传入一个Hashtable,并且应用到当前的window上
        /// </summary>
        /// <param name="window"> Active HALCON window </param>
        /// <param name="cContext"> List that contains graphical modes for window </param>
        public void ApplyContext( HWindow window , Hashtable cContext )
        {
            GCSetType key = GCSetType.GC_COLOR;
            string valS = "";
            int valI = -1;
            HTuple valH = null;

            iterator = cContext.Keys.GetEnumerator( );

            try
            {
                while( iterator.MoveNext( ) )
                {
                    key = ( GCSetType )iterator.Current;

                    if( _stateOfSettings.Contains( key ) &&
                        _stateOfSettings[ key ] == cContext[ key ] )
                        continue;

                    switch( key )
                    {
                        case GCSetType.GC_COLOR:
                            valS = ( string )cContext[ key ];
                            window.SetColor( valS );
                            if( _stateOfSettings.Contains( GCSetType.GC_COLORED ) )
                                _stateOfSettings.Remove( GCSetType.GC_COLORED );

                            break;

                        case GCSetType.GC_COLORED:
                            valI = ( int )cContext[ key ];
                            window.SetColored( valI );

                            if( _stateOfSettings.Contains( GCSetType.GC_COLORED ) )
                                _stateOfSettings.Remove( GCSetType.GC_COLOR );

                            break;

                        case GCSetType.GC_DRAWMODE:
                            valS = ( string )cContext[ key ];
                            window.SetDraw( valS );
                            break;

                        case GCSetType.GC_LINEWIDTH:
                            valI = ( int )cContext[ key ];
                            window.SetLineWidth( valI );
                            break;

                        case GCSetType.GC_LUT:
                            valS = ( string )cContext[ key ];
                            window.SetLut( valS );
                            break;

                        case GCSetType.GC_PAINT:
                            valS = ( string )cContext[ key ];
                            window.SetPaint( valS );
                            break;

                        case GCSetType.GC_SHAPE:
                            valS = ( string )cContext[ key ];
                            window.SetShape( valS );
                            break;

                        case GCSetType.GC_LINESTYLE:
                            valH = ( HTuple )cContext[ key ];
                            window.SetLineStyle( valH );
                            break;

                        default:
                            break;
                    }

                    if( valI != -1 )
                    {
                        if( _stateOfSettings.Contains( key ) )
                            _stateOfSettings[ key ] = valI;
                        else
                            _stateOfSettings.Add( key , valI );

                        valI = -1;
                    }
                    else if( valS != "" )
                    {
                        if( _stateOfSettings.Contains( key ) )
                            _stateOfSettings[ key ] = valI;
                        else
                            _stateOfSettings.Add( key , valI );

                        valS = "";
                    }
                    else if( valH != null )
                    {
                        if( _stateOfSettings.Contains( key ) )
                            _stateOfSettings[ key ] = valI;
                        else
                            _stateOfSettings.Add( key , valI );

                        valH = null;
                    }
                }//while
            }
            catch( HOperatorException e )
            {
                GcNotificationDel( e.Message );
                return;
            }
        }

        /// <summary>
        /// 清除GraphicalSettings
        /// </summary>
        public void ClearGraphicalSettings( )
        {
            _graphicalSettings.Clear( );
        }

        /// <summary>
        /// 清除当前的settings
        /// </summary>
        public void ClearStateSettings( )
        {
            _stateOfSettings.Clear( );
        }

        /// <summary>
        /// 获取当前GraphicalSettings对应的对象本身的克隆
        /// </summary>
        public GraphicsContext Copy( )
        {
            return new GraphicsContext( ( Hashtable )this._graphicalSettings.Clone( ) );
        }

        /// <summary>
        /// 克隆GraphicalSettings的HashTable
        /// </summary>
        /// <returns> current graphical context </returns>
        public Hashtable CopyContextList( )
        {
            return ( Hashtable )_graphicalSettings.Clone( );
        }

        /// <summary>
        /// 需要通过委托往外传的string 信息
        /// </summary>
        /// <param name="val"> </param>
        public void Dummy( string val )
        { }

        /// <summary>
        /// 根据graphicalSetting 获取key对应的值
        /// </summary>
        /// <param name="key"> One of the graphical keys starting with GC_* </param>
        public object GetGraphicsAttribute( string key )
        {
            if( _graphicalSettings.ContainsKey( key ) )
                return _graphicalSettings[ key ];

            return null;
        }

        /// <summary>
        /// 设置color
        /// </summary>
        /// <param name="val"> A single color, e.g. "blue", "green" ...etc. </param>
        public void SetColorAttribute( string val )
        {
            if( _graphicalSettings.ContainsKey( GCSetType.GC_COLORED ) )
                _graphicalSettings.Remove( GCSetType.GC_COLORED );

            AddValue( GCSetType.GC_COLOR , val );
        }

        /// <summary>
        /// 设置colored
        /// </summary>
        /// <param name="val">
        /// The colored mode, which can be either "colored3" or "colored6" or "colored12"
        /// </param>
        public void SetColoredAttribute( int val )
        {
            if( _graphicalSettings.ContainsKey( GCSetType.GC_COLOR ) )
                _graphicalSettings.Remove( GCSetType.GC_COLOR );

            AddValue( GCSetType.GC_COLORED , val );
        }

        /// <summary>
        /// 设置drawmode(margin/fill)
        /// </summary>
        /// <param name="val"> One of the possible draw modes: "margin" or "fill" </param>
        public void SetDrawModeAttribute( string val )
        {
            AddValue( GCSetType.GC_DRAWMODE , val );
        }

        /// <summary>
        /// 设置linestyle
        /// </summary>
        /// <param name="val">
        /// A line style mode, which works identical to the input for the HDevelop operator
        /// 'set_line_style'. For particular information on this topic, please refer to the
        /// Reference Manual entry of the operator set_line_style.
        /// </param>
        public void SetLineStyleAttribute( HTuple val )
        {
            AddValue( GCSetType.GC_LINESTYLE , val );
        }

        /// <summary>
        /// 设置Linewidth
        /// </summary>
        /// <param name="val"> The line width, which can range from 1 to 50 </param>
        public void SetLineWidthAttribute( int val )
        {
            AddValue( GCSetType.GC_LINEWIDTH , val );
        }

        /// <summary>
        /// 设置Lut
        /// </summary>
        /// <param name="val">
        /// One of the possible modes of look up tables. For further information on particular
        /// setups, please refer to the Reference Manual entry of the operator set_lut.
        /// </param>
        public void SetLutAttribute( string val )
        {
            AddValue( GCSetType.GC_LUT , val );
        }

        /// <summary>
        /// 设置Paint
        /// </summary>
        /// <param name="val">
        /// One of the possible paint modes. For further information on particular setups, please
        /// refer refer to the Reference Manual entry of the operator set_paint.
        /// </param>
        public void SetPaintAttribute( string val )
        {
            AddValue( GCSetType.GC_PAINT , val );
        }

        /// <summary>
        /// 设置Shape
        /// </summary>
        /// <param name="val">
        /// One of the possible shape modes. For further information on particular setups, please
        /// refer refer to the Reference Manual entry of the operator set_shape.
        /// </param>
        public void SetShapeAttribute( string val )
        {
            AddValue( GCSetType.GC_SHAPE , val );
        }

        /// <summary>
        /// 添加值为int的key到GraphicalSettings中
        /// </summary>
        /// <param name="key"> A graphical mode defined by the constant GC_* </param>
        /// <param name="val"> Defines the value as an int for this graphical mode 'key' </param>
        private void AddValue( GCSetType key , int val )
        {
            if( _graphicalSettings.ContainsKey( key ) )
                _graphicalSettings[ key ] = val;
            else
                _graphicalSettings.Add( key , val );
        }

        /// <summary>
        /// 添加值为string的key到GraphicalSettings中
        /// </summary>
        /// <param name="key"> A graphical mode defined by the constant GC_* </param>
        /// <param name="val"> Defines the value as a string for this graphical mode 'key' </param>
        private void AddValue( GCSetType key , string val )
        {
            if( _graphicalSettings.ContainsKey( key ) )
                _graphicalSettings[ key ] = val;
            else
                _graphicalSettings.Add( key , val );
        }

        /// <summary>
        /// 添加值为Htuple的key到GraphicalSettings中
        /// </summary>
        /// <param name="key"> A graphical mode defined by the constant GC_* </param>
        /// <param name="val"> Defines the value as a HTuple for this graphical mode 'key' </param>
        private void AddValue( GCSetType key , HTuple val )
        {
            if( _graphicalSettings.ContainsKey( key ) )
                _graphicalSettings[ key ] = val;
            else
                _graphicalSettings.Add( key , val );
        }

        /********************************************************************/
    }//end of class
}