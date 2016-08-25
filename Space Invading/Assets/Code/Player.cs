using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    
    public float maxSpeed;						//max speed for player movement
    public float horizontalMove;				//check for horizontal input
    public float moveForce;						//frce to make player move
    public float stopTime;						//how long it takes for the character to stop moving
    public float drainingTimer;					//how long it takes for the draining begin
    public bool speedCheck;						//flag to see if the player is moving
	public float screenSize;					//how big is the screen
	public int bulletCount;						//how many bullets the player have available
	public bool invisFrames;					//flag to see if the player is in invincibility frames
	public float invisTime;						//how long the player stays in invincibility frames
	public int maxBulletCount;					//maximun bullets the player can hold
	public bool oddevenTime;					//flag to help with invicibility frames blinking
	public float maxSpeedDrain;
	public float baseMoveForce;
	public bool playing;


    //draining variables
    public bool drainingCall;
    public bool drainingFlag;
    public bool draining;


	//shooting variables
	public bool shootPress;
	public bool shootCall;
	public float shootCD;
	public GameObject shoot;
	public bool okToShoot;

	public static Player instance = null;


	//internal flags and callings
	private int i;
	private int horizFlag;
	private int shootFlag;
	public Rigidbody2D rb2d;
    private Animator animator;
	private Renderer rendSprite;
	private float touchX;
	private float touchY;
	private Vector3 accDir;

	void Awake() {
		//Check if instance already exists
		if (instance == null)

			//if not, set instance to this
			instance = this;

		//If instance already exists and it's not this:
		else if (instance != this)

			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy (gameObject);
	}

    void Start () {

		playing = true;
		maxSpeed = 5f;
		horizontalMove = 0f;
		baseMoveForce = 150f;
		moveForce = baseMoveForce;
		stopTime = 0.05f;
		drainingTimer = 0.001f;
		speedCheck = false;
		screenSize = 5f;
		invisFrames = true;
		invisTime = 1f;
		maxBulletCount = 10;
		horizFlag = 10;
		shootFlag = 10;
		maxSpeedDrain = Repo.instance.drainSpeed;
		okToShoot = ApplicationModel.okToShoot;

		bulletCount = BulletManager.instance.startBullet;

		shootPress = false;
		shootCall = false;
		shootCD = 0.5f;

		drainingCall = false;
		drainingFlag = false;
		draining = false;

		accDir = Vector3.zero;

        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
		rendSprite = GetComponent<Renderer> ();
		StartCoroutine (InvicibilityBlinking ());
		BulletManager.instance.ShootBullet (bulletCount);
    }

    void Update () {
		if (playing) {
			#if UNITY_STANDALONE || UNITY_WEBPLAYER
		//read input and controls for keyboard
        horizontalMove = Input.GetAxis("Horizontal");
		shootPress = Input.GetKey (KeyCode.S) && okToShoot;
		if (Input.GetKeyDown(KeyCode.P)) Manager.instance.PauseGame ();


			#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
			//acceleration reader
			accDir = Vector3.zero;
			accDir.x = Input.acceleration.x;

			if (accDir.sqrMagnitude >= 0.01f) {
				if (accDir.sqrMagnitude >= 1f)
					accDir.Normalize ();
				moveForce = accDir.sqrMagnitude * baseMoveForce;
				accDir.Normalize ();
			} else
				accDir = Vector3.zero;

			horizontalMove = accDir.x;

			if (Input.touchCount > 0) {
				Touch myTouch = Input.GetTouch (0);
				touchY = myTouch.position.y;
				if (myTouch.phase == TouchPhase.Began && touchY < (Screen.height / 2f) && okToShoot)
					shootPress = true;
				if (myTouch.phase == TouchPhase.Ended && shootPress && touchY < (Screen.height / 2f))
					shootPress = false;
				if (myTouch.phase == TouchPhase.Began && touchY > (Screen.height / 2f) && !Manager.instance.pausedFlag)
					Manager.instance.PauseGame ();
			}


			#endif
			transform.localScale = new Vector3 (1f + bulletCount / 10f, 1f + bulletCount / 10f, 1);
		}
    }

	void FixedUpdate () {

		//increases speed
		if (horizontalMove * rb2d.velocity.x < maxSpeed)
            rb2d.AddForce(Vector2.right * horizontalMove * moveForce);

		//cap max speed
		if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
			rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);

		//controls slowdown after button released
        if (Mathf.Abs(rb2d.velocity.x) > 0 && horizontalMove == 0) {
			Vector2 tempVel = rb2d.velocity;
			rb2d.velocity = Vector2.SmoothDamp(rb2d.velocity, Vector2.zero, ref tempVel, stopTime);
        }
		if (horizontalMove == 0 && Mathf.Abs(rb2d.velocity.x) < 0.01f)
			rb2d.velocity = new Vector3(0f,0f,0f);

		//controls the calling for draining state
		if (Mathf.Abs(rb2d.velocity.x) > maxSpeedDrain && draining && !drainingFlag) {
			animator.SetBool("drainBool", false);
			drainingCall = false;
			StartCoroutine (StopDraining ());
			drainingFlag = true;
		}

        if (Mathf.Abs(rb2d.velocity.x) < 0.5f && drainingFlag == false) drainingCall = true;

        if (drainingFlag == false && drainingCall == true) {
            drainingFlag = true;
            StartCoroutine (StartDraining ());
        }

		//shooting code, time to kill bad guys!
		if (shootPress && !shootCall) {
			if (bulletCount > 0) {
				shootCall = true;
				bulletCount--;
				BulletManager.instance.ShootBullet (bulletCount);
				Instantiate (shoot, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.identity);
				Manager.instance.PlaySound (2);
				StartCoroutine (ShootingCDTimer ());
			} else {
				Manager.instance.ColorUpdateCaller ();
				shootCall = true;
				StartCoroutine (ShootingCDTimer ());
			}
		}

		//fixing collision with objects spinning
		transform.rotation = Quaternion.identity;

    }

    IEnumerator StartDraining() {
        yield return new WaitForSeconds(drainingTimer);
        if (drainingCall == true) {
            draining = true;
            animator.SetBool("drainBool", true);
        }
        drainingFlag = false;
    }

	IEnumerator StopDraining() {
		yield return new WaitForSeconds(drainingTimer*250);
		if (drainingCall == false) {
			draining = false;
		}
		drainingFlag = false;
	}

	void OnCollisionEnter2D (Collision2D collision) {
		if (collision.gameObject.tag == "enemy") {
			collision.gameObject.SendMessage ("DieKeep");
			if (invisFrames == false)
				DieKeep ();
		}
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if(invisFrames == false){
					
			switch (collision.gameObject.tag) {
			case "energyEnemy":
				if (draining == true) {
					if (bulletCount < maxBulletCount) {
						bulletCount++;
						Manager.instance.PlaySound (5);
						BulletManager.instance.ShootBullet (bulletCount);
						collision.gameObject.SendMessage ("DrainShrink");
					}
				} else {
					Manager.instance.PlaySound (6);
					Destroy (collision.gameObject);
					DieKeep ();
				}
					
				break;
			case "missile":
				Destroy (collision.gameObject);
				Manager.instance.PlaySound (6);
				DieKeep ();
				break;
			case "enemy":
				DieKeep ();
				collision.gameObject.SendMessage ("DieKeep");
				break;
			}
		}
	}

	public IEnumerator InvicibilityBlinking(){
		for (i = 0; i < 10; i ++){
			yield return new WaitForSeconds (invisTime / 10f);
			rendSprite.enabled = !rendSprite.enabled;
		}
		rendSprite.enabled = true;
		invisFrames = false;
	}

	public IEnumerator ShootingCDTimer (){
		yield return new WaitForSeconds (shootCD);
		shootCall = false;
	}

	public void DieKeep () {
		ExplodesHolder.instance.PoolRunner (transform.position,1);
		transform.position = new Vector2 (20f, 0f);
		if (ApplicationModel.gameType == 3)
			BulletManager.instance.startBullet = bulletCount;
		bulletCount = BulletManager.instance.startBullet;
		gameObject.SetActive (false);
	}
}
