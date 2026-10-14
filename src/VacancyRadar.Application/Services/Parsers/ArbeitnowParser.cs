using System.Net.Http.Json;
using System.Text.Json;
using VacancyRadar.Domain.Entities;

namespace VacancyRadar.Application.Services.Parsers;

public class ArbeitnowParser : IJobParser
{
    private readonly HttpClient _http;

    public ArbeitnowParser(HttpClient http) => _http = http;

    public string SourceName => "arbeitnow";

    public async Task<List<Vacancy>> ParseAsync(CancellationToken ct = default)
    {
        var response = await _http.GetFromJsonAsync<JsonElement>(
            "https://arbeitnow.com/api/job-board-api", ct);

        var jobs = response.GetProperty("data");
        var result = new List<Vacancy>();

        foreach (var job in jobs.EnumerateArray())
        {
            var vacancy = new Vacancy
            {
                Source = SourceName,
                ExternalId = job.TryGetProperty("slug", out var slug) ? slug.GetString() ?? "" : "",
                Title = job.TryGetProperty("title", out var title) ? title.GetString() ?? "" : "",
                Description = job.TryGetProperty("description", out var desc) ? desc.GetString() ?? "" : "",
                Location = job.TryGetProperty("location", out var loc) ? loc.GetString() : "Remote",
                Url = job.TryGetProperty("url", out var url) ? url.GetString() ?? "" : "",
                PublishedAt = job.TryGetProperty("created_at", out var date) && date.TryGetInt64(out var ts)
                    ? DateTimeOffset.FromUnixTimeSeconds(ts).UtcDateTime : DateTime.UtcNow,
                Company = new Company
                {
                    Name = job.TryGetProperty("company_name", out var comp) ? comp.GetString() ?? "Unknown" : "Unknown"
                }
            };

            result.Add(vacancy);
        }

        return result;
    }
}