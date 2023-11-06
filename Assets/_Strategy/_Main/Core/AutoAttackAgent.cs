using _Strategy._Main.Abstractions;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;


namespace _Strategy._Main.Core
{
    
    public sealed class AutoAttackAgent : MonoBehaviour
    {

        [FormerlySerializedAs("_chomperCommandsQueue")]
        [SerializeField] private UnitCommandsQueue unitCommandsQueue;

        
        private void Start()
        {
            AutoAttackEvaluator.AutoAttackCommands
                .ObserveOnMainThread()
                .Where(command => command.Attacker == gameObject)
                .Where(command => command.Attacker != null && command.Target != null)
                .Subscribe(command => AutoAttack(command.Target))
                .AddTo(this);
        }


        private void AutoAttack(GameObject target)
        {
            unitCommandsQueue.Clear();
            unitCommandsQueue.EnqueueCommand(new AutoAttackUnitCommand(target.GetComponent<IAttackable>()));
        }
        

    }
}