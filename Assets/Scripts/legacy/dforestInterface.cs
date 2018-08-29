using UnityEngine;
using System.Collections;

public interface dforestInterface
{
	int train (ref double[,] xy, int nclasses, int ntrees, double r);
	int predict (double[] x, double[] confidenceArray);
	double rmserror(ref double[,] testxy, int npoints);
	void serialize (out string s_out);
	void unserialize(string s_in);
}
