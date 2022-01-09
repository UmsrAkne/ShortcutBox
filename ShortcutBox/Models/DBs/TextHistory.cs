namespace ShortcutBox.Models.DBs
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TextHistory
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
