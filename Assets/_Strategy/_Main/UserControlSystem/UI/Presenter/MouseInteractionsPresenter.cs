using System.Linq;
using _Strategy._Main.Abstractions;
using _Strategy._Main.UserControlSystem.UI.Model;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using static _Strategy._Main.Utils.UniRxExtensions.UniRxExtensions;


namespace _Strategy._Main.UserControlSystem.UI.Presenter
{
    
    internal sealed class MouseInteractionsPresenter : MonoBehaviour
    {

        [SerializeField] private EventSystem _eventSystem;
        [SerializeField] private Camera _camera;
        [SerializeField] private SelectableValue _selectedObject;
        
        [SerializeField] private Vector3Value _groundClicksRMB;
        [SerializeField] private AttackableValue _attackablesRMB;
        [SerializeField] private Vector3Value _commandClicksRMB;
        
        [SerializeField] private Transform _groundTransform;

        private Plane _groundPlane;



        [Inject]
        private void Init()
        {
            _groundPlane = new Plane(_groundTransform.up, 0.0f);
            
            var nonBlockedByUIFrameStream = Observable.EveryUpdate()
                .Where(_ => !_eventSystem.IsPointerOverGameObject());

            var leftClickStream = nonBlockedByUIFrameStream.Where(_ => Input.GetMouseButtonUp(0));
            var rightClickStream = nonBlockedByUIFrameStream.Where(_ => Input.GetMouseButtonUp(1));

            var lmbRays = leftClickStream.Select(_ => _camera.ScreenPointToRay(Input.mousePosition));
            var rmbRays = rightClickStream.Select(_ => _camera.ScreenPointToRay(Input.mousePosition));

            var lmbHitsStream = lmbRays.Select(ray => Physics.RaycastAll(ray));
            var rmbHitsStream = rmbRays.Select(ray => (ray, Physics.RaycastAll(ray)));
            
            lmbHitsStream.Subscribe(hits =>
            {
                if (HitByInterface<ISelectable>(hits, out var selectable))
                    _selectedObject.SetValue(selectable);
            });

            rmbHitsStream.Subscribe((ray, hits) =>
            {
                if (HitByInterface<IAttackable>(hits, out var attackable))
                    _attackablesRMB.SetValue(attackable);
                
                else if (_groundPlane.Raycast(ray, out var enter))
                {
                    _groundClicksRMB.SetValue(ray.origin + ray.direction * enter);
                    _commandClicksRMB.SetValue(ray.origin + ray.direction * enter);
                }
                    
            });

        }
        

        private bool HitByInterface<T>(RaycastHit[] hits, out T result) where T : class
        {
            result = default;
            
            if (hits.Length > 0)
            {
                result = hits
                    .Select(hit => hit.collider.GetComponentInParent<T>())
                    .FirstOrDefault(s => s != null);
            }
            return result != default;
        }
    
        
    }
}

