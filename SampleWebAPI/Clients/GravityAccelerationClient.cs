using SampleWebAPI.Models;

namespace SampleWebAPI.Clients;

public class GravityAccelerationClient
{
	private readonly HttpClient _httpClient;

	public GravityAccelerationClient(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<GravityAcceleration?> GetGravityAcceleration(string gaName)
	{
		try
		{
			var uri = $"gravityAcceleration/{gaName}";
			return await _httpClient.GetFromJsonAsync<GravityAcceleration>(uri);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return null;
		}
	}
}
