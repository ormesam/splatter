using NUnit.Framework;
using Splatter.AI.BehaviourTree;

namespace Splatter.Tests {
    public class ParallelWaitForAllToComplete : TestBase {
        [Test]
        public void Parallel_Success() {
            Parallel parallel = new Parallel(Tree, ParallelMode.WaitForAll);
            parallel.Children = new[]{
                CreateSuccessNode(),
                CreateSuccessNode(),
                CreateSuccessNode(),
            };

            Assert.AreEqual(NodeResult.Success, parallel.Execute());
        }

        [Test]
        public void Parallel_Failure() {
            Parallel parallel = new Parallel(Tree, ParallelMode.WaitForAll);
            parallel.Children = new[]{
                CreateFailureNode(),
                CreateFailureNode(),
                CreateFailureNode(),
            };

            Assert.AreEqual(NodeResult.Success, parallel.Execute());
        }

        [Test]
        public void Parallel_Running() {
            Parallel parallel = new Parallel(Tree, ParallelMode.WaitForAll);
            parallel.Children = new[]{
                CreateSuccessNode(),
                CreateRunningNode(),
                CreateFailureNode(),
            };

            Assert.AreEqual(NodeResult.Running, parallel.Execute());
        }
    }
}