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
        // A Test behaves as an ordinary method
        [Test]
        public void FindPath()
        {
            Path path = new Path(new GridVector());
            path.FindPath(new GridVector(3, 0, 0));

            List<Vector3> ActualResult = (List<Vector3>)path;
            CollectionAssert.AreEqual(
                expected: new List<Vector3>
                {
                    new Vector3(0, 0, 0),
                    new Vector3(1, 0, 0),
                    new Vector3(2, 0, 0),
                    new Vector3(3, 0, 0)
                },
                actual: ActualResult
            );
        }
    }
}
