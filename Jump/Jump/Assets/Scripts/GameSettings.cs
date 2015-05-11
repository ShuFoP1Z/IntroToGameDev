using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour 
{
    [SerializeField] private float mVerticalDistanceBetweenPlatforms;
    [SerializeField] private int mPointsPerPlatform;
    [SerializeField] private int mStartingLives;
    [SerializeField] private int mNumberOfPlatformsClimbed;
    [SerializeField] private DifficultyLevel[] mDifficulty;

    public float VerticalDistanceBetweenPlatforms { get { return mVerticalDistanceBetweenPlatforms; } set { mVerticalDistanceBetweenPlatforms = value; } }
    public int PointsPerPlatform { get { return mPointsPerPlatform; } set { mPointsPerPlatform = value; } }
    public int StartingLives { get { return mStartingLives; } set { mStartingLives = value; } }
    public int NumberOfPlatformsClimbed { get { return mNumberOfPlatformsClimbed; } set { mNumberOfPlatformsClimbed = value; } }

    void Start()
    {
        mDifficulty = new DifficultyLevel[5];
        for (int i = 0; i < mDifficulty.Length; ++i)
        {
            mDifficulty[i] = new DifficultyLevel();
            mDifficulty[i].StartingPlatformLocation = (1.00f + (0.05f * i));
            mDifficulty[i].StartingPlatformDestoryTime = (5.0f - (0.3f * i));
        }
    }

    public float getDifficultyLocation(int i)
    {
        return mDifficulty[i].StartingPlatformLocation;
    }

    public float getDifficultyDestroy(int i)
    {
        return mDifficulty[i].StartingPlatformDestoryTime;
    }

    public void resetPlatformsClimbed()
    {
        mNumberOfPlatformsClimbed = 0;
    }
}
