using System;
using System.Collections.Generic;
using System.Threading;
using UnityTest;
using NUnit.Framework;
using UnityEngine;

namespace UnityTest {
	[TestFixture]
	[Category("SensorData")]
	internal class sensorDataTests {
		[Test]
		public void SensorDataWriteTest(){
			double[] initialValue   = { 0.5, 0.6, 0.7, 0.8, 0.9, 1.0, 1.1, 1.2, 1.3, 1.4, 1.5, 1.6 };
			double[] expectedResult = { 1.4, 1.5, 1.6, 1.1, 1.2, 1.3, 2.1, 2.2, 2.3, 3.1, 3.2, 3.3 };
			SensorData sensorData = new SensorData (3, 4, initialValue);
			sensorData.write (samplePoint1);
			sensorData.write (samplePoint2);
			sensorData.write (samplePoint3);
			Assert.AreEqual (expectedResult, sensorData.x ());
		}

		[Test]
		public void partiallyEmptySensorDataWriteTest(){
			double[] expectedResult = { 0.0, 0.0, 0.0, 1.1, 1.2, 1.3, 2.1, 2.2, 2.3 };
			SensorData sensorData = new SensorData (3, 3);
			sensorData.write ( samplePoint1 );
			sensorData.write ( samplePoint2 );
			Assert.AreEqual (expectedResult, sensorData.x ());
		}

		[Datapoint] public double[] samplePoint1 = { 1.1, 1.2, 1.3 };
		[Datapoint]	public double[] samplePoint2 = { 2.1, 2.2, 2.3 };
		[Datapoint]	public double[] samplePoint3 = { 3.1, 3.2, 3.3 };
		[Datapoint]	public double[] samplePoint4 = { 4.1, 4.2, 4.3 };
		[Datapoint]	public double[] samplePoint5 = { 5.1, 5.2, 5.3 };
		[Datapoint]	public double[] samplePoint6 = { 6.1, 6.2, 6.3 };
		[Datapoint]	public double[] samplePoint7 = { 7.1, 7.2, 7.3 };
		[Datapoint]	public double[] samplePoint8 = { 8.1, 8.2, 8.3 };
		[Datapoint]	public double[] samplePoint9 = { 9.1, 9.2, 9.3 };
		[Datapoint]	public double[] samplePoint10 = {10.1,10.2,10.3 };
		[Datapoint]	public double[] samplePoint11 = {11.1,11.2,11.3 };
		[Datapoint]	public double[] samplePoint12 = {12.1,12.2,12.3 };
		[Datapoint]	public double[] samplePoint13 = {13.1,13.2,13.3 };
		[Datapoint]	public double[] samplePoint14 = {14.1,14.2,14.3 };
		[Datapoint]	public double[] samplePoint15 = {15.1,15.2,15.3 };
		[Datapoint]	public double[] samplePoint16 = {16.1,16.2,16.3 };
	}
}
