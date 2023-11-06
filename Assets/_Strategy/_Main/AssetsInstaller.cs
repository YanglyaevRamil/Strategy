using System;
using _Strategy._Main.Abstractions;
using _Strategy._Main.UserControlSystem.UI.Model;
using _Strategy._Main.Utils.AssetsInjector;
using _Strategy._Main.Utils.AsyncExtensions;
using UnityEngine;
using Zenject;


namespace _Strategy._Main
{
    
    [CreateAssetMenu(fileName = "AssetsInstaller", menuName = "Installers/AssetsInstaller")]
    public class AssetsInstaller : ScriptableObjectInstaller<AssetsInstaller>
    {
        
        [SerializeField] private AssetsContext _legacyContext;
        [SerializeField] private Vector3Value _groundClicksRMB;
        [SerializeField] private SelectableValue _selectables;
        [SerializeField] private AttackableValue _attackableClicksRMB;
        [SerializeField] private Vector3Value _commandClicksRMB;
        

        [SerializeField] private Sprite _chomperSprite;


        public override void InstallBindings()
        {
            Container.BindInstances(_legacyContext, _groundClicksRMB, _attackableClicksRMB, _selectables, _commandClicksRMB);

            Container.Bind<IAwaitable<IAttackable>>().FromInstance(_attackableClicksRMB);
            Container.Bind<IAwaitable<Vector3>>().FromInstance(_groundClicksRMB);
            Container.Bind<IObservable<Vector3>>().WithId("Commands").FromInstance(_commandClicksRMB);
            Container.Bind<IObservable<ISelectable>>().FromInstance(_selectables);

            Container.Bind<Vector3Value>().WithId("GroundClicks").FromInstance(_groundClicksRMB);

            Container.Bind<Sprite>().WithId("Chomper").FromInstance(_chomperSprite);
        }
        
        
    }
}