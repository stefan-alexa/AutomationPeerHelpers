using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Media;

namespace AutomationPeerHelpers
{
    public class BaseElementAutomationPeer : UIElementAutomationPeer
    {
        private const int DefaultSearchDepth = 10;

        public BaseElementAutomationPeer(UIElement owner) : this(owner, DefaultSearchDepth, XamlFramework.WPF)
        {
        }

        public BaseElementAutomationPeer(UIElement owner, int searchDepth) : this(owner, searchDepth, XamlFramework.WPF)
        {
        }

        public BaseElementAutomationPeer(UIElement owner, XamlFramework xamlFramework) : this(owner, DefaultSearchDepth, xamlFramework)
        {

        }

        public BaseElementAutomationPeer(UIElement owner, int searchDepth, XamlFramework xamlFramework) : base(owner)
        {
            this.SearchDepth = searchDepth;
            this.AutomationPeerFactory = AutomationPeerAbstractFactory.Create(xamlFramework);
        }

        public IAutomationPeerFactory AutomationPeerFactory { get; protected set; }

        public int SearchDepth { get; protected set; }

        protected override List<AutomationPeer> GetChildrenCore()
        {
            var peers = base.GetChildrenCore() ?? new List<AutomationPeer>();

            peers.AddRange(this.GetChildPeers(this.Owner, this.SearchDepth));

            return peers;
        }

        protected IEnumerable<AutomationPeer> GetChildPeers(DependencyObject element, int searchDepth)
        {
            var automationPeers = new List<AutomationPeer>();
            var queue = new Queue<Tuple<DependencyObject, int>>();
            queue.Enqueue(new Tuple<DependencyObject, int>(element, 0));

            while (queue.Count != 0)
            {
                var currentElement = queue.Dequeue();
                if (currentElement.Item2 <= searchDepth)
                {
                    var parentElement = currentElement.Item1;
                    var currentDepth = currentElement.Item2 + 1;
                    for (var index = 0; index < VisualTreeHelper.GetChildrenCount(currentElement.Item1); index++)
                    {
                        var child = VisualTreeHelper.GetChild(parentElement, index);
                        var peer = this.AutomationPeerFactory.CreatePeer(child);
                        if (peer != null)
                        {
                            automationPeers.Add(peer);
                        }
                        else
                        {
                            queue.Enqueue(new Tuple<DependencyObject, int>(child, currentDepth));
                        }
                    }
                }
            }

            return automationPeers;
        }
    }
}
