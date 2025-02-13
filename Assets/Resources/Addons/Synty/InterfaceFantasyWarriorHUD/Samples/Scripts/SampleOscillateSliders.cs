// Copyright (c) 2024 Synty Studios Limited. All rights reserved.
//
// Use of this software is subject to the terms and conditions of the Synty Studios End User Licence Agreement (EULA)
// available at: https://syntystore.com/pages/end-user-licence-agreement
//
// Sample scripts are included only as examples and are not intended as production-ready.

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Synty.Interface.FantasyWarriorHUD.Samples
{
    /// <summary>
    ///     Oscillates the value of a list of sliders.
    /// </summary>
    public class SampleOscillateSliders : MonoBehaviour
    {
        [Header("References")]
        public List<Slider> sliders;

        [Header("Parameters")]
        public bool autoGetSliders = true;
        public float speed = 1f;
        public float offset = 0.5f;

        private void Reset()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            sliders = FindObjectsOfType<Slider>().ToList();
#pragma warning restore CS0618 // Type or member is obsolete
        }

        private void Start()
        {
            if (autoGetSliders)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                sliders = FindObjectsOfType<Slider>().ToList();
#pragma warning restore CS0618 // Type or member is obsolete
            }
        }

        private void Update()
        {
            for (int i = 0; i < sliders.Count; i++)
            {
                sliders[i].value = (Mathf.Sin((Time.time * speed) + (i * offset)) * 0.5f) + 0.5f;
            }
        }
    }
}
