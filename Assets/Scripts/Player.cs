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

	// game manager
	private GameManager gameManager;

	private Vector3 scaling = new Vector3(0.01f, 0.01f, 1);

	private Vector3 scalingFactor = new Vector3(0.01f, 0.01f, 1);

	// Start is called before the first frame update
	void Start()
	{
		gameManager = new GameManager();
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
					Debug.Log("Swipe left");
					stopTouch = true;
					this.NextLevel(DIRECTION.LEFT);
				}
				else if (distance.x > swipeRange) {
					Debug.Log("Swipe right");
					stopTouch = true;
					this.NextLevel(DIRECTION.RIGHT);
				}
				else if (distance.y < -swipeRange) {
					Debug.Log("Swipe down");
					stopTouch = true;
					this.NextLevel(DIRECTION.DOWN);
				}
				else if (distance.y > swipeRange) {
					Debug.Log("Swipe up");
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
		if (direction == gameManager.getDirection()) {
			// status text
			this.status.text = "Play";

			// change angle
			float angle = gameManager.getRandomAngle();
			Debug.Log("### Angle " + angle);
			this.imageOptotype.transform.localEulerAngles =
					new Vector3(0f, 0f, angle);

			// change size
			this.imageOptotype.transform.localScale -= scalingFactor;
			//Vector3 scale = this.imageOptotype.transform.localScale;
			//Debug.Log(message: $"{scale.x},{scale.y},{scale.z}");
		}
		else if (direction != gameManager.getDirection()) {
			// status text
			this.status.text = "Try again";
		}
	}
}
