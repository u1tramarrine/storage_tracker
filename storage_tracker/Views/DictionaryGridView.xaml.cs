using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Windows.Controls;

namespace storage_tracker.Views
{
    public partial class DictionaryGridView : UserControl
    {
        public DictionaryGridView()
        {
            InitializeComponent();
        }

        protected void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            // Получаем информацию о свойстве
            var property = e.PropertyDescriptor as System.ComponentModel.PropertyDescriptor;

            if (property != null)
            {
                // 1. Проверяем атрибут [ScaffoldColumn(false)] - если есть, скрываем колонку
                var scaffoldAttr = property.Attributes.OfType<ScaffoldColumnAttribute>().FirstOrDefault();
                if (scaffoldAttr != null && !scaffoldAttr.Scaffold)
                {
                    e.Cancel = true;
                    return;
                }

                // 2. Проверяем атрибут [Display(Name = "...")] - устанавливаем заголовок
                var displayAttr = property.Attributes.OfType<DisplayAttribute>().FirstOrDefault();
                if (displayAttr != null && !string.IsNullOrEmpty(displayAttr.Name))
                {
                    e.Column.Header = displayAttr.Name;
                }

                // 3. Проверяем атрибут [DisplayFormat] для форматирования дат
                var formatAttr = property.Attributes.OfType<DisplayFormatAttribute>().FirstOrDefault();
                if (formatAttr != null && !string.IsNullOrEmpty(formatAttr.DataFormatString))
                {
                    if (e.Column is DataGridTextColumn textColumn)
                    {
                        textColumn.Binding.StringFormat = formatAttr.DataFormatString;
                    }
                }

                if (new string[] {"Price"}.Contains(e.PropertyName))
                {
                    e.Column.CanUserSort = true;
                }

            }

        }
    }
}
