using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class testDataGenerator : MonoBehaviour {

	[Test]
	public void testDataGeneratorSimplePasses() {
		// Use the Assert class to test conditions.
	}

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator testDataGeneratorWithEnumeratorPasses() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}

	[UnityTest]
	public IEnumerator testDataGeneratorJustCode(){
		int i;
		//create an input simulator that generates a mocked series of inputs, each time it's called.
		for(i=0; i<10;i++){
			
		}
		yield return null;
	}
}
