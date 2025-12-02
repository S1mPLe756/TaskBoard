using System.ComponentModel.DataAnnotations;

namespace UserProfile.Application.Attributes;

public class GuidNotEmptyAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is Guid guid)
        {
            return guid != Guid.Empty;
        }

        return false;
    }
}