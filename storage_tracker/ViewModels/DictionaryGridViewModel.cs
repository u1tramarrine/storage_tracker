using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

using storage_tracker.Services;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;


namespace storage_tracker.ViewModels
{


    public partial class DictionaryGridViewModel<T> : ObservableObject where T : class
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDialogService _dialogService;
        private readonly IRepository<T> _repository;
        private readonly string _dictionaryName;

        [ObservableProperty]
        private ObservableCollection<T> _items = new();

        [ObservableProperty]
        private T? _selectedItem;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private string _statusMessage = string.Empty;

        public DictionaryGridViewModel(IServiceProvider serviceProvider, IDialogService dialogService, string dictionaryName)
        {
            _serviceProvider = serviceProvider;
            _dialogService = dialogService;
            _dictionaryName = dictionaryName;
            _repository = serviceProvider.GetRequiredService<IRepository<T>>();
        }

        public async Task LoadDataAsync()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Загрузка данных...";

                var items = await _repository.GetAllAsync();
                Items.Clear();
                foreach (var item in items)
                {
                    Items.Add(item);
                }

                StatusMessage = $"Загружено записей: {Items.Count}";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Ошибка загрузки: {ex.Message}";
               _dialogService.ShowMessage($"Ошибка загрузки данных: {ex.Message}", "Ошибка");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task Add()
        {
            try
            {
                T newItem = Activator.CreateInstance<T>();
                var editedItem = await _dialogService.ShowEditDialog(newItem, $"Добавление: {_dictionaryName}");

                if (editedItem != null)
                {
                    var result = await _repository.AddAsync(editedItem);
                    Items.Add(result);
                    StatusMessage = $"Запись добавлена. Всего: {Items.Count}";
                    _dialogService.ShowMessage("Запись успешно добавлена", "Успех");
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage($"Ошибка при добавлении: {ex.Message}", "Ошибка");
            }
        }

        [RelayCommand]
        private async Task Edit()
        {
            if (SelectedItem == null)
            {
                 _dialogService.ShowMessage("Пожалуйста, выберите запись для редактирования", "Предупреждение");
                return;
            }

            try
            {
                var editedItem = await _dialogService.ShowEditDialog(SelectedItem, $"Редактирование: {_dictionaryName}");

                if (editedItem != null)
                {
                    await _repository.UpdateAsync(editedItem);

                    var index = Items.IndexOf(SelectedItem);
                    if (index >= 0)
                    {
                        Items[index] = editedItem;
                    }

                    StatusMessage = $"Запись обновлена. Всего: {Items.Count}";
                     _dialogService.ShowMessage("Запись успешно обновлена", "Успех");
                }
            }
            catch (Exception ex)
            {
                 _dialogService.ShowMessage($"Ошибка при редактировании: {ex.Message}", "Ошибка");
            }
        }

        [RelayCommand]
        private async Task Delete()
        {
            if (SelectedItem == null)
            {
                _dialogService.ShowMessage("Пожалуйста, выберите запись для удаления", "Предупреждение");
                return;
            }

            var propertyInfo = typeof(T).GetProperty("Name");
            var itemName = propertyInfo != null ? propertyInfo.GetValue(SelectedItem)?.ToString() : SelectedItem.ToString();

            var confirmed =  await _dialogService.ShowConfirmationDialog(
                $"Вы уверены, что хотите удалить запись '{itemName}'?",
                "Подтверждение удаления");

            if (!confirmed) return;

            try
            {
                var idProperty = typeof(T).GetProperty("Id");
                if (idProperty != null)
                {
                    var id = (Guid)idProperty.GetValue(SelectedItem)!;
                    await _repository.DeleteAsync(id);
                    Items.Remove(SelectedItem);
                    StatusMessage = $"Запись удалена. Всего: {Items.Count}";
                    _dialogService.ShowMessage("Запись успешно удалена", "Успех");
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage($"Ошибка при удалении: {ex.Message}", "Ошибка");
            }
        }
    }
}
