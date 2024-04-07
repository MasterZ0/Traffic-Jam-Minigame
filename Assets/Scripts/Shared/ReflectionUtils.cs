using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace Hasbro.TheGameOfLife.Shared
{
    public static class ReflectionUtils
    {
        /// <summary> All Public and Private. Don't incluside private inherited </summary>
        public const BindingFlags PublicAndPrivate = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        /// <summary> Single Attribute from <see cref="FieldInfo"/></summary>
        public static List<(FieldInfo, TAttribute)> GetAllFieldsWithAttribute<TAttribute>(object target, string ignoredNamespace = null) where TAttribute : Attribute
        {
            return GetAllMembersWithAttribute<FieldInfo, TAttribute>(target, type => type.GetFields(PublicAndPrivate), ignoredNamespace);
        }

        /// <summary> Single Attribute from <see cref="PropertyInfo"/></summary>
        public static List<(PropertyInfo, TAttribute)> GetAllPropertyWithAttribute<TAttribute>(object target, string ignoredNamespace = null, BindingFlags flags = PublicAndPrivate) where TAttribute : Attribute
        {
            return GetAllMembersWithAttribute<PropertyInfo, TAttribute>(target, type => type.GetProperties(flags), ignoredNamespace);
        }

        /// <summary> Single Attribute from generic member </summary>
        private static List<(TMember, TAttribute)> GetAllMembersWithAttribute<TMember, TAttribute>(object target, Func<Type, TMember[]> getMembers, string ignoredNamespace = null) where TAttribute : Attribute where TMember : MemberInfo
        {
            return GetAllMembersWithAttributes<TMember, TAttribute>(target, getMembers, ignoredNamespace)
                .Select(d => (d.Item1, d.Item2.First())).ToList();
        }

        /// <summary> Multiple Attribute from generic member </summary>
        private static List<(TMember, IEnumerable<TAttribute>)> GetAllMembersWithAttributes<TMember, TAttribute>(object target, Func<Type, TMember[]> getMembers, string ignoredNamespace = null) where TAttribute : Attribute where TMember : MemberInfo
        {
            return GetAllMembers(target, getMembers, ignoredNamespace)
                .Select(member => (member, member.GetCustomAttributes<TAttribute>()))
                .Where(tupla => tupla.Item2.Any())
                .ToList();
        }

        public static List<TMember> GetAllMembers<TMember>(object target, Func<Type, TMember[]> getMembers, string ignoredNamespace = null) where TMember : MemberInfo
        {
            List<TMember> memberList = new();

            Type currentType = target.GetType();

            // Small optimization
            Func<Type, bool> checkWhile;
            if (ignoredNamespace == null)
            {
                checkWhile = (type) => type is not null;
            }
            else
            {
                checkWhile = (type) => type is not null && type.Namespace != null && !type.Namespace.Contains(ignoredNamespace);
            }

            // Get all members from all inherit classes
            while (checkWhile(currentType))
            {
                // Get all members with attribute and into the dictionary
                TMember[] newMembers = getMembers(currentType);
                memberList.AddRange(newMembers);

                // Go to base class
                currentType = currentType.BaseType;
            }

            return memberList;
        }
    }
}
