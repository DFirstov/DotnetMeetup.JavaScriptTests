using Microsoft.AspNetCore.Mvc;

namespace SampleWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FallingTimeController : ControllerBase
{
	private const double GravityAcceleration = 9.81;

	[HttpGet]
	public ActionResult<Model> Get(double startHeight)
	{
		if (startHeight < 0)
		{
			return BadRequest("Start height must be nonnegative!");
		}
		
		return new Model(GravityAcceleration, startHeight);
	}

	public record Model(
		double GravityAcceleration,
		double StartHeight)
	{
		public double FallingTime => Math.Sqrt(2 * StartHeight / GravityAcceleration);
	}
}
