using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace TempCentreProductKeyGen
{
    public partial class KeyGen : Form
    {
        private int m_KeyCount;
        private Random random ;
        public List<long> KeysList { get; set; }
        public KeyGen()
        {
            InitializeComponent();
            KeysList = new List<long>();
            random = new Random();
        }
        private void GenKey()
        {
            if (m_KeyCount == 0)
                return;
            else
            {
                //if (KeysList.Count < m_KeyCount)
                //{
                //    Random random = new Random();
                //    long key=GeneraterLongNumm();
                //    if (VerifyMode7(key))
                //    {
                //        KeysList.Add(key);
                //    }
                //    GenKey();
                //}
                //else
                //{
                //    return;
                //}
                while (true)
                {
                    if (KeysList.Count < m_KeyCount)
                    {
                        long key = GeneraterLongNumm();
                        if (VerifyMode7(key))
                        {
                            KeysList.Add(key);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

        }
        private long GeneraterLongNumm()
        {
            long a= random.Next(10000000, 99999999);
            long b = random.Next(10000000, 99999999);
            return a * 100000000 + b;
        }
        private void GenerateGrid()
        {
            dgvKeyList.Rows.Clear();
            dgvKeyList.Columns.Clear();
            dgvKeyList.Columns.Add(new DataGridViewColumn()
            {
                CellTemplate = new DataGridViewTextBoxCell(),
                HeaderText = "ID",
                Name = "ID",
                Width = 30,
                ReadOnly = true
            });
            dgvKeyList.Columns.Add(new DataGridViewColumn()
            {
                CellTemplate = new DataGridViewTextBoxCell(),
                HeaderText = "Key Item",
                Name = "Key",
                Width = 20,
                AutoSizeMode=DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true
            });
            if (m_KeyCount > 0)
            {
                dgvKeyList.Rows.Add(m_KeyCount);
                for (int i = 0; i < m_KeyCount; i++)
                {
                    string key = KeysList[i].ToString();
                    dgvKeyList.Rows[i].Cells[0].Value = i + 1;
                    dgvKeyList.Rows[i].Cells[1].Value = string.Format(@"{4} {0}-{1}-{2}-{3} {5}", key.Substring(0, 4), key.Substring(4, 4), key.Substring(8, 4), key.Substring(12, 4), "{", "}");
                }
            }
        }
        private bool VerifyMode7(long number)
        {
            try
            {
                string key = number.ToString();
                int raw = 0;
                for (int i = 0; i < key.Length; i++)
                {
                    raw += Convert.ToInt32(key[i].ToString());
                }
                if (raw % 7 == 0)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        private void ExtractKeys2File()
        {
            if (KeysList.Count > 0)
            {
                SaveFileDialog file = new SaveFileDialog();
                file.FileName = "TempCentre Pro Keys.txt";
                file.InitialDirectory = Environment.CurrentDirectory;
                if (file.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter stream = new StreamWriter(file.FileName, false))
                    {
                        //stream.SetLength(text.Length);
                        foreach (var item in KeysList)
                        {
                            string key = item.ToString();
                            key = string.Format(@"{0}-{1}-{2}-{3}", key.Substring(0, 4), key.Substring(4, 4), key.Substring(8, 4), key.Substring(12, 4));
                            stream.WriteLine(key);
                        }
                        stream.Close();
                    }
                }
            }
        }
        private void btnGen_Click(object sender, EventArgs e)
        {
            KeysList.Clear();
            int.TryParse(tbItemCount.Text, out m_KeyCount);
            GenKey();
            GenerateGrid();
        }

        private void btnExtract_Click(object sender, EventArgs e)
        {
            ExtractKeys2File();
        }
    }


}
