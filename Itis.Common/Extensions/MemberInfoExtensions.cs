using System;
using System.ComponentModel;
using System.Reflection;

namespace Itis.Common.Extensions
{
    public static class MemberInfoExtensions
    {
        public static Attribute GetAttribute(this MemberInfo memberInfo, Type attributeType)
        {
            if (!Attribute.IsDefined(memberInfo, attributeType))
                return null;

            return Attribute.GetCustomAttribute(memberInfo, attributeType);
        }

        public static T GetAttribute<T>(this MemberInfo memberInfo) 
            where T : Attribute
        {
            return (T)GetAttribute(memberInfo, typeof (T));
        }

        public static bool AttributeIsDefined<T>(this MemberInfo memberInfo)
            where T : Attribute
        {
            return AttributeIsDefined(memberInfo, typeof (T));
        }

        public static bool AttributeIsDefined(this MemberInfo memberInfo, Type attributeType)
        {
            return Attribute.IsDefined(memberInfo, attributeType);
        }

        public static string GetDescription(this MemberInfo memberInfo)
        {
            if (!AttributeIsDefined(memberInfo, typeof (DescriptionAttribute)))
                return null;

            return GetAttribute<DescriptionAttribute>(memberInfo).Description;
        }
    }
}