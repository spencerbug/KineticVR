using System;
using System.Collections.Generic;
using System.Threading;
using UnityTest;
using NUnit.Framework;
using UnityEngine;


namespace UnityTest
{

	[TestFixture]
	[Category("alglib")]
	internal class dforestTests : alglib {

		/*************************************************************************
		        Processing functions test
		*************************************************************************/
		private static void testprocessing(ref bool err)
		{
			int nvars = 0;
			int nclasses = 0;
			int nsample = 0;
			int ntrees = 0;
			int flags = 0;
			dforest.decisionforest df1 = new dforest.decisionforest();
			dforest.decisionforest df2 = new dforest.decisionforest();
			int npoints = 0;
			double[,] xy = new double[0,0];
			int pass = 0;
			int passcount = 0;
			int i = 0;
			int j = 0;
			bool allsame = new bool();
			int info = 0;
			dforest.dfreport rep = new dforest.dfreport();
			double[] x1 = new double[0];
			double[] x2 = new double[0];
			double[] y1 = new double[0];
			double[] y2 = new double[0];
			double v = 0;

			passcount = 100;

			//
			// Main cycle
			//
			for(pass=1; pass<=passcount; pass++)
			{

				//
				// initialize parameters
				//
				nvars = 1+math.randominteger(5);
				nclasses = 1+math.randominteger(3);
				ntrees = 1+math.randominteger(4);
				flags = 0;
				if( (double)(math.randomreal())>(double)(0.5) )
				{
					flags = flags+2;
				}

				//
				// Initialize arrays and data
				//
				npoints = 10+math.randominteger(50);
				nsample = Math.Max(10, math.randominteger(npoints));
				x1 = new double[nvars-1+1];
				x2 = new double[nvars-1+1];
				y1 = new double[nclasses-1+1];
				y2 = new double[nclasses-1+1];
				xy = new double[npoints-1+1, nvars+1];
				for(i=0; i<=npoints-1; i++)
				{
					for(j=0; j<=nvars-1; j++)
					{
						if( j%2==0 )
						{
							xy[i,j] = 2*math.randomreal()-1;
						}
						else
						{
							xy[i,j] = math.randominteger(2);
						}
					}
					if( nclasses==1 )
					{
						xy[i,nvars] = 2*math.randomreal()-1;
					}
					else
					{
						xy[i,nvars] = math.randominteger(nclasses);
					}
				}

				//
				// create forest
				//
				//dforest.dfbuildinternal(xy, npoints, nvars, nclasses, ntrees, nsample, nfeatures, flags, ref info, df1, rep);
				dforest.dfbuildrandomdecisionforest (xy, npoints, nvars, nclasses, ntrees, 0.5, ref info, df1, rep);
				if( info<=0 )
				{
					err = true;
					return;
				}

				//
				// Same inputs leads to same outputs
				//
				for(i=0; i<=nvars-1; i++)
				{
					x1[i] = 2*math.randomreal()-1;
					x2[i] = x1[i];
				}
				for(i=0; i<=nclasses-1; i++)
				{
					y1[i] = 2*math.randomreal()-1;
					y2[i] = 2*math.randomreal()-1;
				}
				dforest.dfprocess(df1, x1, ref y1);
				dforest.dfprocess(df1, x2, ref y2);
				allsame = true;
				for(i=0; i<=nclasses-1; i++)
				{
					allsame = allsame && (double)(y1[i])==(double)(y2[i]);
				}
				err = err || !allsame;

				//
				// Same inputs on original forest leads to same outputs
				// on copy created using DFCopy
				//
				unsetdf(df2);
				dforest.dfcopy(df1, df2);
				for(i=0; i<=nvars-1; i++)
				{
					x1[i] = 2*math.randomreal()-1;
					x2[i] = x1[i];
				}
				for(i=0; i<=nclasses-1; i++)
				{
					y1[i] = 2*math.randomreal()-1;
					y2[i] = 2*math.randomreal()-1;
				}
				dforest.dfprocess(df1, x1, ref y1);
				dforest.dfprocess(df2, x2, ref y2);
				allsame = true;
				for(i=0; i<=nclasses-1; i++)
				{
					allsame = allsame && (double)(y1[i])==(double)(y2[i]);
				}
				err = err || !allsame;

				//
				// Same inputs on original forest leads to same outputs
				// on copy created using DFSerialize
				//
				unsetdf(df2);
				{
					//
					// This code passes data structure through serializers
					// (serializes it to string and loads back)
					//
					serializer _local_serializer;
					string _local_str;

					_local_serializer = new serializer();
					_local_serializer.alloc_start();
					dforest.dfalloc(_local_serializer, df1);
					_local_serializer.sstart_str();
					dforest.dfserialize(_local_serializer, df1);
					_local_serializer.stop();
					_local_str = _local_serializer.get_string();

					_local_serializer = new serializer();
					_local_serializer.ustart_str(_local_str);
					dforest.dfunserialize(_local_serializer, df2);
					_local_serializer.stop();
				}
				for(i=0; i<=nvars-1; i++)
				{
					x1[i] = 2*math.randomreal()-1;
					x2[i] = x1[i];
				}
				for(i=0; i<=nclasses-1; i++)
				{
					y1[i] = 2*math.randomreal()-1;
					y2[i] = 2*math.randomreal()-1;
				}
				dforest.dfprocess(df1, x1, ref y1);
				dforest.dfprocess(df2, x2, ref y2);
				allsame = true;
				for(i=0; i<=nclasses-1; i++)
				{
					allsame = allsame && (double)(y1[i])==(double)(y2[i]);
				}
				err = err || !allsame;

				//
				// Normalization properties
				//
				if( nclasses>1 )
				{
					for(i=0; i<=nvars-1; i++)
					{
						x1[i] = 2*math.randomreal()-1;
					}
					dforest.dfprocess(df1, x1, ref y1);
					v = 0;
					for(i=0; i<=nclasses-1; i++)
					{
						v = v+y1[i];
						err = err || (double)(y1[i])<(double)(0);
					}
					err = err || (double)(Math.Abs(v-1))>(double)(1000*math.machineepsilon);
				}
			}
		}

		/*************************************************************************
        Unsets DF
        *************************************************************************/
		private static void unsetdf(dforest.decisionforest df)
		{
			double[,] xy = new double[0,0];
			int info = 0;
			dforest.dfreport rep = new dforest.dfreport();

			xy = new double[0+1, 1+1];
			xy[0,0] = 0;
			xy[0,1] = 0;
			dforest.dfbuildinternal(xy, 1, 1, 1, 1, 1, 1, 0, ref info, df, rep);
		}
			

		[Test]
		public void dfProcessingTest(){
			bool wasErrors = new bool ();
			testprocessing (ref wasErrors);
			Assert.False(wasErrors);
		}
	}

}
