using VacancyRadar.Domain.Entities;

namespace VacancyRadar.Application.Services.Parsers;

public interface IJobParser
{
    string SourceName { get; }
    Task<List<Vacancy>> ParseAsync(CancellationToken ct = default);
}