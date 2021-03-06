namespace ShortcutBox.Models.DBs
{
    using System;
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.IO;
    using System.Linq;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    public class FileHistoryDbContext : DbContext
    {
        public DbSet<FileHistory> FileHistories { get; set; }

        public DbSet<TextHistory> TextHistories { get; set; }

        public string DBFileName => "file_history.sqlite";

        public void CreateDatabase()
        {
            if (!File.Exists(DBFileName))
            {
                Database.EnsureCreated();
            }
        }

        public List<FileHistory> GetFileHistoriesSortedByDate()
        {
            return FileHistories.OrderBy(f => f.AditionDate).ToList();
        }

        public void AddHistory(ExFileInfo exfileInfo)
        {
            // 既に同じファイルの情報が履歴にあれば更新。無ければ DB にデータを追加する。
            var sameFileInfo = FileHistories.Where(f => f.FullPath == exfileInfo.FullName);

            if (sameFileInfo.Count() != 0)
            {
                sameFileInfo.First().AditionDate = DateTime.Now;
            }
            else
            {
                var fileHistory = new FileHistory();
                fileHistory.Id = FileHistories.Count() + 1;
                fileHistory.AditionDate = DateTime.Now;
                fileHistory.FullPath = exfileInfo.FullName;
                fileHistory.IsDirectory = exfileInfo.IsDirectory;
                fileHistory.Name = exfileInfo.Name;
                FileHistories.Add(fileHistory);
            }

            SaveChanges();
        }

        public void SaveCopiedText(string text)
        {
            TextHistories.Add(new TextHistory() { Text = text, Id = TextHistories.Count() + 1 });
            SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!File.Exists(DBFileName))
            {
                SQLiteConnection.CreateFile(DBFileName);
            }

            var connectionString = new SqliteConnectionStringBuilder { DataSource = DBFileName }.ToString();
            optionsBuilder.UseSqlite(new SQLiteConnection(connectionString));
        }
    }
}
