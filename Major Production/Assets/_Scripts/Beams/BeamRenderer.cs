using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class BeamRenderer : MonoBehaviour
{

    LineRenderer lineRenderer;
    Transform source;
    [HideInInspector]
    public GameObject target;
    public float timer;
    Vector3[] pts = new Vector3[2];
    public float uvSpeed = 1.0f;
    public BeamParticles beamParticles;

    // Use this for initialization
    void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.enabled = false;
        }
    }

    public void Activate(Transform src, Transform tgt, float duration, Material mat, float beamWidth, Color col, BeamParticles bp = null)
    {
        // make the particles a child of this
        if (bp != null && beamParticles == null)
        {
            beamParticles = Instantiate(bp);
            beamParticles.Init(src);
        }

        gameObject.SetActive(true);
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = (mat != null);
        timer = duration;
        source = src;
        target = tgt.gameObject;

        if (mat != null)
        {
            lineRenderer.material = mat;

            // set the color and width of the linerenderer
            lineRenderer.startColor = lineRenderer.endColor = col;
            lineRenderer.startWidth = lineRenderer.endWidth = beamWidth;

            // set UV titling to match distance
            float distance = Vector3.Distance(src.position, tgt.position);
            lineRenderer.material.SetTextureScale("_MainTex", new Vector2(2 * distance, 1));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                gameObject.SetActive(false);
                lineRenderer.enabled = false;
                if (beamParticles)
                {
                    Debug.Log("Beam Particles finished");
                    beamParticles.Finish();
                    beamParticles = null;
                }
            }

            // set UV titling to match distance
            float distance = Vector3.Distance(source.position, target.transform.position);

            if (lineRenderer.enabled)
            {
                // repeat the texture once per metre, so the beam doesn't distort with length
                lineRenderer.material.SetTextureScale("_MainTex", new Vector2(2 * distance, 1));

                // scroll the beam according to our speed
                lineRenderer.material.SetTextureOffset("_MainTex", new Vector2(timer * uvSpeed, 0));

                // fade out at last 0.2 secs
                Color col = lineRenderer.startColor;
                col.a = timer * 5.0f;
                lineRenderer.startColor = lineRenderer.endColor = col;
                if (target)
                {
                    pts[0] = source.position;
                    pts[1] = target.transform.position;
                    lineRenderer.SetPositions(pts);
                }
                else
                    Debug.Log("Beam has no target?");
            }

            // update any particles on the beam
            if (beamParticles)
                beamParticles.UpdateParticles(this);

        }
    }
}
