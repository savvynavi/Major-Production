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

        public override AudioClip GetFootstep(Vector3 position)
        {
            int terrainIndex = terrain.GetMainTexture(position);
			if (terrainIndex < Footsteps.Count)
			{
				return Footsteps[terrainIndex];
			}
			else
			{
				return null;
			}
        }
    }
}