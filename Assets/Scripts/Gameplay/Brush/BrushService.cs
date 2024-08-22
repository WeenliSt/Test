using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace.Gameplay
{
    public class PointColorLink 
    {
        public Vector2Int Point;
        public Color Color;

        public PointColorLink(Vector2Int point, Color color)
        {
            Point = point;
            Color = color;
        }
    }
    public class BrushServiceData
    {
        public Color Color;
        public int Size;
    }
    public class BrushService : IService
    {
        private readonly BrushServiceData _brushServiceData = new()
        {
            Size = 10,
            Color = Color.green
        }; 
        
        
        public List<PointColorLink> GetPaintedCoords(Vector2Int startPoint, Vector2Int endPoint)
        {
            return GetPaintedCoords(startPoint, endPoint, _brushServiceData.Size, _brushServiceData.Color);
        }
        public List<PointColorLink> GetPaintedCoords(Vector2Int startPoint, Vector2Int endPoint, int size, Color color)
        {
            var line = CalculateBresenhamLine(startPoint, endPoint, color);
            var result = new HashSet<PointColorLink>(line);
            if (size == 1) return line;
            foreach (var point in line)
            {
                var newCirclePoints = GetCirclePoints(point.Point.x, point.Point.y, size);
                foreach (var circlePoint in newCirclePoints)
                {
                    result.Add(new PointColorLink(circlePoint, color));
                }
            }
            return result.ToList();
        }

        private List<PointColorLink> CalculateBresenhamLine(Vector2Int startPoint, Vector2Int endPoint, Color color)
        {
            var result = new List<PointColorLink>();
            Vector2Int lineDiv =
                new Vector2Int(Mathf.Abs(startPoint.x - endPoint.x), Mathf.Abs(startPoint.y - endPoint.y));
            var err = lineDiv.x - lineDiv.y;
            var sx = startPoint.x < endPoint.x ? 1 : -1;
            var sy = startPoint.y < endPoint.y ? 1 : -1;
            var x = startPoint.x;
            var y = startPoint.y;
            
            while (true)
            {
                result.Add(new PointColorLink(new Vector2Int(x,y), color));
                if (x == endPoint.x && y == endPoint.y)
                    break;
                int e2 = 2 * err;
                if (e2 > -lineDiv.y)
                {
                    err -= lineDiv.y;
                    x += sx;
                }
                if (e2 < lineDiv.x)
                {
                    err += lineDiv.x;
                    y += sy;
                }
            }
            return result;
        }
        static List<Vector2Int> GetCirclePoints(int centerX, int centerY, int radius)
        {
            var points = new List<Vector2Int>();

            int x = radius;
            int y = 0;
            int err = 0;

            while (x >= y)
            {
                // Включаем точки на окружности
                points.Add(new Vector2Int(centerX + x, centerY + y));
                points.Add(new Vector2Int(centerX + y, centerY + x));
                points.Add(new Vector2Int(centerX - y, centerY + x));
                points.Add(new Vector2Int(centerX - x, centerY + y));
                points.Add(new Vector2Int(centerX - x, centerY - y));
                points.Add(new Vector2Int(centerX - y, centerY - x));
                points.Add(new Vector2Int(centerX + y, centerY - x));
                points.Add(new Vector2Int(centerX + x, centerY - y));

                y += 1;
                err += 1 + 2 * y;

                if (2 * (err - x) + 1 > 0)
                {
                    x -= 1;
                    err += 1 - 2 * x;
                }
            }

            // Включаем точки внутри окружности
            List<Vector2Int> filledPoints = new List<Vector2Int>();
            for (int i = centerX - radius; i <= centerX + radius; i++)
            {
                for (int j = centerY - radius; j <= centerY + radius; j++)
                {
                    if (Math.Sqrt((i - centerX) * (i - centerX) + (j - centerY) * (j - centerY)) <= radius)
                    {
                        filledPoints.Add(new Vector2Int(i, j));
                    }
                }
            }

            return filledPoints;
        }        
        public void SetSize(string size)
        {
            if (int.TryParse(size, out var sizeInt))
                _brushServiceData.Size = sizeInt;
            
            _brushServiceData.Size = _brushServiceData.Size;
        }
        public void SetColor(Color color) => _brushServiceData.Color = color;
        public int GetSize() => _brushServiceData.Size;
        public Color GetColor() => _brushServiceData.Color;
        public void Initialize() { }
        public void PostInitialize() { }
        public void Dispose() {}
    }
}