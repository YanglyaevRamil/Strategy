using _Strategy._Main.Abstractions;
using UnityEngine;
using Zenject;


namespace _Strategy._Main.Core
{
    
    public class CoreInstaller : MonoInstaller
    {

        [SerializeField] private GameStatus _gameStatus; 
        
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<TimeModel>().AsSingle();
            Container.Bind<IGameStatus>().FromInstance(_gameStatus);
        }
        
        
    }
}