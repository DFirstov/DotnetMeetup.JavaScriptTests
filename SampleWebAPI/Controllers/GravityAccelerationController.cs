using System.Globalization;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using SampleWebAPI.Data;
using SampleWebAPI.Kafka;
using SampleWebAPI.Models;

namespace SampleWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class GravityAccelerationController : ControllerBase
{
	private readonly GravityAccelerationContext _gaContext;
	private readonly KafkaProducer _kafkaProducer;

	public GravityAccelerationController(GravityAccelerationContext gaContext, KafkaProducer kafkaProducer)
	{
		_gaContext = gaContext;
		_kafkaProducer = kafkaProducer;
	}

	[HttpPost]
	public async Task<ActionResult> PostGravityAcceleration(GravityAcceleration ga, CancellationToken ct)
	{
		await SaveGravityAcceleration(ga, ct);
		await ProduceMessage(ga, ct);

		return Ok();
	}

	private Task SaveGravityAcceleration(GravityAcceleration ga, CancellationToken ct)
	{
		_gaContext.GravityAccelerations.Add(ga);
		return _gaContext.SaveChangesAsync(ct);
	}

	private Task ProduceMessage(GravityAcceleration ga, CancellationToken ct)
	{
		return _kafkaProducer.ProduceAsync(
			new Message<string, string>
			{
				Key = ga.Name,
				Value = ga.Value.ToString(CultureInfo.InvariantCulture)
			},
			ct);
	}
}
