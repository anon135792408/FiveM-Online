using System;
using System.Threading.Tasks;
using MySqlConnector.Utilities;

namespace MySqlConnector.Protocol.Serialization
{
	internal sealed class StandardPayloadHandler : IPayloadHandler
	{
		public StandardPayloadHandler(IByteHandler byteHandler)
		{
			ByteHandler = byteHandler;
			m_getNextSequenceNumber = () => m_sequenceNumber++;
		}

		public void Dispose()
		{
			m_bufferedByteReader = null;
			Utility.Dispose(ref m_byteHandler);
		}

		public void StartNewConversation()
		{
			m_sequenceNumber = 0;
		}

		public IByteHandler ByteHandler
		{
			get => m_byteHandler;
			set
			{
				m_byteHandler = value ?? throw new ArgumentNullException(nameof(value));
				m_bufferedByteReader = new BufferedByteReader();
			}
		}

		public ValueTask<ArraySegment<byte>> ReadPayloadAsync(ArraySegmentHolder<byte> cache, ProtocolErrorBehavior protocolErrorBehavior, IOBehavior ioBehavior) =>
			ProtocolUtility.ReadPayloadAsync(m_bufferedByteReader, m_byteHandler, m_getNextSequenceNumber, cache, protocolErrorBehavior, ioBehavior);

		public ValueTask<int> WritePayloadAsync(ArraySegment<byte> payload, IOBehavior ioBehavior) =>
			ProtocolUtility.WritePayloadAsync(m_byteHandler, m_getNextSequenceNumber, payload, ioBehavior);

		readonly Func<int> m_getNextSequenceNumber;
		IByteHandler m_byteHandler;
		BufferedByteReader m_bufferedByteReader;
		int m_sequenceNumber;
	}
}
