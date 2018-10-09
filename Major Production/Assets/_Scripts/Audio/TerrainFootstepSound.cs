using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Audio
{
    [RequireComponent(typeof(Terrain))]
    public class TerrainFootstepSound : FootstepSounds
    {
        [SerializeField]
        List<AudioClip> Footsteps;

        Terrain terrain;

        private void Start()
        {
            terrain = GetComponent<Terrain>();
        }

        public override AudioClip GetFootstep(GameObject other)
        {
            int terrainIndex = terrain.GetMainTexture(other.transform.position);
            return Footsteps[terrainIndex];
            // TODO return default if out of range?
        }
    }
}