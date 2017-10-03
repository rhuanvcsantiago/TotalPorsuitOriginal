using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableItem : MonoBehaviour {

	private Rigidbody2D theRB;
	public float throwSpeedX;
	public float throwSpeedY;
	public GameObject destroyEffect;

	// Use this for initialization
	void Start () {
		theRB = GetComponent<Rigidbody2D>();
		theRB.velocity = new Vector2(throwSpeedX * transform.localScale.x, throwSpeedY);

	}
	
	// Update is called once per frame
	void Update () {
		//transform.Rotate(Vector3.right * Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D other)
    {

		Debug.Log("colidiu");

		if (other.tag == "Player1")
        {
            //other.gameObject.SendMessage("HurtP1");
			FindObjectOfType<GameManager>().HurtP1();
			FindObjectOfType<GameManager>().HurtP1();
        }

        if (other.tag == "Player2") {
			//other.gameObject.SendMessage("HurtP2");
            FindObjectOfType<GameManager>().HurtP2();
			FindObjectOfType<GameManager>().HurtP2();
        }

        Instantiate(destroyEffect, transform.position, transform.rotation);
		
		Destroy(gameObject);

	}

}
