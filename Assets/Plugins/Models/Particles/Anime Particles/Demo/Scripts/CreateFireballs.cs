using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFireballs : MonoBehaviour {

    public Rigidbody Fireball;

    private bool CreateInstances = true;
    private Rigidbody Instance;

	void Start () {
        InvokeRepeating("Create", 1, 1);
	}
    void Update()
    {
        if(Instance == null)
        {
            CreateInstances = true;
        }
    }

    void Create()
    {
        if (CreateInstances)
        {
            Instance = Instantiate(Fireball, transform.position, transform.rotation); 
            CreateInstances = false;
        }
    }
}
