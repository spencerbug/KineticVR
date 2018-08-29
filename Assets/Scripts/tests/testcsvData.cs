using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityTest;


public class testtrainJumpData : MonoBehaviour{
	GameObject go;

	[Test]
	public void testtrainJumpDataSimplePasses() {
		// Use the Assert class to test conditions.
	}

	[UnityTest]
	public IEnumerator testReadCSVFiles (){
		go = new GameObject();
		csvData csv = go.AddComponent<csvData> () as csvData;
		csv.readCSVFile("jumping_data", "1", true);
		string[][] csvstringarray1 = csv.dataset.ToArray ();
		Assert.AreEqual ("2018-04-29 19:05:22.503 -0500", csvstringarray1 [0] [1]);
		Assert.AreEqual (119, csvstringarray1.Length);
		csv.readCSVFile ("jumping_data", label: "2", hasHeaders: true);
		string[][] csvstringarray2 = csv.dataset.ToArray ();
		Assert.AreEqual (238, csvstringarray2.Length);
		yield return null;
	}
		
	//test aggregating data into a list of column headers and type names.
	[UnityTest]
	public IEnumerator testAggregateCSVFile(){
		go = new GameObject();
		csvData csv = go.AddComponent<csvData> () as csvData;
		csv.readCSVFile("jumping_data", "1", true);
		object[][] x = csv.aggregateDataByName(new []{"label","locationTimestamp_since1970(s)","accelerometerAccelerationX(G)"},new []{typeof(int),typeof(double), typeof(double)});
		//print (x [0] [0]);
		Assert.AreEqual(1, x[0][0]);
		//Assert.AreEqual (1525046721.997436D, (double)x [0] [1]);
		Assert.AreEqual (-0.0343780517578125D, (double)x [1] [2]);
		yield return null;
	}

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator testtrainJumpDataWithEnumeratorPasses() {
		// Use the Assert class to test conditions.

		yield return null;
	}
}
