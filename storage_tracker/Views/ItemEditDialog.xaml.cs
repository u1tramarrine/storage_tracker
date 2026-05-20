using System;
using System.Linq;
using System.Windows;
using storage_tracker.Models;
using storage_tracker.Services;
using Microsoft.Extensions.DependencyInjection;

namespace storage_tracker.Views
{
   

    public partial class ItemEditDialog : Window
    {
        private IServiceProvider? _serviceProvider;
        private IEnumerable<Box>? _boxes;
        private IEnumerable<Category>? _categories;

        public ItemEditDialog()
        {
            InitializeComponent();
            Loaded += ItemEditDialog_Loaded;
            
        }

        private async void ItemEditDialog_Loaded(object sender, RoutedEventArgs e)
        {
            // Получаем ServiceProvider из App
            _serviceProvider = (App.Current as App)?.ServiceProvider;

            // Определяем тип редактируемого объекта и показываем соответствующую панель
            if (DataContext is ViewModels.ItemEditViewModel<Category> categoryModel)
            {
                CategoryPanel.Visibility = Visibility.Visible;
                BoxPanel.Visibility = Visibility.Collapsed;
                ItemPanel.Visibility = Visibility.Collapsed;
                categoryModel.Header = "Редактирование категории";
            }
            else if (DataContext is ViewModels.ItemEditViewModel<Box> boxModel)
            {
                CategoryPanel.Visibility = Visibility.Collapsed;
                BoxPanel.Visibility = Visibility.Visible;
                ItemPanel.Visibility = Visibility.Collapsed;
                boxModel.Header = "Редактирование коробки";
                await LoadComboBoxData();
            }
            else if (DataContext is ViewModels.ItemEditViewModel<Item> itemModel)
            {
                CategoryPanel.Visibility = Visibility.Collapsed;
                BoxPanel.Visibility = Visibility.Collapsed;
                ItemPanel.Visibility = Visibility.Visible;
                itemModel.Header = "Редактирование предмета";

                // Загружаем данные для выпадающих списков
                await LoadComboBoxData();
            }

            // Устанавливаем максимальную дату для DatePicker (только для Item)
            if (CreatedAtDatePicker != null && ItemPanel.Visibility == Visibility.Visible)
            {
                CreatedAtDatePicker.DisplayDateEnd = DateTimeOffset.Now.Date;
            }
        }

        private async Task LoadComboBoxData()
        {
            try
            {
                if (_serviceProvider == null)
                {
                    System.Diagnostics.Debug.WriteLine("ServiceProvider is null");
                    return;
                }

                // Загружаем коробки для BoxComboBox
                var boxRepository = _serviceProvider.GetRequiredService<IRepository<Box>>();
                _boxes = await boxRepository.GetAllAsync();

                // Устанавливаем ItemsSource для BoxComboBox
                BoxComboBox.ItemsSource = _boxes;
                BoxComboBox.DisplayMemberPath = "Name";
                BoxComboBox.SelectedValuePath = "Id";

                System.Diagnostics.Debug.WriteLine($"Загружено коробок: {_boxes.Count()}");

                // Загружаем категории для CategoryComboBox
                var categoryRepository = _serviceProvider.GetRequiredService<IRepository<Category>>();
                _categories = await categoryRepository.GetAllAsync();

                // Устанавливаем ItemsSource для CategoryInItemComboBox
                CategoryInItemComboBox.ItemsSource = _categories;
                CategoryInItemComboBox.DisplayMemberPath = "Name";
                CategoryInItemComboBox.SelectedValuePath = "Id";

                CategoryInBoxComboBox.ItemsSource = _categories;
                CategoryInBoxComboBox.DisplayMemberPath = "Name";
                CategoryInBoxComboBox.SelectedValuePath = "Id";
                System.Diagnostics.Debug.WriteLine($"Загружено категорий: {_categories.Count()}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
