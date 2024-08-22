using UnityEngine;

namespace DefaultNamespace.HUD.Core
{
    public class HUDContainer : MonoBehaviour
    {
        [SerializeField] private Transform _parentTransform;
        
        public Transform ParentTransform => _parentTransform;
    }
}