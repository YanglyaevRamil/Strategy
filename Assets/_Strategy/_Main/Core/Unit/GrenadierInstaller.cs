﻿using _Strategy._Main.Abstractions;
using _Strategy._Main.Abstractions.Commands;
using UnityEngine;
using Zenject;


namespace _Strategy._Main.Core
{
    
    public class GrenadierInstaller : MonoInstaller
    {

        [SerializeField] private AttackerParallelInfoUpdater _attackerParallelInfoUpdater;
        [SerializeField] private FractionMemberParallelInfoUpdater _fractionMemberParallelInfoUpdater;
        

        public override void InstallBindings()
        {
            Container.Bind<IHealthHolder>().FromComponentInChildren();
            Container.Bind<float>().WithId("AttackDistance").FromInstance(2.0f);
            Container.Bind<int>().WithId("AttackPeriod").FromInstance(500);

            Container.Bind<IAutomaticAttacker>().FromComponentInChildren();
            Container.Bind<ITickable>().FromInstance(_attackerParallelInfoUpdater);
            Container.Bind<ITickable>().FromInstance(_fractionMemberParallelInfoUpdater);
            Container.Bind<IFractionMember>().FromComponentInChildren();
            Container.Bind<ICommandsQueue>().FromComponentInChildren();
        }
        
    }
}