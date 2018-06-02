using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class DetectedPlanes : Singleton {

    public static List<DetectedPlane> detectedPlaneList = new List<DetectedPlane>();
    
	
	void Update () {
        
        Session.GetTrackables<DetectedPlane>(detectedPlaneList, TrackableQueryFilter.New);
	}
}
