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
	private readonly GravityAccelerationContext _gravityAccelerationContext;
	private readonly KafkaProducer _kafkaProducer;

	public GravityAccelerationController(
		GravityAccelerationContext gravityAccelerationContext,
		KafkaProducer kafkaProducer)
	{
		_gravityAccelerationContext = gravityAccelerationContext;
		_kafkaProducer = kafkaProducer;
	}

	[HttpPost]
	public async Task<ActionResult> PostGravityAcceleration(GravityAcceleration gravityAcceleration)
	{
		_gravityAccelerationContext.GravityAccelerations.Add(gravityAcceleration);
		await _gravityAccelerationContext.SaveChangesAsync();

		await _kafkaProducer.ProduceAsync(new Message<string, string>()
		{
			Key = gravityAcceleration.Name,
			Value = gravityAcceleration.Value.ToString(CultureInfo.InvariantCulture)
		});

		return Ok();
	}
}