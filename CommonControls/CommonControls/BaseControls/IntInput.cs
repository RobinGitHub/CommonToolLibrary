using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CommonControls
{
    /// <summary>
    /// 整数框
    /// </summary>
    public class IntInput : TextBoxEx
    {
        string lastValue = "0";
        /// <summary>
        /// 最大值
        /// </summary>
        private int maxValue = 10000;
        /// <summary>
        /// 最小值
        /// </summary>
        private int minValue = 0;

        /// <summary>
        /// 最大值
        /// </summary>
        [DefaultValue(10000), Category("Edu345"), DisplayName("1.MaxValue"), Description("最大值")]
        public int MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }

        /// <summary>
        /// 最小值
        /// </summary>
        [DefaultValue(0), Category("Edu345"), DisplayName("2.MinValue"), Description("最小值")]
        public int MinValue
        {
            get { return minValue; }
            set { minValue = value; lastValue = value.ToString(); }
        }


        public IntInput()
            : base()
        {
            this.MaxLength = 3;
            this.TextChanged += IntInput_TextChanged;
        }

        
        void IntInput_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (string.IsNullOrEmpty(txt.Text.Trim()))
            {
                return;
            }
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"^-?[1-9]\d*$");
            if (reg.IsMatch(txt.Text))
            {
                if (int.Parse(txt.Text) >= minValue && int.Parse(txt.Text) <= maxValue)
                {
                    lastValue = txt.Text;
                }
                else
                    this.TextValue = lastValue;
            }
            else
            {
                this.TextValue = lastValue;
            }
        }


    }
}
