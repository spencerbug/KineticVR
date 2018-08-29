using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class csvData : MonoBehaviour {
	public string fileDataTmp;
	public Queue<string[]> dataset { get; internal set; }
	public string[] headers;
	public string[][] dataArray;
	public csvData(){
		fileDataTmp = "";
		dataset = new Queue<string[]>();
		headers = null;
		dataArray = null;
	}

	//reads a csvfile
	public void readCSVFile(string resourcename, string label, bool hasHeaders){
		TextAsset csvfile = Resources.Load ("jumping_data") as TextAsset;
		string[] lines = csvfile.text.Split(new char[] { '\n','\r' });
		for (int i = 0; i < lines.Length; i++) {
			string[] entriesInCSV = lines [i].Split (',');
			string[] entries = new string[lines.Length + 1];
			if (i == 0 && hasHeaders == true) {
				entries [0] = "label";
			} else {
				entries [0] = label;
			}
			entriesInCSV.CopyTo (entries, 1);

			if (entries.Length > 1 && entries[1] != "") {
				if (hasHeaders==true && i == 0) {
					headers = new string[entries.Length+1];
					entries.CopyTo (headers, 0);
				} else {
					dataset.Enqueue (entries);
				}
			}

		}
		dataArray = dataset.ToArray ();
	}

	public object[][] aggregateDataByIndex(int[] requestedIndices, Type[] types){
		object[][] ret = new object[dataset.Count][];
		for (int row = 0; row < dataset.Count; row++) {
			ret [row] = new object[requestedIndices.Length];
			for (int col = 0; col < requestedIndices.Length; col++) {
				ret[row][col] = Convert.ChangeType(dataArray[row][requestedIndices[col]], types[col]);
			}
		}
		return ret;
	}

	//Returns an array of mixed-type arrays
	public object[][] aggregateDataByName(string[] requestedColumns, Type[] types){
		if (requestedColumns.Length != types.Length) {
			print ("arrayLengths are not equal for arguments of aggregateData");
			return new object[][]{null};
		}
		//get a list of column indices that match the header strings
		Queue<int> AggregateIndices = new Queue<int> ();
		for (int reqColIndex = 0; reqColIndex < requestedColumns.Length; reqColIndex++) {
			for (int headerIndex = 0; headerIndex < headers.Length; headerIndex++) {
				if (headers [headerIndex].Equals (requestedColumns [reqColIndex])) {
					AggregateIndices.Enqueue (headerIndex);

					break;
				}
			}
		}

		int[] aggInd = AggregateIndices.ToArray ();
		
		return aggregateDataByIndex (aggInd, types);

	}
		
}

