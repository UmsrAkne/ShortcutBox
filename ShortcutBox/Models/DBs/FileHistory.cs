namespace ShortcutBox.Models.DBs
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class FileHistory
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime AditionDate { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string FullPath { get; set; }

        [Required]
        public bool IsDirectory { get; set; }

        [Required]
        public bool UsedLastTime { get; set; }
    }
}
