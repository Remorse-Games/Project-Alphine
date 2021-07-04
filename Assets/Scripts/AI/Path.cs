using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Remorse.Utility;

namespace Remorse.AI
{
    public class Node
    {
        public Node parent;
        public GridVector pos;

        public float g, h;

        public float f 
        {
            get 
            {
                return g + h;
            }
        }

        public Node(Node parent, GridVector pos, float g = 0, float h = 0)
        {
            this.parent = parent;
            this.pos = pos;
            
            this.g = g;
            this.h = h;
        }

        public Node(GridVector pos)
        {
            this.parent = null;
            this.pos = pos;
        
            g = h = 0;
        }

        public Node()
        {
            parent = null;
            pos = new GridVector();

            g = h = 0;
        }
    }

    public class Path
    {
        private List<GridVector> path = new List<GridVector>();

        private int avoidLayer = 0;

        public Path(int groundLayer, int playerLayer)
        {
            int unavoidableLayer = (1 << groundLayer) | (1 << playerLayer);
            avoidLayer = ~unavoidableLayer;
        }

        public void FindPath(GridVector start, GridVector target)
        {
            if (start.y != target.y)
                throw new System.ArgumentException("Start Y Position and Target Y Position Must Be Same!");

            Node startNode = new Node(start);
            Node endNode = new Node(target);

            List<Node> openSet = new List<Node>();
            List<Node> closeSet = new List<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];

                foreach (Node node in openSet)
                {
                    if (node.f < currentNode.f)
                    {
                        currentNode = node;
                    }
                }

                openSet.Remove(currentNode);
                closeSet.Add(currentNode);

                if (currentNode.pos == endNode.pos)
                {
                    List<GridVector> path = new List<GridVector>();

                    Node current = currentNode;

                    while (current != null)
                    {
                        path.Add(current.pos);
                        current = current.parent;
                    }

                    path.Reverse();

                    this.path = path;

                    return;
                }

                List<Node> children = new List<Node>();

                foreach (GridVector nextPos in currentNode.pos.Neighbour())
                {
                    //if (isWalkable(currentNode.pos, nextPos))
                    //    continue;

                    Node newNode = new Node(currentNode, nextPos);
                    children.Add(newNode);
                }

                foreach (Node child in children)
                {
                    foreach (Node closedChild in closeSet)
                    {
                        if (closedChild == child)
                        {
                            continue;
                        }
                    }

                    child.g = currentNode.g + 1;
                    child.h = GridVector.ManhattanDistance(currentNode.pos, endNode.pos);

                    foreach (Node openNode in openSet)
                    {
                        if (child == openNode && child.g > openNode.g)
                            continue;
                    }

                    openSet.Add(child);
                }
            }
        }

        private bool isWalkable(GridVector currentPos, GridVector target) => Physics.Raycast(currentPos, target - currentPos, 1, avoidLayer);

        public static implicit operator List<GridVector>(Path path) => path.path;

        public static explicit operator List<Vector3>(Path path)
        {
            List<Vector3> vectorList = new List<Vector3>();

            foreach (GridVector pos in path.path)
            {
                vectorList.Add(pos);
            }

            return vectorList;
        }
    }
}
