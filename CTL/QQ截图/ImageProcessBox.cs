using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Drawing;


namespace QQ截图
{
    public partial class ImageProcessBox : Control
    {
        #region 属性
        #region 获取或设置用于被操作的图像
        /// <summary>
        /// 获取或设置用于被操作的图像
        /// </summary>
        private Image baseImage;
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), Category("Custom"), Description("获取或设置用于被操作的图像")]
        public Image BaseImage
        {
            get { return baseImage; }
            set { baseImage = value; }
        } 
        #endregion

        #region 获取或设置操作框点的颜色
        /// <summary>
        /// 获取或设置操作框点的颜色
        /// </summary>
        private Color dotColor = Color.Yellow;
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(typeof(Color), "Yellow"), Category("Custom"), Description("获取或设置操作框点的颜色")]
        public Color DotColor
        {
            get { return dotColor; }
            set { dotColor = value; }
        } 
        #endregion

        #region 获取或设置操作框线条的颜色
        /// <summary>
        /// 获取或设置操作框线条的颜色
        /// </summary>
        private Color lineColor;
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(typeof(Color), "Yellow"), Category("Custom"), Description("获取或设置操作框线条的颜色")]
        public Color LineColor
        {
            get { return lineColor; }
            set { lineColor = value; }
        } 
        #endregion

        #region 获取当前选中的区域
        /// <summary>
        /// 获取当前选中的区域
        /// </summary>
        private Rectangle selectedRectangle;
        /// <summary>
        /// 获取当前选中的区域
        /// </summary>
        [Browsable(false)]
        public Rectangle SelectedRectangle
        {
            get
            {
                Rectangle rectTemp = selectedRectangle;
                rectTemp.Width++;
                rectTemp.Height++;
                return rectTemp;
            }
        } 
        #endregion

        #region 获取或设置图像放大的倍数
        /// <summary>
        /// 获取或设置图像放大的倍数
        /// </summary>
        private Size magnifySize;
        /// <summary>
        /// 获取或设置放大图像的原图大小尺寸
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(typeof(Size), "15,15"), Category("Custom"), Description("获取或设置放大图像的原图大小尺寸")]
        public Size MagnifySize
        {
            get { return magnifySize; }
            set
            {
                magnifySize = value;
                if (magnifySize.Width < 5) magnifySize.Width = 5;
                if (magnifySize.Width > 20) magnifySize.Width = 20;
                if (magnifySize.Height < 5) magnifySize.Height = 5;
                if (magnifySize.Height > 20) magnifySize.Height = 20;
            }
        } 
        #endregion

        #region 获取或设置图像放大的倍数
        /// <summary>
        /// 获取或设置图像放大的倍数
        /// </summary>
        private int magnifyTimes;
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(7), Category("Custom"), Description("获取或设置图像放大的倍数")]
        public int MagnifyTimes
        {
            get { return magnifyTimes; }
            set
            {
                magnifyTimes = value;
                if (magnifyTimes < 3) magnifyTimes = 3;
                if (magnifyTimes > 10) magnifyTimes = 10;
            }
        } 
        #endregion



        #endregion


        #region 构造函数
        public ImageProcessBox()
        {
            InitializeComponent();
        } 
        #endregion



    }
}
