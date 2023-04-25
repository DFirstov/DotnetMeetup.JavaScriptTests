using Microsoft.AspNetCore.Mvc;
using SampleWebAPI.Models;

namespace SampleWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FallingTimeController : ControllerBase
{
	[HttpGet]
	public ActionResult<FallingTime> Get(double startHeight)
	{
		if (startHeight < 0)
		{
			return BadRequest("Start height must be nonnegative!");
		}

		return new FallingTime(startHeight);
	}
}
