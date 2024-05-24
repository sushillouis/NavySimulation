using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wake : MonoBehaviour {

    public List<ParticleSystem> wakeParticles;
    float wakeThresholdSpeed = 0.77f;
    public float wakeLengthFactor = 0.13f;

    public float wakeSpeed;

    public Gradient dayGradient;
    public Gradient nightGradient;
    public Gradient duskGradient;

    public ParticleSystem.MinMaxGradient dayMMGradient;
    public ParticleSystem.MinMaxGradient nightMMGradient;
    public ParticleSystem.MinMaxGradient duskMMGradient;

    public Entity381 entity;
    // Use this for initialization
    void Start () {
        foreach (ParticleSystem wakeParticleSystem in wakeParticles)
            wakeParticleSystem.Stop();
        entity = GetComponentInParent<Entity381>();

        dayMMGradient = new ParticleSystem.MinMaxGradient(dayGradient);
        nightMMGradient = new ParticleSystem.MinMaxGradient(nightGradient);
        duskMMGradient = new ParticleSystem.MinMaxGradient(duskGradient);

    }

    bool isNight()
    {
        return false;
    }

    bool isDusk()
    {
        return false;
    }

    public bool shouldDisappear = false;
    
    public void Disappear(ParticleSystem wakeParticleSystem)
    {
        var cofm = wakeParticleSystem.colorOverLifetime;
        cofm.color = new ParticleSystem.MinMaxGradient(Color.clear);
    }



	// Update is called once per frame
	void Update () {
        foreach(ParticleSystem wakeParticleSystem in wakeParticles)
        {
            if (entity.speed <= wakeThresholdSpeed)
            {
                wakeParticleSystem.Stop();
            }
            else if (!wakeParticleSystem.isPlaying)
            {
                wakeParticleSystem.Play();
            }
            wakeSpeed = entity.speed * wakeLengthFactor;// * sd.shipStatic.width;
            var main = wakeParticleSystem.main;
            main.startSpeed = wakeSpeed;
            if (shouldDisappear)
            {
                Disappear(wakeParticleSystem);
            }
            else
            {
                var cofm = wakeParticleSystem.colorOverLifetime;
                if (isNight())
                {
                    cofm.color = nightMMGradient;
                }
                else if (isDusk())
                {
                    cofm.color = duskMMGradient;
                }
                else
                {
                    cofm.color = dayMMGradient; // daytime
                }
            }
        }
    }
}
