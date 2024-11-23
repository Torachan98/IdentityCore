using IdentityCore.EFs.Attributes;
using System;
using System.ComponentModel;
using System.Reflection;

namespace IdentityCore.EFs.Helpers
{
    public static class EnumHelper
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            var field = value.GetType().GetField(value.ToString());
            return field?.GetCustomAttribute<TAttribute>();
        }

        public static string GetEnumDescription<TEnum>(TEnum enumValue) where TEnum : Enum
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            var attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? enumValue.ToString();
        }

        private static void SetProperty<T>(T instance, string propertyName, object value)
        {
            var property = typeof(T).GetProperty(propertyName);
            if (property != null && property.CanWrite)
            {
                property.SetValue(instance, value);
            }
        }


        #region Permission
        public static List<T> ConvertPermissionToList<TEnum, T>()
        where TEnum : Enum
        where T : new()
        {
            var result = new List<T>();

            foreach (var enumValue in Enum.GetValues(typeof(TEnum)).Cast<TEnum>())
            {
                var permissionType = GetEnumPermissionType(enumValue);
                var description = GetEnumDescription(enumValue);

                var instance = new T();

                SetProperty(instance, "PermissionType", permissionType);
                SetProperty(instance, "Permission", enumValue);
                SetProperty(instance, "Description", description);

                result.Add(instance);
            }

            return result;
        }

        private static PermissionType GetEnumPermissionType<TEnum>(TEnum enumValue) where TEnum : Enum
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            var attribute = field.GetCustomAttribute<PermissionAttribute>();
            return attribute?.PermissionType ?? PermissionType.Services;
        }
        #endregion

        #region Permission Enum
        public static List<T> ConvertRoleToList<TEnum, T>()
        where TEnum : Enum
        where T : new()
        {
            var result = new List<T>();

            foreach (var enumValue in Enum.GetValues(typeof(TEnum)).Cast<TEnum>())
            {
                var description = GetEnumDescription(enumValue);

                var instance = new T();

                SetProperty(instance, "Role", enumValue);
                SetProperty(instance, "Description", description);

                result.Add(instance);
            }

            return result;
        }
        #endregion
    }
}
