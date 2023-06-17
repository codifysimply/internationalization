using CS.Localizer.Razor.StateProvider;
using CS.Services.Localizer.Interfaces;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS.Localizer.Razor.Localizer
{
    public class CSLocalizerComponent:ComponentBase
    {
        [Inject]
        protected ILocalizerService LocalizerService { get; set; }
        [CascadingParameter] CSStateProvider CSStateProvider { get; set; }

        [Parameter]
        public string Key { get; set; }
    }
}
