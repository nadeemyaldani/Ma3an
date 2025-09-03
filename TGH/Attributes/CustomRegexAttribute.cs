using System.ComponentModel.DataAnnotations;

namespace TGH
{
    public class CustomRegexAttribute : RegularExpressionAttribute
    {
        public CustomRegexAttribute(string pattern, string resourceKey) : base(pattern)
        {
            ErrorMessage = ResourceHelper.GetKey(resourceKey);
        }
    }
}
