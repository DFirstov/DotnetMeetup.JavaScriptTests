using SampleWebAPI.Models;

namespace SampleWebAPI.Clients;

public class GravityAccelerationClient
{
	private readonly HttpClient _httpClient;

	public GravityAccelerationClient(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<GravityAcceleration?> GetGravityAcceleration(string name, CancellationToken ct)
	{
		try
		{
			return await _httpClient.GetFromJsonAsync<GravityAcceleration>($"gravityAcceleration/{name}", ct);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return null;
		}
	}

	public async Task PostGravityAcceleration(GravityAcceleration gravityAcceleration, CancellationToken ct)
	{
		try
		{
			await _httpClient.PostAsJsonAsync("gravityAcceleration", gravityAcceleration, ct);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}
	}
}
