using UnityEngine;
using QuickOutline;


namespace _Strategy._Main.Abstractions
{
    
    public interface ISelectable : IHealthHolder, IIconHandler
    {
        
        Outline Outline { get; }

        Transform PivotPoint { get; }

    }
}