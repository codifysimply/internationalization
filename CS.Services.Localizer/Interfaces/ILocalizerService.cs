using CS.Services.Localizer.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Services.Localizer.Interfaces
{
    public interface ILocalizerService
    {
        Task<Preference> GetOrSetPreferences();
        string GetLocalizedString(string key);
        Task<string> ChangeLanguage(string language);
        Preference Preference { get; }
    }
}
