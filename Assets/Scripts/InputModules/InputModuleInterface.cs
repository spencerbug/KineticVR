using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InputModuleInterface {
	string getName();
	float getRoll();
	float getYaw();
	float getTilt();
	float getX();
	float getY();
	float getZ ();
	int getA();
	int getB();
	int getSelect ();
	bool isEnabled ();
	void enable();
	void disable();

}


/*
 * input interface module
 * 
*/