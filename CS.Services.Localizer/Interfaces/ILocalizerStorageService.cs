using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Services.Localizer.Interfaces
{
    public interface ILocalizerStorageService
    {
        Task<IDictionary<string, string>> GetLocalizedDictionary(string language);
        string GetLocalizedString(IDictionary<string, string> localizedDictionary, string key);
    }
}
