using UnityEngine;
using System.Collections;

public class DifficultyLevel : MonoBehaviour
{
	[SerializeField] private float mStartingPlatformDestoryTime;
	[SerializeField] private float mPlatformDestoryTimeRamp;
	[SerializeField] private float mStartingPlatformLocation;
	[SerializeField] private float mPlatformLocationRamp;
    [SerializeField] private int mNumberOfPlatformsClimbed;
	
	public float StartingPlatformDestoryTime { get { return mStartingPlatformDestoryTime; } set { mStartingPlatformDestoryTime = value; } }
	public float PlatformDestoryTimeRamp { get { return mPlatformDestoryTimeRamp; } set { mPlatformDestoryTimeRamp = value; } }
	public float StartingPlatformLocation { get { return mStartingPlatformLocation; } set { mStartingPlatformLocation = value; } }
	public float PlatformLocationRamp { get { return mPlatformLocationRamp; } set { mPlatformLocationRamp = value; } }
    public int NumberOfPlatformsClimbed { get { return mNumberOfPlatformsClimbed; } set { mNumberOfPlatformsClimbed = value; } }
}
