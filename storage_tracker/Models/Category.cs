using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace storage_tracker.Models
{
    public class Category
    {
        [ScaffoldColumn(false)]  // Скрываем колонку в UI
        public Guid Id { get; set; }

        [Display(Name = "Название категории")]
        [Required(ErrorMessage = "Название обязательно")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [ScaffoldColumn(false)]  // Скрываем навигационные свойства
        public virtual ICollection<Box> Boxes { get; set; } = new List<Box>();

        [ScaffoldColumn(false)]
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();

        public override string ToString() => Name;
    }
}
