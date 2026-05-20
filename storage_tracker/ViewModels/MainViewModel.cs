using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using storage_tracker.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;


namespace storage_tracker.ViewModels
{


    public partial class MainViewModel : ObservableObject
    {
        private readonly IDialogService _dialogService;
        private readonly IServiceProvider _serviceProvider;

        [ObservableProperty]
        private string _studentInfo = "Шапиро Марина Михайловна | 3 курс | Группа 2 | 2026 год";

        [ObservableProperty]
        private ObservableCollection<DictionaryInfo> _dictionaries = new();

        [ObservableProperty]
        private DictionaryInfo? _selectedDictionary;

        [ObservableProperty]
        private object? _currentGridView;

        public MainViewModel(IDialogService dialogService, IServiceProvider serviceProvider)
        {
            _dialogService = dialogService;
            _serviceProvider = serviceProvider;

            InitializeDictionaries();
        }

        private void InitializeDictionaries()
        {
            Dictionaries.Add(new DictionaryInfo
            {
                Name = "Категории",
                Type = typeof(Models.Category),
                RepositoryType = typeof(CategoryRepository)
            });
            Dictionaries.Add(new DictionaryInfo
            {
                Name = "Коробки",
                Type = typeof(Models.Box),
                RepositoryType = typeof(BoxRepository)
            });
            Dictionaries.Add(new DictionaryInfo
            {
                Name = "Предметы",
                Type = typeof(Models.Item),
                RepositoryType = typeof(ItemRepository)
            });
        }

        partial void OnSelectedDictionaryChanged(DictionaryInfo? value)
        {
            if (value != null)
            {
                LoadDictionaryGrid(value);
            }
        }

        private async void LoadDictionaryGrid(DictionaryInfo dictionary)
        {
            try
            {
                var genericType = typeof(DictionaryGridViewModel<>).MakeGenericType(dictionary.Type);
                var vm = Activator.CreateInstance(genericType, _serviceProvider, _dialogService, dictionary.Name);

                if (vm != null)
                {
                    var loadMethod = genericType.GetMethod("LoadDataAsync");
                    if (loadMethod != null)
                    {
                        await (Task)loadMethod.Invoke(vm, null);
                    }

                    // Создаем View и устанавливаем DataContext
                    var view = new Views.DictionaryGridView();
                    view.DataContext = vm;
                    CurrentGridView = view;  // Теперь присваиваем View, а не ViewModel
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage($"Ошибка загрузки справочника: {ex.Message}", "Ошибка");
            }
        }
    }

    public class DictionaryInfo
    {
        public string Name { get; set; } = string.Empty;
        public Type Type { get; set; } = typeof(object);
        public Type RepositoryType { get; set; } = typeof(object);

        public override string ToString() => Name;
    }
}
