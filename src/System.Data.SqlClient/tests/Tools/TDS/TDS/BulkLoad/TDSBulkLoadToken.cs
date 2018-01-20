using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TDS.RPC;

namespace Microsoft.SqlServer.TDS.BulkLoad
{
    public class TDSBulkLoadToken : TDSPacketToken
    {
        public List<TDSColMetaData> Columns { get; set; }

        public TDSBulkLoadToken()
        {
            Columns = new List<TDSColMetaData>();
        }

        /// <summary>
        /// Inflating constructor
        /// </summary>
        public TDSBulkLoadToken(Stream source)
        {
            Columns = new List<TDSColMetaData>();
            Inflate(source);
        }

        public override bool Inflate(Stream source)
        {
            //See: TdsParser.WriteBulkCopyMetaData

            //SQLCOLMETADATA
            source.ReadByte();

            var count = TDSUtilities.ReadShort(source);
            for (int i = 0; i < count; i++)
            {
                var metadata = new TDSColMetaData();
                metadata.State = TDSUtilities.ReadInt(source);
                metadata.Flags = TDSUtilities.ReadShort(source);

                var type = source.ReadByte();
                switch ((TDSDataType)type)
                {
                    case TDSDataType.Bit:
                    case TDSDataType.Int1:
                    {
                        metadata.Length = source.ReadByte();
                        break;
                    }
                    case TDSDataType.Int2:
                    {
                        metadata.Length = TDSUtilities.ReadShort(source);
                        break;
                    }
                    case TDSDataType.Int4:
                    {
                        metadata.Length = TDSUtilities.ReadInt(source);
                        break;
                    }
                    case TDSDataType.NVarChar:
                    {
                        metadata.Length = TDSUtilities.ReadShort(source);
                        metadata.Collation = TDSUtilities.ReadUInt(source);
                        metadata.SortId = source.ReadByte();
                        break;
                    }
                    default:
                        throw new NotImplementedException("Can't parse type " + (TDSDataType)type);
                }

                var columnNameLength = source.ReadByte();
                metadata.ColumnName = TDSUtilities.ReadString(source, (ushort)(columnNameLength * 2));
                Columns.Add(metadata);
            }

            //SQL done
            source.ReadByte();

            source.ReadByte();
            source.ReadByte();

            source.ReadByte();
            source.ReadByte();

            source.ReadByte();
            source.ReadByte();
            source.ReadByte();
            source.ReadByte();

            return true;
        }

        public override void Deflate(Stream destination)
        {
            throw new NotImplementedException();
        }
    }
}
