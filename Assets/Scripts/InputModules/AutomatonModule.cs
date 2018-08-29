using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomatonModule : MonoBehaviour, InputModuleInterface {
	private bool m_enabled;
	float x,y,z,r,t,w;
	int a, b, s;
	public AutomatonModule(){
		m_enabled = true;
		x = y = z = r = t = w = 0.0F;
		a = b = s = 0;
	}
	public string getName(){
		return "Automaton";
	}
	public float getRoll(){
		return 0.0F;
	}
	public float getTilt(){
		return 0.0F;
	}
	public float getYaw(){
		return 0.0F;
	}
	public float getX(){
		return 0.0F;
	}
	public float getY(){
		return 0.0F;
	}
	public float getZ(){
		return 0.0F;
	}
	public int getA(){
		return 0;
	}
	public int getB(){
		return 0;
	}
	public int getSelect (){
		return 0;
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
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
