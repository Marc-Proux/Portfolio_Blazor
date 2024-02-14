using System;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Portfolio.Data.Models.Interface;
using Portfolio.Data.Models;
using BlazorBootstrap;

namespace Portfolio.Data
{
	public class PortfolioApiJsonDirectAccess : IPortfolioApi
    {
        FormationApiJsonDirectAccessSetting _settings;
        private List<Formation>? _formations;
        private List<Experience>? _experiences;
        private List<Project>? _projects;

        public PortfolioApiJsonDirectAccess(IOptions<FormationApiJsonDirectAccessSetting> option)
        {
            _settings = option.Value;
            if (!Directory.Exists(_settings.DataPath))
            {
                Directory.CreateDirectory(_settings.DataPath);
            }
            if (!Directory.Exists($@"{_settings.DataPath}/{_settings.FormationsFolder}"))
            {
                Directory.CreateDirectory($@"{_settings.DataPath}/{_settings.FormationsFolder}");
            }
            if (!Directory.Exists($@"{_settings.DataPath}/{_settings.ExperiencesFolder}"))
            {
                Directory.CreateDirectory($@"{_settings.DataPath}/{_settings.ExperiencesFolder}");
            }
            if (!Directory.Exists($@"{_settings.DataPath}/{_settings.ProjectsFolder}"))
            {
                Directory.CreateDirectory($@"{_settings.DataPath}/{_settings.ProjectsFolder}");
            }
        }

        private void Load<T>(ref List<T>? list, string folder)
        {
            if (list == null)
            {
                list = new();
                var fullpath = $@"{_settings.DataPath}/{folder}";
                foreach (var f in Directory.GetFiles(fullpath))
                {
                    var attributes = File.GetAttributes(f);
                    if (attributes.HasFlag(FileAttributes.Hidden) || attributes.HasFlag(FileAttributes.System))
                    {
                        continue;
                    }
                    var json = File.ReadAllText(f);
                    var bp = JsonSerializer.Deserialize<T>(json);
                    if (bp != null)
                    {
                        list.Add(bp);
                    }
                }
            }
        }

        private async Task SaveAsync<T>(List<T>? list, string folder, string filename, T item)
        {
            var filepath = $@"{_settings.DataPath}/{folder}/{filename}";
            await File.WriteAllTextAsync(filepath, JsonSerializer.Serialize<T>(item));
            if (list == null)
            {
                list = new();
            }
            if (!list.Contains(item))
            {
                list.Add(item);
            }
        }

        private void DeleteAsync<T>(List<T>? list, string folder, int id)
        {
            var filepath = $@"{_settings.DataPath}/{folder}/{id}.json";
            try
            {
                File.Delete(filepath);
            }
            catch { }
        }


        // Method use to edit a Formation
        private Task LoadFormationAsync()
        {
            Load<Formation>(ref _formations, _settings.FormationsFolder);
            return Task.CompletedTask;
        }

        public async Task<List<Formation>?> GetFormationsAsync()
        {
            await LoadFormationAsync();
            return _formations ?? new();
        }

        public async Task<Formation?> GetFormationAsync(int id)
        {
            await LoadFormationAsync();
            if (_formations == null)
                throw new Exception("Formations not found");
            return _formations.FirstOrDefault(b => b.Id == id);
        }

        public async Task<Formation?> SaveFormationAsync(Formation item)
        {
            if (item.Id == null)
            {
                item.Id = Convert.ToInt32(Guid.NewGuid().ToString());
            }
            await SaveAsync<Formation>(_formations, _settings.FormationsFolder, $"{item.Id}.json", item);
            return item;
        }

        public Task DeleteFormationAsync(int id)
        {
            DeleteAsync(_formations, _settings.FormationsFolder, id);
            if (_formations != null)
            {
                var item = _formations.FirstOrDefault(b => b.Id == id);
                if (item != null)
                {
                    _formations.Remove(item);
                }
            }
            return Task.CompletedTask;
        }


        //Experiences method

        private Task LoadExperiencesAsync()
        {
            Load<Experience>(ref _experiences, _settings.ExperiencesFolder);
            return Task.CompletedTask;
        }

        public async Task<List<Experience>?> GetExperiencesAsync()
        {
            await LoadExperiencesAsync();
            return _experiences ?? new();
        }

        public async Task<Experience?> GetExperienceAsync(int id)
        {
            await LoadExperiencesAsync();
            if (_experiences == null)
                throw new Exception("Experiences not found");
            return _experiences.FirstOrDefault(b => b.Id == id);
        }

        public async Task<Experience?> SaveExperienceAsync(Experience item)
        {
            if (item.Id == null)
            {
                item.Id = Convert.ToInt32(Guid.NewGuid().ToString());
            }
            await SaveAsync<Experience>(_experiences, _settings.ExperiencesFolder, $"{item.Id}.json", item);
            return item;
        }

        public Task DeleteExperienceAsync(int id)
        {
            DeleteAsync(_experiences, _settings.ExperiencesFolder, id);
            if (_experiences != null)
            {
                var item = _experiences.FirstOrDefault(b => b.Id == id);
                if (item != null)
                {
                    _experiences.Remove(item);
                }
            }
            return Task.CompletedTask;
        }


        // Projects method

        private Task LoadProjectsAsync()
        {
            Load<Project>(ref _projects, _settings.ProjectsFolder);
            return Task.CompletedTask;
        }

        public async Task<List<Project>?> GetProjectsAsync()
        {
            await LoadProjectsAsync();
            return _projects ?? new();
        }

        public async Task<Project?> GetProjectAsync(int id)
        {
            await LoadProjectsAsync();
            if (_projects == null)
                throw new Exception("Experiences not found");
            return _projects.FirstOrDefault(b => b.Id == id);
        }

        public async Task<Project?> SaveProjectAsync(Project item)
        {
            if (item.Id == null)
            {
                item.Id = Convert.ToInt32(Guid.NewGuid().ToString());
            }
            await SaveAsync<Project>(_projects, _settings.ProjectsFolder, $"{item.Id}.json", item);
            return item;
        }

        public Task DeleteProjectAsync(int id)
        {
            DeleteAsync(_projects, _settings.ProjectsFolder, id);
            if (_projects != null)
            {
                var item = _projects.FirstOrDefault(b => b.Id == id);
                if (item != null)
                {
                    _projects.Remove(item);
                }
            }
            return Task.CompletedTask;
        }



        public Task InvalidateCacheAsync()
        {
            _formations = null;
            _experiences = null;
            _projects = null;
            return Task.CompletedTask;
        }
    }
}

