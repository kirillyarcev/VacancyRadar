using System.Net.Http.Json;
using System.Text.Json;
using VacancyRadar.Domain.Entities;

namespace VacancyRadar.Application.Services.Parsers;

public class RemoteOkParser : IJobParser
{
    private readonly HttpClient _http;

    public RemoteOkParser(HttpClient http) => _http = http;

    public string SourceName => "remoteok";

    public async Task<List<Vacancy>> ParseAsync(CancellationToken ct = default)
    {
        _http.DefaultRequestHeaders.UserAgent.ParseAdd("VacancyRadar/1.0");

        var response = await _http.GetFromJsonAsync<JsonElement>(
            "https://remoteok.com/api", ct);

        var result = new List<Vacancy>();

        for (int i = 1; i < response.GetArrayLength(); i++)
        {
            var job = response[i];

            var vacancy = new Vacancy
            {
                Source = SourceName,
                ExternalId = job.TryGetProperty("id", out var id) ? id.GetString() ?? "" : "",
                Title = job.TryGetProperty("position", out var pos) ? pos.GetString() ?? "" : "",
                Description = job.TryGetProperty("description", out var desc) ? desc.GetString() ?? "" : "",
                Location = job.TryGetProperty("location", out var loc) ? loc.GetString() : "Remote",
                Url = job.TryGetProperty("url", out var url) ? url.GetString() ?? "" : "",
                PublishedAt = DateTime.UtcNow,
                Company = new Company
                {
                    Name = job.TryGetProperty("company", out var comp) ? comp.GetString() ?? "Unknown" : "Unknown"
                }
            };

            result.Add(vacancy);
        }

        return result;
    }
}