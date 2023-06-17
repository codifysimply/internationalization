using CS.Services.Localizer.Interfaces;
using CS.Services.Localizer.Objects;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

namespace CS.Services.Localizer
{
    public class LocalizerStorageService : ILocalizerStorageService
    {
        private readonly IHostEnvironment hostEnvironment;

        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);

        public LocalizerStorageService(IHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        public Task<IDictionary<string, string>> GetLocalizedDictionary(string language)
        {
            var storage = GetOrCreateStorage();

            return Task.FromResult((IDictionary<string, string>)storage.
                SelectMany(v => v).
                ToDictionary(
                kvp => kvp.Key,
                kvp => JObject.Parse(JsonConvert.SerializeObject(kvp.Value))[language].ToString()));

        }

        public string GetLocalizedString(IDictionary<string, string> localizedDictionary, string key)
        {
            if (key == null)
            {
                return string.Empty;
            }
            else if (localizedDictionary.ContainsKey(key))
            {
                if (localizedDictionary.TryGetValue(key, out string value) && !string.IsNullOrEmpty(value))
                {
                    return value;
                }
                else
                {
                    return "{" + key + "}";
                }
            }
            else
            {
                InsertTranslation(key).ConfigureAwait(false);
                return "{" + key + "}";
            }
        }

        private async Task InsertTranslation(string key)
        {
            if (hostEnvironment.IsDevelopment())
            {
                try
                {
                    await semaphore.WaitAsync();

                    var list = GetOrCreateStorage().ToList();

                    var test = list.SelectMany(v => v);

                    if (!list.SelectMany(v => v).Any(kvp => string.Compare(kvp.Key, key, true) == 0))
                    {
                        list.Add(new Dictionary<string, Translation>
                        {
                            { key, new Translation() }
                        });

                        var path = Directory.GetCurrentDirectory();
                        var fileName = $"{path}/translations.json";

                        using StreamWriter writer = File.CreateText(fileName);
                        var json = JsonConvert.SerializeObject(list, Formatting.Indented);

                        writer.Write(json);
                    }
                }
                finally
                {
                    semaphore.Release();
                }
            }
        }

        private IEnumerable<IDictionary<string, Translation>> GetOrCreateStorage()
        {
            var result = new List<IDictionary<string, Translation>>();

            var path = Directory.GetCurrentDirectory();

            var fileName = $"{path}/translations.json";

            if (!File.Exists(fileName))
            {
                if (hostEnvironment.IsDevelopment())
                {
                    using var writer = File.CreateText(fileName);
                    writer.Write(JsonConvert.SerializeObject(new List<IDictionary<string, Translation>>(), Formatting.Indented));
                }
                else
                {
                    throw new NotSupportedException("You can not create json file in Development Environment");
                }
            }

            using (var reader = File.OpenText(fileName))
            {
                var json = reader.ReadToEnd();
                result = JsonConvert.DeserializeObject<List<IDictionary<string, Translation>>>(json);
            }

            return result ?? new List<IDictionary<string, Translation>>();
        }
    }
}
