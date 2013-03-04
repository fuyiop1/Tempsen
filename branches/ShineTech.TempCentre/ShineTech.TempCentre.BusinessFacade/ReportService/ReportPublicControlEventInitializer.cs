using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.Platform;
using System.Drawing;

namespace ShineTech.TempCentre.BusinessFacade
{
    public class ReportPublicControlEventInitializer
    {

        #region Report Title TextBox Events

        public static void InitEventForTitleTextBox(TextBox tb)
        {
            if (tb != null)
            {
                tb.TextChanged += new EventHandler(tbReportTitle_TextChanged);
                tb.GotFocus += new EventHandler(tbReportTitle_GotFocus);
                tb.Leave += new EventHandler(tbReportTitle_Leave);
            }
        }

        private static void tbReportTitle_Leave(object sender, EventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null)
            {
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    tb.Text = ReportConstString.TitleDefaultString;
                }
            }
        }

        private static void tbReportTitle_GotFocus(object sender, EventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null)
            {
                if (tb.Text.Trim() == ReportConstString.TitleDefaultString)
                {
                    tb.Text = "";
                }
            }
        }

        private static void tbReportTitle_TextChanged(object sender, EventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null)
            {
                if (tb.Text.Trim() == ReportConstString.TitleDefaultString)
                {
                    tb.ForeColor = Color.Silver;
                }
                else
                {
                    tb.ForeColor = Color.Black;
                }
            }
        }
        #endregion

        #region Report Comment TextBox Events
        public static void InitEventForCommentTextBox(TextBox tb)
        {
            if (tb != null)
            {
                tb.TextChanged += new EventHandler(tbReportComment_TextChanged);
                tb.MouseClick += new MouseEventHandler(tbReportComment_LeftClick);
                tb.Leave += new EventHandler(tbReportComment_Leave);
            }
        }

        public static void InitEventForCoupleTextBox(TextBox tb1, TextBox tb2)
        {
            if (tb1 != null && tb2 != null)
            {
                tb1.TextChanged += new EventHandler((sender, e) =>
                {
                    tb2.Text = tb1.Text;
                });
                tb2.TextChanged += new EventHandler((sender, e) =>
                {
                    tb1.Text = tb2.Text;
                });
            }
        }


        private static void tbReportComment_Leave(object sender, EventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null)
            {
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    tb.Text = ReportConstString.CommentDefaultString;
                }
            }
        }

        private static void tbReportComment_LeftClick(object sender, MouseEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null && e.Button == MouseButtons.Left)
            {
                if (tb.Text.Trim() == ReportConstString.CommentDefaultString)
                {
                    tb.Text = "";
                }
            }
        }

        private static void tbReportComment_TextChanged(object sender, EventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null)
            {
                if (tb.Text.Trim() == ReportConstString.CommentDefaultString)
                {
                    tb.ForeColor = Color.Silver;
                }
                else
                {
                    tb.ForeColor = Color.Black;
                }
            }
        }
        #endregion

    }
}
