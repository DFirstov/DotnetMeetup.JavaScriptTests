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
	private readonly GravityAccelerationContext _gaContext;
	private readonly GravityAccelerationClient _gaClient;

	public FallingTimeController(GravityAccelerationContext gaContext, GravityAccelerationClient gaClient)
	{
		_gaContext = gaContext;
		_gaClient = gaClient;
	}

	[HttpGet]
	public async Task<ActionResult<FallingTime>> Get(double startHeight, string? gaName, CancellationToken ct)
	{
		if (startHeight < 0)
		{
			return BadRequest("Start height must be nonnegative!");
		}

		var gravityAcceleration = gaName != null
			? await GetGravityAcceleration(gaName, ct)
			: GravityAcceleration.Default;

		return new FallingTime(gravityAcceleration, startHeight);
	}

	private async Task<GravityAcceleration> GetGravityAcceleration(string gaName, CancellationToken ct) =>
		await _gaContext.GravityAccelerations.FirstOrDefaultAsync(ga => ga.Name == gaName, ct) ??
		await _gaClient.GetGravityAcceleration(gaName, ct) ??
		GravityAcceleration.Default;
}
