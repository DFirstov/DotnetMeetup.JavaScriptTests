using JetBrains.Annotations;

namespace SampleWebAPI.Models;

[PublicAPI]
public record FallingTime(
	GravityAcceleration GravityAcceleration,
	double StartHeight)
{
	public double Value => Math.Sqrt(2 * StartHeight / GravityAcceleration.Value);
}
