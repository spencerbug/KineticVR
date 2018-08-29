using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMouseModule : MonoBehaviour, InputModuleInterface {
	private bool m_enabled;
	float x,y,z,r,t,w;
	int a,b,s;


	public string SelectKey = "tab", Akey="mouse0", Bkey="space";

	public KeyboardMouseModule(){
		m_enabled = true;
		x = y = z = r = t = w = 0.0F;
		a = b = s = 0;
	}
	public string getName(){
		return "KeyboardMouse";
	}
	public float getRoll(){
		r = Input.GetAxis ("Mouse Y");
		return r;
	}
	public float getYaw(){
		w = Input.GetAxis ("Mouse X");
		return w;
	}
	public float getTilt(){
		return 0.0F;
	}
	public float getX(){
		return Input.GetAxis ("Horizontal");
	}
	public float getY(){
		return Input.GetAxis ("Vertical");
	}
	public float getZ (){
		return z;
	}
	public int getA(){
		return getIntInput (Akey);
	}
	public int getB(){
		return getIntInput (Bkey);
	}
	public int getSelect (){
		return getIntInput (SelectKey);
	}
	public void enable(){
		m_enabled = true;
	}
	public void disable(){
		m_enabled = false;
	}
	public bool isEnabled (){
		return m_enabled;
	}
		
	private int getMouseNum(string s){
		uint mnum = 0;
		UInt32.TryParse (s.Substring (s.Length - 1), out mnum);
		return (int)mnum;
	}

	private int getIntInput(string buttonName){
		if (buttonName.StartsWith("mouse")) {
			int mouseNum = getMouseNum (buttonName);
			if (Input.GetMouseButton (mouseNum))
				return 1;
			else
				return 0;
		} else {
			if (Input.GetKey (buttonName)) {
				return 1;
			} else {
				return 0;
			}
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
