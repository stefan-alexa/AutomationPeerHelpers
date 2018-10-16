using System;
using System.Windows;
using System.Windows.Automation.Peers;

namespace AutomationPeerHelpers
{
    internal class UwpAutomationPeerFactory : IAutomationPeerFactory
    {
        public AutomationPeer CreatePeer(DependencyObject dependencyObject)
        {
            throw new NotImplementedException();
        }
    }
}
