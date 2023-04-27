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

	[HttpPost]
	public async Task<ActionResult> PostGravityAcceleration(GravityAcceleration ga)
	{
		await SaveGravityAcceleration(ga);
		await ProduceMessage(ga);

		return Ok();
	}

	private Task SaveGravityAcceleration(GravityAcceleration ga)
	{
		_gaContext.GravityAccelerations.Add(ga);
		return _gaContext.SaveChangesAsync();
	}

	private Task ProduceMessage(GravityAcceleration ga)
	{
		return _kafkaProducer.ProduceAsync(
			new Message<string, string>
			{
				Key = ga.Name,
				Value = ga.Value.ToString(CultureInfo.InvariantCulture)
			});
	}

	public GravityAccelerationController(GravityAccelerationContext gaContext, KafkaProducer kafkaProducer)
	{
		_gaContext = gaContext;
		_kafkaProducer = kafkaProducer;
	}
}