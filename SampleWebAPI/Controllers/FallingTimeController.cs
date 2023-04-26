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

	[HttpGet]
	public async Task<ActionResult<FallingTime>> Get(double startHeight, string? gaName)
	{
		if (startHeight < 0)
		{
			return BadRequest("Start height must be nonnegative!");
		}

		var gravityAcceleration = gaName != null
			? await GetGravityAcceleration(gaName)
			: GravityAcceleration.Default;

		return new FallingTime(gravityAcceleration, startHeight);
	}

	private async Task<GravityAcceleration> GetGravityAcceleration(string gaName) =>
		await _gaContext.GravityAccelerations.FirstOrDefaultAsync(ga => ga.Name == gaName) ??
		await _gaClient.GetGravityAcceleration(gaName) ??
		GravityAcceleration.Default;

	public FallingTimeController(GravityAccelerationContext gaContext, GravityAccelerationClient gaClient)
	{
		_gaContext = gaContext;
		_gaClient = gaClient;
	}
}
