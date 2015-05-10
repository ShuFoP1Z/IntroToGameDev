using UnityEngine;
using System.Collections;

public class Hud : MonoBehaviour 
{
	[SerializeField] private SpriteRenderer [] Lives;
	
	[SerializeField] private TextMesh Score;
	[SerializeField] private TextMesh Title;
	[SerializeField] private TextMesh SubTitle;
	[SerializeField] private TextMesh InfoText;
	[SerializeField] private TextMesh[] mNames;
	[SerializeField] private TextMesh[] mScores;
	
	[SerializeField] private bool mNameEntry;
	
	void Start() 
	{
		Lives = GetComponentsInChildren<SpriteRenderer>();
		Score = GetComponentInChildren<TextMesh>();
		
		Game.OnGameRules += HandleOnGameRules;
		Game.OnNewGame += HandleHudChangedEvent;
		Game.OnGameOver += HandleOnGameOver;
		Game.OnRestart += HandleOnRestart;
		Game.OnLostLife += HandleHudChangedEvent;
		Game.OnScoreChange += HandleHudChangedEvent;
		Game.OnNameEntry += HandleOnNameEntry;
		Game.OnLeaderboardUpdated += HandleOnLeaderboardUpdated;
		
		for( int count = 0; count < Lives.Length; count++ )
		{
			Lives[count].enabled = false;
			
			ParticleSystem sys = Lives[count].GetComponentInChildren<ParticleSystem>();
			if(sys != null)
			{
				sys.enableEmission = false;
				sys.Stop();
			}
		}
		
		Transform scoreStrings = transform.FindChild("Score");
		if(scoreStrings != null)
		{
			mNames = new TextMesh[Game.MAX_SCORES];
			mScores = new TextMesh[Game.MAX_SCORES];
			for (int count = 0; count < Game.MAX_SCORES; count++) 
			{
				Transform nameStr = scoreStrings.FindChild("Name" + (count + 1));
				if(nameStr != null)
				{
					mNames[count] = nameStr.GetComponent<TextMesh>();
				}
				
				Transform scoreStr = scoreStrings.FindChild("Score" + (count + 1));
				if(scoreStr != null)
				{
					mScores[count] = scoreStr.GetComponent<TextMesh>();
				}
				
			}
		}
		
		StartCoroutine( "EnableLeaderboard", false);
	}
	
	void HandleOnLeaderboardUpdated (Game.ScoreEntry[] scores)
	{
		for (int count = 0; count < Game.MAX_SCORES; count++) 
		{
			if (mNames[count] != null) 
			{
				mNames[count].text = scores[count].Name;
			}
			if (mScores[count] != null) 
			{
				mScores[count].text = scores[count].Score.ToString( "D6" );
			}
		}
	}
	
	void Update()
	{
		if(mNameEntry)
		{
			foreach(char c in Input.inputString)
			{
				if(c == '\b')
				{
					if(Title.text.Length != 0)
					{
						Title.text = Title.text.Substring(0, Title.text.Length - 1);
					}
				}
				else
				{
					Title.text += c;
				}
			}
			
			Game.LastKnownPlayerName = Title.text;
		}
	}
	
	void HandleOnNameEntry (int lives, int score)
	{
		if(Title != null && SubTitle != null)
		{
			Title.gameObject.SetActive(true);
			SubTitle.gameObject.SetActive(true);
			
			Title.text = "";
			SubTitle.text = "Enter Your Name!";
		}
		
		mNameEntry = true;
	}
	
	private IEnumerator DoParticleOnOff( SpriteRenderer sr)
	{
		ParticleSystem sys = sr.GetComponentInChildren<ParticleSystem>();
		if(sys != null)
		{
			sys.enableEmission = true;
			sys.Play();
			
			yield return new WaitForSeconds(0.7f);
			
			sys.enableEmission = false;
		}
	}
	
	private IEnumerator EnableLeaderboard (bool enable)
	{
		if(enable == true)
		{
			for (int count = 0; count < Game.MAX_SCORES; count++) 
			{
				
				if (mNames[count] != null) 
				{
					mNames[count].gameObject.SetActive(enable);
					
				}
				if (mScores[count] != null) 
				{
					mScores[count].gameObject.SetActive(enable);
					
				}
				
				yield return new WaitForSeconds(0.5f);
				
			}
		}
		else 
		{
			for (int count = 0; count < Game.MAX_SCORES; count++) 
			{
				
				if (mNames[count] != null) 
				{
					mNames[count].gameObject.SetActive(enable);
					
				}
				if (mScores[count] != null) 
				{
					mScores[count].gameObject.SetActive(enable);
					
				}
			}
		}
	}
	
	void HandleOnGameRules (int lives, int score)
	{
		if (Title != null && SubTitle != null) 
		{
			Title.text = "Rules...";
			SubTitle.text = "Jump don't Fall";
			InfoText.gameObject.SetActive(true);
		}
	}
	
	void HandleNewGame( int lives, int score )
	{
		for( int count = 0; count < Lives.Length; count++ )
		{
			Lives[count].enabled = true;
		}
		
		if( Title != null && SubTitle != null )
		{
			Title.gameObject.SetActive( false );
			SubTitle.gameObject.SetActive( false );
			InfoText.gameObject.SetActive(false);
		}
	}
	
	void HandleOnRestart( int lives, int score )
	{
		if( Title != null && SubTitle != null )
		{
			Title.gameObject.SetActive( true );
			SubTitle.gameObject.SetActive( true );
			InfoText.gameObject.SetActive(false);
			
			Title.text = "Jump!";
			SubTitle.text = "Press Space...";
		}
		
		StartCoroutine( "EnableLeaderboard", false);
	}
	
	void HandleOnGameOver( int lives, int score )
	{
		if( Title != null && SubTitle != null )
		{
			Title.gameObject.SetActive( true );
			SubTitle.gameObject.SetActive( true );
			
			Title.text = "Score: " + score.ToString( "D5" );
			SubTitle.text = "Press Space to continue...";
		}
		
		StartCoroutine( "EnableLeaderboard", true);
		mNameEntry = false;
	}
	
	void HandleHudChangedEvent( int lives, int score )
	{
		if( Lives != null && Score != null )
		{
			for( int count = 0; count < Lives.Length; count++ )
			{
				bool oldEnabled = Lives[count].enabled;
				Lives[count].enabled = ( count < lives );
				
				if(oldEnabled && !Lives[count].enabled)
				{
					StartCoroutine( DoParticleOnOff(Lives[count] ) );
				}
			}
			
			Score.text = score.ToString( "D5" );
		}
		
		if( Title != null && SubTitle != null )
		{
			Title.gameObject.SetActive( false );
			SubTitle.gameObject.SetActive( false );
		}
	}
}
