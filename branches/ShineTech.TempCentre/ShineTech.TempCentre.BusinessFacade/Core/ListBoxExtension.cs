using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShineTech.TempCentre.BusinessFacade
{
    public static class ListBoxExtension
    {
        public static bool MoveSelectedItem(this ListBox lb1, ListBox lb2,bool remove,Action action )
        {
            if (lb1.SelectedItems.Count <= 0)
            {
                action();
                return false;
            }
            else
            {
                string right = lb1.SelectedItem.ToString();
                if (remove)
                {
                    lb1.Items.Remove(right);
                }
                else
                {
                    if (lb2.Items.Contains(right))
                        return false;
                    else
                        lb2.Items.Add(right);
                }
            }
            return true;
        }
    }
}
