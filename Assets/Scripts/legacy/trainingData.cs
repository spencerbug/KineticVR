using System;
using System.Collections.Generic;

public class TrainingData {
	public double yTarget;
	public Stack<double[]> data { get; internal set; }
	private int numSensors,	pointsPerRow, numCols, colIndex, rowIndex;
	public int waitIterations;

	public TrainingData( int _pointsPerRow=10, int _numSensors=6, double _yTarget=0.0){
		numSensors = _numSensors;
		pointsPerRow = _pointsPerRow;
		numCols = pointsPerRow * numSensors + 1;
		colIndex = 0;
		waitIterations = 0;
		yTarget = _yTarget;
		data = new Stack<double[]> ();
		data.Push (new double[numCols]);
		data.Peek () [numCols - 1] = yTarget;
		//data = new double[numRows, numCols];
		//data[rowIndex,numCols-1] = yTarget;
	}

	public double writeDataWithinMagnitude(double[] writeData, double minThreshold, double maxThreshold){
		double magn = 0.0;
		for (int i = 0; i < numSensors; i++)
			magn += (writeData [i] * writeData [i]);
		magn = Math.Sqrt (magn);
		if (magn >= minThreshold && magn < maxThreshold)
			write (writeData);
		return magn;
	}

	public void write(double[] writeData){
		if (waitIterations > 0) {
			waitIterations--;
			return;
		}
		if (colIndex >= numCols-1) {
			data.Push (new double[numCols]);
			colIndex = 0;
		}
		for (int i = 0; i < numSensors; i++) {
			data.Peek () [colIndex] = writeData [i];
			//data [rowIndex, colIndex] = writeData [i];
			colIndex++;
		}
		data.Peek () [numCols - 1] = yTarget;
		//data[rowIndex,numCols-1] = yTarget;
	}

	public double[] getLastCompletedRow(){
		if (colIndex < numCols - 1)
			data.Pop ();
		return (double[])data.Peek ();
	}

	public void to2dArray(out double[,] arr){
		if (colIndex < numCols - 1)
			data.Pop ();
		int numRows = data.Count;
		arr = new double[numRows, numCols];
		for (int row = numRows-1; row >= 0; row--) {
			double[] currRow = data.Pop ();
			for (int col = 0; col < numCols; col++) {
				arr [row, col] = currRow [col];
			}
		}
	}
};
