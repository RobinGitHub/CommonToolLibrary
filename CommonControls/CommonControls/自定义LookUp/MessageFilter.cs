using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//_5_1_a_s_p_x
namespace CommonControls
{
    public delegate void MouseEvent();
    public class MessageFilter : IMessageFilter
    {
        public MouseEvent MouseClickEvent;
        const int WM_NCLBUTTONDOWN = 0x00A1;
        const int HTCAPTION = 2;

        //实现PreFilterMessage方法     
        public bool PreFilterMessage(ref Message message) 
        {
            if (message.Msg == 513)
            {
                MouseClickEvent();
                return false;
            }
            if (message.Msg == 516)
            {
                MouseClickEvent();
                return false;
            }
            //判断鼠标在标题栏，关闭LookupContext
            if (message.Msg == WM_NCLBUTTONDOWN && message.WParam.ToInt32() == HTCAPTION)
            {
                MouseClickEvent();
                return false;
            }
            return false;
        }
    }
}
