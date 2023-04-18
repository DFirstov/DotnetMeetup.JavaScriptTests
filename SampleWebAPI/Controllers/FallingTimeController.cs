using Microsoft.AspNetCore.Mvc;
using SampleWebAPI.Data;
using SampleWebAPI.Models;

namespace SampleWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FallingTimeController : ControllerBase
{
	private readonly GravityAccelerationContext _gravityAccelerationContext;

	public FallingTimeController(GravityAccelerationContext gravityAccelerationContext)
	{
		_gravityAccelerationContext = gravityAccelerationContext;
	}

	[HttpGet]
	public ActionResult<FallingTime> Get(double startHeight, string? gaName)
	{
		if (startHeight < 0)
		{
			return BadRequest("Start height must be nonnegative!");
		}

		var gravityAcceleration =
			_gravityAccelerationContext.GravityAccelerations.FirstOrDefault(ga => ga.Name == gaName) ??
			GravityAcceleration.Default;

		return new FallingTime(gravityAcceleration, startHeight);
	}
}
