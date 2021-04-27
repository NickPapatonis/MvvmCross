using System.Collections.Generic;

namespace Playground.Core.G1T
{
    /// <summary>
    /// This class is used by Mvvm Cross Expandable List View
    /// Each dashboard subsection (under a header) is made up of rows of buttons
    /// Each row contains 3 buttons across
    /// </summary>

    public interface IDashboardSectionViewModel : IList<IDashboardButtonRowViewModel>, IHasIcon
    {
        string HeaderName { get; }
    }

    public class DashboardSectionViewModel : List<IDashboardButtonRowViewModel>, IDashboardSectionViewModel
    {
        public string HeaderName { get; set; } = string.Empty;
        public string IconName { get; set; } = string.Empty;
        public DashboardSectionViewModel(IEnumerable<IDashboardButtonRowViewModel> collection) : base(collection)
        {
        }
    }
}
