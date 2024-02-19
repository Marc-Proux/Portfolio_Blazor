using System;
namespace Portfolio.Model.Data.Interface
{
	public interface IPortfolioApi
    {
		Task<List<Formation>?> GetFormationsAsync();
        Task<Formation?> GetFormationAsync(string id);
        Task<Formation?> SaveFormationAsync(Formation item);
		Task DeleteFormationAsync(string id);
        Task<int> GetFormationsCountAsync();


        Task<List<Experience>?> GetExperiencesAsync();
        Task<Experience?> GetExperienceAsync(string id);
        Task<Experience?> SaveExperienceAsync(Experience item);
        Task DeleteExperienceAsync(string id);
        Task<int> GetExperiencesCountAsync();

        Task<List<Project>?> GetProjectsAsync();
        Task<Project?> GetProjectAsync(string id);
        Task<Project?> SaveProjectAsync(Project item);
        Task DeleteProjectAsync(string id);
        Task<int> GetProjectsCountAsync();

        Task<List<DataHistory>?> GetHistoryAsync();
        Task<DataHistory?> SaveHistoryAsync(DataHistory item);

        Task InvalidateCacheAsync();
    }
}

