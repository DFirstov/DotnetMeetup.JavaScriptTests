using JetBrains.Annotations;

namespace SampleWebAPI.Models;

[PublicAPI]
public record FallingTime(double StartHeight)
{
	public double GravityAcceleration => 9.81;

	public double Value => Math.Sqrt(2 * StartHeight / GravityAcceleration);
}
