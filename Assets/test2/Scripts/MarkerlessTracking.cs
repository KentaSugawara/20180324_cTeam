using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Kudan.AR;

/// <summary>
/// SampleApp を いじったもの
/// </summary>
public class MarkerlessTracking : MonoBehaviour
{
    public KudanTracker _kudanTracker;  // The tracker to be referenced in the inspector. This is the Kudan Camera object.
    [SerializeField]
    EggSpawner _eggSpawner = null;
    [SerializeField]
    EggSearcher _eggSearcher = null;
    [SerializeField]
    GameObject _markerlessObj = null;

    private void OnEnable()
    {
        SetActiveArbiTrack(false);
    }

    private void Update()
    {
        if (!_kudanTracker.ArbiTrackIsTracking() && _eggSearcher.IsSearching)
        {
            SetActiveArbiTrack(false);
            SetActiveArbiTrack(true);
        }
    }

    public void SetActiveArbiTrack(bool b)
    {
        if (b)
        {
            // from the floor placer.
            Vector3 floorPosition;          // The current position in 3D space of the floor
            Quaternion floorOrientation;    // The current orientation of the floor in 3D space, relative to the device

            _kudanTracker.FloorPlaceGetPose(out floorPosition, out floorOrientation);   // Gets the position and orientation of the floor and assigns the referenced Vector3 and Quaternion those values
            _kudanTracker.ArbiTrackStart(floorPosition, floorOrientation);              // Starts markerless tracking based upon the given floor position and orientations
            _eggSearcher.StartSearching();
        }
        else
        {
            _kudanTracker.ArbiTrackStop();
            _eggSearcher.StopSearching();
        }
    }
}
