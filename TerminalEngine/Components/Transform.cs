namespace TerminalEngine.Components {

    public struct Transform : IGameComponent {

        public int LocalLeft;
        public int LocalTop;
        public int LocalRotation;


        public Transform(int localLeft, int localTop) : this(localLeft, localTop, 0) { }
        public Transform(int localLeft, int localTop, int localRotation) {
            LocalLeft = localLeft;
            LocalTop = localTop;
            LocalRotation = localRotation;
        }

    }

}
