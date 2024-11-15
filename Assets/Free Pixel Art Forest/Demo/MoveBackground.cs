using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour {



	public float speed;
	private float x;
	public float PontoDeDestino;
	public float PontoOriginal;




	// Use this for initialization
	void Start () {
		//PontoOriginal = transform.position.x;
		x = transform.position.x;
    }

    // Update is called once per frame
    void Update () {


		x += speed * Time.deltaTime;
		transform.position = new Vector3 (x, transform.position.y, transform.position.z);



		if (x <= PontoDeDestino){

			x = PontoOriginal;
			transform.position = new Vector3 (x, transform.position.y, transform.position.z);
		}


	}
}
