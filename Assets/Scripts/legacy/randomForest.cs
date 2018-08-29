using UnityEngine;
using System.Collections;

public class randomForest : alglib {
	private dforest.decisionforest df;
	private int npoints, nvars, nclasses, ntrees, r;
	private serializer srly;
	private double confidenceThreshold;
	private dforest.dfreport rep;

	private int oldPredictionIndex = 0;
		
	public randomForest(double _confidenceThreshold=0.5){
		df = new dforest.decisionforest ();
		rep = new dforest.dfreport ();
		srly = new serializer ();
		confidenceThreshold = _confidenceThreshold;
	}

	public int train(ref double[,] xy, int _nclasses, int _ntrees, double r){
		int ret = -1;
		//dforest.dfbuildrandomdecisionforest (xy, npoints, nvars, nclasses, ntrees, r, ref ret, df, rep);
		dforest.dfbuildrandomdecisionforest (xy, xy.GetLength(0), xy.GetLength(1)-1, _nclasses, _ntrees, r, ref ret, df, rep);
		nclasses = _nclasses;
		return ret;
	}
		

	public int predict (double[] x, double[] weightArray){
		double[] y = new double[nclasses];
		dforest.dfprocess (df, x, ref y);
		double prediction = 0.0;
		double avg_exclude_prediction = 0.0;
		int highestIndex = 0;

		for (int i = 0; i < y.Length; i++) {
			y [i] *= weightArray[i];
			if (y [i] > prediction)
			{
				prediction = y [i];
				highestIndex = i;
			}
		}

		//only return new prediction if it is threshold % higher than the average of other values
		for (int j = 0; j < y.Length; j++) {
			if (j == highestIndex)
				break;
			avg_exclude_prediction += y [j];
		}
		avg_exclude_prediction /= y.Length;

		if ( (prediction / avg_exclude_prediction) >= (1.0 + confidenceThreshold) ) {
			oldPredictionIndex = highestIndex;
			return highestIndex;
		}
		else
			return oldPredictionIndex;
	}
	public double rmserror(ref double[,] testxy, int npoints){
		return dforest.dfrmserror (df, testxy, npoints);
	}
	public void serialize (out string s_out){
		srly.alloc_start ();
		dforest.dfalloc (srly, df);
		srly.sstart_str ();
		dforest.dfserialize (srly, df);
		srly.stop ();
		s_out = srly.get_string ();
	}
	public void unserialize (string s_in){
		srly.ustart_str (s_in);
		dforest.dfunserialize (srly, df);
		srly.stop ();
	}
}