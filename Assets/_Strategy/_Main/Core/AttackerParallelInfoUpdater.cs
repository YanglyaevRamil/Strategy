using _Strategy._Main.Abstractions;
using _Strategy._Main.Abstractions.Commands;
using UnityEngine;
using Zenject;


namespace _Strategy._Main.Core
{
    
    public sealed class AttackerParallelInfoUpdater : MonoBehaviour, ITickable
    {

        [Inject] private IAutomaticAttacker _automaticAttacker;
        [Inject] private ICommandsQueue _commandsQueue;

        
        public void Tick()
        {

            AutoAttackEvaluator.AttackerInfo.AddOrUpdate(
                gameObject,
                new AutoAttackEvaluator.AttackerParallelInfo( _commandsQueue.CurrentCommand, _automaticAttacker.VisionRadius),
                (go, value) =>
                {
                    value.VisionRadius = _automaticAttacker.VisionRadius;
                    value.CurrentCommand = _commandsQueue.CurrentCommand;
                    return value;
                });
        }


        private void OnDestroy()
        {
            AutoAttackEvaluator.AttackerInfo.TryRemove(gameObject, out _);
        }
        

    }
}