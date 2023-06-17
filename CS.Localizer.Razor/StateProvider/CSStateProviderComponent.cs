using CS.Services.Localizer.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Localizer.Razor.StateProvider
{
    public class CSStateProviderComponent: ComponentBase
    {
        [Inject]
        ILocalizerService LocalizerService { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
        public bool HasLoaded { get; set; }
        public string CurrentLanguage => LocalizerService.Preference.Language;

        public async Task ChangeLanguage(string language)
        {
            await LocalizerService.ChangeLanguage(language);

            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LocalizerService.GetOrSetPreferences();
                HasLoaded = true;
                StateHasChanged();
            }
        }
    }
}
