using _Strategy._Main.Abstractions;
using _Strategy._Main.Utils.UniRxExtensions;
using UnityEngine;


namespace _Strategy._Main.UserControlSystem.UI.Model
{
    
    [CreateAssetMenu(fileName = nameof(AttackableValue), menuName = "Configs/" + nameof(AttackableValue))]
    public sealed class AttackableValue : StatelessScriptableObjectValueBase<IAttackable>
    {
        
        
    }
}