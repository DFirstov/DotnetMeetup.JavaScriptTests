using SampleWebAPI.Models;

namespace SampleWebAPI.Clients;

public class GravityAccelerationClient
{
	private readonly HttpClient _httpClient;

	public GravityAccelerationClient(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<GravityAcceleration?> GetGravityAcceleration(string gaName, CancellationToken ct)
	{
		try
		{
			return await _httpClient.GetFromJsonAsync<GravityAcceleration>($"gravityAcceleration/{gaName}", ct);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return null;
		}
	}

	public async Task PostGravityAcceleration(GravityAcceleration ga, CancellationToken ct)
	{
		await _httpClient.PostAsJsonAsync("gravityAcceleration", ga, ct);
	}
}
