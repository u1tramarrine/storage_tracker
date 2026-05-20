using System.Threading.Tasks;

namespace storage_tracker.Services
{


    public interface IDialogService
    {
        Task<bool> ShowConfirmationDialog(string message, string title = "Подтверждение");
        void ShowMessage(string message, string title = "Информация");
        Task<T?> ShowEditDialog<T>(T item, string title) where T : class;
    }
}
