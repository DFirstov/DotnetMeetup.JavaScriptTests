using SampleWebAPI.Models;

namespace SampleWebAPI.Clients;

public class GravityAccelerationClient
{
	private readonly HttpClient _httpClient;

	public GravityAccelerationClient(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<GravityAcceleration?> GetGravityAcceleration(string name, CancellationToken cancellationToken)
	{
		try
		{
			return await _httpClient.GetFromJsonAsync<GravityAcceleration>($"gravityAcceleration/{name}", cancellationToken);
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return null;
		}
	}
}
