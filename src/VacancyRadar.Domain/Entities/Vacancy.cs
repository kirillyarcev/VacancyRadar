namespace VacancyRadar.Domain.Entities;

public class Vacancy
{
    public int Id { get; set; }
    public string Source { get; set; } = string.Empty;
    public string ExternalId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Location { get; set; }
    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }
    public string? Currency { get; set; }
    public string Url { get; set; } = string.Empty;
    public DateTime PublishedAt { get; set; }
    public int? CompanyId { get; set; }
    public Company? Company { get; set; }
    public List<Skill> Skills { get; set; } = new();
}