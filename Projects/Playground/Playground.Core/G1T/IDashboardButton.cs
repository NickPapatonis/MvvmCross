using System.Windows.Input;

namespace Playground.Core.G1T
{
    public interface IDashboardButtonViewModel : IHasIcon
    {
        string Text { get; }
        ICommand Navigate { get; }
        bool IsVisible { get; }
        bool IsClickable { get; }
    }

    public interface IDashboardButtonRowViewModel
    {
        IDashboardButtonViewModel Button0 { get; }
        IDashboardButtonViewModel Button1 { get; }
        IDashboardButtonViewModel Button2 { get; }
    }

    public interface IHasIcon
    {
        string IconName { get; }
    }
}
