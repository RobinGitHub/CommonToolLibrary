using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Text;

namespace RichTextBox消息处理
{
    internal class RichEditOle
    {
        private ChatRichTextBox _richEdit;
        private IRichEditOle _richEditOle;
        /// <summary>
        /// 记录插入的OLE对象
        /// </summary>
        Dictionary<uint, Control> oleList = new Dictionary<uint, Control>();

        public RichEditOle(ChatRichTextBox richEdit)
        {
            _richEdit = richEdit;
        }

        public IRichEditOle IRichEditOle
        {
            get
            {
                if (_richEditOle == null)
                {
                    _richEditOle = NativeMethods.SendMessage(
                        _richEdit.Handle, NativeMethods.EM_GETOLEINTERFACE, 0);
                }
                return _richEditOle;
            }
        }
        /// <summary>
        /// 控件索引
        /// </summary>
        uint ctlIndex = 0;
        public void InsertControl(Control control)
        {
            if (control != null)
            {
                ILockBytes bytes;
                IStorage storage;
                IOleClientSite site;
                Guid guid = Marshal.GenerateGuidForType(control.GetType());
                NativeMethods.CreateILockBytesOnHGlobal(IntPtr.Zero, true, out bytes);
                NativeMethods.StgCreateDocfileOnILockBytes(bytes, (uint)(STGM.STGM_SHARE_EXCLUSIVE | STGM.STGM_CREATE | STGM.STGM_READWRITE), 0, out storage);
                IRichEditOle.GetClientSite(out site);

                REOBJECT lpreobject = new REOBJECT();
                lpreobject.cp = _richEdit.SelectionStart;
                lpreobject.clsid = guid;
                lpreobject.pstg = storage;
                lpreobject.poleobj = Marshal.GetIUnknownForObject(control);
                lpreobject.polesite = site;
                lpreobject.dvAspect = (uint)(DVASPECT.DVASPECT_CONTENT);
                lpreobject.dwFlags = (uint)(REOOBJECTFLAGS.REO_BELOWBASELINE);
                lpreobject.dwUser = ctlIndex;
                IRichEditOle.InsertObject(lpreobject);
                Marshal.ReleaseComObject(bytes);
                Marshal.ReleaseComObject(site);
                Marshal.ReleaseComObject(storage);

                oleList.Add(ctlIndex, control);
                ctlIndex++;
            }
        }

        public bool InsertImageFromFile(string strFilename)
        {
            ILockBytes bytes;
            IStorage storage;
            IOleClientSite site;
            object obj2;
            NativeMethods.CreateILockBytesOnHGlobal(IntPtr.Zero, true, out bytes);
            NativeMethods.StgCreateDocfileOnILockBytes(bytes, 0x1012, 0, out storage);
            IRichEditOle.GetClientSite(out site);
            FORMATETC pFormatEtc = new FORMATETC();
            pFormatEtc.cfFormat = (CLIPFORMAT)0;
            pFormatEtc.ptd = IntPtr.Zero;
            pFormatEtc.dwAspect = DVASPECT.DVASPECT_CONTENT;
            pFormatEtc.lindex = -1;
            pFormatEtc.tymed = TYMED.TYMED_NULL;
            Guid riid = new Guid("{00000112-0000-0000-C000-000000000046}");
            Guid rclsid = new Guid("{00000000-0000-0000-0000-000000000000}");
            NativeMethods.OleCreateFromFile(ref rclsid, strFilename, ref riid, 1, ref pFormatEtc, site, storage, out obj2);
            if (obj2 == null)
            {
                Marshal.ReleaseComObject(bytes);
                Marshal.ReleaseComObject(site);
                Marshal.ReleaseComObject(storage);
                return false;
            }
            IOleObject pUnk = (IOleObject)obj2;
            Guid pClsid = new Guid();
            pUnk.GetUserClassID(ref pClsid);
            NativeMethods.OleSetContainedObject(pUnk, true);
            REOBJECT lpreobject = new REOBJECT();
            lpreobject.cp = _richEdit.SelectionStart;
            lpreobject.clsid = pClsid;
            lpreobject.pstg = storage;
            lpreobject.poleobj = Marshal.GetIUnknownForObject(pUnk);
            lpreobject.polesite = site;
            lpreobject.dvAspect = (uint)(DVASPECT.DVASPECT_CONTENT);
            lpreobject.dwFlags = (uint)(REOOBJECTFLAGS.REO_BELOWBASELINE);
            lpreobject.dwUser = 0;
            IRichEditOle.InsertObject(lpreobject);
            Marshal.ReleaseComObject(bytes);
            Marshal.ReleaseComObject(site);
            Marshal.ReleaseComObject(storage);
            Marshal.ReleaseComObject(pUnk);
            return true;
        }

