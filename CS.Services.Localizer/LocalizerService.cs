using Blazored.LocalStorage;
using CS.Services.Localizer.Interfaces;
using CS.Services.Localizer.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Services.Localizer
{
    public class LocalizerService : ILocalizerService
    {
        private readonly ILocalizerStorageService storageService;
        private readonly ILocalStorageService localStorageService;

        private Preference preference;
        private IDictionary<string, string> translations;

        public LocalizerService(ILocalizerStorageService storageService, ILocalStorageService localStorageService)
        {
            this.storageService = storageService;
            this.localStorageService = localStorageService;
        }
        public Preference Preference => preference;

        public string GetLocalizedString(string key)
        {
            return storageService.GetLocalizedString(translations, key);
        }

        public async Task<Preference> GetOrSetPreferences()
        {
            var pref = await localStorageService.GetItemAsync<Preference>("preference");

            preference = pref ?? new Preference() { Language = "English" };

            translations = await storageService.GetLocalizedDictionary(preference.Language);

            await SavePreference();

            return preference;
        }

        public async Task<string> ChangeLanguage(string language)
        {
            preference.Language = language;

            translations = await storageService.GetLocalizedDictionary(preference.Language);

            await SavePreference();

            return preference.Language;
        }

        // Helper methods
        private async Task SavePreference()
        {
            await localStorageService.SetItemAsync("preference", preference);
        }
    }
}
