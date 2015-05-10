using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Environment : MonoBehaviour 
{
	[SerializeField] private float PlatformStart;

	private List<Platform> mPlatforms;
	private PlatformFactory mPlatformFactory;
    private GameSettings mSettings = new GameSettings();
	private float mPlatformNextSpawnDistance;
	private float mPlatformLocation;
	private float mDestoryTime;
	private bool mLeftPlatform;
    private int mDiffLevel = 0;

	void Awake()
	{
		mPlatforms = new List<Platform>();
		mPlatformFactory = GetComponentInChildren<PlatformFactory>();
	}

	void Start() 
	{
		mPlatformNextSpawnDistance = PlatformStart;
    }
	
	void Update() 
	{
		Platform platform = mPlatformFactory.GetNextPlatform();

        if (mSettings.NumberOfPlatformsClimbed >= 0 && mSettings.NumberOfPlatformsClimbed < 4)
            mDiffLevel = 0;
        if (mSettings.NumberOfPlatformsClimbed >= 4 && mSettings.NumberOfPlatformsClimbed < 8)
            mDiffLevel = 1;
        if (mSettings.NumberOfPlatformsClimbed >= 8 && mSettings.NumberOfPlatformsClimbed < 12)
            mDiffLevel = 2;
        if (mSettings.NumberOfPlatformsClimbed >= 12 && mSettings.NumberOfPlatformsClimbed < 16)
            mDiffLevel = 3;
        if (mSettings.NumberOfPlatformsClimbed >= 16 && mSettings.NumberOfPlatformsClimbed < 20)
            mDiffLevel = 4;
       
		if( platform != null )
		{
			float location = mLeftPlatform ? -mPlatformLocation : mPlatformLocation;
			platform.transform.position = new Vector3( location, mPlatformNextSpawnDistance, 0.0f );
			platform.gameObject.SetActive( true );
			platform.DestroyTime = mDestoryTime;
			platform.IsAlive = true;
			mPlatforms.Add( platform ); 

			mPlatformNextSpawnDistance += mSettings.VerticalDistanceBetweenPlatforms;
			mLeftPlatform = !mLeftPlatform;

            mPlatformLocation = mSettings.getDifficultyLocation(mDiffLevel);
            mDestoryTime = mSettings.getDifficultyDestroy(mDiffLevel);
        }
	}

	public void ApplySettings( GameSettings settings )
	{
		mSettings = settings;
	}

	public void Reset()
	{
		for( int count = 0; count < mPlatforms.Count; count++ )
		{
			mPlatforms[count].gameObject.SetActive( false );
			mPlatforms[count].IsAlive = false;
		}
        mDiffLevel = 0;
		mPlatformFactory.ForceRecycle();
		mPlatformNextSpawnDistance = PlatformStart;
        mPlatformLocation = mSettings.getDifficultyLocation(mDiffLevel);
        mDestoryTime = mSettings.getDifficultyDestroy(mDiffLevel);
	}
}
