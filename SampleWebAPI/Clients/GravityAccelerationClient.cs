using SampleWebAPI.Models;

namespace SampleWebAPI.Clients;

public class GravityAccelerationClient
{
	private readonly HttpClient _httpClient;

	public GravityAccelerationClient(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public Task<GravityAcceleration?> GetGravityAcceleration(string name, CancellationToken cancellationToken)
	{
		return _httpClient.GetFromJsonAsync<GravityAcceleration>($"gravityAcceleration/{name}", cancellationToken);
	}
}
