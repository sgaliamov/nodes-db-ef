namespace Nodes.EfDbContext.Entities
{
    public sealed class Edges
    {
        public Nodes FromNode { get; set; }
        public long FromNodeId { get; set; }
        public long Id { get; set; }
        public Nodes ToNode { get; set; }
        public long ToNodeId { get; set; }
        public string Value { get; set; }
        public byte[] Version { get; set; }
    }
}
