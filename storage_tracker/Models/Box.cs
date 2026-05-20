using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace storage_tracker.Models
{
    public class Box
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        [Display(Name = "Название коробки")]
        [Required(ErrorMessage = "Название обязательно")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Местоположение")]
        public string? Location { get; set; }

        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "URL фото")]
        public string? PhotoUrl { get; set; }

        [ScaffoldColumn(false)]  // Скрываем внешний ключ
        public Guid? CategoryId { get; set; }

        [ScaffoldColumn(false)]  // Скрываем навигационное свойство
        public virtual Category? Category { get; set; }

        [ScaffoldColumn(false)]
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();

        public override string ToString() => Name;
        
    }
}
