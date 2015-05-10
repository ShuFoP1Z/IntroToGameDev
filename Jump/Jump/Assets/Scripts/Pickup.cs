using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

	public delegate void PlayerTrigger();
	public static event PlayerTrigger OnPlayerCollectedPickup;

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter2D( Collider2D other)
	{
		if( other as CircleCollider2D)
		{
			Player player = other.GetComponent<Player>();
			if( player != null)
			{
				if( OnPlayerCollectedPickup != null)
				{
					OnPlayerCollectedPickup();
				}
				gameObject.SetActive(false);
			}
		}
	}
	

	// Update is called once per frame
	void Update () {
	
	}
}
