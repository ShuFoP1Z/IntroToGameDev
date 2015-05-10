using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour 
{
	public delegate void PlayerTrigger();
    private const float EXIT_SPEED = 3.0f;
	public static event PlayerTrigger OnPlayerJoinedPlatform;
	public static event PlayerTrigger OnPlayerLeftPlatform;

	public bool IsAlive { get; set; }

	private Shake mShake;
	private float mTimeSinceActive = 0.0f;
	private bool mHasBeenActivated = false;
    private int mPlatformDirection = 0;
    private bool mWillMove; //Will the platform change which direction it's moving in
    
	public float DestroyTime { set; get; }

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
				if( mShake != null && !mWillMove)
				{
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

	void Update()
	{
        
		if( mHasBeenActivated )
		{
			mTimeSinceActive += Time.deltaTime;
			if( mTimeSinceActive > DestroyTime && !mWillMove)
			{
				gameObject.SetActive( false );
				IsAlive = false;
           
			}
            else if ( mTimeSinceActive > DestroyTime && mWillMove)
            {
                transform.Translate(mPlatformDirection * EXIT_SPEED * Time.deltaTime, 0, 0);

                //If this object's x & y are off the cameras view
                if(Camera.main.WorldToViewportPoint(transform.position).x < 0 || Camera.main.WorldToViewportPoint(transform.position).x > 1)
                {//set it to inactive
                    gameObject.SetActive( false );
                    IsAlive = false;
                }
            }
		}
        
	}

    public void setType(int randomNumber)
    {
        mWillMove = (randomNumber < 10); //True if less than 10, false if greater
        while (mPlatformDirection == 0) //randomise it's direction
            mPlatformDirection = Random.Range(-1, 1);
    }

}
