using ExpertWaves;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Player :MonoBehaviour
{
	// touch control
	public double swipeRange = 50;

	public double tapRange = 10;

	private Vector2 startPosition;

	private Vector2 currentPosition;

	private Vector2 endPosition;

	private bool stopTouch = false;

	// Optotype
	public Image imageOptotype;

	public Text status;

	public Camera sceneCamera;

	// game manager
	private DistanceEyeTest distanceEyeTest;

	// Start is called before the first frame update
	void Start()
	{
		// init game manager
		distanceEyeTest = new DistanceEyeTest();

		// status text
		this.status.text = "K53 Eye Test";
	}

	// Update is called once per frame
	void Update()
	{
			Swipe();
	}

	public void Swipe()
	{
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
			startPosition = Input.GetTouch(0).position;
		}
		else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
			currentPosition = Input.GetTouch(0).position;
			Vector2 distance = currentPosition - startPosition;

			if (!stopTouch) {
				if (distance.x < -swipeRange) {
					Debug.Log("$$$ Swipe left");
					stopTouch = true;
					this.NextLevel(DIRECTION.LEFT);
				}
				else if (distance.x > swipeRange) {
					Debug.Log("$$$ Swipe right");
					stopTouch = true;
					this.NextLevel(DIRECTION.RIGHT);
				}
				else if (distance.y < -swipeRange) {
					Debug.Log("$$$ Swipe down");
					stopTouch = true;
					this.NextLevel(DIRECTION.DOWN);
				}
				else if (distance.y > swipeRange) {
					Debug.Log("$$$ Swipe up");
					stopTouch = true;
					this.NextLevel(DIRECTION.UP);
				}
			}
		}
		else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {
			endPosition = Input.GetTouch(0).position;
			Vector2 distance = endPosition - startPosition;
			stopTouch = false;

			if (Mathf.Abs(distance.x) < tapRange && Mathf.Abs(distance.y) < tapRange) {
			}
		}
	}

	public void NextLevel(DIRECTION direction)
	{
		if (( distanceEyeTest.getGameOver() ) && ( direction == distanceEyeTest.Direction )) {
			// game manager move to next level
			distanceEyeTest.NextLevel();

			// change angless
			float angle = distanceEyeTest.Angle;
			this.imageOptotype.transform.localEulerAngles = new Vector3(0f, 0f, angle);

			// change size
			Vector3 scale = new Vector3(distanceEyeTest.OptotypeScale, distanceEyeTest.OptotypeScale, 1.0f);
			this.imageOptotype.transform.localScale = scale;

			// score 
			this.status.text = $"{distanceEyeTest.Score}%";
		}
		else {
			// game over on incorrect answer or end of levels
			distanceEyeTest.setGameOver(true);

			// status text
			this.status.text = $"{distanceEyeTest.Score}%";
		}
	}
}
