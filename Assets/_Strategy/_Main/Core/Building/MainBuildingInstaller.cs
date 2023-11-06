using _Strategy._Main.Abstractions;
using UnityEngine;
using Zenject;


namespace _Strategy._Main.Core
{
    
    public sealed class MainBuildingInstaller : MonoInstaller
    {

        [SerializeField] private FractionMemberParallelInfoUpdater _fractionMemberParallelInfoUpdater;


        public override void InstallBindings()
        {
            Container.Bind<ITickable>().FromInstance(_fractionMemberParallelInfoUpdater);
            Container.Bind<IFractionMember>().FromComponentInChildren();
        }

        
    }
}