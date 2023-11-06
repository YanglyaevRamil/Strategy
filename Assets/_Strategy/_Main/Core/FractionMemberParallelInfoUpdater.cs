using System;
using _Strategy._Main.Abstractions;
using UnityEngine;
using Zenject;


namespace _Strategy._Main.Core
{
    
    public class FractionMemberParallelInfoUpdater : MonoBehaviour, ITickable
    {

        [Inject] private IFractionMember _fractionMember;

        
        public void Tick()
        {
            AutoAttackEvaluator.FractionMembersInfo.AddOrUpdate(
            gameObject, 
            new AutoAttackEvaluator.FractionMemberParallelInfo(transform.position, _fractionMember.FractionId),
            (go, value) => 
            {
                value.Position = transform.position;
                value.Fraction = _fractionMember.FractionId;
                return value;
            });
        }


        private void OnDestroy()
        {
            AutoAttackEvaluator.FractionMembersInfo.TryRemove(gameObject, out _);
        }
        
        
    }
}