using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace.Gameplay.PainObject
{
    [RequireComponent(typeof(Renderer))]
    public class PaintObject : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        [SerializeField] private Renderer _mesh;

        public Action<Vector2Int> OnPointerDownAction;
        public Action<Vector2Int> OnDragAction;

        //force додана просто для спрощення, в реальному проекті бізнес логіка має бути іншою.
        public void InitializeMeshForTexture(bool force, Texture2D texture = null)
        {
            if(force) _mesh.material.mainTexture = null;
            if (texture == null && _mesh.material.mainTexture == null)
                _mesh.material.mainTexture = new Texture2D(256 * 6, 256 * 6);
            else
                _mesh.material.mainTexture = texture;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            if (!Physics.Raycast(ray, out RaycastHit hit))
            {
                return;
            }

            var tex = _mesh.material.mainTexture as Texture2D;
            var xCoordCenter = (int)(hit.textureCoord.x * tex.width);
            var yCoordCenter = (int)(hit.textureCoord.y * tex.height);
            OnPointerDownAction?.Invoke(new Vector2Int(xCoordCenter, yCoordCenter));
        }

        public void OnDrag(PointerEventData eventData)
        {
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            if (!Physics.Raycast(ray, out RaycastHit hit))
            {
                return;
            }

            var tex = _mesh.material.mainTexture as Texture2D;
            var xCoordCenter = (int)(hit.textureCoord.x * tex.width);
            var yCoordCenter = (int)(hit.textureCoord.y * tex.height);
            OnDragAction?.Invoke(new Vector2Int(xCoordCenter, yCoordCenter));
        }

        public void ApplyTextureChanges(List<PointColorLink> newPoints)
        {
            var tex = _mesh.material.mainTexture as Texture2D;
            foreach (var pointColorLink in newPoints)
            {
                tex.SetPixel(pointColorLink.Point.x, pointColorLink.Point.y, pointColorLink.Color);
            }

            tex.Apply();
        }
    }
}