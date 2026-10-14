using System.Net.Http.Json;
using System.Text.Json;
using VacancyRadar.Domain.Entities;

namespace VacancyRadar.Application.Services.Parsers;

public class JobicyParser : IJobParser
{
    private readonly HttpClient _http;

    public JobicyParser(HttpClient http) => _http = http;

    public string SourceName => "jobicy";

    public async Task<List<Vacancy>> ParseAsync(CancellationToken ct = default)
    {
        var response = await _http.GetFromJsonAsync<JsonElement>(
            "https://jobicy.com/api/v2/remote-jobs?count=200", ct);

        var jobs = response.GetProperty("jobs");
        var result = new List<Vacancy>();

        foreach (var job in jobs.EnumerateArray())
        {
            var vacancy = new Vacancy
            {
                Source = SourceName,
                ExternalId = job.TryGetProperty("id", out var id) ? id.GetInt32().ToString() : "",
                Title = job.TryGetProperty("jobTitle", out var title) ? title.GetString() ?? "" : "",
                Description = job.TryGetProperty("jobDescription", out var desc) ? desc.GetString() ?? "" : "",
                Location = job.TryGetProperty("jobGeo", out var geo) ? geo.GetString() : "Remote",
                Url = job.TryGetProperty("url", out var url) ? url.GetString() ?? "" : "",
                SalaryMin = job.TryGetProperty("annualSalaryMin", out var smin) && smin.ValueKind != JsonValueKind.Null
                    ? decimal.Parse(smin.GetString() ?? "0") : null,
                SalaryMax = job.TryGetProperty("annualSalaryMax", out var smax) && smax.ValueKind != JsonValueKind.Null
                    ? decimal.Parse(smax.GetString() ?? "0") : null,
                Currency = job.TryGetProperty("salaryCurrency", out var cur) ? cur.GetString() : null,
                PublishedAt = DateTime.UtcNow,
                Company = new Company
                {
                    Name = job.TryGetProperty("companyName", out var comp) ? comp.GetString() ?? "Unknown" : "Unknown"
                }
            };

            result.Add(vacancy);
        }

        return result;
    }
}