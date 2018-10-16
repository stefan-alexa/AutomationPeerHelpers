using System;

namespace AutomationPeerHelpers
{
    public class AutomationPeerAbstractFactory
    {
        public static IAutomationPeerFactory Create(XamlFramework xamlFramework)
        {
            switch (xamlFramework)
            {
                case XamlFramework.WPF: return new WpfAutomationPeerFactory();
                case XamlFramework.UWP: return new UwpAutomationPeerFactory();

                default: throw new InvalidOperationException($"There is no AutomationPeerFactory implemented for {xamlFramework}");
            }
        }
    }
}
