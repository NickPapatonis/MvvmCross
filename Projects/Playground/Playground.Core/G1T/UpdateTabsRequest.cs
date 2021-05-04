namespace Playground.Core.G1T
{
    public class UpdateTabsRequest
    {
        public bool ReloadExistingTabs { get; set; }

        public UpdateTabsRequest(bool reloadExistingTabs = true)
        {
            ReloadExistingTabs = reloadExistingTabs;
        }
    }
}
