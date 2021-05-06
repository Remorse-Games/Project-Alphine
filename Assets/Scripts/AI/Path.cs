using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Remorse.Utility;

namespace Remorse.AI
{
    public class PathNode
    {
        public GridVector pos = new GridVector();
        public PathNode next = null;

        public PathNode(GridVector pos)
        {
            this.pos = pos;
        }
    }

    public class Path
    {
        private PathNode head;

        public int avoidLayer = 0;

        public Path(GridVector pos, int groundLayer, int playerLayer)
        {
            head = new PathNode(pos);

            int unavoidableLayer = (1 << groundLayer) | (1 << playerLayer);
            avoidLayer = ~unavoidableLayer;
        }

        public void FindPath(GridVector target)
        {
            // reset node
            if (head.next != null) head.next = null;

            PathNode currentNode = head;

            while (currentNode.pos != target)
            {
                GridVector nextNodePos = FindNextDirection(currentNode.pos, target);
                currentNode.next = new PathNode(nextNodePos);

                currentNode = currentNode.next;
            }
        }

        private GridVector FindNextDirection(GridVector pos, GridVector target)
        {
            /* find the closest point using dot product */

            GridVector nearestPoint = pos;
            float nearestPointMagnitude = (target - nearestPoint).magnitude;

            for (int i = 0; i < 4; i++)
            {
                GridVector direction = pos + GridVector.direction(i);

                if (Physics.Raycast(pos, direction, 1, avoidLayer))
                {
                    continue;
                }

                float magnitude = (target - direction).magnitude;

                if (magnitude < nearestPointMagnitude)
                {
                    nearestPoint = direction;
                    nearestPointMagnitude = magnitude;
                }
            }

            return nearestPoint;
        }

        public static implicit operator List<GridVector>(Path path)
        {
            List<GridVector> result = new List<GridVector>();
            PathNode currentNode = path.head;

            while (currentNode.next != null)
            {
                result.Add(currentNode.pos);
                currentNode = currentNode.next;

                if (currentNode.next == null)
                {
                    result.Add(currentNode.pos);
                }
            }

            return result;
        }

        public static explicit operator List<Vector3>(Path path)
        {
            List<Vector3> result = new List<Vector3>();
            PathNode currentNode = path.head;

            while (currentNode.next != null)
            {
                result.Add(currentNode.pos);
                currentNode = currentNode.next;

                if (currentNode.next == null)
                {
                    result.Add(currentNode.pos);
                }
            }

            return result;
        }
    }
}
