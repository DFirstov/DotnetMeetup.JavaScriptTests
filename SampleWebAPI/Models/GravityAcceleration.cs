namespace SampleWebAPI.Models;

public record GravityAcceleration(
	string Name,
	double Value)
{
	public static GravityAcceleration Default { get; } = new("default", 9.81);
}
