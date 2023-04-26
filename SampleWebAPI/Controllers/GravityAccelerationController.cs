using Microsoft.AspNetCore.Mvc;
using SampleWebAPI.Data;
using SampleWebAPI.Models;

namespace SampleWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class GravityAccelerationController : ControllerBase
{
	private readonly GravityAccelerationContext _gaContext;

	public GravityAccelerationController(GravityAccelerationContext gaContext)
	{
		_gaContext = gaContext;
	}

	[HttpPost]
	public async Task<ActionResult> PostGravityAcceleration(GravityAcceleration ga)
	{
		_gaContext.GravityAccelerations.Add(ga);
		await _gaContext.SaveChangesAsync();

		return Ok();
	}
}
