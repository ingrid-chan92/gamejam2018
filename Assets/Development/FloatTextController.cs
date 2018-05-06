using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatTextController : MonoBehaviour {

    public float lifetime = 1.0f;
    public float floatspeed = 1.0f;
    public Color textColor = Color.white;
    public string text = "sssss";
    private float hasLivedFor = 0f;
    private TextMesh mesh;

	// Use this for initialization
	void Start () {
        this.mesh = GetComponent<TextMesh>();
        this.mesh.text = this.text;
        this.mesh.color = textColor;
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.up * floatspeed * Time.deltaTime);
        hasLivedFor += Time.deltaTime;
        if (hasLivedFor > lifetime)
        {
            Destroy(gameObject);
        }
	}

    public void setText (string text)
    {
        Debug.Log("setting text to " + text);
        this.text = text;
    }

    public void setColor(Color newColor)
    {
        this.textColor = newColor;
    }
}
