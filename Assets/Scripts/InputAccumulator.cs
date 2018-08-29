using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//this interface will be implemented by the accelerometerInput, mockInput, and gameControllerInput classes
/* 8 inputs
 * Roll = R
 * TilT = T
 * Yaw = W
 * Fwd/Back = Y
 * Left/Right = X
 * A = Jump
 * B = Crouch
 * S = Select/Interact
 */
public class InputAccumulator 
{
	public List<InputModuleInterface> inputModules;
	public InputAccumulator(){
		inputModules = new List<InputModuleInterface> ();
	}
	public void addModule(InputModuleInterface module){
		inputModules.Add (module);
	}

	public void removeModule(string modulename){
		inputModules.RemoveAll (module => module.getName () == modulename);
	}

	public void disableModule (string modulename){
		inputModules.FindAll(module => module.getName() == modulename).ForEach(module => module.disable());
	}
	public void enableModule(string modulename){
		inputModules.FindAll(module => module.getName() == modulename).ForEach(module => module.enable());
	}

	public float getRoll(){
		float R = 0.0F;
		R += inputModules.FindAll (module => module.isEnabled() == true).Sum (module => module.getRoll ());
		return R;
	}

	public float getTilt(){
		float T = 0.0F;
		T += inputModules.FindAll (module => module.isEnabled() == true).Sum (module => module.getTilt ());
		return T;
	}

	public float getYaw(){
		float W = 0.0F;
		W += inputModules.FindAll (module => module.isEnabled() == true).Sum (module => module.getYaw ());
		return W;
	}

	public float getX(){
		float X = 0.0F;
		X += inputModules.FindAll (module => module.isEnabled() == true).Sum (module => module.getX ());
		return X;
	}

	public float getY(){
		float Y = 0.0F;
		Y += inputModules.FindAll (module => module.isEnabled() == true).Sum (module => module.getY ());
		return Y;
	}

	public int getA (){
		float As = 0.0F;
		int A = 0;
		As += inputModules.FindAll (module => module.isEnabled () == true).Sum (module => module.getA ());
		if (As > 0.5) {
			A = 1;
		}
		return A;
	}

	public int getB(){
		float Bs = 0.0F;
		int B = 0;
		Bs += inputModules.FindAll (module => module.isEnabled () == true).Sum (module => module.getB());
		if (Bs > 0.5) {
			B = 1;
		}
		return B;
	}

	public int getSelect(){
		float selects = 0.0F;
		int ret = 0;
		selects += inputModules.FindAll (module => module.isEnabled () == true).Sum (module => module.getSelect ());
		if (selects > 0.1) {
			ret = 1;
		}
		return ret;
	}


}

