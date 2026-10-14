using System.Text.RegularExpressions;

namespace VacancyRadar.Application.Services;

public static class SkillExtractor
{
    private static readonly string[] KnownSkills =
    {
        "Python", "C#", "C\\+\\+", "Java", "JavaScript", "TypeScript",
        "SQL", "PostgreSQL", "MySQL", "MongoDB", "Redis",
        "Docker", "Kubernetes", "AWS", "Azure", "Git",
        "React", "Angular", "Vue", "\\.NET", "ASP\\.NET",
        "Django", "Flask", "Node\\.js", "Go", "Rust", "PHP"
    };

    public static List<string> Extract(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return new List<string>();

        var found = new List<string>();

        foreach (var skill in KnownSkills)
        {
            var pattern = $@"\b{skill}\b";
            if (Regex.IsMatch(description, pattern, RegexOptions.IgnoreCase))
            {
                found.Add(skill.Replace("\\", ""));
            }
        }

        return found.Distinct().ToList();
    }
}