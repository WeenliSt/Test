using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class LaunchView : MonoBehaviour
    {
        [SerializeField] private Image _fillImage;
        [SerializeField] private GameObject _contentHandler;

        public void SetFillValue(float fillValue)
        {
            _fillImage.fillAmount = fillValue;
        }
        public void ToggleContentHandler(bool value)
        {
            _contentHandler.SetActive(value);
        }
    }
}