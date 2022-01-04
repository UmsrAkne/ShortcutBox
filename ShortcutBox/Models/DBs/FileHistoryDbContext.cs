namespace ShortcutBox.Models.DBs
{
    using System.Data.SQLite;
    using System.IO;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;

    public class FileHistoryDbContext : DbContext
    {
        public DbSet<FileHistory> FIleHistories { get; set; }

        public string DBFileName => "file_history.sqlite";

        public void CreateDatabase()
        {
            if (!File.Exists(DBFileName))
            {
                Database.EnsureCreated();
            }
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
