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
	private GameManager gameManager;

	// Start is called before the first frame update
	void Start()
	{
		// init game manager
		gameManager = new GameManager();

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
		if (( gameManager.getGameOver() ) && ( direction == gameManager.Direction )) {
			// game manager move to next level
			gameManager.NextLevel();

			// change angless
			float angle = gameManager.Angle;
			this.imageOptotype.transform.localEulerAngles = new Vector3(0f, 0f, angle);

			// change size
			Vector3 scale = new Vector3(gameManager.OptotypeScale, gameManager.OptotypeScale, 1.0f);
			this.imageOptotype.transform.localScale = scale;

			// acuity logging
			float percent_acuity = this.computeAcuityPercent();
			Debug.Log($"$$$ Acuity Score: {percent_acuity}");
			this.status.text = $"Keep Going, Score: {percent_acuity}%";
		}
		else {
			// game over on incorrect answer or end of levels
			gameManager.setGameOver(true);

			// compute acuity score
			float percent_acuity = this.computeAcuityPercent();

			// status text
			this.status.text = $"The End! Acuity Score: {percent_acuity}%";
		}
	}

	public float computeAcuityPercent()
	{
		decimal acuity = (decimal) computeAcuity();
		return (float) Math.Round(acuity * 100, 2);
	}

	public float computeAcuity()
	{
		// optotype size in meters
		float optotype_letter_size_m_unit = this.computeCurrentOptotypeSize().y;

		// device to user eyes approx. 30 cm
		float device2user = 0.4f;

		// acuity measure | ability to read an optotype M print at user-device distance
		float acuity = device2user / optotype_letter_size_m_unit;

		return acuity;
	}

	public Vector2 computeCurrentOptotypeSize()
	{
		// scaling factors of the image rect transfrom
		Vector3 scale = this.imageOptotype.transform.localScale;

		// The minimal point of the box. This is always equal to center-extents.
		Vector3 imageBoundMin = this.imageOptotype.GetComponent<Image>().sprite.bounds.min;
		Vector3 ImageScreenBoundMin = sceneCamera.WorldToScreenPoint(imageBoundMin);

		// The maximal point of the box. This is always equal to center+extents.
		Vector3 imageBoundMax = this.imageOptotype.GetComponent<Image>().sprite.bounds.max;
		Vector3 ImageScreenBoundMax = sceneCamera.WorldToScreenPoint(imageBoundMax);

		// Image screen width and height in pixels. Screenspace is defined in pixels.
		float imageScreenWidth  = (ImageScreenBoundMax.x - ImageScreenBoundMin.x) * scale.x;
		float imageScreenHeight = (ImageScreenBoundMax.y - ImageScreenBoundMin.y) * scale.y;

		// px to mm
		float px2mm = 0.2645833333f;
		Vector2 imageMillimeters = new Vector2(imageScreenWidth * px2mm, imageScreenHeight * px2mm);

		// mm to m-unit | 1M = size x 0.69 mm | 
		float mm2Mu = 0.69f;
		Vector2 imageMUnit = new Vector2(imageMillimeters.x * mm2Mu, imageMillimeters.y * mm2Mu);

		return imageMUnit;
	}
}
