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
	public async Task<ActionResult> PostGravityAcceleration(GravityAcceleration ga)
	{
		_gaContext.GravityAccelerations.Add(ga);
		await _gaContext.SaveChangesAsync();

		await _kafkaProducer.ProduceAsync(new Message<string, string>
		{
			Key = ga.Name,
			Value = ga.Value.ToString(CultureInfo.InvariantCulture)
		});

		return Ok();
	}
}