using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boombEffectSetttings : MonoBehaviour
{
    public float tremortime;
    public float Magnitude;
    GameObject myCamera;
    public bool CamTremorControl = true;

    // Start is called before the first frame update
    void Start()
    {
        myCamera = GameObject.FindWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        if (CamTremorControl)
        {
            StartCoroutine(CamTremor(tremortime, Magnitude));
        }
        
    }


    IEnumerator CamTremor(float tremortime, float Magnitude)
    {

        Vector3 OriginalCamPoz = myCamera.transform.localPosition;
        float elapsedtime = 0.0f;//geçen zaman
        while (elapsedtime < tremortime)
        {
            float x = Random.Range(-1f, 1) * Magnitude;
            myCamera.transform.localPosition = new Vector3(x, OriginalCamPoz.y, OriginalCamPoz.z);
            elapsedtime += Time.deltaTime;
            yield return null;
        }
        CamTremorControl = false;
        myCamera.transform.localPosition = OriginalCamPoz;
    }
}
