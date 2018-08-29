using System;
using System.Collections.Generic;

public class SensorData {
	public Queue<double> queue { get; internal set; }
	private int numSensors, numPoints;
	public int size { get; internal set; }


	public SensorData( int _numSensors, int _numPoints, double[] initialValues){
		numSensors = _numSensors;
		numPoints = _numPoints;
		size = numSensors * numPoints;
		queue = new Queue<double>(size);
		for (int i = 0; i < initialValues.Length; i++) {
			queue.Enqueue (initialValues [i]);
		}
	}


	public SensorData(int _numSensors, int _numPoints){
		numSensors = _numSensors;
		numPoints = _numPoints;
		size = numSensors * numPoints;
		queue = new Queue<double>(size);
		for (int i = 0; i < size; i++) {
			queue.Enqueue (0.0);
		}
	}
	public void write(double[] sensorPoint){
		for (int i = 0; i < numSensors; i++) {
			queue.Dequeue ();
			queue.Enqueue (sensorPoint [i]);
		}
	}
	public double[] x(){
		return queue.ToArray();
	}
};

