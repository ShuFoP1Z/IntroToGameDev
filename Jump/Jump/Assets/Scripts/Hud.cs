using UnityEngine;
using System.Collections;

public class Hud : MonoBehaviour 
{
	[SerializeField] private SpriteRenderer [] Lives;
	[SerializeField] private TextMesh Score;
	[SerializeField] private TextMesh Title;
	[SerializeField] private TextMesh SubTitle;
	[SerializeField] private TextMesh InfoMessage;
	private bool mNameEntry;
	private TextMesh[] mNames;
	private TextMesh[] mScores;

	void Start() 
	{
		Lives = GetComponentsInChildren<SpriteRenderer>();
		Score = GetComponentInChildren<TextMesh>();
		InfoMessage.gameObject.SetActive (false);

		Game.OnGameRules += HandleOnGameRules;
		Game.OnNewGame += HandleHudChangedEvent;
		Game.OnGameOver += HandleOnGameOver;
		Game.OnRestart += HandleOnRestart;
		Game.OnLostLife += HandleHudChangedEvent;
		Game.OnScoreChange += HandleHudChangedEvent;
		Game.OnNameEntry += HandleOnNameEntry;

		for( int count = 0; count < Lives.Length; count++ )
		{
			Lives[count].enabled = false;

			ParticleSystem aya = Lives[count].GetComponentInChildren<ParticleSystem>();
			if(aya!=null)
			{
				aya.enableEmission = false;
				aya.Stop();
			}
		}		
	}

	void HandleOnNameEntry (int lives, int score)
	{
		if (Title != null && SubTitle != null) 
		{
			Title.gameObject.SetActive (true);
			SubTitle.gameObject.SetActive (true);

			Title.text = "";
			SubTitle.text = "Enter Your Name";
		}

		mNameEntry = true;
	}

	void HandleOnGameRules (int lives, int score)
	{
		if (Title != null && SubTitle != null) {
			InfoMessage.gameObject.SetActive (true);
						Title.text = "Rules...";
						SubTitle.text = "Jump don't fall!";
						InfoMessage.text = "Press Escape to Exit";
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
			InfoMessage.gameObject.SetActive (false);
		}
	}

	void HandleOnRestart( int lives, int score )
	{
		if( Title != null && SubTitle != null )
		{
			Title.gameObject.SetActive( true );
			SubTitle.gameObject.SetActive( true );

			Title.text = "Jump!";
			SubTitle.text = "Press Space...";
			InfoMessage.gameObject.SetActive (false);
		}
	}

	void HandleOnGameOver( int lives, int score )
	{
		if( Title != null && SubTitle != null )
		{
			Title.gameObject.SetActive( true );
			SubTitle.gameObject.SetActive( true );
			InfoMessage.gameObject.SetActive (true);

			Title.text = "Score: " + score.ToString( "D5" );
			SubTitle.text = "Press Space to continue...";
			InfoMessage.text = "Try Again";
		}
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
					StartCoroutine(DoParticleOnOff(Lives[count]));
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
	private IEnumerator DoParticleOnOff(SpriteRenderer sr)
	{
				ParticleSystem sys = sr.GetComponentInChildren<ParticleSystem> ();
				if (sys != null) {
						sys.enableEmission = true;
						sys.Play ();

						yield return new WaitForSeconds (0.1f);

						sys.enableEmission = false;
				}
		}

	void Update()
	{
		if(mNameEntry)
		{
			foreach(char c in Input.inputString)
			{
				if (c == '\b')
				{
					if (Title.text.Length != 0)
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
}

