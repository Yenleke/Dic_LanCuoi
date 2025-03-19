using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Dic_AppTest
{
    class SearchHistory
    {
        private readonly string historyFile = "history.txt"; // File lưu lịch sử

        // Danh sách gồm từ đã tìm và thời gian tìm thấy mới nhất
        public List<(string Word, DateTime Timestamp)> HistoryList { get; private set; } = new List<(string, DateTime)>();

        public SearchHistory()
        {
            LoadHistory();
        }

        // Thêm từ vào lịch sử, nếu tồn tại thì xóa trước khi thêm mới
        public void AddToHistory(string word)
        {
            HistoryList.RemoveAll(h => h.Word == word);
            HistoryList.Insert(0, (word, DateTime.Now));

            // Giữ tối đa từ, vd là 15 từ
            if (HistoryList.Count > 15)
            {
                HistoryList.RemoveAt(HistoryList.Count - 1);
            }

            SaveHistory();
        }

        // Xóa một từ khỏi lịch sử
        public void RemoveFromHistory(string word)
        {
            HistoryList.RemoveAll(h => h.Word == word);
            SaveHistory();
        }

        // Xóa toàn bộ lịch sử
        public void ClearHistory()
        {
            HistoryList.Clear();
            File.WriteAllText(historyFile, "");
        }

        // Lưu lịch sử vào file
        private void SaveHistory()
        {
            File.WriteAllLines(historyFile, HistoryList.Select(h => $"{h.Word}|{h.Timestamp:yyyy-MM-dd HH:mm:ss}"));
        }

        // Tải lịch sử từ file
        private void LoadHistory()
        {
            if (File.Exists(historyFile))
            {
                var lines = File.ReadAllLines(historyFile);
                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length == 2 && DateTime.TryParse(parts[1], out DateTime timestamp))
                    {
                        HistoryList.Add((parts[0], timestamp));
                    }
                }
            }
        }

        // Hiển thị lịch sử dưới dạng chuỗi
        public List<string> GetHistoryDisplay()
        {
            return HistoryList.Select(h => $"{h.Word} - {h.Timestamp:HH:mm:ss dd/MM/yyyy}").ToList();
        }
    }
}
