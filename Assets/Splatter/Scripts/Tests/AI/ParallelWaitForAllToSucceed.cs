using NUnit.Framework;
using Splatter.AI.BehaviourTree;

namespace Splatter.Tests {
    public class ParallelWaitForAllToSucceed : TestBase {
        [Test]
        public void Parallel_Success() {
            Parallel parallel = new Parallel(Tree, ParallelMode.WaitForAllSuccess);
            parallel.Children = new[]{
                CreateSuccessNode(),
                CreateSuccessNode(),
                CreateSuccessNode(),
            };

            Assert.AreEqual(NodeResult.Success, parallel.Execute());
        }

        [Test]
        public void Parallel_Failure() {
            Parallel parallel = new Parallel(Tree, ParallelMode.WaitForAllSuccess);
            parallel.Children = new[]{
                CreateSuccessNode(),
                CreateSuccessNode(),
                CreateFailureNode(),
            };

            Assert.AreEqual(NodeResult.Failure, parallel.Execute());
        }

        [Test]
        public void Parallel_Running() {
            Parallel parallel = new Parallel(Tree, ParallelMode.WaitForAllSuccess);
            parallel.Children = new[]{
                CreateSuccessNode(),
                CreateRunningNode(),
                CreateSuccessNode(),
            };

            Assert.AreEqual(NodeResult.Running, parallel.Execute());
        }
    }
}