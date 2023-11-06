using System;
using System.Collections.Generic;
using System.Linq;
using _Strategy._Main.Abstractions;
using UnityEngine;


namespace _Strategy._Main.Core
{
    
    public sealed class FractionMember : MonoBehaviour, IFractionMember
    {

        [field: SerializeField] public int FractionId { get; private set; }

        private static Dictionary<int, List<int>> _membersCount = new();


        
        private void Awake()
        {
            Register();
        }


        private void OnDestroy()
        {
            UnRegister();
        }


        public void SetFraction(int fractionId)
        {
            FractionId = fractionId;
            Register();
        }


        public static int FractionsCount
        {
            get
            {
                lock (_membersCount)
                    return _membersCount.Count;
            }
        }


        public static int GetWinner()
        {
            lock (_membersCount)
                return _membersCount.Keys.First();
        }


        private void Register()
        {
            lock (_membersCount)
            {
                if (!_membersCount.ContainsKey(FractionId))
                    _membersCount.Add(FractionId, new List<int>());
                
                if (!_membersCount[FractionId].Contains(GetInstanceID())) 
                    _membersCount[FractionId].Add(GetInstanceID());
            }
        }


        private void UnRegister()
        {
            lock (_membersCount)
            {
                if (_membersCount[FractionId].Contains(GetInstanceID()))
                    _membersCount[FractionId].Remove(GetInstanceID());

                if (_membersCount[FractionId].Count == 0)
                    _membersCount.Remove(FractionId);
            }
        }
        

    }
}