        public REOBJECT InsertOleObject(IOleObject oleObject, int index)
        {
            if (oleObject == null)
            {
                return null;
            }

            ILockBytes pLockBytes;
            NativeMethods.CreateILockBytesOnHGlobal(IntPtr.Zero, true, out pLockBytes);

            IStorage pStorage;
            NativeMethods.StgCreateDocfileOnILockBytes(
                pLockBytes,
                (uint)(STGM.STGM_SHARE_EXCLUSIVE | STGM.STGM_CREATE | STGM.STGM_READWRITE),
                0,
                out pStorage);

            IOleClientSite pOleClientSite;
            IRichEditOle.GetClientSite(out pOleClientSite);

            Guid guid = new Guid();

            oleObject.GetUserClassID(ref guid);
            NativeMethods.OleSetContainedObject(oleObject, true);

            REOBJECT reoObject = new REOBJECT();

            reoObject.cp = _richEdit.TextLength;
            reoObject.clsid = guid;
            reoObject.pstg = pStorage;
            reoObject.poleobj = Marshal.GetIUnknownForObject(oleObject);
            reoObject.polesite = pOleClientSite;
            reoObject.dvAspect = (uint)DVASPECT.DVASPECT_CONTENT;
            reoObject.dwFlags = (uint)REOOBJECTFLAGS.REO_BELOWBASELINE;
            reoObject.dwUser = (uint)index;

            IRichEditOle.InsertObject(reoObject);

            Marshal.ReleaseComObject(pLockBytes);
            Marshal.ReleaseComObject(pOleClientSite);
            Marshal.ReleaseComObject(pStorage);

            return reoObject;
        }

        public void UpdateObjects()
        {
            int objectCount = this.IRichEditOle.GetObjectCount();
            for (int i = 0; i < objectCount; i++)
            {
                REOBJECT lpreobject = new REOBJECT();
                IRichEditOle.GetObject(i, lpreobject, GETOBJECTOPTIONS.REO_GETOBJ_ALL_INTERFACES);
                Point positionFromCharIndex = this._richEdit.GetPositionFromCharIndex(lpreobject.cp);

                GifBox gif = this.oleList[lpreobject.dwUser] as GifBox;
                Rectangle rc = new Rectangle(positionFromCharIndex.X, positionFromCharIndex.Y, gif.Width, gif.Height);

                _richEdit.Invalidate(rc, false);
            }
        }

        public void UpdateObjects(int pos)
        {
            REOBJECT lpreobject = new REOBJECT();
            IRichEditOle.GetObject(
                pos,
                lpreobject,
                GETOBJECTOPTIONS.REO_GETOBJ_ALL_INTERFACES);
            UpdateObjects(lpreobject);
        }

        public void UpdateObjects(REOBJECT reObj)
        {
            Point positionFromCharIndex = _richEdit.GetPositionFromCharIndex(
                    reObj.cp);
            Size size = GetSizeFromMillimeter(reObj);
            Rectangle rc = new Rectangle(positionFromCharIndex, size);
            _richEdit.Invalidate(rc, false);
        }

        private Size GetSizeFromMillimeter(REOBJECT lpreobject)
        {
            using (Graphics graphics = Graphics.FromHwnd(_richEdit.Handle))
            {
                Point[] pts = new Point[1];
                graphics.PageUnit = GraphicsUnit.Millimeter;

                pts[0] = new Point(
                    lpreobject.sizel.Width / 100,
                    lpreobject.sizel.Height / 100);
                graphics.TransformPoints(
                    CoordinateSpace.Device,
                    CoordinateSpace.Page,
                    pts);
                return new Size(pts[0]);
            }
        }
        /// <summary>
        /// 获取所有图片信息
        /// </summary>
        /// <returns>
        /// 返回Json格式数据
        /// </returns>
        public List<GifBox> GetGIFInfo()
        {
            List<GifBox> gifList = new List<GifBox>();
            REOBJECT reObject = new REOBJECT();
            try
            {
                for (int i = 0; i < this.IRichEditOle.GetObjectCount(); i++)
                {
                    IRichEditOle.GetObject(i, reObject, GETOBJECTOPTIONS.REO_GETOBJ_ALL_INTERFACES);
                    GifBox gif = this.oleList[reObject.dwUser] as GifBox;
                    if (gif != null)
                    {
                        //sb.Append("[");
                        //sb.Append("Index:" + reObject.cp.ToString() + ",FilePath:" + gif.FilePath);
                        //sb.Append("],");
                        gif.Index = reObject.cp;
                        gifList.Add(gif);
                    }
                }
                return gifList;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
