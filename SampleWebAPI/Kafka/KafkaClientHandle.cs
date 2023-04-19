using Confluent.Kafka;

namespace SampleWebAPI.Kafka;

public class KafkaClientHandle : IDisposable
{
	private readonly IProducer<byte[], byte[]> _producer;

	public KafkaClientHandle(IConfiguration configuration)
	{
		ProducerConfig producerConfig = new();
		configuration.GetSection("Kafka:ProducerSettings").Bind(producerConfig);
		_producer = new ProducerBuilder<byte[], byte[]>(producerConfig).Build();
	}

	public Handle Handle => _producer.Handle;

	public void Dispose()
	{
		_producer.Flush();
		_producer.Dispose();
	}
}