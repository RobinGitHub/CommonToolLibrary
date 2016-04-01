using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace 图标格式转换
{
    public class ImageHelper
    {
        #region 正方型裁剪并缩放
        /// <summary>
        /// 正方型裁剪
        /// 以图片中心为轴心，截取正方型，然后等比缩放
        /// 用于头像处理
        /// </summary>
        /// <param name="initimage">原图</param>
        /// <param name="filesaveurl">缩略图存放地址</param>
        /// <param name="side">指定的边长（正方型）</param>
        /// <param name="quality">质量（范围0-100）</param>
        public static void CutForSquare(Image initimage, string filesaveurl, int side, int quality)
        {
            //创建目录
            string dir = Path.GetDirectoryName(filesaveurl);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            //原始图片（获取原始图片创建对象，并使用流中嵌入的颜色管理信息）

            //原图宽高均小于模版，不作处理，直接保存
            if (initimage.Width <= side && initimage.Height <= side)
            {
                initimage.Save(filesaveurl, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
            {
                //原始图片的宽、高
                int initwidth = initimage.Width;
                int initheight = initimage.Height;

                //非正方型先裁剪为正方型
                if (initwidth != initheight)
                {
                    //截图对象
                    Image pickedimage = null;
                    Graphics pickedg = null;

                    //宽大于高的横图
                    if (initwidth > initheight)
                    {
                        //对象实例化
                        pickedimage = new Bitmap(initheight, initheight);
                        pickedg = Graphics.FromImage(pickedimage);
                        //设置质量
                        pickedg.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        pickedg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        //定位
                        Rectangle fromr = new Rectangle((initwidth - initheight) / 2, 0, initheight, initheight);
                        Rectangle tor = new Rectangle(0, 0, initheight, initheight);
                        //画图
                        pickedg.DrawImage(initimage, tor, fromr, System.Drawing.GraphicsUnit.Pixel);
                        //重置宽
                        initwidth = initheight;
                    }
                    //高大于宽的竖图
                    else
                    {
                        //对象实例化
                        pickedimage = new System.Drawing.Bitmap(initwidth, initwidth);
                        pickedg = System.Drawing.Graphics.FromImage(pickedimage);
                        //设置质量
                        pickedg.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        pickedg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        //定位
                        Rectangle fromr = new Rectangle(0, (initheight - initwidth) / 2, initwidth, initwidth);
                        Rectangle tor = new Rectangle(0, 0, initwidth, initwidth);
                        //画图
                        pickedg.DrawImage(initimage, tor, fromr, System.Drawing.GraphicsUnit.Pixel);
                        //重置高
                        initheight = initwidth;
                    }

                    //将截图对象赋给原图
                    initimage = (System.Drawing.Image)pickedimage.Clone();
                    //释放截图资源
                    pickedg.Dispose();
                    pickedimage.Dispose();
                }

                //缩略图对象
                System.Drawing.Image resultimage = new System.Drawing.Bitmap(side, side);
                System.Drawing.Graphics resultg = System.Drawing.Graphics.FromImage(resultimage);
                //设置质量
                resultg.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                resultg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //用指定背景色清空画布
                resultg.Clear(Color.White);
                //绘制缩略图
                resultg.DrawImage(initimage, new System.Drawing.Rectangle(0, 0, side, side), new System.Drawing.Rectangle(0, 0, initwidth, initheight), System.Drawing.GraphicsUnit.Pixel);

                //关键质量控制
                //获取系统编码类型数组,包含了jpeg,bmp,png,gif,tiff
                ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo ici = null;
                foreach (ImageCodecInfo i in icis)
                {
                    if (i.MimeType == "image/jpeg" || i.MimeType == "image/bmp" || i.MimeType == "image/png" || i.MimeType == "image/gif")
                    {
                        ici = i;
                    }
                }
                EncoderParameters ep = new EncoderParameters(1);
                ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);

                //保存缩略图
                resultimage.Save(filesaveurl, ici, ep);

                //释放关键质量控制所用资源
                ep.Dispose();

                //释放缩略图资源
                resultg.Dispose();
                resultimage.Dispose();

                //释放原始图片资源
                initimage.Dispose();
            }
        }

        /// <summary>
        /// 正方型裁剪
        /// 以图片中心为轴心，截取正方型，然后等比缩放
        /// 用于头像处理
        /// </summary>
        /// <param name="fromfile">原图</param>
        /// <param name="filesaveurl">缩略图存放地址</param>
        /// <param name="side">指定的边长（正方型）</param>
        /// <param name="quality">质量（范围0-100）</param>
        public static void CutForSquare(Stream fromfile, string filesaveurl, int side, int quality)
        {
            //创建目录
            string dir = Path.GetDirectoryName(filesaveurl);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            //原始图片（获取原始图片创建对象，并使用流中嵌入的颜色管理信息）
            System.Drawing.Image initimage = System.Drawing.Image.FromStream(fromfile, true);

            //原图宽高均小于模版，不作处理，直接保存
            if (initimage.Width <= side && initimage.Height <= side)
            {
                initimage.Save(filesaveurl, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
            {
                //原始图片的宽、高
                int initwidth = initimage.Width;
                int initheight = initimage.Height;

                //非正方型先裁剪为正方型
                if (initwidth != initheight)
                {
                    //截图对象
                    System.Drawing.Image pickedimage = null;
                    System.Drawing.Graphics pickedg = null;

                    //宽大于高的横图
                    if (initwidth > initheight)
                    {
                        //对象实例化
                        pickedimage = new System.Drawing.Bitmap(initheight, initheight);
                        pickedg = System.Drawing.Graphics.FromImage(pickedimage);
                        //设置质量
                        pickedg.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        pickedg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        //定位
                        Rectangle fromr = new Rectangle((initwidth - initheight) / 2, 0, initheight, initheight);
                        Rectangle tor = new Rectangle(0, 0, initheight, initheight);
                        //画图
                        pickedg.DrawImage(initimage, tor, fromr, System.Drawing.GraphicsUnit.Pixel);
                        //重置宽
                        initwidth = initheight;
                    }
                    //高大于宽的竖图
                    else
                    {
                        //对象实例化
                        pickedimage = new System.Drawing.Bitmap(initwidth, initwidth);
                        pickedg = System.Drawing.Graphics.FromImage(pickedimage);
                        //设置质量
                        pickedg.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        pickedg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        //定位
                        Rectangle fromr = new Rectangle(0, (initheight - initwidth) / 2, initwidth, initwidth);
                        Rectangle tor = new Rectangle(0, 0, initwidth, initwidth);
                        //画图
                        pickedg.DrawImage(initimage, tor, fromr, System.Drawing.GraphicsUnit.Pixel);
                        //重置高
                        initheight = initwidth;
                    }

                    //将截图对象赋给原图
                    initimage = (System.Drawing.Image)pickedimage.Clone();
                    //释放截图资源
                    pickedg.Dispose();
                    pickedimage.Dispose();
                }

                //缩略图对象
                System.Drawing.Image resultimage = new System.Drawing.Bitmap(side, side);
                System.Drawing.Graphics resultg = System.Drawing.Graphics.FromImage(resultimage);
                //设置质量
                resultg.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                resultg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //用指定背景色清空画布
                resultg.Clear(Color.White);
                //绘制缩略图
                resultg.DrawImage(initimage, new System.Drawing.Rectangle(0, 0, side, side), new System.Drawing.Rectangle(0, 0, initwidth, initheight), System.Drawing.GraphicsUnit.Pixel);

                //关键质量控制
                //获取系统编码类型数组,包含了jpeg,bmp,png,gif,tiff
                ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo ici = null;
                foreach (ImageCodecInfo i in icis)
                {
                    if (i.MimeType == "image/jpeg" || i.MimeType == "image/bmp" || i.MimeType == "image/png" || i.MimeType == "image/gif")
                    {
                        ici = i;
                    }
                }
                EncoderParameters ep = new EncoderParameters(1);
                ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);

                //保存缩略图
                resultimage.Save(filesaveurl, ici, ep);

                //释放关键质量控制所用资源
                ep.Dispose();

                //释放缩略图资源
                resultg.Dispose();
                resultimage.Dispose();

                //释放原始图片资源
                initimage.Dispose();
            }
        }
        #endregion

        #region 固定模版裁剪并缩放
        /// <summary>
        /// 指定长宽裁剪
        /// 按模版比例最大范围的裁剪图片并缩放至模版尺寸
        /// </summary>
        /// <param name="initimage">原图httppostedfile对象</param>
        /// <param name="filesaveurl">保存路径</param>
        /// <param name="maxwidth">最大宽(单位:px)</param>
        /// <param name="maxheight">最大高(单位:px)</param>
        /// <param name="quality">质量（范围0-100）</param>
        public static void CutForCustom(Image initimage, string filesaveurl, int maxwidth, int maxheight, int quality)
        {
            //原图宽高均小于模版，不作处理，直接保存
            if (initimage.Width <= maxwidth && initimage.Height <= maxheight)
            {
                initimage.Save(filesaveurl, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
            {
                //模版的宽高比例
                double templaterate = (double)maxwidth / maxheight;
                //原图片的宽高比例
                double initrate = (double)initimage.Width / initimage.Height;

                //原图与模版比例相等，直接缩放
                if (templaterate == initrate)
                {
                    //按模版大小生成最终图片
                    System.Drawing.Image templateimage = new System.Drawing.Bitmap(maxwidth, maxheight);
                    System.Drawing.Graphics templateg = System.Drawing.Graphics.FromImage(templateimage);
                    templateg.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    templateg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    templateg.Clear(Color.White);
                    templateg.DrawImage(initimage, new System.Drawing.Rectangle(0, 0, maxwidth, maxheight), new System.Drawing.Rectangle(0, 0, initimage.Width, initimage.Height), System.Drawing.GraphicsUnit.Pixel);
                    templateimage.Save(filesaveurl, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                //原图与模版比例不等，裁剪后缩放
                else
                {
                    //裁剪对象
                    System.Drawing.Image pickedimage = null;
                    System.Drawing.Graphics pickedg = null;

                    //定位
                    Rectangle fromr = new Rectangle(0, 0, 0, 0);//原图裁剪定位
                    Rectangle tor = new Rectangle(0, 0, 0, 0);//目标定位

                    //宽为标准进行裁剪
                    if (templaterate > initrate)
                    {
                        //裁剪对象实例化
                        pickedimage = new System.Drawing.Bitmap(initimage.Width, (int)Math.Floor(initimage.Width / templaterate));
                        pickedg = System.Drawing.Graphics.FromImage(pickedimage);

                        //裁剪源定位
                        fromr.X = 0;
                        fromr.Y = (int)Math.Floor((initimage.Height - initimage.Width / templaterate) / 2);
                        fromr.Width = initimage.Width;
                        fromr.Height = (int)Math.Floor(initimage.Width / templaterate);

                        //裁剪目标定位
                        tor.X = 0;
                        tor.Y = 0;
                        tor.Width = initimage.Width;
                        tor.Height = (int)Math.Floor(initimage.Width / templaterate);
                    }
                    //高为标准进行裁剪
                    else
                    {
                        pickedimage = new System.Drawing.Bitmap((int)Math.Floor(initimage.Height * templaterate), initimage.Height);
                        pickedg = System.Drawing.Graphics.FromImage(pickedimage);

                        fromr.X = (int)Math.Floor((initimage.Width - initimage.Height * templaterate) / 2);
                        fromr.Y = 0;
                        fromr.Width = (int)Math.Floor(initimage.Height * templaterate);
                        fromr.Height = initimage.Height;

                        tor.X = 0;
                        tor.Y = 0;
                        tor.Width = (int)Math.Floor(initimage.Height * templaterate);
                        tor.Height = initimage.Height;
                    }

                    //设置质量
                    pickedg.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    pickedg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    //裁剪
                    pickedg.DrawImage(initimage, tor, fromr, System.Drawing.GraphicsUnit.Pixel);

                    //按模版大小生成最终图片
                    System.Drawing.Image templateimage = new System.Drawing.Bitmap(maxwidth, maxheight);
                    System.Drawing.Graphics templateg = System.Drawing.Graphics.FromImage(templateimage);
                    templateg.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    templateg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    templateg.Clear(Color.White);
                    templateg.DrawImage(pickedimage, new System.Drawing.Rectangle(0, 0, maxwidth, maxheight), new System.Drawing.Rectangle(0, 0, pickedimage.Width, pickedimage.Height), System.Drawing.GraphicsUnit.Pixel);

                    //关键质量控制
                    //获取系统编码类型数组,包含了jpeg,bmp,png,gif,tiff
                    ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
                    ImageCodecInfo ici = null;
                    foreach (ImageCodecInfo i in icis)
                    {
                        if (i.MimeType == "image/jpeg" || i.MimeType == "image/bmp" || i.MimeType == "image/png" || i.MimeType == "image/gif")
                        {
                            ici = i;
                        }
                    }
                    EncoderParameters ep = new EncoderParameters(1);
                    ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);

                    //保存缩略图
                    templateimage.Save(filesaveurl, ici, ep);
                    //templateimage.Save(filesaveurl, System.Drawing.Imaging.ImageFormat.Jpeg);

                    //释放资源
                    templateg.Dispose();
                    templateimage.Dispose();

                    pickedg.Dispose();
                    pickedimage.Dispose();
                }
            }

            //释放资源
            initimage.Dispose();
        }
        #endregion

        #region 等比缩放
        /// <summary>
        /// 图片等比缩放
        /// </summary>
        /// <param name="initimage">原图</param>
        /// <param name="savepath">缩略图存放地址</param>
        /// <param name="targetwidth">指定的最大宽度</param>
        /// <param name="targetheight">指定的最大高度</param>
        /// <param name="watermarktext">水印文字(为""表示不使用水印)</param>
        /// <param name="watermarkimage">水印图片路径(为""表示不使用水印)</param>
        public static void ZoomAuto(Image initimage, string savepath, double targetwidth, double targetheight, string watermarktext, string watermarkimage)
        {
            //创建目录
            string dir = Path.GetDirectoryName(savepath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            //原图宽高均小于模版，不作处理，直接保存
            if (initimage.Width <= targetwidth && initimage.Height <= targetheight)
            {
                //文字水印
                if (watermarktext != "")
                {
                    using (System.Drawing.Graphics gwater = System.Drawing.Graphics.FromImage(initimage))
                    {
                        System.Drawing.Font fontwater = new Font("黑体", 10);
                        System.Drawing.Brush brushwater = new SolidBrush(Color.White);
                        gwater.DrawString(watermarktext, fontwater, brushwater, 10, 10);
                        gwater.Dispose();
                    }
                }

                //透明图片水印
                if (watermarkimage != "")
                {
                    if (File.Exists(watermarkimage))
                    {
                        //获取水印图片
                        using (System.Drawing.Image wrimage = System.Drawing.Image.FromFile(watermarkimage))
                        {
                            //水印绘制条件：原始图片宽高均大于或等于水印图片
                            if (initimage.Width >= wrimage.Width && initimage.Height >= wrimage.Height)
                            {
                                Graphics gwater = Graphics.FromImage(initimage);

                                //透明属性
                                ImageAttributes imgattributes = new ImageAttributes();
                                ColorMap ColorMap = new ColorMap();
                                ColorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                                ColorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                                ColorMap[] remaptable = { ColorMap };
                                imgattributes.SetRemapTable(remaptable, ColorAdjustType.Bitmap);

                                float[][] colormatrixelements = { 
                                   new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                   new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                   new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                   new float[] {0.0f,  0.0f,  0.0f,  0.5f, 0.0f},//透明度:0.5
                                   new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                };

                                ColorMatrix wmcolormatrix = new ColorMatrix(colormatrixelements);
                                imgattributes.SetColorMatrix(wmcolormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                                gwater.DrawImage(wrimage, new Rectangle(initimage.Width - wrimage.Width, initimage.Height - wrimage.Height, wrimage.Width, wrimage.Height), 0, 0, wrimage.Width, wrimage.Height, GraphicsUnit.Pixel, imgattributes);

                                gwater.Dispose();
                            }
                            wrimage.Dispose();
                        }
                    }
                }

                //保存
                initimage.Save(savepath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
            {
                //缩略图宽、高计算
                double newwidth = initimage.Width;
                double newheight = initimage.Height;

                //宽大于高或宽等于高（横图或正方）
                if (initimage.Width > initimage.Height || initimage.Width == initimage.Height)
                {
                    //如果宽大于模版
                    if (initimage.Width > targetwidth)
                    {
                        //宽按模版，高按比例缩放
                        newwidth = targetwidth;
                        newheight = initimage.Height * (targetwidth / initimage.Width);
                    }
                }
                //高大于宽（竖图）
                else
                {
                    //如果高大于模版
                    if (initimage.Height > targetheight)
                    {
                        //高按模版，宽按比例缩放
                        newheight = targetheight;
                        newwidth = initimage.Width * (targetheight / initimage.Height);
                    }
                }

                //生成新图
                //新建一个bmp图片
                System.Drawing.Image newimage = new System.Drawing.Bitmap((int)newwidth, (int)newheight);
                //新建一个画板
                System.Drawing.Graphics newg = System.Drawing.Graphics.FromImage(newimage);

                //设置质量
                newg.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                newg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                //置背景色
                newg.Clear(Color.White);
                //画图
                newg.DrawImage(initimage, new System.Drawing.Rectangle(0, 0, newimage.Width, newimage.Height), new System.Drawing.Rectangle(0, 0, initimage.Width, initimage.Height), System.Drawing.GraphicsUnit.Pixel);

                //文字水印
                if (watermarktext != "")
                {
                    using (System.Drawing.Graphics gwater = System.Drawing.Graphics.FromImage(newimage))
                    {
                        System.Drawing.Font fontwater = new Font("宋体", 10);
                        System.Drawing.Brush brushwater = new SolidBrush(Color.White);
                        gwater.DrawString(watermarktext, fontwater, brushwater, 10, 10);
                        gwater.Dispose();
                    }
                }

                //透明图片水印
                if (watermarkimage != "")
                {
                    if (File.Exists(watermarkimage))
                    {
                        //获取水印图片
                        using (System.Drawing.Image wrimage = System.Drawing.Image.FromFile(watermarkimage))
                        {
                            //水印绘制条件：原始图片宽高均大于或等于水印图片
                            if (newimage.Width >= wrimage.Width && newimage.Height >= wrimage.Height)
                            {
                                Graphics gwater = Graphics.FromImage(newimage);

                                //透明属性
                                ImageAttributes imgattributes = new ImageAttributes();
                                ColorMap ColorMap = new ColorMap();
                                ColorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                                ColorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                                ColorMap[] remaptable = { ColorMap };
                                imgattributes.SetRemapTable(remaptable, ColorAdjustType.Bitmap);

                                float[][] colormatrixelements = { 
                                   new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                   new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                   new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                   new float[] {0.0f,  0.0f,  0.0f,  0.5f, 0.0f},//透明度:0.5
                                   new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                };

                                ColorMatrix wmcolormatrix = new ColorMatrix(colormatrixelements);
                                imgattributes.SetColorMatrix(wmcolormatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                                gwater.DrawImage(wrimage, new Rectangle(newimage.Width - wrimage.Width, newimage.Height - wrimage.Height, wrimage.Width, wrimage.Height), 0, 0, wrimage.Width, wrimage.Height, GraphicsUnit.Pixel, imgattributes);
                                gwater.Dispose();
                            }
                            wrimage.Dispose();
                        }
                    }
                }

                //保存缩略图
                newimage.Save(savepath, System.Drawing.Imaging.ImageFormat.Jpeg);

                //释放资源
                newg.Dispose();
                newimage.Dispose();
                initimage.Dispose();
            }
        }

        #endregion

        #region 其它
        /// <summary>
        /// 判断文件类型是否为web格式图片
        /// (注：jpg,gif,bmp,png)
        /// </summary>
        /// <param name="contentType">httppostedfile.contenttype</param>
        /// <returns></returns>
        public static bool IsWebImage(string contentType)
        {
            if (contentType == "image/pjpeg" || contentType == "image/jpeg" || contentType == "image/gif" || contentType == "image/bmp" || contentType == "image/png")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion


    }
}
