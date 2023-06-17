using CS.Localizer.Razor.StateProvider;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Localizer.Razor.Switcher
{
    public class CSSwitcherComponent:ComponentBase
    {
        [CascadingParameter] CSStateProviderComponent CSStatePtovider { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        protected string CurrentLanguage { get; set; }

        protected Dictionary<string, string> languages = new Dictionary<string, string>
        {
            {"English", "English" },
            {"Russian", "Русский" },
            {"Arabic", "العربية" }
        };

        protected override async Task OnInitializedAsync()
        {
            CurrentLanguage = CSStatePtovider.CurrentLanguage;
            await ChangeDirection(CurrentLanguage);
        }

        public async Task OnLanguageChange(ChangeEventArgs e)
        {
            await CSStatePtovider.ChangeLanguage(e.Value.ToString());
            await ChangeDirection(e.Value.ToString());
        }

        private async Task ChangeDirection(string lang)
        {
            if (lang == "Arabic")
            {
                await JSRuntime.InvokeVoidAsync("document.body.setAttribute", "dir", "rtl");
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("document.body.setAttribute", "dir", "ltr");

            }
        }
    }
}
