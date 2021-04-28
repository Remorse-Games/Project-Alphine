using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Remorse.Utility
{
    public class Grid
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

        public Grid(float x, float y)
        {
            _x = Mathf.Round(x);
            _y = Mathf.Round(y);
        }

        public Grid(Vector2 pos)
        {
            _x = Mathf.Round(pos.x);
            _y = Mathf.Round(pos.y);
        }

        public Grid(Vector3 pos)
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

        public static implicit operator Vector3(Grid grid) => new Vector3(grid.x, 0, grid.y);
        public static implicit operator Vector2(Grid grid) => new Vector2(grid.x, grid.y);

        public static Grid operator +(Grid a, Grid b) => new Grid(a.x + b.x, a.y + b.y);
        public static Grid operator -(Grid a, Grid b) => new Grid(a.x - b.x, a.y - b.y);
        public static Grid operator *(Grid a, Grid b) => new Grid(a.x * b.x, a.y * b.y);
        public static Grid operator /(Grid a, Grid b)
        {
            if (b.x == 0 || b.y == 0)
            {
                throw new DivideByZeroException();
            }

            return new Grid(a.x / b.x, a.y / b.y);
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", _x, _y);
        }
    }

}
