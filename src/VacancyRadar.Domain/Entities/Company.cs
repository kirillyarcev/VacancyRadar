namespace VacancyRadar.Domain.Entities;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Url { get; set; }
    public List<Vacancy> Vacancies { get; set; } = new();
}