using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Remorse.Utility
{
    public class GridVector
    {
        private float _x, _y, _z;

        public float x
        {
            get { return Mathf.Round(_x); }
        }

        public float y
        {
            get { return _y; }
        }

        public float z
        {
            get { return Mathf.Round(_z); }
        }

        public Vector3 origin
        {
            get { return new Vector3(_x, _y, _z); }
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }

        public Vector2 ToVector2()
        {
            return new Vector2(x, z);
        }

        public GridVector(float x = 0, float y = 0, float z = 0)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public GridVector(Vector2 pos)
        {
            _x = pos.x;
            _y = 0;
            _z = pos.y;
        }

        public GridVector(Vector3 pos)
        {
            _x = pos.x;
            _y = pos.y;
            _z = pos.z;
        }

        public static GridVector operator +(GridVector a, GridVector b) => new GridVector(a.x + b.x, a.y + b.y, a.z + b.z);
        public static GridVector operator -(GridVector a, GridVector b) => new GridVector(a.x - b.x, a.y - b.y, a.z - b.z);
        public static GridVector operator *(GridVector a, GridVector b) => new GridVector(a.x * b.x, a.y * b.y, a.z * b.z);
        public static GridVector operator /(GridVector a, GridVector b)
        {
            if (b.x == 0 || b.y == 0 || b.z == 0)
            {
                throw new DivideByZeroException();
            }

            return new GridVector(a.x / b.x, a.y / b.y, a.z / b.z);
        }

        public static GridVector operator +(GridVector a, Vector3 b) => new GridVector(a.origin + b);
        public static GridVector operator -(GridVector a, Vector3 b) => new GridVector(a.origin - b);
        public static GridVector operator *(GridVector a, Vector3 b) => new GridVector(a.origin.x * b.x, a.origin.y * b.y, a.origin.z * b.z);
        public static GridVector operator /(GridVector a, Vector3 b)
        {
            if (b.x == 0 || b.y == 0)
            {
                throw new DivideByZeroException();
            }

            return new GridVector(a.origin.x / b.x, a.origin.y / b.y, a.origin.z / b.z);
        }

        public static GridVector operator +(GridVector a, Vector2 b) => new GridVector(a.origin.x + b.x, a.origin.y, a.origin.z + b.y);
        public static GridVector operator -(GridVector a, Vector2 b) => new GridVector(a.origin.x - b.x, a.origin.y, a.origin.z - b.y);
        public static GridVector operator *(GridVector a, Vector2 b) => new GridVector(a.origin.x * b.x, a.origin.y, a.origin.z * b.y);
        public static GridVector operator /(GridVector a, Vector2 b)
        {
            if (b.x == 0 || b.y == 0)
            {
                throw new DivideByZeroException();
            }

            return new GridVector(a.origin.x / b.x, a.origin.y, a.origin.z / b.y);
        }

        public static implicit operator Vector3(GridVector grid) => new Vector3(grid.x, grid.y, grid.z);
        public static implicit operator Vector2(GridVector grid) => new Vector2(grid.x, grid.z);

        public static implicit operator GridVector(Vector3 pos) => new GridVector(pos);
        public static implicit operator GridVector(Vector2 pos) => new GridVector(pos);

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", x, y, z);
        }
    }

}
