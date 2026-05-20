using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace storage_tracker.Models
{
    public class Item
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        [Display(Name = "Название предмета")]
        [Required(ErrorMessage = "Название обязательно")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [Display(Name = "Количество")]
        [Required(ErrorMessage = "Количество обязательно")]
        [Range(0, 9999, ErrorMessage = "Количество должно быть от 0 до 9999")]
        public int Quantity { get; set; } = 1;

        [Display(Name = "Цена (BYN)")]
        [Range(0, 999999, ErrorMessage = "Цена должна быть от 0 до 999999")]
        public decimal? Price { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "URL фото")]
        public string? PhotoUrl { get; set; }

        [ScaffoldColumn(false)]  // Скрываем внешние ключи
        public Guid? BoxId { get; set; }

        [ScaffoldColumn(false)]
        public Guid? CategoryId { get; set; }

        [ScaffoldColumn(true)] 
        [Display(Name = "Коробка")]
        public virtual Box? Box { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "Категория")]
        public virtual Category? Category { get; set; }

        [Display(Name = "Заметки")]
        [DataType(DataType.MultilineText)]
        public string? Notes { get; set; }

        [Display(Name = "Дата создания")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Today.Date);

        public override string ToString() => Name;
    }
}
