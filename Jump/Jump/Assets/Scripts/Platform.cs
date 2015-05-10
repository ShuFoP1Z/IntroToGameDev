using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour 
{
	public delegate void PlayerTrigger();
	
	public static event PlayerTrigger OnPlayerJoinedPlatform;
	public static event PlayerTrigger OnPlayerLeftPlatform;

	public bool IsAlive { get; set; }

	private Shake mShake;
	private float mTimeSinceActive = 0.0f;
	private bool mHasBeenActivated = false;

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
				if( mShake != null )
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
			if( mTimeSinceActive > DestroyTime )
			{
				gameObject.SetActive( false );
				IsAlive = false;
			}
		}
	}
}
