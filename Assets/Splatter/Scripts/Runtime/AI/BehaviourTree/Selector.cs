using System;

namespace Splatter.AI.BehaviourTree {
    public class Selector : Composite {
        private int currentNode = 0;

        public Selector(BehaviourTree tree, AbortType abortType = AbortType.None, Func<bool> condition = null)
            : base(tree, abortType, condition) {
        }

        public override NodeResult Execute() {
            if (CanAbortSelf && !Condition()) {
                return NodeResult.Failure;
            }

            // Check previous nodes conditions
            for (int i = 0; i < currentNode; i++) {
                if (CanHigherPriorityNodeInterrupt(Children[i] as Composite)) {
                    currentNode = i;

                    break;
                }
            }

            if (currentNode < Children.Count) {
                var result = Children[currentNode].Execute();

                if (result == NodeResult.Running) {
                    return NodeResult.Running;
                } else if (result == NodeResult.Success) {
                    currentNode = 0;
                    return NodeResult.Success;
                } else {
                    currentNode++;

                    if (currentNode < Children.Count) {
                        return NodeResult.Running;
                    } else {
                        currentNode = 0;
                        return NodeResult.Failure;
                    }
                }
            }

            return NodeResult.Failure;
        }

#if UNITY_INCLUDE_TESTS
        // Useful for debugging tests
        public int CurrentIndex => currentNode;
#endif
    }
}
