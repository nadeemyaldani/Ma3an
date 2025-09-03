using System.ComponentModel.DataAnnotations;

namespace TGH
{
    public class CustomCompareAttribute : CompareAttribute
    {
        public CustomCompareAttribute(string otherProperty, string resourceKey) : base(otherProperty)
        {
            ErrorMessage = ResourceHelper.GetKey(resourceKey);
        }
    }
}
