using UnityEngine;
using System.Collections;

public class FieldOfVision: MonoBehaviour
{
	public string objectName;
	private GameObject player;
	private bool visible;
    private Color kColor;
	private float playerVisibleRange;

	private const float MAX_TORCH_RANGE = 8f;
	private const float EXTRA_TORCH_RANGE = 5f;
	private const float MAX_PLAYER_RANGE = 10f;
	private const float TORCH_VISION_RANGE = 3f;
	private const float HUMAN_VISION_RANGE = 3f;
	private const float MONSTER_VISION_RANGE = 4f;

	void Start ()
    {
        /* Make sure you register your event methods in the 'OnStart' or 'OnAwake' methods
         * The delegate which is used has the following structure
         * Light2DDelegate(Light2d _light, GameObject _gameObject); */
        Light2D.RegisterEventListener(LightEventListenerType.OnEnter, OnLightEnter);
        Light2D.RegisterEventListener(LightEventListenerType.OnStay, OnLightStay);
        Light2D.RegisterEventListener(LightEventListenerType.OnExit, OnLightExit);

		visible = false;
		Display();

        // Keep the initial object color [For Visualization]
//        kColor = gameObject.renderer.material.color;
    }
	void Update(){
		visible = false;
		Display();
	}

	void Display() {
		GameObject[] torches = GameObject.FindGameObjectsWithTag("torch");
		float minTorchDistance = 100;
		float torchAlpha = 0f;
		
		for (int i = 0; i < torches.Length; i ++) {
			if (!visible) {
				float torchDistance = Vector3.Distance (torches[i].transform.position, this.gameObject.transform.position);

				if (torchDistance < MAX_TORCH_RANGE) {
					if (torchDistance < TORCH_VISION_RANGE) {
						torchAlpha = 1f;
					} else {
						torchAlpha = (EXTRA_TORCH_RANGE - (torchDistance - TORCH_VISION_RANGE)) / EXTRA_TORCH_RANGE;
					}
					visible = true;
				} else {
					torchAlpha = 0f;
				}
			}
		}

		if (player == null) {
			player = GameObject.Find (objectName);
			if (objectName == "Human(Clone)") {
				playerVisibleRange = HUMAN_VISION_RANGE;
			} else if (objectName == "Monster(Clone)") {
				playerVisibleRange = MONSTER_VISION_RANGE;
			}
		}

		float distance = 100;

		if (player != null) {
			distance = Vector3.Distance (player.transform.position, this.gameObject.transform.position);
		}

		float playerAlpha = 0f;
		if (distance < playerVisibleRange) {
			playerAlpha = 1f;
		} else if (distance < MAX_PLAYER_RANGE) {
			playerAlpha = ((MAX_PLAYER_RANGE - playerVisibleRange) - (distance - playerVisibleRange)) / (MAX_PLAYER_RANGE - playerVisibleRange);
		} else {
			playerAlpha = 0f;
		}

		gameObject.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, Mathf.Max (torchAlpha, playerAlpha));
	}

    void OnDisable()
    {
        /* Make sure you remove your event methods in the 'OnDisable' or 'OnDestroy' method
         * If you forget to do this you may get errors pertaining to objects that no longer exist */
        Light2D.UnregisterEventListener(LightEventListenerType.OnEnter, OnLightEnter);
        Light2D.UnregisterEventListener(LightEventListenerType.OnStay, OnLightStay);
        Light2D.UnregisterEventListener(LightEventListenerType.OnExit, OnLightExit);
    }

    void OnLightEnter(Light2D _light, GameObject _go)
    {
        /* Function called everytime a new object enters the light.
         * Use the _go object that is passed to determin if the current
         * gameObject is equal to the one this script is in (if needed) */
        if (_go.GetInstanceID() == gameObject.GetInstanceID())
        {
            // GameObject just became visible by light object
            Debug.Log("THIS SHOULD SAY SOMETHING ELSE");
            // Change color [For Visualization]
//            gameObject.renderer.material.color = Color.blue;
        }
    }

    void OnLightStay(Light2D _light, GameObject _go)
    {
        /* Function called every LateUpdate if an object is in the light.
         * Use the _go object that is passed to determin if the current
         * gameObject is equal to the one this script is in (if needed) */
        if (_go.GetInstanceID() == gameObject.GetInstanceID())
        {
            // GameObject is currently visible by light object
            Debug.Log("Object Inside Light");

            // Change color [For Visualization]
//            gameObject.renderer.material.color = Color.Lerp(gameObject.renderer.material.color, Color.red, Time.deltaTime * 0.5f);
        }
    }

    void OnLightExit(Light2D _light, GameObject _go)
    {
        /* Function called everytime an object exits the light.
         * Use the _go object that is passed to determin if the current
         * gameObject is equal to the one this script is in (if needed) */
        if (_go.GetInstanceID() == gameObject.GetInstanceID())
        {
            // GameObject just left the visibility of the light object
            Debug.Log("Object Exited Light");
            // Change color [For Visualization]
//            gameObject.renderer.material.color = kColor;
        }
    }
}
