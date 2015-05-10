using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour 
{
	[SerializeField] private float mVerticalDistanceBetweenPlatforms;
	[SerializeField] private int mPointsPerPlatform;
	[SerializeField] private int mStartingLives;
	[SerializeField] private DifficultyLevel[] mDifficulty;

	public float VerticalDistanceBetweenPlatforms { get { return mVerticalDistanceBetweenPlatforms; } set { mVerticalDistanceBetweenPlatforms = value; } }
	public int PointsPerPlatform { get { return mPointsPerPlatform; } set { mPointsPerPlatform = value; } }
	public int StartingLives { get { return mStartingLives; } set { mStartingLives = value; } }
	public DifficultyLevel[] Difficulty { get { return mDifficulty; } set { mDifficulty = value; } }

    void Start()
    {
        mDifficulty = new DifficultyLevel[5];

        mDifficulty[0].StartingPlatformDestoryTime = 0.0f;
        mDifficulty[0].StartingPlatformLocation = 0.0f;
        mDifficulty[0].PlatformDestoryTimeRamp = 0.0f;
        mDifficulty[0].PlatformLocationRamp = 0.01f;

        mDifficulty[1].StartingPlatformDestoryTime = 0.0f;
        mDifficulty[1].StartingPlatformLocation = 0.0f;
        mDifficulty[1].PlatformDestoryTimeRamp = 0.0f;
        mDifficulty[1].PlatformLocationRamp = 0.03f;

        mDifficulty[2].StartingPlatformDestoryTime = 0.0f;
        mDifficulty[2].StartingPlatformLocation = 0.0f;
        mDifficulty[2].PlatformDestoryTimeRamp = 0.0f;
        mDifficulty[2].PlatformLocationRamp = 0.05f;

        mDifficulty[3].StartingPlatformDestoryTime = 0.0f;
        mDifficulty[3].StartingPlatformLocation = 0.0f;
        mDifficulty[3].PlatformDestoryTimeRamp = 0.0f;
        mDifficulty[3].PlatformLocationRamp = 0.07f;

        mDifficulty[4].StartingPlatformDestoryTime = 0.0f;
        mDifficulty[4].StartingPlatformLocation = 0.0f;
        mDifficulty[4].PlatformDestoryTimeRamp = 0.0f;
        mDifficulty[4].PlatformLocationRamp = 0.10f;
    }
}
