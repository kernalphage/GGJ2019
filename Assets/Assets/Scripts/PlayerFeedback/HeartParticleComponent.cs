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


    public void UpdateBurstAmount(int _curIdx, bool _doSizeIncrease, float _sizeMultiplier)
    {
        //Basically just affecting the particle based on how far through the curIdx we are.

        //Clamp it so curIndex is always at least 1
        _curIdx = Mathf.Clamp(_curIdx, 3, _curIdx + 1);
        // myParticleSystem.emission.burstCount =
        if (null != GetComponent<ParticleSystem>())
            myParticleSystem = GetComponent<ParticleSystem>();


        //  myParticleSystem.emission.SetBurst(_curIdx * (Random.Range(minBurstBase, maxBurstBase)), myParticleSystem.emission.GetBurst(0));
        //myParticleSystem.emission.GetBurst(0).count = _curIdx * (Random.Range(minBurstBase, maxBurstBase));
        myParticleSystem.emission.SetBurst(0, new ParticleSystem.Burst(0.0f, _curIdx * (Random.Range(minBurstBase, maxBurstBase))));

        ParticleSystem.MainModule main = myParticleSystem.main;
        if (_doSizeIncrease)
        {
            main.startSizeMultiplier = _sizeMultiplier;
            main.loop = true;
         
            killParticle = false;
          
        }

        main.startSize = _curIdx * (Random.Range(.3f, .4f));

        myParticleSystem.Play();

    }

    private bool killParticle = true;
    // Update is called once per frame
    void Update()
    {

        if (killParticle == true)
        {
            timer += Time.deltaTime;
            if (timer >= lifespan)
            {
                Destroy(gameObject);
            }
        }
       
    }
}
