#if UNITY_WINRT && !UNITY_EDITOR && !UNITY_WP8
#region License
// Copyright (c) 2007 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Security;
using Newtonsoft.Json.Utilities;
using System.Linq;
using System.Runtime.Serialization;

namespace Newtonsoft.Json.Serialization
{
  internal static class JsonTypeReflector
  {
    private static bool? _dynamicCodeGeneration;
    private static bool? _fullyTrusted;

    public const string IdPropertyName = "$id";
    public const string RefPropertyName = "$ref";
    public const string TypePropertyName = "$type";
    public const string ValuePropertyName = "$value";
    public const string ArrayValuesPropertyName = "$values";

    public const string ShouldSerializePrefix = "ShouldSerialize";
    public const string SpecifiedPostfix = "Specified";

    private static readonly ThreadSafeStore<object, Type> JsonConverterTypeCache = new ThreadSafeStore<object, Type>(GetJsonConverterTypeFromAttribute);
    public static JsonContainerAttribute GetJsonContainerAttribute(Type type)
    {
      return CachedAttributeGetter<JsonContainerAttribute>.GetAttribute(type);
    }

    public static JsonObjectAttribute GetJsonObjectAttribute(Type type)
    {
      return GetJsonContainerAttribute(type) as JsonObjectAttribute;
    }

    public static JsonArrayAttribute GetJsonArrayAttribute(Type type)
    {
      return GetJsonContainerAttribute(type) as JsonArrayAttribute;
    }

    public static JsonDictionaryAttribute GetJsonDictionaryAttribute(Type type)
    {
      return GetJsonContainerAttribute(type) as JsonDictionaryAttribute;
    }

    public static DataContractAttribute GetDataContractAttribute(Type type)
    {
      // DataContractAttribute does not have inheritance
      Type currentType = type;

      while (currentType != null)
      {
        DataContractAttribute result = CachedAttributeGetter<DataContractAttribute>.GetAttribute(currentType);
        if (result != null)
          return result;

        currentType = currentType.BaseType();
      }

      return null;
    }

    public static DataMemberAttribute GetDataMemberAttribute(MemberInfo memberInfo)
    {
      // DataMemberAttribute does not have inheritance

      // can't override a field
      if (memberInfo.MemberType() == MemberTypes.Field)
        return CachedAttributeGetter<DataMemberAttribute>.GetAttribute(memberInfo);

      // search property and then search base properties if nothing is returned and the property is virtual
      PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
      DataMemberAttribute result = CachedAttributeGetter<DataMemberAttribute>.GetAttribute(propertyInfo);
      if (result == null)
      {
        if (propertyInfo.IsVirtual())
        {
          Type currentType = propertyInfo.DeclaringType;

          while (result == null && currentType != null)
          {
            PropertyInfo baseProperty = (PropertyInfo)ReflectionUtils.GetMemberInfoFromType(currentType, propertyInfo);
            if (baseProperty != null && baseProperty.IsVirtual())
              result = CachedAttributeGetter<DataMemberAttribute>.GetAttribute(baseProperty);

            currentType = currentType.BaseType();
          }
        }
      }

      return result;
    }

    public static MemberSerialization GetObjectMemberSerialization(Type objectType, bool ignoreSerializableAttribute)
    {
      JsonObjectAttribute objectAttribute = GetJsonObjectAttribute(objectType);
      if (objectAttribute != null)
        return objectAttribute.MemberSerialization;

      DataContractAttribute dataContractAttribute = GetDataContractAttribute(objectType);
      if (dataContractAttribute != null)
        return MemberSerialization.OptIn;

      // the default
      return MemberSerialization.OptOut;
    }

    private static Type GetJsonConverterType(object attributeProvider)
    {
      return JsonConverterTypeCache.Get(attributeProvider);
    }

    private static Type GetJsonConverterTypeFromAttribute(object attributeProvider)
    {
      JsonConverterAttribute converterAttribute = GetAttribute<JsonConverterAttribute>(attributeProvider);
      return (converterAttribute != null)
        ? converterAttribute.ConverterType
        : null;
    }

    public static JsonConverter GetJsonConverter(object attributeProvider, Type targetConvertedType)
    {
      Type converterType = GetJsonConverterType(attributeProvider);

      if (converterType != null)
      {
        JsonConverter memberConverter = JsonConverterAttribute.CreateJsonConverterInstance(converterType);

        return memberConverter;
      }

      return null;
    }

    private static T GetAttribute<T>(Type type) where T : Attribute
    {
      T attribute;

      attribute = ReflectionUtils.GetAttribute<T>(type, true);
      if (attribute != null)
        return attribute;

      foreach (Type typeInterface in type.GetInterfaces())
      {
        attribute = ReflectionUtils.GetAttribute<T>(typeInterface, true);
        if (attribute != null)
          return attribute;
      }

      return null;
    }

    private static T GetAttribute<T>(MemberInfo memberInfo) where T : Attribute
    {
      T attribute;

      attribute = ReflectionUtils.GetAttribute<T>(memberInfo, true);
      if (attribute != null)
        return attribute;

      if (memberInfo.DeclaringType != null)
      {
        foreach (Type typeInterface in memberInfo.DeclaringType.GetInterfaces())
        {
          MemberInfo interfaceTypeMemberInfo = ReflectionUtils.GetMemberInfoFromType(typeInterface, memberInfo);

          if (interfaceTypeMemberInfo != null)
          {
            attribute = ReflectionUtils.GetAttribute<T>(interfaceTypeMemberInfo, true);
            if (attribute != null)
              return attribute;
          }
        }
      }

      return null;
    }

    public static T GetAttribute<T>(object provider) where T : Attribute
    {
      Type type = provider as Type;
      if (type != null)
        return GetAttribute<T>(type);

      MemberInfo memberInfo = provider as MemberInfo;
      if (memberInfo != null)
        return GetAttribute<T>(memberInfo);

      return ReflectionUtils.GetAttribute<T>(provider, true);
    }

#if DEBUG
    internal static void SetFullyTrusted(bool fullyTrusted)
    {
      _fullyTrusted = fullyTrusted;
    }

    internal static void SetDynamicCodeGeneration(bool dynamicCodeGeneration)
    {
      _dynamicCodeGeneration = dynamicCodeGeneration;
    }
#endif

    public static bool DynamicCodeGeneration
    {
      get
      {
        if (_dynamicCodeGeneration == null)
        {
          _dynamicCodeGeneration = false;
        }

        return _dynamicCodeGeneration.Value;
      }
    }

    public static bool FullyTrusted
    {
      get
      {
        if (_fullyTrusted == null)
        {
          _fullyTrusted = false;
        }

        return _fullyTrusted.Value;
      }
    }

    public static ReflectionDelegateFactory ReflectionDelegateFactory
    {
      get
      {
        return ExpressionReflectionDelegateFactory.Instance;
      }
    }
  }
}
#endif