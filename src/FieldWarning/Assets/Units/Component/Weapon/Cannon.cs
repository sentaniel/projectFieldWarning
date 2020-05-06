/**
 * Copyright (c) 2017-present, PFW Contributors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in
 * compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under the License is
 * distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See
 * the License for the specific language governing permissions and limitations under the License.
 */

using System;
using UnityEngine;
using PFW.Model.Armory.JsonContents;

namespace PFW.Units.Component.Weapon
{
    /// <summary>
    /// A non-howitzer cannon.
    /// </summary>
    public class Cannon : IWeapon
    {
        private CannonConfig _data { get; }
        private float _reloadTimeLeft { get; set; }
        private AudioSource _source { get; }

        // TODO Should aim to make actual objects fire and not effects:
        private readonly ParticleSystem _shotEffect;
        private readonly AudioClip _shotSound;
        private readonly ParticleSystem _muzzleFlashEffect;
        private readonly float _shotVolume;
        private static System.Random _random;

        public Cannon(
            CannonConfig data,
            AudioSource source,
            ParticleSystem shotEffect,
            AudioClip shotSound,
            ParticleSystem muzzleFlashEffect,
            float shotVolume = 1.0f)
        {
            _data = data;
            _source = source;
            _shotEffect = shotEffect;
            _shotSound = shotSound;
            _muzzleFlashEffect = muzzleFlashEffect;
            _shotVolume = shotVolume;
            _random = new System.Random(Environment.TickCount);
        }

        private void FireWeapon(TargetTuple target, Vector3 displacement, bool isServer)
        {
            // sound
            _source.PlayOneShot(_shotSound, _shotVolume);
            // particle
            _shotEffect.Play();

            if (_muzzleFlashEffect != null)
            {
                _muzzleFlashEffect.Play();
            }

            if (isServer)
            {
                if (target.IsUnit)
                {
                    float roll = _random.NextFloat(0.0, 100.0);
                    // HIT
                    if (roll <= _data.Accuracy)
                    {
                        Debug.LogWarning("Cannon shell dispersion is not implemented yet");
                        target.Enemy.HandleHit(_data.Damage, displacement, null);
                    }
                }
                else
                {
                    // TODO: fire pos damage not implemented
                }
            }
        }

        public bool TryShoot(
                TargetTuple target, 
                float deltaTime, 
                Vector3 displacement, 
                bool isServer)
        {
            _reloadTimeLeft -= deltaTime;
            if (_reloadTimeLeft > 0)
                return false;

            // TODO implement salvo + shot reload
            _reloadTimeLeft = _data.SalvoReload;
            FireWeapon(target, displacement, isServer);
            return true;
        }
    }
}
