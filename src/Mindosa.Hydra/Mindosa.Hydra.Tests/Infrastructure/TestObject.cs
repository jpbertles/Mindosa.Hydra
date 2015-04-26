using Mindosa.Hydra.Attributes;
using Microsoft.SqlServer.Types;

namespace Mindosa.Hydra.Tests.Infrastructure
{
    public class TestObject
    {
        public string Name { get; set; }
        //[ComputedColumn]
        public int Comp { get; set; }
        [CustomMapping("Id", typeof(IdMapper))]
        public string StringId { get; set; }
        //[CustomMapping("Location", typeof(GeographyMapper))]
        public SqlGeography Location { get; set; }
    }
}
