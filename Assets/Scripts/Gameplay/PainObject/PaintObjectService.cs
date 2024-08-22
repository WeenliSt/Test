using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace DefaultNamespace.Gameplay.PainObject
{
    [Serializable]
    public class PaintLine
    {
        public Vector2Int Start;
        public Vector2Int End;
        public int Size;
        [JsonConverter(typeof(ColorHandler))]public Color Color;

        public PaintLine(Vector2Int start, Vector2Int end, int size, Color color)
        {
            Start = start;
            End = end;
            Size = size;
            Color = color;
        }
    }
    [Serializable]
    public class PaintData
    {
        public int SaveIndex;
        public List<PaintLine> Points = new();
    }

    [Serializable]
    public class PaintServiceData
    {
        public List<PaintData> PaintData = new();
    }
    public class PaintObjectService : DataService<PaintServiceData>
    {
        protected override string FILE_NAME => "PaintServiceData";
        
        private PrefabLoader _prefabLoader;
        private PaintObject _paintObject;
        private BrushService _brushService;
        private Transform _paintObjectParent;

        private PaintData _currentDrawPaintData;
        
        private Vector2Int _lastPoint;

        public Action OnSave;
        
        public PaintObjectService(BrushService brushService, Transform paintObjectParent)
        {
            _brushService = brushService;
            _paintObjectParent = paintObjectParent;
            _prefabLoader = new PrefabLoader();
        }


        protected override void OnDataInitialized()
        {
            _paintObject = _prefabLoader.LoadPrefabSync<PaintObject>(PrefabName.PaintObject);
            _paintObject.transform.SetParent(_paintObjectParent);
            _paintObject.transform.localPosition = Vector3.zero;
            ClearAll();
            _paintObject.OnDragAction += OnDrag;
            _paintObject.OnPointerDownAction += OnPointerDown;
        }

        protected override void OnDispose()
        {
            _paintObject.OnDragAction -= OnDrag;
            _paintObject.OnPointerDownAction -= OnPointerDown;
        }

        private void OnPointerDown(Vector2Int point)
        {
            _lastPoint = point;
        }
        
        private void OnDrag(Vector2Int point)
        {
            var points = _brushService.GetPaintedCoords(_lastPoint, point);
            _currentDrawPaintData.Points.Add(new PaintLine(_lastPoint, point, _brushService.GetSize(), _brushService.GetColor()));
            _lastPoint = point;
            _paintObject.ApplyTextureChanges(points);
            DataUpdated();
        }
        
        public List<int> GetSavesIndexes()
        {
            return _data.PaintData.Select(x => x.SaveIndex).OrderBy(x => x).ToList();
        }
        public void LoadSave(int saveIndex)
        {
            ClearAll(true);
            _currentDrawPaintData = _data.PaintData.FirstOrDefault(x => x.SaveIndex == saveIndex);
            if (_currentDrawPaintData == null)
            {
                return;
            }

            var points = new List<PointColorLink>();
            foreach (var point in _currentDrawPaintData.Points)
            {
                points.AddRange(_brushService.GetPaintedCoords(point.Start, point.End, point.Size, point.Color));
            }

            _paintObject.ApplyTextureChanges(points);
        }

        public void Save()
        {
            if (_data.PaintData.All(x => x.SaveIndex != _currentDrawPaintData.SaveIndex))
            {
                _data.PaintData.Add(_currentDrawPaintData);
            }
            TrySaveData();
            ClearAll(true);
            OnSave?.Invoke();
        }
        public void ClearAll(bool force = false)
        {
            _paintObject.InitializeMeshForTexture(force:force);
            _currentDrawPaintData = CreateNewPaintData();
        }

        private PaintData CreateNewPaintData()
        {
            var paintData = new PaintData();
            paintData.SaveIndex = _data.PaintData.Count;
            return paintData;
        }
    }
}