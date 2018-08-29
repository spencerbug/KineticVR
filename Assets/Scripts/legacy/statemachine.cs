using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;



public class statemachine : MonoBehaviour {
	private GameManager gameManager;
	private string savedRandomForest;

	public int secondsPerTrainingSession = 6;
	public int secondsInBetweenTrainingSessions = 1;
	public int pointsPerRow = 10;

	public  double stopWeight = 1.0D;
	public  double headTurnWeight = 1.0D;
	public  double walkWeight = 1.2D;
	public  double runWeight = 1.3D;
	public double confidenceThreshold = 0.5;
	public fixedUpdateStates state = fixedUpdateStates.idle;
	public bool isRunning = false;
	public float responseTime = 0.2f;
	public float currentSpeed = 0.0f;

	public Text displayLabel;
	public Text countdownLabel;
	public Button trainButton;	

	private int numSensors = 6;
	private const int numClasses=4;

	public const double stoppedNumber = 0.0D, headTurningNumber = 1.0D, walkingNumber = 2.0D, runningNumber = 3.0D;
	public const double notJumpingNumber = 0.0D, jumpingNumber = 1.0D;
	//biases above 1 or below 1 for balance

	public  enum fixedUpdateStates { idle, gatherDataHeadTurning, gatherDataStop, gatherDataWalk, gatherDataRun, gatherDataJump, predictReady, predicting };

	public randomForest rf;
	public randomForest jumprf;
	private TrainingData trainingData;
	private TrainingData jumpingTrainingData;
	private SensorData sensorData;
	private double[,] trainxy;
	private double[,] jumpTrainxy;

	private float oldSpeed = 0.0f;
	private float targetSpeed = 0.0f;
	private float startLerpTime = 0.0f;

	void OnEnable(){
		GameManager.onBeforeSave += runBeforeSave;
	}

	void OnDisable(){
		GameManager.onBeforeSave -= runBeforeSave;
	}

	void runBeforeSave(){
		if (state == fixedUpdateStates.predicting) {
			rf.serialize (out savedRandomForest);
		}
		gameManager.game.randomForestString = savedRandomForest;
	}

	void Start(){
		gameManager = GameManager.gameManager;

		if (gameManager.game.states.isRandomForestTrained) {
			savedRandomForest = gameManager.game.randomForestString;
			rf = new randomForest (confidenceThreshold);
			sensorData = new SensorData(numSensors, pointsPerRow);
			rf.unserialize (savedRandomForest);
			trainButton.gameObject.SetActive (false);
			state = fixedUpdateStates.predicting;
		} 
		else {
			state = fixedUpdateStates.idle;
			trainButton.gameObject.SetActive (true);
		}
	}

	void Update(){
		float elapsedTime = Time.time - startLerpTime;
		currentSpeed = Mathf.Lerp (oldSpeed, targetSpeed, elapsedTime / responseTime);
	}

	public void setSpeedTarget(float newSpeed){
		if (oldSpeed != newSpeed) {
			oldSpeed = currentSpeed;
			targetSpeed = newSpeed;
			startLerpTime = Time.time;
		}
	}

	public void calibrationButton(){
		StartCoroutine(runCalibrationSequence());
	}



	double[] getSensorData(){
		Vector3 userAccel = Input.gyro.userAcceleration;
		Vector3 gravity = Input.gyro.gravity;
		Vector3 rotationRate = Input.gyro.rotationRateUnbiased;
		Vector3 alignedAccel = Vector3.Cross (userAccel, gravity);
		return new double[] { alignedAccel.x, alignedAccel.y, alignedAccel.z, rotationRate.x, rotationRate.y, rotationRate.z };
	}


	void FixedUpdate(){
		double[] lastSensorDatapoints = getSensorData ();
		switch (state) {
		case fixedUpdateStates.idle:
			break;
		case fixedUpdateStates.gatherDataStop:
			setSpeedTarget (0.0F);
			trainingData.yTarget = stoppedNumber;
			trainingData.write (lastSensorDatapoints);
			jumpingTrainingData.yTarget = notJumpingNumber;
			jumpingTrainingData.write (lastSensorDatapoints);
			break;
		case fixedUpdateStates.gatherDataHeadTurning:
			trainingData.yTarget = headTurningNumber;
			trainingData.write (lastSensorDatapoints);
			jumpingTrainingData.yTarget = notJumpingNumber;
			jumpingTrainingData.write (lastSensorDatapoints);
			break;
		case fixedUpdateStates.gatherDataWalk:
			setSpeedTarget (0.5f);
			trainingData.yTarget = walkingNumber;
			isRunning = false;
			trainingData.write (lastSensorDatapoints);
			jumpingTrainingData.yTarget = notJumpingNumber;
			jumpingTrainingData.write (lastSensorDatapoints);
			break;
		case fixedUpdateStates.gatherDataRun:
			trainingData.yTarget = runningNumber;
			setSpeedTarget (1.2f);
			isRunning = true;
			trainingData.write (getSensorData ());
			jumpingTrainingData.yTarget = notJumpingNumber;
			jumpingTrainingData.write (lastSensorDatapoints);
			break;
		case fixedUpdateStates.gatherDataJump:
			setSpeedTarget (0.0f);
			isRunning = false;
			jumpingTrainingData.yTarget = jumpingNumber;
			jumpingTrainingData.write (lastSensorDatapoints);
			break;
		case fixedUpdateStates.predictReady:
			sensorData = new SensorData(numSensors, pointsPerRow);
			state = fixedUpdateStates.predicting;
			break;
		case fixedUpdateStates.predicting:
			sensorData.write (lastSensorDatapoints);
			double[] multiplierArray = new double[] { stopWeight, headTurnWeight, walkWeight, runWeight };
			int movementResult = rf.predict (sensorData.x (), multiplierArray);
			int isJumping = jumprf.predict (sensorData.x (), new double[] { 1.0, 1.0 });
			if (isJumping == 1) {
				displayLabel.text = "Jumping ";
			} else {
				displayLabel.text = "NotJumping ";
			}
			switch (movementResult) {
			case 0:
				setSpeedTarget (0.0f);
				displayLabel.text += "stopped";
				break;
			case 1:
				setSpeedTarget (-0.2f);
				displayLabel.text += "turning head";
				break;
			case 2:
				setSpeedTarget (0.5f);
				isRunning = false;
				displayLabel.text += "walking";
				break;
			case 3:
				setSpeedTarget (1.2f);
				isRunning = true;
				displayLabel.text += "running";
				break;
			default:
				break;
			}
			break;
		default:
			break;
		}
	}



