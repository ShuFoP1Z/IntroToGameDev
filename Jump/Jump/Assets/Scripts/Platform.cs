using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour 
{
	public delegate void PlayerTrigger();
    private const float EXIT_SPEED = 4.2f;
    private const float ROTATE_SPEED = 3.5f;
	public static event PlayerTrigger OnPlayerJoinedPlatform;
	public static event PlayerTrigger OnPlayerLeftPlatform;

	public bool IsAlive { get; set; }

	private Shake mShake;
	private float mTimeSinceActive = 0.0f;
	private bool mHasBeenActivated = false;
    private int mPlatformDirection = 0;
    private bool mWillMove; //Will the platform change which direction it's moving in
    private bool mWillRotate;
    private bool playerLeft = false;
	public float DestroyTime { set; get; }

	void Start()
	{
		ParticleSystem sys = gameObject.GetComponentInChildren<ParticleSystem>();
		if(sys != null)
		{
			sys.enableEmission = false;
			sys.Stop();
		}
	}

	void OnEnable()
	{
		mHasBeenActivated = false;
		mTimeSinceActive = 0.0f;
		mShake = GetComponentInChildren<Shake>();
		if( mShake != null )
		{
			mShake.enabled = false;
		}
	}

	void OnTriggerEnter2D( Collider2D other )
	{
		if( other as CircleCollider2D )
		{
			Player player = other.GetComponent<Player>();
			if( player != null )
			{
				if( OnPlayerJoinedPlatform != null )
				{
					OnPlayerJoinedPlatform();
				}
								
				mHasBeenActivated = true;
				if( mShake != null)
				{
					StartCoroutine(DoParticleOnOff());
					mShake.enabled = true;
				}
			}
		}
	}

	void OnTriggerExit2D( Collider2D other )
	{
		if( other as CircleCollider2D )
		{
			Player player = other.GetComponent<Player>();
			if( player != null )
			{
				if( OnPlayerLeftPlatform != null )
				{
					OnPlayerLeftPlatform();
				}
			}
		}
	}

	
	private IEnumerator DoParticleOnOff()
	{
		ParticleSystem sys = gameObject.GetComponentInChildren<ParticleSystem>();
		if(sys != null)
		{
			sys.enableEmission = true;
			sys.Play();
			
			yield return new WaitForSeconds(DestroyTime);
			
			sys.enableEmission = false;
		}
	}

	void Update()
	{
        
		if( mHasBeenActivated )
		{
			mTimeSinceActive += Time.deltaTime;
			if( mTimeSinceActive > DestroyTime && !mWillMove && !mWillRotate)
			{
				gameObject.SetActive( false );
				IsAlive = false;
           
			}
            else if ( mTimeSinceActive > DestroyTime && mWillMove && !mWillRotate)
            {
                transform.Translate(mPlatformDirection * EXIT_SPEED * Time.deltaTime, 0, 0);

                //If this object's x & y are off the cameras view
                if(Camera.main.WorldToViewportPoint(transform.position).x < 0 || Camera.main.WorldToViewportPoint(transform.position).x > 1)
                {//set it to inactive
                    gameObject.SetActive( false );
                    IsAlive = false;
                }
            }else if(mTimeSinceActive > DestroyTime && mWillRotate && !mWillMove)
            {
                transform.Rotate(0, 0, mPlatformDirection * EXIT_SPEED * Time.deltaTime);
                Debug.Log(playerLeft);
                if (Camera.main.WorldToViewportPoint(transform.position).y > 1 || Camera.main.WorldToViewportPoint(transform.position).y < 0)
                {
                    gameObject.SetActive(false);
                    IsAlive = false;
                    transform.rotation.SetAxisAngle(new Vector3(0, 0, 1), 0);
                }
            }
		}
        
	}

    public void setType(int randomNumber)
    {
        mWillMove = (randomNumber < 30 && randomNumber > 20); //True if less than 10, false if greater
        mWillRotate = (randomNumber < 20);

        while (mPlatformDirection == 0) //randomise it's direction
            mPlatformDirection = Random.Range(-1, 1);
    }

}
