using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TSVFile
{
    public partial class frmTSVFile : Form
    {
        /// <summary>
        /// 關於視窗
        /// </summary>
        frmAbout about = new frmAbout();

        /// <summary>
        /// 單字清單
        /// </summary>
        WordCollection _WordList = new WordCollection();

        private ToolStripTextBox tsTxtSearch;

        public frmTSVFile()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 更新 ListView 的內容
        /// </summary>
        private void UpdateListView(string keyword = "")
        {
            lvwWord.BeginUpdate(); //暫停重繪
            // 清除 ListView 的所有項目
            lvwWord.Items.Clear();

            int rowIndex = 0; // 用來計算目前是第幾行

            // 將 WordCollection 物件中的資料載入到 ListView 中
            foreach (WordItem item in _WordList)
            {
                // 篩選邏輯：如果 keyword 是空的，或者「單字」或「解釋」中包含 keyword (忽略大小寫)，就顯示它
                if (string.IsNullOrWhiteSpace(keyword) ||
                    item.Word.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    item.Explain.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    // 建立 ListViewItem 物件
                    ListViewItem lvi = new ListViewItem(item.Word);
                    lvi.SubItems.Add(item.Phonogram);
                    lvi.SubItems.Add(item.SoundPath);
                    lvi.SubItems.Add(item.Explain);

                    // 根據 rowIndex 設定背景顏色 (交替列顏色)
                    if (rowIndex % 2 == 0)
                    {
                        lvi.BackColor = Color.LightBlue; // 偶數行顏色
                    }
                    else
                    {
                        lvi.BackColor = Color.White; // 奇數行顏色 (您可以換成任何喜歡的顏色，例如 Color.AliceBlue)
                    }

                    // 將 ListViewItem 物件加入到 ListView 中
                    lvwWord.Items.Add(lvi);
                    
                    rowIndex++; // 行數增加
                }
            }
            lvwWord.EndUpdate(); //重繪
        }

        // 在您讀取完檔案並將所有項目加入 ListView(lvwWord) 之後，呼叫此方法
        private void AutoResizeListViewColumns()
        {
            // 遍歷 ListView 中的每一個欄位
            foreach (ColumnHeader column in lvwWord.Columns)
            {
                // 寬度設為 -1 表示根據「項目內容」來自動調整寬度
                // 反之，若設為 -2 則是根據「欄位標題」的長度來調整
                column.Width = -1;
            }
        }

        private void tsmiAbout_Click(object sender, EventArgs e)
        {
            // 顯示關於視窗
            about.ShowDialog(this);
        }

        private void tsmiOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            // 設定檔案類型過濾器，讓使用者只能選擇 TSV、TXT 或所有檔案
            ofd.Filter = "TSV files (*.tsv)|*.tsv|Text files (*.txt)|*.txt|All files (*.*) | *.* ";
            // 設定視窗標題
            ofd.Title = "開啟檔案";
            // 設定初始目錄為程式所在目錄
            ofd.InitialDirectory = Application.StartupPath;

            DialogResult dr = ofd.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                // 讀取檔案並且將每一行的資料放入字串陣列
                string[] lines = File.ReadAllLines(ofd.FileName, Encoding.UTF8);

                // 將字串陣列的資料載入到 WordCollection 物件中
                _WordList.LoadFromStringArray(lines);

                // 將 WordCollection 物件中的資料載入到 ListView 中
                UpdateListView();
                // 自動調整欄位寬度
                AutoResizeListViewColumns();

                this.tsslMessage.Text = $"{_WordList.Count} 單字已成功載入";
            }
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmTSVFile_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("確定要離開嗎?", "離開", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
            {
                e.Cancel = true; // 取消關閉
            }
        }

        private void frmTSVFile_Load(object sender, EventArgs e)
        {
            this.tsslMessage.Text = "";

            // 建立搜尋的 TextBox
            tsTxtSearch = new ToolStripTextBox();
            tsTxtSearch.Alignment = ToolStripItemAlignment.Right; // 靠右對齊
            tsTxtSearch.Width = 200; // 控制寬度
            tsTxtSearch.ToolTipText = "在此輸入單字或解釋進行搜尋...";
            tsTxtSearch.Text = "搜尋...";
            tsTxtSearch.ForeColor = Color.Gray;

            // 加入文字焦點事件，實作 Placeholder (預設文字) 的效果
            tsTxtSearch.GotFocus += (s, args) =>
            {
                if (tsTxtSearch.Text == "搜尋...")
                {
                    tsTxtSearch.Text = "";
                    tsTxtSearch.ForeColor = Color.Black;
                }
            };
            tsTxtSearch.LostFocus += (s, args) =>
            {
                if (string.IsNullOrWhiteSpace(tsTxtSearch.Text))
                {
                    tsTxtSearch.Text = "搜尋...";
                    tsTxtSearch.ForeColor = Color.Gray;
                }
            };

            // 加入 TextChanged 事件，當使用者輸入文字時自動過濾
            tsTxtSearch.TextChanged += (s, args) =>
            {
                // 如果是預設提示字串，則以空字串進行過濾 (顯示全部)
                string searchString = (tsTxtSearch.Text == "搜尋...") ? "" : tsTxtSearch.Text;
                UpdateListView(searchString);
            };

            // 將搜尋方塊加入現有的 MenuStrip
            mnsWord.Items.Add(tsTxtSearch);
        }
    }
}
