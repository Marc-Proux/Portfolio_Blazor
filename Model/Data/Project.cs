using System;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Model.Data
{
	public class Project
	{
        public string? Id { get; set; }

        [Required(ErrorMessage = "Ce champ est obligatoire.")]
        [MinLength(3, ErrorMessage = "Le titre doit contenir au moins 3 caractères.")]
        public string Title { get; set; } = string.Empty;

        public string Path { get; set; } = string.Empty;

        public bool IsFinished { get; set; } = false;

        public string Type { get; set; } = string.Empty;
        
        public string? RepoLink { get; set; }

        [Required(ErrorMessage = "Ce champ est obligatoire.")]
        [MinLength(10, ErrorMessage = "La description doit contenir au moins 10 caractères.")]
        public string Description { get; set; } = string.Empty;

        public int CreationDate { get; set; }
    }
}

