﻿using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Runtime.InteropServices;


public class testRandomForest : MonoBehaviour {


	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator testTrainedDataWithEnumeratorPasses() {
		GameObject go = new GameObject ();
		randomForest rf = new randomForest ();
		csvData csv = go.AddComponent<csvData> () as csvData;
		csv.readCSVFile("jumping_data", "1", true);
		object[][] dataset = csv.aggregateDataByName(new []{"label","locationTimestamp_since1970(s)","accelerometerAccelerationX(G)"},
			new []{typeof(int),typeof(double), typeof(double)});
		
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}
}
