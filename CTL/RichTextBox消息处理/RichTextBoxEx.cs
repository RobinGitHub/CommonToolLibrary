using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace RichTextBox消息处理
{
    public class RichTextBoxEx : RichTextBox
    {
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam);
        [DllImport("User32.dll", CharSet = CharSet.Auto, PreserveSig = false)]
        internal static extern IRichEditOle SendMessage(IntPtr hWnd, int message, int wParam);

        [StructLayout(LayoutKind.Sequential)]
        private struct CHARFORMAT2_STRUCT
        {
            public UInt32 cbSize;
            public UInt32 dwMask;
            public UInt32 dwEffects;
            public Int32 yHeight;
            public Int32 yOffset;
            public Int32 crTextColor;
            public byte bCharSet;
            public byte bPitchAndFamily;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] szFaceName;
            public UInt16 wWeight;
            public UInt16 sSpacing;
            public int crBackColor;
            public int lcid;
            public int dwReserved;
            public Int16 sStyle;
            public Int16 wKerning;
            public byte bUnderlineType;
            public byte bAnimation;
            public byte bRevAuthor;
            public byte bReserved1;
        }

        private const int WM_USER = 0x0400;
        private const int EM_GETCHARFORMAT = WM_USER + 58;
        private const int EM_SETCHARFORMAT = WM_USER + 68;
        private const int SCF_SELECTION = 0x0001;
        private const UInt32 CFE_LINK = 0x0020;
        private const UInt32 CFM_LINK = 0x00000020;


        IRichEditOle _richEditOle = null;
        IRichEditOle richEditOle
        {
            get
            {
                if (this._richEditOle == null)
                {
                    IntPtr ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(IntPtr)));
                    IntPtr richEditOleIntPtr = IntPtr.Zero;
                    Marshal.WriteIntPtr(ptr, IntPtr.Zero);
                    try
                    {
                        int msgResult = SendMessage(this.Handle, RichEditOle.EM_GETOLEINTERFACE, IntPtr.Zero, ptr);
                        if (msgResult != 0)
                        {
                            IntPtr intPtr = Marshal.ReadIntPtr(ptr);
                            try
                            {
                                if (intPtr != IntPtr.Zero)
                                {
                                    Guid guid = new Guid("00020D00-0000-0000-c000-000000000046");
                                    Marshal.QueryInterface(intPtr, ref guid, out richEditOleIntPtr);

                                    this._richEditOle = (IRichEditOle)Marshal.GetTypedObjectForIUnknown(richEditOleIntPtr, typeof(IRichEditOle));
                                }
                            }
                            finally
                            {
                                Marshal.Release(intPtr);
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        Trace.WriteLine(err.ToString());
                    }
                    finally
                    {
                        Marshal.FreeCoTaskMem(ptr);
                    }
                }
                return this._richEditOle;
            }
        }

        class RichEditOle
        {
            [DllImport("ole32.dll", PreserveSig = false)]
            static extern int CreateILockBytesOnHGlobal(IntPtr hGlobal, bool fDeleteOnRelease, [Out] out ILockBytes ppLkbyt);
            [DllImport("ole32.dll")]
            static extern int StgCreateDocfileOnILockBytes(ILockBytes plkbyt, uint grfMode, uint reserved, out IStorage ppstgOpen);

            public const int EM_GETOLEINTERFACE = 0x0400 + 60;

            private RichTextBoxEx richTextBox;
            private IRichEditOle ole;

            internal RichEditOle(RichTextBoxEx richEdit)
            {
                this.richTextBox = richEdit;
                this.ole = SendMessage(this.richTextBox.Handle, EM_GETOLEINTERFACE, 0);
            }

            internal void InsertControl(Control ctrl)
            {
                if (ctrl == null)
                    return;

                Guid guid = Marshal.GenerateGuidForType(ctrl.GetType());

                ILockBytes lockBytes;
                CreateILockBytesOnHGlobal(IntPtr.Zero, true, out lockBytes);

                IStorage storage;
                StgCreateDocfileOnILockBytes(lockBytes, (uint)(STGM.STGM_SHARE_EXCLUSIVE | STGM.STGM_CREATE | STGM.STGM_READWRITE), 0, out storage);

                IOleClientSite oleClientSite;
                this.ole.GetClientSite(out oleClientSite);

                REOBJECT reObject = new REOBJECT()
                {
                    cp = this.richTextBox.SelectionStart,
                    clsid = guid,
                    pstg = storage,
                    poleobj = Marshal.GetIUnknownForObject(ctrl),
                    polesite = oleClientSite,
                    dvAspect = (uint)(DVASPECT.DVASPECT_CONTENT),
                    dwFlags = (uint)0x00000002,
                };

                try
                {
                    reObject.dwUser = 1;
                }
                catch { }

                this.ole.InsertObject(reObject);

                Marshal.ReleaseComObject(lockBytes);
                Marshal.ReleaseComObject(oleClientSite);
                Marshal.ReleaseComObject(storage);
            }
        }


        public RichTextBoxEx()
            : base()
        {
            base.BorderStyle = BorderStyle.FixedSingle;
            this.DetectUrls = false;
        }

        public new bool ReadOnly { get; set; }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0100:
                    if (this.ReadOnly)
                        return;
                    break;
                case 0X0102:
                    if (this.ReadOnly)
                        return;
                    break;
                default:
                    break;
            }
            base.WndProc(ref m);
        }

        //List<MyGIF> gifList = new List<MyGIF>();
        //Panel gifPanel = new Panel();

        //[DefaultValue(false)]
        //public new bool DetectUrls
        //{
        //    get { return base.DetectUrls; }
        //    set { base.DetectUrls = value; }
        //}

        //public void InsertGIF(string Name, Image Data)
        //{
        //    //MyGIF gif = new MyGIF(Name, Data);
        //    //gif.Box.Invalidate();
        //    //this.gifPanel.Controls.Add(gif.Box);
        //    //this.gifList.Add(gif);

        //    //RichEditOle ole = new RichEditOle(this);
        //    //ole.InsertControl(gif);

        //    //this.Invalidate();
        //}

        //public void InsertControl(Control ctrl)
        //{
        //    this.gifPanel.Controls.Add(ctrl);
        //    RichEditOle ole = new RichEditOle(this);
        //    ole.InsertControl(ctrl);

        //    this.Invalidate();
        //}

        //public string GetGIFInfo()
        //{
        //    string imageInfo = "";
        //    REOBJECT reObject = new REOBJECT();
        //    for (int i = 0; i < this.richEditOle.GetObjectCount(); i++)
        //    {
        //        this.richEditOle.GetObject(i, reObject, GETOBJECTOPTIONS.REO_GETOBJ_ALL_INTERFACES);
        //        MyGIF gif = this.gifList.Find(p => p != null && p.Index == reObject.dwUser);
        //        if (gif != null)
        //        {
        //            imageInfo += reObject.cp.ToString() + ":" + gif.Name + "|";
        //        }
        //    }
        //    return imageInfo;
        //}

    }
}
