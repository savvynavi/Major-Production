﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Audio
{
    [DisallowMultipleComponent]
    public abstract class FootstepSounds : MonoBehaviour
    {
        // HACK maybe use something else for argument?
        public abstract AudioClip GetFootstep(Vector3 position);
    }

    public class SimpleFootstep : FootstepSounds
    {
        [SerializeField]
        AudioClip Footstep;

        public override AudioClip GetFootstep(Vector3 position)
        {
            return Footstep;
        }
    }
}