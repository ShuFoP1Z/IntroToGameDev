using UnityEngine;
using System.Collections;

public class DifficultyLevel : MonoBehaviour
{
    [SerializeField] private float mStartingPlatformDestoryTime;
    [SerializeField] private float mStartingPlatformLocation;

    public float StartingPlatformDestoryTime { get { return mStartingPlatformDestoryTime; } set { mStartingPlatformDestoryTime = value; } }
    public float StartingPlatformLocation { get { return mStartingPlatformLocation; } set { mStartingPlatformLocation = value; } }
}
