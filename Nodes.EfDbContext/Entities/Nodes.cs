using System.Collections.Generic;

namespace Nodes.EfDbContext.Entities
{
    public sealed class Nodes
    {
        public Nodes()
        {
            EdgesFromNode = new HashSet<Edges>();
            EdgesToNode = new HashSet<Edges>();
        }

        public ICollection<Edges> EdgesFromNode { get; set; }
        public ICollection<Edges> EdgesToNode { get; set; }

        public long Id { get; set; }
        public byte[] Sha256 { get; set; }
        public string Uid { get; set; }
        public string Value { get; set; }
        public byte[] Version { get; set; }
    }
}
