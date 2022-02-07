namespace Splatter.AI.BehaviourTree {
    /// <summary>
    /// Base class for all nodes on a behaviour tree.
    /// </summary>
    public abstract class Node {
        private int lastExecutedTick;

        /// <summary>
        /// Node name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Behaviour tree this node is on.
        /// </summary>
        protected BehaviourTree Tree { get; private set; }

#if UNITY_EDITOR
        public bool ExecutedLastTick => Tree.Ticks == lastExecutedTick + 1;
        public NodeResult LastResult { get; private set; }
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="tree">Behaviour tree this node is on.</param>
        public Node(string name, BehaviourTree tree) {
            this.Name = name;

            Tree = tree;
        }

        /// <summary>
        /// Behaviour of the node.
        /// </summary>
        /// <returns>The result of the execution.</returns>
        public NodeResult Execute() {
            var result = ExecuteNode();

            lastExecutedTick = Tree.Ticks;
            LastResult = result;

            return result;
        }

        /// <summary>
        /// Behaviour of the node.
        /// </summary>
        /// <returns>The result of the execution.</returns>
        protected abstract NodeResult ExecuteNode();
    }
}
