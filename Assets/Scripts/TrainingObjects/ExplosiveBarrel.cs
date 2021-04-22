using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    public ParticleSystem[] particles = new ParticleSystem[3];

    public void Explode()
    {
        for(var x = 0; x < 3; x++)
        {
            particles[x].transform.position = transform.GetChild(x).position;
            particles[x].Play();
        }
    }

    #region DEBUG
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Explode();
        }   
    }
    #endregion
}
