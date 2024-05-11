using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    [Header("DamageNumbers")]
    public TextMeshProUGUI text;
    public float lifetime = 0.6f;
    public float minDist = 2f;
    public float MaxDist = 3f;
    public float ScaleSpeed = 3f;
    public AnimationCurve DistanceScaling;
    Color startCol;
    private Vector3 iniPos;
    private Vector3 targetPos;
    private float Timer;
    Vector3 DisToCam;
    private void Start()
    {
        transform.LookAt(2 * transform.position - Camera.main.transform.position);
        DisToCam = transform.localPosition - Camera.main.transform.position;
        float Direction = Random.Range(0,180);
        iniPos = transform.position+ new Vector3(0,1,0);
        float dist = Random.Range(minDist, MaxDist);
        targetPos = iniPos + (Quaternion.Euler(0, Direction, 0) * new Vector3(dist, 1.3f*dist, 0));
        transform.localScale = Vector3.zero;
        
    }


    private void Update()
    {
        
        Timer += Time.deltaTime;

        float fraction = lifetime / 2f;


        
        if (Timer > lifetime) Destroy(gameObject);
        else if (Timer > fraction) text.color = Color.Lerp(text.color, Color.clear, (Timer - fraction) / (lifetime - fraction));

        transform.position = Vector3.Lerp(iniPos, targetPos, Mathf.Sin(Timer/lifetime));
        transform.localScale = Vector3.Lerp(Vector3.zero, DistanceScaling.Evaluate(DisToCam.magnitude) *Vector3.one, ScaleSpeed * Mathf.Exp((Timer / lifetime) - Mathf.PI)) ;
    }


    public void SetDamageText(int damage, Color color)
    {
        text.text = damage.ToString();
        text.color = color;
    }


 

}
