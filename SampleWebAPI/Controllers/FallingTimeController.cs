using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleWebAPI.Clients;
using SampleWebAPI.Data;
using SampleWebAPI.Models;

namespace SampleWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FallingTimeController : ControllerBase
{
	private readonly GravityAccelerationContext _gravityAccelerationContext;
	private readonly GravityAccelerationClient _gravityAccelerationClient;

	public FallingTimeController(
		GravityAccelerationContext gravityAccelerationContext,
		GravityAccelerationClient gravityAccelerationClient)
	{
		_gravityAccelerationContext = gravityAccelerationContext;
		_gravityAccelerationClient = gravityAccelerationClient;
	}

	[HttpGet]
	public async Task<ActionResult<FallingTime>> Get(double startHeight, string? gaName, CancellationToken ct)
	{
		if (startHeight < 0)
		{
			return BadRequest("Start height must be nonnegative!");
		}

		var gravityAcceleration = gaName != null
			? await _gravityAccelerationContext.GravityAccelerations.FirstOrDefaultAsync(ga => ga.Name == gaName, ct) ??
			  await _gravityAccelerationClient.GetGravityAcceleration(gaName, ct) ??
			  GravityAcceleration.Default
			: GravityAcceleration.Default;

		return new FallingTime(gravityAcceleration, startHeight);
	}
}
