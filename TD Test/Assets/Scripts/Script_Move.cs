using UnityEngine;
using System.Collections;

public class Script_Move : MonoBehaviour {

    public float speed = 10.0f;

    void Start()
    {
        rigidbody.velocity = transform.forward * speed;
    }

}
