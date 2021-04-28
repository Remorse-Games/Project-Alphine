using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Remorse.Utility
{
    public class GridVector
    {
        private float _x, _y;

        public float x
        {
            get { return _x; }
            set
            {
                _x = Mathf.Round(value);
            }
        }

        public float y
        {
            get { return _y; }
            set
            {
                _y = Mathf.Round(value);
            }
        }

        public GridVector()
        {
            _x = _y = 0;
        }

        public GridVector(float x, float y)
        {
            _x = Mathf.Round(x);
            _y = Mathf.Round(y);
        }

        public GridVector(Vector2 pos)
        {
            _x = Mathf.Round(pos.x);
            _y = Mathf.Round(pos.y);
        }

        public GridVector(Vector3 pos)
        {
            _x = Mathf.Round(pos.x);
            _y = Mathf.Round(pos.y);
        }

        public Vector3 ToVector3()
        {
            return new Vector3(_x, 0, _y);
        }

        public Vector2 ToVector2()
        {
            return new Vector3(_x, _y);
        }

        public static implicit operator Vector3(GridVector grid) => new Vector3(grid.x, 0, grid.y);
        public static implicit operator Vector2(GridVector grid) => new Vector2(grid.x, grid.y);

        public static GridVector operator +(GridVector a, GridVector b) => new GridVector(a.x + b.x, a.y + b.y);
        public static GridVector operator -(GridVector a, GridVector b) => new GridVector(a.x - b.x, a.y - b.y);
        public static GridVector operator *(GridVector a, GridVector b) => new GridVector(a.x * b.x, a.y * b.y);
        public static GridVector operator /(GridVector a, GridVector b)
        {
            if (b.x == 0 || b.y == 0)
            {
                throw new DivideByZeroException();
            }

            return new GridVector(a.x / b.x, a.y / b.y);
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", _x, _y);
        }
    }

}
