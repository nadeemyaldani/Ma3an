using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TGH
{
    public class CustomRequiredAttribute : RequiredAttribute
    {
        public CustomRequiredAttribute(string resourceKey) : base()
        {
            ErrorMessage = ResourceHelper.GetKey(resourceKey);
        }
    }
}
