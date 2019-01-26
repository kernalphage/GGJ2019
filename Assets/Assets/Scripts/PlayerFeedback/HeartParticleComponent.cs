using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartParticleComponent : MonoBehaviour
{

    float timer = 0.0f;
    float lifespan = 0.0f;

    private ParticleSystem myParticleSystem = null;


    [SerializeField]
    private float minLife = 0, maxLife = 0;


    private int minBurstBase = 1, maxBurstBase = 2;

    void Start()
    {
        lifespan = Random.Range(minLife, maxLife);



        //myParticleSystem.duration= lifespan;
    }


    public void UpdateBurstAmount(int _curIdx)
    {
        //Basically just affecting the particle based on how far through the curIdx we are.

        //Clamp it so curIndex is always at least 1
        _curIdx = Mathf.Clamp(_curIdx, 1, _curIdx + 1);
        // myParticleSystem.emission.burstCount =
        if (null != GetComponent<ParticleSystem>())
            myParticleSystem = GetComponent<ParticleSystem>();


        //  myParticleSystem.emission.SetBurst(_curIdx * (Random.Range(minBurstBase, maxBurstBase)), myParticleSystem.emission.GetBurst(0));
        //myParticleSystem.emission.GetBurst(0).count = _curIdx * (Random.Range(minBurstBase, maxBurstBase));
        myParticleSystem.emission.SetBurst(0, new ParticleSystem.Burst(0.0f, _curIdx * (Random.Range(minBurstBase, maxBurstBase))));

        ParticleSystem.MainModule main = myParticleSystem.main;
        //main.startSizeMultiplier = _curIdx;
        main.startSize = _curIdx * (Random.Range(.3f, .4f));

        myParticleSystem.Play();

    }
    // Update is called once per frame
    void Update()
    {


        timer += Time.deltaTime;
        if (timer >= lifespan)
        {
            Destroy(gameObject);
        }
    }
}
