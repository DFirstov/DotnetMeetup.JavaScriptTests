using Microsoft.AspNetCore.Mvc;
using SampleWebAPI.Data;
using SampleWebAPI.Models;

namespace SampleWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class GravityAccelerationController : ControllerBase
{
	private readonly GravityAccelerationContext _gravityAccelerationContext;

	public GravityAccelerationController(GravityAccelerationContext gravityAccelerationContext)
	{
		_gravityAccelerationContext = gravityAccelerationContext;
	}

	[HttpPost]
	public async Task<ActionResult> PostGravityAcceleration(GravityAcceleration gravityAcceleration)
	{
		_gravityAccelerationContext.GravityAccelerations.Add(gravityAcceleration);
		await _gravityAccelerationContext.SaveChangesAsync();

		return Ok();
	}
}
