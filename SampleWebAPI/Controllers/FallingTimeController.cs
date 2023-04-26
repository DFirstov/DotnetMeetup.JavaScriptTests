using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleWebAPI.Clients;
using SampleWebAPI.Models;

namespace SampleWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FallingTimeController : ControllerBase
{
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
		await _gaClient.GetGravityAcceleration(gaName) ??
		GravityAcceleration.Default;

	public FallingTimeController(GravityAccelerationClient gaClient)
	{
		_gaClient = gaClient;
	}
}
