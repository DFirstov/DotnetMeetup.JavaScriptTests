using Confluent.Kafka;

namespace SampleWebAPI.Kafka;

public class KafkaProducer
{
	private readonly string _topic;
	private readonly IProducer<string, string> _kafkaHandle;

	public KafkaProducer(KafkaClientHandle handle, IConfiguration configuration)
	{
		_topic = configuration.GetValue<string>("Kafka:Topic");
		_kafkaHandle = new DependentProducerBuilder<string, string>(handle.Handle).Build();
	}

	public Task ProduceAsync(Message<string, string> message, CancellationToken ct)
	{
		return _kafkaHandle.ProduceAsync(_topic, message, ct);
	}
}