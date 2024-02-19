using System;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Portfolio.Model.Data.Interface;
using System.Text.Json.Serialization;
using Enum;

namespace Portfolio.Model.Data
{
	public class PortfolioApiJsonDirectAccess : IPortfolioApi
    {
        FormationApiJsonDirectAccessSetting _settings;
        private List<Formation>? _formations;
        private List<Experience>? _experiences;
        private List<Project>? _projects;
        private List<DataHistory>? _history;

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
            if (!Directory.Exists($@"{_settings.DataPath}/{_settings.HistoryFolder}"))
            {
                Directory.CreateDirectory($@"{_settings.DataPath}/{_settings.HistoryFolder}");
            }
        }

        private void Load<T>(ref List<T>? list, string folder, JsonSerializerOptions options = null)
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
                    var bp = JsonSerializer.Deserialize<T>(json, options);

                    if (bp != null)
                    {
                        list.Add(bp);
                    }
                }
            }
        }

        private async Task SaveAsync<T>(List<T>? list, string folder, string filename, T item, JsonSerializerOptions options = null)
        {
            var filepath = $@"{_settings.DataPath}/{folder}/{filename}";
            await File.WriteAllTextAsync(filepath, JsonSerializer.Serialize<T>(item, options));
            if (list == null)
            {
                list = new();
            }
            if (!list.Contains(item))
            {
                list.Add(item);
            }
        }

        private void DeleteAsync<T>(List<T>? list, string folder, string id)
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

        public async Task<Formation?> GetFormationAsync(string id)
        {
            await LoadFormationAsync();
            if (_formations == null)
                throw new Exception("Formations not found");
            return _formations.FirstOrDefault(b => b.Id == id);
        }

        public async Task<Formation?> SaveFormationAsync(Formation item)
        {
            DataHistory history = new()
            {
                ElementId = item.Id,
                ElementName = item.Title,
                ElementType = PortfolioElementEnum.education,
                Action = ObjectHistoryEnum.Edit
            };
            if (item.Id == null)
            {
                string id = Guid.NewGuid().ToString();
                item.Id = id;
                item.CreationDate = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                history.Action = ObjectHistoryEnum.Add;
                history.ElementId = id;
            }
            await SaveAsync<Formation>(_formations, _settings.FormationsFolder, $"{item.Id}.json", item);
            await SaveHistoryAsync(history);
            return item;
        }

        public Task DeleteFormationAsync(string id)
        {
            DataHistory history = new()
            {
                ElementId = id,
                ElementName = _formations.FirstOrDefault(b => b.Id == id).Title,
                ElementType = PortfolioElementEnum.education,
                Action = ObjectHistoryEnum.Delete
            };
            DeleteAsync(_formations, _settings.FormationsFolder, id);
            if (_formations != null)
            {
                var item = _formations.FirstOrDefault(b => b.Id == id);
                if (item != null)
                {
                    _formations.Remove(item);
                }
            }
            return SaveHistoryAsync(history);
        }

        public async Task<int> GetFormationsCountAsync()
        {
            await LoadFormationAsync();
            if (_formations == null)
                return 0;
            else
                return _formations.Count();
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

        public async Task<Experience?> GetExperienceAsync(string id)
        {
            await LoadExperiencesAsync();
            if (_experiences == null)
                throw new Exception("Experiences not found");
            return _experiences.FirstOrDefault(b => b.Id == id);
        }

        public async Task<Experience?> SaveExperienceAsync(Experience item)
        {
            DataHistory history = new()
            {
                ElementId = item.Id,
                ElementName = item.Title,
                ElementType = PortfolioElementEnum.experience,
                Action = ObjectHistoryEnum.Edit
            };
            if (item.Id == null)
            {
                string id = Guid.NewGuid().ToString();
                item.Id = id;
                item.CreationDate = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                history.Action = ObjectHistoryEnum.Add;
                history.ElementId = id;
            }
            await SaveAsync<Experience>(_experiences, _settings.ExperiencesFolder, $"{item.Id}.json", item);
            await SaveHistoryAsync(history);
            return item;
        }

        public Task DeleteExperienceAsync(string id)
        {
            DataHistory history = new()
            {
                ElementId = id,
                ElementName = _experiences.FirstOrDefault(b => b.Id == id).Title,
                ElementType = PortfolioElementEnum.experience,
                Action = ObjectHistoryEnum.Delete
            };

            DeleteAsync(_experiences, _settings.ExperiencesFolder, id);
            if (_experiences != null)
            {
                var item = _experiences.FirstOrDefault(b => b.Id == id);
                if (item != null)
                {
                    _experiences.Remove(item);
                }
            }
            return SaveHistoryAsync(history);
        }

        public async Task<int> GetExperiencesCountAsync()
        {
            await LoadExperiencesAsync();
            if (_experiences == null)
                return 0;
            else
                return _experiences.Count();
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

        public async Task<Project?> GetProjectAsync(string id)
        {
            await LoadProjectsAsync();
            if (_projects == null)
                throw new Exception("Experiences not found");
            return _projects.FirstOrDefault(b => b.Id == id);
        }

        public async Task<int> GetProjectsCountAsync()
        {
            await LoadProjectsAsync();
            if (_projects == null)
                return 0;
            else
                return _projects.Count();
        }

        public async Task<Project?> SaveProjectAsync(Project item)
        {
            DataHistory history = new()
            {
                ElementId = item.Id,
                ElementName = item.Title,
                ElementType = PortfolioElementEnum.projects,
                Action = ObjectHistoryEnum.Edit
            };
            if (item.Id == null)
            {
                string id = Guid.NewGuid().ToString();
                item.Id = id;
                item.CreationDate = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                history.Action = ObjectHistoryEnum.Add;
                history.ElementId = id;
            }
            await SaveAsync<Project>(_projects, _settings.ProjectsFolder, $"{item.Id}.json", item);
            await SaveHistoryAsync(history);
            return item;
        }

        public Task DeleteProjectAsync(string id)
        {
            DataHistory history = new()
            {
                ElementId = id,
                ElementName = _projects.FirstOrDefault(b => b.Id == id).Title,
                ElementType = PortfolioElementEnum.projects,
                Action = ObjectHistoryEnum.Delete
            };
            DeleteAsync(_projects, _settings.ProjectsFolder, id);
            if (_projects != null)
            {
                var item = _projects.FirstOrDefault(b => b.Id == id);
                if (item != null)
                {
                    _projects.Remove(item);
                    FileUtility.DeleteImageAsync("wwwroot/"+item.Path);
                }
            }
            return SaveHistoryAsync(history);
        }


        // History method
        private Task LoadHistoryAsync()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            options.Converters.Add(new JsonStringEnumConverter());
            Load<DataHistory>(ref _history, _settings.HistoryFolder, options);
            return Task.CompletedTask;
        }

        public async Task<List<DataHistory>?> GetHistoryAsync()
        {
            await LoadHistoryAsync();
            return _history ?? new();
        }

        public async Task<DataHistory?> SaveHistoryAsync(DataHistory item)
        {
            if (GetHistoryCountAsync().Result >= 10){
                await DeleteOldestHistory();
            }
            if (item.Id == null)
            {
                item.Id = Guid.NewGuid().ToString();
            }
            item.CreationDate = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            var options = new JsonSerializerOptions { WriteIndented = true };
            options.Converters.Add(new JsonStringEnumConverter());
            await SaveAsync<DataHistory>(_history, _settings.HistoryFolder, $"{item.Id}.json", item, options);
            return item;
        }

        public async Task<int> GetHistoryCountAsync()
        {
            await LoadHistoryAsync();
            if (_history == null)
                return 0;
            else
                return _history.Count();
        }

        public async Task DeleteOldestHistory(){
            await LoadHistoryAsync();
            if (_history != null)
            {
                var item = _history.OrderBy(b => b.CreationDate).FirstOrDefault();
                if (item != null)
                {
                    DeleteAsync(_history, _settings.HistoryFolder, item.Id);
                    _history.Remove(item);
                }
            }
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

