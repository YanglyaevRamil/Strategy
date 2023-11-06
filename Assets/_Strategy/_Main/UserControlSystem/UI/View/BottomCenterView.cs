using System;
using _Strategy._Main.Abstractions;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;


namespace _Strategy._Main.UserControlSystem.UI.View
{
    
    public sealed class BottomCenterView : MonoBehaviour
    {

        [SerializeField] private GameObject[] _imageHolders;
        [SerializeField] private Image[] _images;
        [SerializeField] private Button[] _buttons;
        
        [SerializeField] private Slider _productionProgressSlider;
        [SerializeField] private TextMeshProUGUI _currentUnitName;

        public Subject<int> CancelButtonClicks = new Subject<int>();

        private IDisposable _unitProductionTaskCt;


        
        [Inject]
        private void Init()
        {
            for (int i = 0; i < _buttons.Length; i++)
            {
                var index = i;
                _buttons[i].onClick.AddListener(() => CancelButtonClicks.OnNext(index));
            }
        }


        public void Clear()
        {
            for (int i = 0; i < _images.Length; i++)
            {
                _images[i].sprite = null;
                _imageHolders[i].SetActive(false);
            }
            _productionProgressSlider.gameObject.SetActive(false);
            _currentUnitName.text = string.Empty;
            _currentUnitName.enabled = false;
            _unitProductionTaskCt?.Dispose();
        }


        public void SetTask(IUnitProductionTask task, int index)
        {
            if (task == null)
            {
                _imageHolders[index].SetActive(false);
                _images[index].sprite = null;

                if (index == 0)
                {
                    _productionProgressSlider.gameObject.SetActive(false);
                    _currentUnitName.text = String.Empty;
                    _currentUnitName.enabled = false;
                    _unitProductionTaskCt?.Dispose();
                }
            }
            else
            {
                _imageHolders[index].SetActive(true);
                _images[index].sprite = task.Icon;

                if (index == 0)
                {
                    _productionProgressSlider.gameObject.SetActive(true);
                    _currentUnitName.text = task.UnitName;
                    _currentUnitName.enabled = true;
                    _unitProductionTaskCt = Observable.EveryUpdate().Subscribe(_ =>
                        {
                            _productionProgressSlider.value = task.TimeLeft / task.ProductionTime;
                        }
                    );
                }
            }
        }
        
        
    }
}