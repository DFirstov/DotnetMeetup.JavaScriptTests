using Confluent.Kafka;
using SampleWebAPI.Clients;
using SampleWebAPI.Models;

namespace SampleWebAPI.Kafka;

public class KafkaConsumer : BackgroundService
{
	private readonly string _topic;
	private readonly IConsumer<string, string> _kafkaConsumer;
	private readonly GravityAccelerationClient _gravityAccelerationClient;

	public KafkaConsumer(IConfiguration configuration, GravityAccelerationClient gravityAccelerationClient)
	{
		ConsumerConfig consumerConfig = new();
		configuration.GetSection("Kafka:ConsumerSettings").Bind(consumerConfig);
		_topic = configuration.GetValue<string>("Kafka:Topic");
		_kafkaConsumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
		_gravityAccelerationClient = gravityAccelerationClient;
	}

	protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		return Task.Run(() => StartConsumerLoop(stoppingToken), stoppingToken);
	}

	private async Task StartConsumerLoop(CancellationToken stoppingToken)
	{
		_kafkaConsumer.Subscribe(_topic);

		while (!stoppingToken.IsCancellationRequested)
		{
			try
			{
				var consumeResult = _kafkaConsumer.Consume(stoppingToken);

				GravityAcceleration gravityAcceleration = new(
					consumeResult.Message.Key,
					double.Parse(consumeResult.Message.Value));

				await _gravityAccelerationClient.PostGravityAcceleration(gravityAcceleration, stoppingToken);

				_kafkaConsumer.Commit(consumeResult);
			}
			catch (OperationCanceledException)
			{
				break;
			}
		}
	}

	public override void Dispose()
	{
		_kafkaConsumer.Close();
		_kafkaConsumer.Dispose();
		
		base.Dispose();
	}
}