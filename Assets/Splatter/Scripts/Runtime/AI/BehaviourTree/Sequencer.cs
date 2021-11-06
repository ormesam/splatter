namespace Splatter.AI.BehaviourTree {
    // Essentially an AND gate
    public class Sequencer : Composite {
        private readonly bool resetIfInterrupted;
        private int currentNode = 0;
        private int lastRanOnTick = 0;

        public Sequencer(BehaviourTree tree, bool resetIfInterrupted) : base(tree) {
            this.resetIfInterrupted = resetIfInterrupted;
        }

        public override NodeResult Execute() {
            if (resetIfInterrupted && lastRanOnTick != Tree.Ticks - 1) {
                currentNode = 0;
            }

            lastRanOnTick = Tree.Ticks;

            if (currentNode < Children.Count) {
                var result = Children[currentNode].Execute();

                if (result == NodeResult.Running) {
                    return NodeResult.Running;
                } else if (result == NodeResult.Failure) {
                    currentNode = 0;
                    return NodeResult.Failure;
                } else {
                    currentNode++;

                    if (currentNode < Children.Count) {
                        return NodeResult.Running;
                    } else {
                        currentNode = 0;
                        return NodeResult.Success;
                    }
                }
            }

            return NodeResult.Success;
        }
    }
}