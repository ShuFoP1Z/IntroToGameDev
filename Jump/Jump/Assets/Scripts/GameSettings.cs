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
    public DifficultyLevel[] Difficulty { get { return mDifficulty; } set { mDifficulty = value; } }

    void Start()
    {
        mDifficulty = new DifficultyLevel[5];

        mDifficulty[0].StartingPlatformLocation = 1.0f;
        mDifficulty[0].StartingPlatformDestoryTime = 5.0f;
        
        mDifficulty[1].StartingPlatformLocation = 1.1f;
        mDifficulty[1].StartingPlatformDestoryTime = 4.7f;

        mDifficulty[2].StartingPlatformLocation = 1.2f;
        mDifficulty[2].StartingPlatformDestoryTime = 4.3f;

        mDifficulty[3].StartingPlatformLocation = 1.5f;
        mDifficulty[3].StartingPlatformDestoryTime = 4.0f;

        mDifficulty[4].StartingPlatformLocation = 2.7f;
        mDifficulty[4].StartingPlatformDestoryTime = 3.7f;
    }
}
