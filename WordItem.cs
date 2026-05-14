using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSVFile
{
    internal class WordItem
    {
        public string Word { get; set; }
        public string Phonogram { get; set; }
        public string SoundPath { get; set; }
        public string Explain { get; set; }


        /// <summary>
        /// 建構子，從 TSV 字串建立 WordItem 物件
        /// </summary>
        /// <param name=“str”>/param>
        public WordItem(string str)
        { // 用Tab分隔字串
            string[] strLists = str.Split('\t');
            if (strLists.Length >= 3)
            {
                Word = strLists[0];
                Phonogram = strLists[1];
                SoundPath = strLists[2];
                Explain = string.Join(Environment.NewLine, strLists.Skip(3));
            }
        }


        /// <summary>
        /// 覆寫 ToString() 可將物件自動轉換為字串
        /// </summary>
        /// <returns>單字</returns>
        public override string ToString()
        {
            return Word;
        }
    }
}
