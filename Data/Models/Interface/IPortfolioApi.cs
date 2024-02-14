using System;
namespace Portfolio.Data.Models.Interface
{
	public interface IPortfolioApi
    {
		Task<List<Formation>?> GetFormationsAsync();
        Task<Formation?> GetFormationAsync(int id);
        Task<Formation?> SaveFormationAsync(Formation item);
		Task DeleteFormationAsync(int id);

        Task<List<Experience>?> GetExperiencesAsync();
        Task<Experience?> GetExperienceAsync(int id);
        Task<Experience?> SaveExperienceAsync(Experience item);
        Task DeleteExperienceAsync(int id);

        Task<List<Project>?> GetProjectsAsync();
        Task<Project?> GetProjectAsync(int id);
        Task<Project?> SaveProjectAsync(Project item);
        Task DeleteProjectAsync(int id);

        Task InvalidateCacheAsync();
    }
}

