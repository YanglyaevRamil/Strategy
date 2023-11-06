using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;


namespace _Strategy._Main.Utils.AssetsInjector
{
    
    [CreateAssetMenu(fileName = nameof(AssetsContext), menuName = "Configs/" + nameof(AssetsContext), order = 0)]
    public sealed class AssetsContext : ScriptableObject
    {

        [SerializeField] private Object[] _objects;


        public Object GetObjectOfType(Type targetType, string targetName = null)
        {
            Object obj = _objects
                .FirstOrDefault(o =>
                    o.GetType().IsAssignableFrom(targetType) && 
                    (targetName == null || o.name.Equals(targetName)));

            return obj;
        }


    }
}