using Sirenix.OdinInspector;
using System;
using System.Collections;

namespace Toolbox.Messaging
{
    public class ClassDropDownList
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="defaultConstructorOnly"></param>
        /// <returns></returns>
        public static IEnumerable AllTypesFromInterface(Type baseType, bool defaultConstructorOnly)
        {
            ValueDropdownList<string> dropList = new();
            var types = defaultConstructorOnly ? TypeHelper.FindInterfaceImplementationsWithDefaultConstructors(baseType) :
                                                 TypeHelper.FindInterfaceImplementations(baseType);

            foreach (var t in types)
                dropList.Add(t.FullName.Replace('.', '/'), t.FullName);

            return dropList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="defaultConstructorOnly"></param>
        /// <returns></returns>
        public static IEnumerable AllTypesFromClass(Type baseType, bool defaultConstructorOnly)
        {
            ValueDropdownList<string> dropList = new();
            var types = defaultConstructorOnly ? TypeHelper.FindSubClassesWithDefaultConstructors(baseType) :
                                                 TypeHelper.FindSubClasses(baseType);

            foreach (var t in types)
                dropList.Add(t.FullName.Replace('.', '/'), t.FullName);

            return dropList;
        }
    }
}
