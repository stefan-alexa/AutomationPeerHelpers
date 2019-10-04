using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Media;

namespace AutomationPeerHelpers
{
    public class BaseElementAutomationPeer : UIElementAutomationPeer
    {
        private const int DefaultSearchDepth = 10;

        public BaseElementAutomationPeer(
            UIElement owner, 
            string className, 
            AutomationControlType automationControlType = AutomationControlType.Custom,
            int searchDepth = DefaultSearchDepth,
            XamlFramework xamlFramework = XamlFramework.WPF) : base(owner)
        {
            this.ClassName = className;
            this.AutomationControlType = automationControlType;
            this.SearchDepth = searchDepth;
            this.AutomationPeerFactory = AutomationPeerAbstractFactory.Create(xamlFramework);
        }
        public string ClassName { get; set; }

        public AutomationControlType AutomationControlType { get; set; }

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

        protected override string GetClassNameCore()
        {
            return ClassName;
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType;
        }

        protected override string GetAutomationIdCore()
        {
            var automationId = AutomationProperties.GetAutomationId(Owner);
            if (!string.IsNullOrEmpty(automationId))
            {
                return automationId;
            }

            if (Owner is FrameworkElement frameworkElement)
            {
                return frameworkElement.Name;
            }

            return base.GetAutomationIdCore();
        }
    }
}
