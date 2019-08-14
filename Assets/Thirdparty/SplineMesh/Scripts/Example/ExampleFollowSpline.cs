using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SplineMesh {
    /// <summary>
    /// Example of component to show that the spline is an independant mathematical component and can be used for other purposes than mesh deformation.
    /// 
    /// This component is only for demo purpose and is not intended to be used as-is.
    /// 
    /// We only move an object along the spline. Imagine a camera route, a ship patrol...
    /// </summary>
    public class ExampleFollowSpline : MonoBehaviour {
        public Spline spline;
        public float distance = 0;

        public GameObject follower;
        public float duration;

        private void Reset() {
            spline = GetComponent<Spline>(); 

        }
		void Update() {
            distance += Time.deltaTime / duration;
            if (distance > spline.nodes.Count - 1) {
                distance -= spline.nodes.Count - 1;
            }
            PlaceFollower();
        }

        private void PlaceFollower() {
            if (follower != null) {
                CurveSample sample = spline.GetSample(distance);
                follower.transform.localPosition = sample.location;
                follower.transform.localRotation = sample.Rotation;
            }
        }
    }
}