	IEnumerator countDownTimer(int numSeconds, Text label){
		int remaining = numSeconds;
		while (remaining > 0) {
			label.text = remaining.ToString ();
			yield return new WaitForSeconds (1);
			remaining--;
		}
		label.text = "";
	}

	IEnumerator runCalibrationSequence(){

		rf = new randomForest (confidenceThreshold);
		jumprf = new randomForest (confidenceThreshold);
		trainingData = new TrainingData (pointsPerRow, numSensors, stoppedNumber);
		jumpingTrainingData = new TrainingData (pointsPerRow, 6, notJumpingNumber);
		state = fixedUpdateStates.idle;
		displayLabel.text = "stand in place";
		StartCoroutine (countDownTimer (secondsInBetweenTrainingSessions + secondsPerTrainingSession, countdownLabel));
		yield return new WaitForSeconds (secondsInBetweenTrainingSessions);
		state = fixedUpdateStates.gatherDataStop;
		yield return new WaitForSeconds (secondsPerTrainingSession);
		state = fixedUpdateStates.idle;
		displayLabel.text = "good!";
		yield return new WaitForSeconds (secondsInBetweenTrainingSessions);

		displayLabel.text = "turn your head";
		StartCoroutine (countDownTimer (secondsInBetweenTrainingSessions + secondsPerTrainingSession, countdownLabel));
		yield return new WaitForSeconds (secondsInBetweenTrainingSessions);
		state = fixedUpdateStates.gatherDataHeadTurning;
		yield return new WaitForSeconds (secondsPerTrainingSession);
		state = fixedUpdateStates.idle;
		displayLabel.text = "good!";
		yield return new WaitForSeconds (secondsInBetweenTrainingSessions);

		displayLabel.text = "walk in place";
		StartCoroutine (countDownTimer (secondsInBetweenTrainingSessions + secondsPerTrainingSession, countdownLabel));
		yield return new WaitForSeconds (secondsInBetweenTrainingSessions);
		state = fixedUpdateStates.gatherDataWalk;
		yield return new WaitForSeconds (secondsPerTrainingSession);
		state = fixedUpdateStates.idle;
		displayLabel.text = "good!";
		yield return new WaitForSeconds (secondsInBetweenTrainingSessions);

		displayLabel.text = "run in place";
		StartCoroutine (countDownTimer (secondsInBetweenTrainingSessions + secondsPerTrainingSession, countdownLabel));
		yield return new WaitForSeconds (secondsInBetweenTrainingSessions);
		state = fixedUpdateStates.gatherDataRun;
		yield return new WaitForSeconds (secondsPerTrainingSession);
		state = fixedUpdateStates.idle;
		displayLabel.text = "good!";
		yield return new WaitForSeconds (1);

		displayLabel.text = "jump up and down!";
		StartCoroutine (countDownTimer (secondsInBetweenTrainingSessions + secondsPerTrainingSession, countdownLabel));
		yield return new WaitForSeconds (secondsInBetweenTrainingSessions);
		state = fixedUpdateStates.gatherDataJump;
		yield return new WaitForSeconds (secondsPerTrainingSession);
		state = fixedUpdateStates.idle;
		displayLabel.text = "good!";
		yield return new WaitForSeconds (1);

		displayLabel.text = "Please wait... training";
		trainingData.to2dArray (out trainxy);
		rf.train (ref trainxy, numClasses, 20, 0.5);

		jumpingTrainingData.to2dArray (out jumpTrainxy);
		jumprf.train (ref jumpTrainxy, 2, 20, 0.5);

		displayLabel.text = "";
		state = fixedUpdateStates.predictReady;
		gameManager.game.states.isRandomForestTrained = true;


	}
}