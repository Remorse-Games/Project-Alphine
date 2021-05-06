using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Remorse.AI;
using Remorse.Utility;

namespace Tests
{
    public class PathFinderTest
    {
        Path path = new Path(new GridVector(), LayerMask.NameToLayer("Ground"), LayerMask.NameToLayer("Player"));

        // A Test behaves as an ordinary method
        [Test]
        public void Case1()
        {
            path.FindPath(new GridVector(3, 0, 0));
            List<GridVector> ActualResult = path;
            CollectionAssert.AreEqual(
                expected: new List<GridVector>
                {
                    new GridVector(0, 0, 0),
                    new GridVector(1, 0, 0),
                    new GridVector(2, 0, 0),
                    new GridVector(3, 0, 0)
                },
                actual: ActualResult
            );
        }

        [Test]
        public void Case2()
        {
            path.FindPath(new GridVector(2, 0, 2));
            List<GridVector> ActualResult = path;
            CollectionAssert.AreEqual(
                expected: new List<GridVector>
                {
                    new GridVector(0, 0, 0),
                    new GridVector(0, 0, 1),
                    new GridVector(1, 0, 1),
                    new GridVector(1, 0, 2),
                    new GridVector(2, 0, 2)
                },
                actual: ActualResult
            );
        }
    }
}
