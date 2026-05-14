using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSVFile
{
    internal class WordCollection : Collection<WordItem>
    {
        /// <summary>
        /// 從字串陣列載入資料
        /// </summary>
        /// <param name="lines">輸入的單字字串陣列</param>
        public void LoadFromStringArray(string[] lines)
        {   
            this.Clear(); // 清空目前的集合，以便載入新的資料

            // 將字串陣列的資料載入到 WordCollection 物件中
            foreach (string line in lines)
            {
                // 產生 WordItem 物件
                WordItem item = new WordItem(line);
                this.Add(item);
            }
        }
    }
}
