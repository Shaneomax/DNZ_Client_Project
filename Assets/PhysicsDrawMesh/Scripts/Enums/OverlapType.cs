namespace DrawMesh
{
    /// <summary>
    /// How to deal with obstacles when drawing
    /// </summary>
    public enum OverlapType
    {
        /// <summary>
        /// Do not handle
        /// </summary>
        None,

        /// <summary>
        /// Follow the edge
        /// </summary>
        FollowEdge,

        /// <summary>
        /// Disconnect at obstacles
        /// </summary>
        CutOff,
    }
}
