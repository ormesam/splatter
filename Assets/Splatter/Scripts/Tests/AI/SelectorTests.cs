using NUnit.Framework;
using Splatter.AI.BehaviourTree;

namespace Splatter.Tests {
    public class SelectorTests : TestBase {
        [Test]
        public void Selector_Success() {
            Selector selector = new Selector(Tree);
            selector.Children = new[] {
                CreateSuccessNode(),
                CreateRunningNode(),
                CreateRunningNode(),
            };

            Assert.AreEqual(NodeResult.Success, selector.Execute());

            selector.Children = new[] {
                CreateFailureNode(),
                CreateSuccessNode(),
                CreateRunningNode(),
            };

            Assert.AreEqual(NodeResult.Running, selector.Execute());
            Assert.AreEqual(NodeResult.Success, selector.Execute());

            selector.Children = new[] {
                CreateFailureNode(),
                CreateFailureNode(),
                CreateSuccessNode(),
            };

            Assert.AreEqual(NodeResult.Running, selector.Execute());
            Assert.AreEqual(NodeResult.Running, selector.Execute());
            Assert.AreEqual(NodeResult.Success, selector.Execute());
        }

        [Test]
        public void Selector_Failed() {
            Selector selector = new Selector(Tree);
            selector.Children = new[] {
                CreateFailureNode(),
                CreateFailureNode(),
                CreateFailureNode(),
            };

            Assert.AreEqual(NodeResult.Running, selector.Execute());
            Assert.AreEqual(NodeResult.Running, selector.Execute());
            Assert.AreEqual(NodeResult.Failure, selector.Execute());
        }

        [Test]
        public void Selector_Running() {
            Selector selector = new Selector(Tree);
            selector.Children = new[] {
                CreateRunningNode(),
                CreateRunningNode(),
                CreateRunningNode(),
            };

            Assert.AreEqual(NodeResult.Running, selector.Execute());
            Assert.AreEqual(NodeResult.Running, selector.Execute());
            Assert.AreEqual(NodeResult.Running, selector.Execute());
        }

        [Test]
        public void Selector_Cancel_Self() {
            bool shouldCancel = false;

            Selector selector = new Selector(Tree, CompositeCancelType.Self, () => shouldCancel);
            selector.Children = new[] {
                CreateFailureNode(),
                CreateFailureNode(),
                CreateSuccessNode(),
            };

            selector.Execute();
            selector.Execute();
            shouldCancel = true;
            selector.Execute();

            Assert.AreEqual(1, selector.GetCurrentIndex());
        }

        [Test]
        public void Selector_Cancel_Lower() {
            bool shouldCancel = false;

            Selector selector = new Selector(Tree);
            Selector childSelector = new Selector(Tree, CompositeCancelType.Lower, () => shouldCancel);

            childSelector.Children = new[] {
                CreateFailureNode(),
                CreateFailureNode(),
                CreateFailureNode(),
            };

            selector.Children = new[] {
                childSelector,
                CreateFailureNode(),
                CreateSuccessNode(),
            };

            selector.Execute();
            selector.Execute();
            selector.Execute();
            selector.Execute();
            selector.Execute();
            shouldCancel = true;
            selector.Execute();

            Assert.AreEqual(0, selector.GetCurrentIndex());
        }
    }
}