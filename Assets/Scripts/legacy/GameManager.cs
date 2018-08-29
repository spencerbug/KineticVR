using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.SceneManagement;

/*
 * this file holds all the data that Player and other objects load sand saved to memory
 * - it is singleton. only one of them can exist and it persists the entire duratioin even between scene switching
 * There's no easy way to pass data between scene loads, so we store all our data in GameObject, and use flag states 
 * 
*/

[Serializable]
public class States{
	public Boolean isLoading, isTraversingDoor, isRandomForestTrained;
	public States(){
		isLoading = false;
		isTraversingDoor = false;
		isRandomForestTrained = false;
	}
}

[Serializable]
public class Player  {
	public float health, experience;
	public float[] spawnPoint;
	public string lastDoor;
	public float doorSpawnDistance;
	public Player(){
		health = 100;
		experience = 0;
		spawnPoint = new float[3];
		lastDoor = "";
		doorSpawnDistance = 3;
	}
}

[Serializable]
public class Game{
	public Game(){
		player = new Player ();
		states = new States ();
		savedScene = "main";
		randomForestString = "";
	}
	public string randomForestString;
	public States states;
	public Player player;
	public string savedScene;
}

public class GameManager : MonoBehaviour {

	public static GameManager gameManager;

	public delegate void OnBeforeSave();
	public static event OnBeforeSave onBeforeSave;

	//this is information for the PlayerManager class to use at the start of the scene

	public Game game;

	//Use this for initialization
	void Awake ()	{
		if (gameManager == null) {
			DontDestroyOnLoad (gameObject);
			gameManager = this;
		}
		else if (gameManager != this){
			Destroy (gameObject);
		}
	}

	public void Save(){
		if (onBeforeSave != null) {
			onBeforeSave ();
		}
		game.savedScene = SceneManager.GetActiveScene ().name;

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/save.dat");
		bf.Serialize (file, game);
		file.Close ();
	}

	public void Load(){
		if(File.Exists(Application.persistentDataPath + "/save.dat")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
			game = (Game)bf.Deserialize(file);
			file.Close();
		}
		game.states.isLoading = true;
		gameManager = this;
		SceneManager.LoadScene (game.savedScene); //load the last saved scene

	}

	public void NewGame(){
		gameManager = this;
		game = new Game ();
		SceneManager.LoadScene (game.savedScene);
	}
		

	public void MainMenu(){
		SceneManager.LoadScene ("MainTitle");
	}

	public void TeleportScene(String toScene, String doorName, float distanceFromDoor){
		game.states.isTraversingDoor = true;
		game.player.lastDoor = doorName;
		game.player.doorSpawnDistance = distanceFromDoor;

		print ("Teleporting!");
		SceneManager.LoadScene (toScene);
	}
		
}
