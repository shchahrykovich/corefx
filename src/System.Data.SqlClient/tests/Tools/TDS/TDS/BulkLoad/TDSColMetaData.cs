using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.SqlServer.TDS.BulkLoad
{
    public class TDSColMetaData
    {
        public int State { get; set; }
        public short Flags { get; set; }
        public uint Collation { get; set; }
        public int SortId { get; set; }
        public int Length { get; set; }
        public string ColumnName { get; set; }
    }
}
