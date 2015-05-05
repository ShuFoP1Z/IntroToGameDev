using UnityEngine;
using System.Collections;
using System;

public class Game : MonoBehaviour 
{
	[Serializable]
	public class ScoreEntry
	{
		public string Name;
		public int Score;
	}

	public delegate void GameEvent( int lives, int score );
	public static event GameEvent OnGameRules;
	public static event GameEvent OnNewGame;
	public static event GameEvent OnRestart;
	public static event GameEvent OnGameOver;
	public static event GameEvent OnLostLife;
	public static event GameEvent OnScoreChange;
	public static event GameEvent OnNameEntry;
	public static string LastKnownPlayerName { get; set; }

	private enum State { Starting, Playing, Gameover, GameRules, NameEntry}

	[SerializeField] private GameSettings GameSettingsPrefab;

	private ScoreEntry[] mScores;
	private GameSettings mSettings;
	private Environment mEnvironment;
	private Player mPlayer;
	private int mScore;
	private int mLives;
	private State mState;  
	public const int MAX_SCORES = 10;

	void Start() 
	{
		mScores = new ScoreEntry[MAX_SCORES];
		for (int count = 0; count < MAX_SCORES; count++)
		{
			mScores[count] = new ScoreEntry();
			mScores[count].Name = "Player " + (count + 1);
			mScores[count].Score = 30000 - (count *1000);
		}
		LoadScores();
		mSettings = Instantiate( GameSettingsPrefab ) as GameSettings;
		mEnvironment = GetComponentInChildren<Environment>();
		mEnvironment.ApplySettings( mSettings );
		mEnvironment.Reset();
		mPlayer = GetComponentInChildren<Player>();
		mPlayer.Enabled = false;
		mLives = mSettings.StartingLives;
		mScore = 0;
		mState = State.Starting;

		StartingPlatform.OnPlayerStart += HandleOnPlayerStart;
		StartingPlatform.OnPlayerFell += HandleOnPlayerFell;
		Platform.OnPlayerJoinedPlatform += HandleOnPlayerJoinedPlatform;
	}
	
	void Update()
	{
		switch( mState )
		{
		case State.Starting:
			UpdateStarting();
			break;
		case State.Playing:
			UpdatePlaying();
			break;
		case State.Gameover:
			UpdateGameover();
			break;
		case State.GameRules:
			UpdateGameRules();
			break;
		case State.NameEntry:
			UpdateNameEntry();
			break;
		}
	}

	void HandleOnPlayerStart()
	{
	}

	void HandleOnPlayerFell()
	{
		if( mState == State.Playing )
		{
			mEnvironment.Reset();
			mLives--;

			if( OnLostLife != null )
			{
				OnLostLife( mLives, mScore );
			}
		}
	}

	void HandleOnPlayerJoinedPlatform()
	{
		if( mState == State.Playing )
		{
			mScore += mSettings.PointsPerPlatform;
			
			if( OnScoreChange != null )
			{
				OnScoreChange( mLives, mScore );
			}
		}
	}

	private void UpdateStarting()
	{
		if( Input.GetKeyDown( KeyCode.Space ) )
		{
			ChangeState( State.Playing );
		}
		if (Input.GetKeyDown (KeyCode.R)) 
		{
			ChangeState(State.GameRules);		
		}
	}

	private void UpdatePlaying()
	{
		if( mLives <= 0 )
		{
			if(IsHighScore(mScore))
			{
				ChangeState(State.NameEntry);
			}
			else
			{
				ChangeState( State.Gameover );
			}
		}
	}

	private void UpdateGameover()
	{
		if( Input.GetKeyDown( KeyCode.Space ) )
		{
			ChangeState( State.Starting );
		}
	}

	void UpdateGameRules ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			ChangeState( State.Starting);	
		}
	}

	void UpdateNameEntry ()
	{
		if (Input.GetKeyDown (KeyCode.Return)) 
		{
			ChangeState(State.Gameover);		
		}
	}

	private bool IsHighScore(int score)
	{
		bool betterScore = false;
		for( int count = 0; !betterScore && count < MAX_SCORES; count++)
		{
			betterScore = (score > mScores[count].Score);
		}
		return betterScore;
	}

	private void InsertScore(string name, int score)
	{
		bool inserted = false;
		for( int count = 0; !inserted && count < MAX_SCORES; count++)
		{
			if(score > mScores[count].Score)
			{
				for( int shuffle = MAX_SCORES - 1; shuffle > count; shuffle--)
				{
					mScores[shuffle].Name = mScores[shuffle-1].Name;
					mScores[shuffle].Score = mScores[shuffle-1].Score;
				}
				mScores[count].Name = name;
				mScores[count].Score = score;

				inserted = true;
			}
		}
	}

	private void SaveScores()
	{
		for(int count = 0; count < MAX_SCORES; count++)
		{
			PlayerPrefs.SetString( "PlayerName" + count, mScores[count].Name);
			PlayerPrefs.SetInt( "PlayerScore" + count, mScores[count].Score);
		}
	}

	private void LoadScores()
	{
		for(int count = 0; count < MAX_SCORES; count++)
		{
			mScores[count].Name = PlayerPrefs.GetString( "PlayerName" + count, mScores[count].Name);
			mScores[count].Score = PlayerPrefs.GetInt( "PlayerScore" + count, mScores[count].Score);
		}
	}


	private void ChangeState( State s )
	{
		if( mState != s )
		{
			if(mState == State.NameEntry)
			{
				InsertScore(LastKnownPlayerName, mScore);
				SaveScores ();
			}

			switch( s )
			{
			case State.Starting:
				mLives = mSettings.StartingLives;
				mScore = 0;
				if( OnRestart != null )
				{
					OnRestart( mLives, mScore );
				}
				break;
			case State.Playing:
				mPlayer.Enabled = true;
				if( OnNewGame != null )
				{
					OnNewGame( mLives, mScore );
				}
				break;
			case State.Gameover:
				mPlayer.Enabled = false;
				if( OnGameOver != null )
				{
					OnGameOver( mLives, mScore );
				}
				break;
			case State.GameRules:
			if ( OnGameRules != null)
				{
					OnGameRules(0, 0);
				}
				break;
			case State.NameEntry:
				if( OnNameEntry != null)
				{
					OnNameEntry(0, mScore);
				}
				break;
			
			
			}

			mState = s;
		}
	}
}
