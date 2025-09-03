using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Reflection;
using TGH.Models;
using TGH.Resources;

namespace TGH.Helpers
{
    public class LanguageService
    {
        private readonly IStringLocalizer _localizer;
        public bool Arabic => CultureInfo.CurrentUICulture.Name == "ar";
        public static bool IsArabic => CultureInfo.CurrentUICulture.Name == "ar";
        public LanguageService(IStringLocalizerFactory factory)
        {
            var type = typeof(SharedResource);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create("SharedResource", assemblyName.Name);
        }

        public LocalizedString Getkey(string key)
        {
            return _localizer[key];
        }

    }
}
