 using System;
using System.Collections.Generic;
using System.Threading;
using UnityTest;
using NUnit.Framework;
using UnityEngine;

namespace UnityTest
{
	[TestFixture]
	[Category("randomForest")]
	internal class randomForestTests {
		
		[Test]
		public void train_decision_forest_test(){
			int pointsPerRow = 4;
			int numSensors = 3;
			int numClasses = 4;
			int numTrees = 2; 
			int expectedPrediction = 3;
			TrainingData trainingData = new TrainingData ( pointsPerRow, numSensors, 0.0);
			trainingData.write (samplePoint1);
			trainingData.write (samplePoint2);
			trainingData.write (samplePoint3);
			trainingData.write (samplePoint4);
			trainingData.yTarget = 1.0;
			trainingData.write (samplePoint5);
			trainingData.write (samplePoint6);
			trainingData.write (samplePoint7);
			trainingData.write (samplePoint8);
			trainingData.yTarget = 3.0;
			trainingData.write (samplePoint9);
			trainingData.write (samplePoint10);
			trainingData.write (samplePoint11);
			trainingData.write (samplePoint12);
			trainingData.write (samplePoint13);
			trainingData.write (samplePoint14);
			trainingData.write (samplePoint15);
			trainingData.write (samplePoint16);
			randomForest rf = new randomForest ();
			double[,] trainxy;
			trainingData.to2dArray (out trainxy);
			int ret = rf.train (ref trainxy, numClasses, numTrees, 0.5);
			double[] predictionPoint = { 13.7, 13.8, 14.1, 14.2, 14.3, 15.1, 15.2, 15.3, 16.1, 16.2, 16.3, 16.4 };

			int actualPrediction = rf.predict (predictionPoint, thresholdArray);
			Assert.AreEqual (1, ret);
			Console.WriteLine(actualPrediction.ToString());
			Assert.AreEqual (expectedPrediction, actualPrediction);

		}

		[Test]
		public void serializeDeserialize(){
			int pointsPerRow = 4;
			int numSensors = 3;
			int numClasses = 4;
			int numTrees = 2; 
			TrainingData trainingData = new TrainingData ( pointsPerRow, numSensors, 0.0);
			trainingData.write (samplePoint1);
			trainingData.write (samplePoint2);
			trainingData.write (samplePoint3);
			trainingData.write (samplePoint4);
			trainingData.yTarget = 1.0;
			trainingData.write (samplePoint5);
			trainingData.write (samplePoint6);
			trainingData.write (samplePoint7);
			trainingData.write (samplePoint8);
			trainingData.yTarget = 2.0;
			trainingData.write (samplePoint9);
			trainingData.write (samplePoint10);
			trainingData.write (samplePoint11);
			trainingData.write (samplePoint12);
			trainingData.write (samplePoint13);
			trainingData.yTarget = 3.0;
			trainingData.write (samplePoint14);
			trainingData.write (samplePoint15);
			trainingData.write (samplePoint16);
			randomForest rf1 = new randomForest ();
			randomForest rf2 = new randomForest ();
			double[,] trainxy;
			trainingData.to2dArray (out trainxy);
			rf1.train (ref trainxy, numClasses, numTrees, 0.5);
			String to_save;
			rf1.serialize (out to_save);
			rf2.unserialize (to_save);

			double[] predictionPoint = { 13.7, 13.8, 14.1, 14.2, 14.3, 15.1, 15.2, 15.3, 16.1, 16.2, 16.3, 16.4 };
			Assert.AreEqual (rf1.predict (predictionPoint, thresholdArray), rf2.predict (predictionPoint, thresholdArray));
		}
		[Datapoint]
		public double[] thresholdArray = {0.5D, 0.5D, 0.3D, 0.2D};
		[Datapoint]
		public double[] samplePoint1 = { 1.1, 1.2, 1.3 };
		[Datapoint]
		public double[] samplePoint2 = { 2.1, 2.2, 2.3 };
		[Datapoint]
		public double[] samplePoint3 = { 3.1, 3.2, 3.3 };
		[Datapoint]
		public double[] samplePoint4 = { 4.1, 4.2, 4.3 };
		[Datapoint]
		public double[] samplePoint5 = { 5.1, 5.2, 5.3 };
		[Datapoint]
		public double[] samplePoint6 = { 6.1, 6.2, 6.3 };
		[Datapoint]
		public double[] samplePoint7 = { 7.1, 7.2, 7.3 };
		[Datapoint]
		public double[] samplePoint8 = { 8.1, 8.2, 8.3 };
		[Datapoint]
		public double[] samplePoint9 = { 9.1, 9.2, 9.3 };
		[Datapoint]
		public double[] samplePoint10 = {10.1,10.2,10.3 };
		[Datapoint]
		public double[] samplePoint11 = {11.1,11.2,11.3 };
		[Datapoint]
		public double[] samplePoint12 = {12.1,12.2,12.3 };
		[Datapoint]
		public double[] samplePoint13 = {13.1,13.2,13.3 };
		[Datapoint]
		public double[] samplePoint14 = {14.1,14.2,14.3 };
		[Datapoint]
		public double[] samplePoint15 = {15.1,15.2,15.3 };
		[Datapoint]
		public double[] samplePoint16 = {16.1,16.2,16.3 };
	}
}
	