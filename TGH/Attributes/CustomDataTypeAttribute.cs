using System.ComponentModel.DataAnnotations;

namespace TGH
{
    public class CustomDataTypeAttribute : DataTypeAttribute
    {
        public CustomDataTypeAttribute(DataType dataType, string resourceKey) : base(dataType)
        {
            ErrorMessage = ResourceHelper.GetKey(resourceKey);
        }
    }
}
