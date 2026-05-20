using System.Threading.Tasks;
using System.Windows;
using storage_tracker.Views;

namespace storage_tracker.Services
{

    public class DialogService : IDialogService
    {
        public async Task<bool> ShowConfirmationDialog(string message, string title = "Подтверждение")
        {
            var result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return await Task.FromResult(result == MessageBoxResult.Yes);
        }

        public void ShowMessage(string message, string title = "Информация")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public async Task<T?> ShowEditDialog<T>(T item, string title) where T : class
        {
            var dialog = new ItemEditDialog();
            var viewModel = new ViewModels.ItemEditViewModel<T>(item, this);
            dialog.DataContext = viewModel;
            dialog.Title = title;

            var result = dialog.ShowDialog();

            if (result == true)
            {
                return await Task.FromResult(viewModel.EditedItem);
            }

            return await Task.FromResult<T?>(null);
        }
    }
}
