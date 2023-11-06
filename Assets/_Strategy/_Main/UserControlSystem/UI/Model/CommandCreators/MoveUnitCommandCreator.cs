using _Strategy._Main.Abstractions.Commands;
using _Strategy._Main.UserControlSystem.Commands;
using UnityEngine;


namespace _Strategy._Main.UserControlSystem.UI.Model.CommandCreators
{
    
    public sealed class MoveUnitCommandCreator : CancellableCommandCreatorBase<IMoveCommand, Vector3>
    {
        
        protected override IMoveCommand CreateCommand(Vector3 argument) => new MoveUnitCommand(argument);
        
    }
}