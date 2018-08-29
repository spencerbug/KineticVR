using System;
using System.Collections.Generic;
using System.Threading;
using UnityTest;
using NUnit.Framework;
using UnityEngine;

namespace UnityTest
{
	[TestFixture]
	[Category("control")]
	internal class trainingDataTests{

		[Test]
		public void trainingDataWriteTest(){
			double[,] expectedData = {
				{ 1.1, 1.2, 1.3, 2.1, 2.2, 2.3, 1.0 },
				{ 3.1, 3.2, 3.3, 4.1, 4.2, 4.3, 1.0 },
				{ 5.1, 5.2, 5.3, 6.1, 6.2, 6.3, 1.0 }
			};
			double[,] actualData;
			TrainingData trainingData = new TrainingData ( 2, 3, 0.0);
			trainingData.yTarget = 1.0;
			trainingData.write (samplePoint1);
			trainingData.write (samplePoint2);
			trainingData.write (samplePoint3);
			trainingData.write (samplePoint4);
			trainingData.write (samplePoint5);
			trainingData.write (samplePoint6);
			trainingData.write (samplePoint7);
			trainingData.to2dArray (out actualData);
			Assert.AreEqual ( expectedData, actualData );
		}
		[Test]
		public void fillTrainingData(){
			double[,] expectedData = {
				{ 1.1, 1.2, 1.3, 2.1, 2.2, 2.3, 2.0 },
				{ 3.1, 3.2, 3.3, 4.1, 4.2, 4.3, 2.0 },
			};
			double[,] actualData;
			TrainingData trainingData = new TrainingData ( 2, 3, 0.0);
			trainingData.yTarget = 2.0;
			trainingData.write (samplePoint1);
			trainingData.write (samplePoint2);
			trainingData.write (samplePoint3);
			trainingData.write (samplePoint4);
			trainingData.to2dArray (out actualData);
			Assert.AreEqual (actualData, expectedData);
		}

		[Test]
		public void changeYTarget(){
			double[,] expectedData = {
				{ 1.1, 1.2, 1.3, 2.1, 2.2, 2.3, 3.1, 3.2, 3.3, 1.0 },
				{ 4.1, 4.2, 4.3, 5.1, 5.2, 5.3, 6.1, 6.2, 6.3, 1.0 },
				{ 7.1, 7.2, 7.3, 8.1, 8.2, 8.3, 9.1, 9.2, 9.3, 2.0 }
			};
			double[,] actualData;
			TrainingData trainingData = new TrainingData ( 3, 3, 1.0);
			trainingData.write (samplePoint1);
			trainingData.write (samplePoint2);
			trainingData.write (samplePoint3);
			trainingData.write (samplePoint4);
			trainingData.write (samplePoint5);
			trainingData.write (samplePoint6);
			trainingData.yTarget = 2.0;
			trainingData.write (samplePoint7);
			trainingData.write (samplePoint8);
			trainingData.write (samplePoint9);
			trainingData.to2dArray (out actualData);
			Assert.AreEqual (expectedData, actualData);

		}

		[Test]
		public void lastCompletedRow(){
			double[] expectedData = { 4.1, 4.2, 4.3, 5.1, 5.2, 5.3, 6.1, 6.2, 6.3, 1.0 };
			TrainingData trainingData = new TrainingData ( 3, 3, 1.0);
			trainingData.write (samplePoint1);
			trainingData.write (samplePoint2);
			trainingData.write (samplePoint3);
			trainingData.write (samplePoint4);
			trainingData.write (samplePoint5);
			trainingData.write (samplePoint6);
			trainingData.write (samplePoint7);

			Assert.AreEqual (expectedData, trainingData.getLastCompletedRow ());

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

