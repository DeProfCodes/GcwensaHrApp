using System.ComponentModel.DataAnnotations;
using System;
using System.Linq;
using System.Reflection;

namespace GcwensaHrApp.Enums
{
    public static class EnumHelper
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }

        public static string TransalateEnumValueToString(Enum enumType, int value)
        {
            if (enumType is AccountStatus)
            {
                switch (value)
                {
                    case 1: return AccountStatus.Active.GetDisplayName();
                    case 2: return AccountStatus.Suspended.GetDisplayName();
                    case 3: return AccountStatus.Paused.GetDisplayName();
                    case 4: return AccountStatus.Deleted.GetDisplayName();
                }
            }

            return "";
        }
    }
}
