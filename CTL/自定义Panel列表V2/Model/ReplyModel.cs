using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace 自定义Panel列表V2
{
    /// <summary>
    /// 评论
    /// </summary>
    public class ReplyModel : DataPanelViewRow
    {
        /// <summary>
        /// 评论
        /// </summary>
        public DataTable ReplyData { get; set; }

        public bool IsShowReply { get; set; }
        /// <summary>
        /// 输入的文本
        /// </summary>
        public string InputText { get; set; }
    }
}
