using IdentityCore.Attributes;
using System.Reflection;

namespace IdentityCore.Services.Helpers
{
    public static class Validator
    {
        /// <summary>
        /// Validate Properties which have [Required] Attribute
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<string> ValidateRequiredProperties<T>(T obj)
        {
            var errors = new List<string>();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (Attribute.IsDefined(property, typeof(AllowNotValidateAttribute)))
                {
                    continue; 
                }

                var value = property.GetValue(obj);
                if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
                {
                    errors.Add($"{property.Name} is required.");
                }
            }

            return errors;
        }
    }
}
