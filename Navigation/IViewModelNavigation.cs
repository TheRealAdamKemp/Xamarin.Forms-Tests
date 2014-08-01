using System.Threading.Tasks;

namespace Navigation
{
    public interface IViewModelNavigation
    {
        Task<object> PopAsync();

        Task<object> PopModalAsync();

        Task PopToRootAsync();

        Task PushAsync(object viewModel);

        Task PushModalAsync(object viewModel);
    }
}

