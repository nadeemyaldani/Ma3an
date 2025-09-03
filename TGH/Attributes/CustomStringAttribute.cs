using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TGH
{
    public class CustomStringAttribute : StringLengthAttribute
    {
        public CustomStringAttribute(int minimumLength, int maximumLength, string resourceKey) : base(maximumLength)
        {
            MinimumLength = minimumLength;
            ErrorMessage = ResourceHelper.GetKey(resourceKey);
        }
    }
}
