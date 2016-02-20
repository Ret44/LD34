using UnityEngine;
using System.Collections;

public class CameraScreenShake : MonoBehaviour {

    public static CameraScreenShake instance;
    Camera camera;
    public float shake;
    public float shakeAmount;
    public float decreaseFactor;
    public UnityEngine.UI.Text hpUI;
    private int defaultFontSize;

	// Use this for initialization
	void Start () {
        instance = this;
        camera = Camera.main;
	}

    public static void Shake(float shake) 
    {
        instance.shake = 1;
        instance.shakeAmount = Mathf.Min(shake,5f);
        if (instance.hpUI != null)
        {
            instance.defaultFontSize = instance.hpUI.fontSize;
        }
    }

	// Update is called once per frame
	void Update () {
        
        if (shake > 0)
        {
            Vector3 randomVector = Random.insideUnitSphere * shakeAmount;
            randomVector.z = -10f;
            camera.transform.localPosition = randomVector;
            shake -= Time.deltaTime * decreaseFactor;
            if(hpUI!=null)
            {
                hpUI.fontSize = defaultFontSize + ((int)shake * 10);
            }
        }
        else
        {
            shake = 0.0f;
            camera.transform.localPosition = new Vector3(0f, 0f, -10f);
        }
	}
}
