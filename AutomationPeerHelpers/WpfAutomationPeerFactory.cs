using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Documents;

namespace AutomationPeerHelpers
{
    internal class WpfAutomationPeerFactory : IAutomationPeerFactory
    {
        public AutomationPeer CreatePeer(DependencyObject dependencyObject)
        {
            switch (dependencyObject)
            {
                case UIElement uiElement: return UIElementAutomationPeer.CreatePeerForElement(uiElement);

                // These are not UiElements therefore we must create their corresponding automation peer manually
                case Hyperlink hyperlink: return new HyperlinkAutomationPeer(hyperlink);
                case Table table: return new TableAutomationPeer(table);
                case TableCell tableCell: return new TableCellAutomationPeer(tableCell);
                case TextElement textElement: return new TextElementAutomationPeer(textElement);
                case FrameworkContentElement frameworkContentElement: return new FrameworkContentElementAutomationPeer(frameworkContentElement);
                case ContentElement contentElement: return new ContentElementAutomationPeer(contentElement);
            }

            return null;
        }
    }
}
