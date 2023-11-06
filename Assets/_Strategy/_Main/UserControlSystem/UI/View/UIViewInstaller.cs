using Zenject;


namespace _Strategy._Main.UserControlSystem.UI.View
{
    
    public sealed class UIViewInstaller : MonoInstaller
    {

        public override void InstallBindings()
        {
            Container
                .Bind<BottomCenterView>()
                .FromComponentInHierarchy()
                .AsSingle();
        }
        
        
    }
}