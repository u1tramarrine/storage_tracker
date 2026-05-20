using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using storage_tracker.Services;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace storage_tracker.ViewModels
{
    public partial class ItemEditViewModel<T> : ObservableValidator, INotifyPropertyChanged, INotifyDataErrorInfo where T : class
    {
        private readonly IDialogService _dialogService;
        private readonly Dictionary<string, List<string>> _errors = new();

        [ObservableProperty]
        private T _editedItem;

        [ObservableProperty]
        private string _title = string.Empty;

        //[ObservableProperty]
        //private bool _hasErrors;
        public IEnumerable<string> Errors => _errors.SelectMany(x => x.Value).ToList();
        public ItemEditViewModel(T item, IDialogService dialogService)
        {
            _editedItem = item;
            _dialogService = dialogService;
            ValidateAllProperties();
        }
        private string _header;
        public string Header
        {
            get => _header;
            set
            {
                _header = value;
                OnPropertyChanged(nameof(Header));
            }
        }

        //public event PropertyChangedEventHandler? PropertyChanged;

        //protected virtual void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
        [RelayCommand]
        private void Save()
        {
            ValidateAllProperties();

            if (AnyErrors)
            {
                var errorMessages = string.Join("\n", _errors.SelectMany(x => x.Value));
                _dialogService.ShowMessage($"Пожалуйста, исправьте ошибки:\n{errorMessages}", "Ошибка валидации");
                return;
            }

            // Закрываем диалог с результатом OK
            if (System.Windows.Application.Current.Windows.OfType<System.Windows.Window>()
                .LastOrDefault(w => w.IsActive) is System.Windows.Window dialog)
            {
                dialog.DialogResult = true;
                dialog.Close();
            }
        }

        [RelayCommand]
        private void Cancel()
        {
            if (System.Windows.Application.Current.Windows.OfType<System.Windows.Window>()
                .LastOrDefault(w => w.IsActive) is System.Windows.Window dialog)
            {
                dialog.DialogResult = false;
                dialog.Close();
            }
        }

        private void ValidateAllProperties()
        {
            _errors.Clear();
            ValidateProperty(EditedItem, nameof(EditedItem));

            // Дополнительная валидация для конкретных типов
            if (EditedItem is Models.Item item)
            {
                if (string.IsNullOrWhiteSpace(item.Name))
                    AddError(nameof(EditedItem), "Название предмета обязательно");
                if (item.Quantity < 0)
                    AddError(nameof(EditedItem), "Количество не может быть отрицательным");

                if (item.Price < 0)
                    AddError(nameof(EditedItem), "Цена не может быть отрицательной");
                if (item.Price.HasValue)
                {
                    int[] bits = decimal.GetBits(item.Price.Value);
                    int scale = (bits[3] >> 16) & 0x7F; // число знаков после запятой в decimal
                    if (scale > 2)
                        AddError(nameof(EditedItem), "Копеек не может быть больше двух");
                }

                if (item.CreatedAt > DateOnly.FromDateTime(DateTime.Today))
                    AddError(nameof(EditedItem), "Дата не может быть в будущем");
            }

            if (EditedItem is Models.Box box)
            {
                if (string.IsNullOrWhiteSpace(box.Name))
                    AddError(nameof(EditedItem), "Название коробки обязательно");
            }

            if (EditedItem is Models.Category category)
            {
                if (string.IsNullOrWhiteSpace(category.Name))
                    AddError(nameof(EditedItem), "Название категории обязательно");
            }
            
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(string.Empty));
        }

        private void AddError(string propertyName, string error)
        {
            if (!_errors.ContainsKey(propertyName))
                _errors[propertyName] = new List<string>();

            if (!_errors[propertyName].Contains(error))
                _errors[propertyName].Add(error);
        }

        public bool AnyErrors => _errors.Any();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return _errors.SelectMany(x => x.Value);

            return _errors.ContainsKey(propertyName) ? _errors[propertyName] : Enumerable.Empty<string>();
        }
    }
}
