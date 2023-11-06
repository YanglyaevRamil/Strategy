using System.Collections.Concurrent;
using System.Threading.Tasks;
using _Strategy._Main.Abstractions.Commands;
using UniRx;
using UnityEngine;


namespace _Strategy._Main.Core
{
    
    public sealed class AutoAttackEvaluator : MonoBehaviour
    {
        
        public static ConcurrentDictionary<GameObject, AttackerParallelInfo> AttackerInfo = new();
        public static ConcurrentDictionary<GameObject, FractionMemberParallelInfo> FractionMembersInfo = new();

        public static Subject<Command> AutoAttackCommands = new();


        private void Update()
        {
            Parallel.ForEach(AttackerInfo, kvp => Evaluate(kvp.Key, kvp.Value));
        }


        private void Evaluate(GameObject go, AttackerParallelInfo info)
        {
            if (info.CurrentCommand is IMoveCommand)
                return;
            
            if (info.CurrentCommand is IAttackCommand && !(info.CurrentCommand is Command))
                return;

            var fractionInfo = default(FractionMemberParallelInfo);
            
            if (!FractionMembersInfo.TryGetValue(go, out fractionInfo))
                return;

            foreach (var (otherGo, otherFractionInfo) in FractionMembersInfo)
            {
                if (fractionInfo.Fraction == otherFractionInfo.Fraction)
                    continue;

                var distance = Vector3.Distance(fractionInfo.Position, otherFractionInfo.Position);
                if (distance > info.VisionRadius)
                    continue;
                
                AutoAttackCommands.OnNext(new Command(go, otherGo));
                break;
            }
            
        }
        

        public class FractionMemberParallelInfo
        {
            public Vector3 Position;
            public int Fraction;

            
            public FractionMemberParallelInfo(Vector3 position, int fraction)
            {
                Position = position;
                Fraction = fraction;
            }
        }


        public class AttackerParallelInfo
        {
            public ICommand CurrentCommand;
            public float VisionRadius;


            public AttackerParallelInfo(ICommand currentCommand, float visionRadius)
            {
                CurrentCommand = currentCommand;
                VisionRadius = visionRadius;
            }
        }
        
        
        public class Command
        {
            public GameObject Attacker;
            public GameObject Target;


            public Command(GameObject attacker, GameObject target)
            {
                Attacker = attacker;
                Target = target;
            }
        }
        
    }
}