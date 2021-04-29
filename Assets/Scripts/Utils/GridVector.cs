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
            get { return Mathf.Round(_x); }
        }

        public float y
        {
            get { return Mathf.Round(_y); }
        }

        public Vector2 origin
        {
            get { return new Vector2(_x, _y); }
        }

        public GridVector()
        {
            _x = _y = 0;
        }

        public GridVector(float x, float y)
        {
            _x = x;
            _y = y;
        }

        public GridVector(Vector2 pos)
        {
            _x = pos.x;
            _y = pos.y;
        }

        public GridVector(Vector3 pos)
        {
            _x = pos.x;
            _y = pos.y;
        }

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

        public static GridVector operator +(GridVector a, Vector3 b) => new GridVector(a.origin.x + b.x, a.origin.y + b.z);
        public static GridVector operator -(GridVector a, Vector3 b) => new GridVector(a.origin.x - b.x, a.origin.y - b.z);
        public static GridVector operator *(GridVector a, Vector3 b) => new GridVector(a.origin.x * b.x, a.origin.y * b.z);
        public static GridVector operator /(GridVector a, Vector3 b)
        {
            if (b.x == 0 || b.y == 0)
            {
                throw new DivideByZeroException();
            }

            return new GridVector(a.origin.x / b.x, a.origin.y / b.z);
        }

        public static GridVector operator +(GridVector a, Vector2 b) => new GridVector(a.origin + b);
        public static GridVector operator -(GridVector a, Vector2 b) => new GridVector(a.origin - b);
        public static GridVector operator *(GridVector a, Vector2 b) => new GridVector(a.origin.x * b.x, a.origin.y * b.y);
        public static GridVector operator /(GridVector a, Vector2 b)
        {
            if (b.x == 0 || b.y == 0)
            {
                throw new DivideByZeroException();
            }

            return new GridVector(a.origin.x / b.x, a.origin.y / b.y);
        }

        public static implicit operator Vector3(GridVector grid) => new Vector3(grid.x, 0, grid.y);
        public static implicit operator Vector2(GridVector grid) => new Vector2(grid.x, grid.y);

        public override string ToString()
        {
            return string.Format("({0}, {1})", x, y);
        }
    }

}
