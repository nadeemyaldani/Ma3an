using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TGH.Models
{
    public class CustomDisplayAttribute : DisplayNameAttribute
    {
        private string _resourceKey;
        public CustomDisplayAttribute(string resourceKey)
        {
            _resourceKey = resourceKey;
        }

        public override string DisplayName => ResourceHelper.GetKey(_resourceKey);
    }
}
