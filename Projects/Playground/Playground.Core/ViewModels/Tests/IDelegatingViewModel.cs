using System.Threading.Tasks;
using MvvmCross.ViewModels;

namespace Playground.Core.ViewModels.Tests
{
    public interface IDelegateViewModel : IMvxViewModel
    {
        Task<bool> HandleBackPressed();
    }

    public interface IDelegatingViewModel
    {
        IDelegateViewModel DelegateViewModel { get; set; }
    }
}
