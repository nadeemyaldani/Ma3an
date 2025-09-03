using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TGH.Helpers;
using System.Resources;
using TGH.Models;
using System.Globalization;
using System.Reflection;

namespace TGH
{
    public static class ResourceHelper
    {
        private static LanguageService _languageService;
        public static void Configure(LanguageService languageService)
        {
            _languageService = languageService;
        }

        public static string GetKey(string key)
        {
            return _languageService.Getkey(key);
        }
    }
}
