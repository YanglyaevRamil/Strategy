﻿using System;
using System.Reflection;


namespace _Strategy._Main.Utils.AssetsInjector
{
    
    public static class AssetsInjector
    {

        private static readonly Type _injectAssetAttributeType = typeof(InjectAssetAttribute);
        
        
        public static T Inject<T>(this AssetsContext context, T target)
        {
            var targetType = target.GetType();

            while (targetType != null)
            {
                var allFields = targetType.GetFields(
                    BindingFlags.NonPublic |
                    BindingFlags.Public |
                    BindingFlags.Instance);

                for (int i = 0; i < allFields.Length; i++)
                {
                    var fieldInfo = allFields[i];
                    var injectAssetAttribute =
                        fieldInfo.GetCustomAttribute(_injectAssetAttributeType) as InjectAssetAttribute;
                    if (injectAssetAttribute == null)
                        continue;

                    var objectToInject = context.GetObjectOfType(fieldInfo.FieldType, injectAssetAttribute.AssetName);
                    fieldInfo.SetValue(target, objectToInject);
                }

                targetType = targetType.BaseType;
            }

            return target;
        }
        
    }
}