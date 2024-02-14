using System;
namespace Portfolio.Data.Models
{
	public class Project
	{
        public int? Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public bool IsFinished { get; set; } = false;
        public string Type { get; set; } = "Development";
        public string? RepoLink { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}

