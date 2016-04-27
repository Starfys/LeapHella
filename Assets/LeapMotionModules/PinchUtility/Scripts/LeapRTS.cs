using UnityEngine;

namespace Leap.PinchUtility {

  /// <summary>
  /// Use this component on a Game Object to allow it to be manipulated by a pinch gesture.  The component
  /// allows rotation, translation, and scale of the object (RTS).
  /// </summary>
  [ExecuteAfter(typeof(LeapPinchDetector))]
  public class LeapRTS : MonoBehaviour {
    [SerializeField]
    private LeapPinchDetector _pinchDetectorA = null;

    [SerializeField]
    private LeapPinchDetector _pinchDetectorB = null;


    private float _defaultNearClip;

	//Custom variables
		public GameObject spawnPrefab;
		private bool currentlyPinching;
		private GameObject currentlySpawning;
		public float createThreshold = 0.25f;
		public float maxScale = 0.25f;
    void Awake() {
      if (_pinchDetectorA == null || _pinchDetectorB == null) {
        Debug.LogWarning("Both Pinch Detectors of the LeapRTS component must be assigned. This component has been disabled.");
        enabled = false;
      }
			currentlyPinching = false;

    }

    void Update() {

			if (currentlyPinching) {
				if (_pinchDetectorA.IsPinching && _pinchDetectorB.IsPinching &&
					Vector3.Distance (_pinchDetectorA.Position, _pinchDetectorB.Position) <= createThreshold) {
					currentlySpawning.transform.position = _pinchDetectorB.Position + 0.5f * (_pinchDetectorA.Position - _pinchDetectorB.Position);
					currentlySpawning.transform.LookAt (_pinchDetectorB.Position);
					float scale = (maxScale / createThreshold) * Vector3.Distance (_pinchDetectorA.Position, _pinchDetectorB.Position);
					currentlySpawning.transform.localScale = new Vector3 (scale, scale, scale);
				} else {
					currentlySpawning.GetComponent<Rigidbody> ().isKinematic = false;
					currentlyPinching = false;
				}
			} else {
				if (_pinchDetectorA.IsPinching && _pinchDetectorB.IsPinching &&
					Vector3.Distance (_pinchDetectorA.Position, _pinchDetectorB.Position) <= createThreshold) {
					currentlyPinching = true;
					currentlySpawning = Instantiate (spawnPrefab,_pinchDetectorB.Position + 0.5f * (_pinchDetectorA.Position - _pinchDetectorB.Position), new Quaternion (0, 0, 0, 0)) as GameObject;
					currentlySpawning.transform.LookAt (_pinchDetectorB.Position);
					float scale = (maxScale/ createThreshold) * Vector3.Distance (_pinchDetectorA.Position, _pinchDetectorB.Position);
					currentlySpawning.transform.localScale = new Vector3 (scale, scale, scale);
				}
			}
				
	  


    }

  }
}
