using System.Windows;
using System.Windows.Automation.Peers;

namespace AutomationPeerHelpers
{
    public interface IAutomationPeerFactory
    {
        AutomationPeer CreatePeer(DependencyObject dependencyObject);
    }
}
