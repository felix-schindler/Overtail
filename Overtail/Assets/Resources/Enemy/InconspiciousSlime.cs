﻿using System;
using System.Collections;
using System.Collections.Generic;
using Overtail.Battle.Entity;
using UnityEngine;

namespace Overtail.Battle.Entity
{
    /// <summary>
    /// Implementation of special slime combabt behaviour
    /// </summary>
    public class InconspiciousSlime : EnemyEntity
    {
        // Private fields

        private bool _enraged = false;
        private readonly List<string> _responses = new List<string>();




        // Public
        public override int HP
        {
            get => base.HP;
            set => base.HP = Mathf.Max(value, 1);
        }

        public override IEnumerator OnGreeting(BattleSystem system)
        {
            yield return system.GUI.StartDialogue($"{Name.ToUpper()}: Hello, traveler!");
            yield return system.GUI.StartDialogue("Let's become friends");

            // <Insert qq animation>

            yield break;
        }

        public override IEnumerator DoTurnLogic(BattleSystem system)
        {
            if (_enraged)
            {
                yield return StartCoroutine(OnAttack(system));
                yield return StartCoroutine(system.Player.OnGetAttacked(system));
            } else if (HP < MaxHP * 0.4)
            {
                yield return StartCoroutine(GoSuperSaiyan(system));
            }
        }

        public override IEnumerator OnAttack(BattleSystem system)
        {
            yield return system.GUI.StartDialogue($"{Name.ToUpper()} body slams {system.Player.Name}.");
            system.Player.HP -= system.Player.HP;
        }

        public override IEnumerator OnGetFlirted(BattleSystem system)
        {
            yield return system.GUI.StartDialogue($"{Name.ToUpper()}: 'I think we should just be friends'");
        }

        public override IEnumerator OnGetBullied(BattleSystem system)
        {
            yield return system.GUI.StartDialogue($"{Name.ToUpper()} Ouchie, that hurt.");
            yield return StartCoroutine(OnGetAttacked(system));
        }

        public override IEnumerator OnGetAttacked(BattleSystem system)
        {
            if (_enraged)
            {
                yield return system.GUI.StartDialogue($". . .", typeWriteDelay:0.4f, skipAvailable:false);
                yield return system.GUI.StartDialogue($"{Name} is unfazed by {system.Player.Name}'s attack.");
                yield break;
            }
            yield return new WaitForSeconds(1f);
            
            // Text Reactions

            _responses.Add("I'll get angry!");
            _responses.Add("I'm warning you!");

            var r = UnityEngine.Random.value * _responses.Count;

            yield return StartCoroutine(FlipAround());
            yield return system.GUI.StartDialogue(_responses[(int)r]);
        }


        // Private Methods
        /// <summary>
        /// Grow larger and enhance stats.
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        private IEnumerator GoSuperSaiyan(BattleSystem system)
        {
            _enraged = true;
            
            var sprite = Resources.Load<Sprite>("Enemy/greenSlime_enraged");
            if (sprite != null)
            {
                yield return StartCoroutine(FlipAround());
                var sprRend = GetComponentInChildren<SpriteRenderer>();
                sprRend.sprite = sprite;
            }

            MaxHP = 999;
            Attack = 999;
            Level = 99;
            Name = Name.Replace("Small", "Small(?)").Replace("small", "small(?)");
            HP = MaxHP;

            yield return StartCoroutine(GrowBig());

            yield return system.GUI.StartDialogue("THAT'S IT, YOU LITTLE SH*T");
        }

        /// <summary>
        /// Animation Coroutine
        /// </summary>
        /// <returns></returns>
        private IEnumerator FlipAround()
        {
            var sprRend = GetComponentInChildren<SpriteRenderer>();
            sprRend.sprite = Resources.Load<Sprite>("Enemy/greenSlime_enraged");
            var sleep = new WaitForSeconds(0.2f);
            for (var i = 0; i < 6; i++)
            {
                sprRend.flipX ^= true;
                yield return sleep;
            }
            sprRend.sprite = Resources.Load<Sprite>("Enemy/greenSlime");
        }
        private IEnumerator GrowBig()
        {
            return ShakeAndEnlarge(3, 2, 2);

            IEnumerator ShakeAndEnlarge(float scaleX, float scaleY, float time)
            {
                Vector3 originalPos = transform.localPosition;
                Vector3 originalScale = transform.localScale;
                float timeElapsed = 0;

                while (timeElapsed < time)
                {
                    timeElapsed += Time.deltaTime;
                    yield return null;

                    var newScale = originalScale;
                    newScale.x = Mathf.SmoothStep(originalScale.x, originalScale.x * scaleX, timeElapsed / time);
                    newScale.y = Mathf.SmoothStep(originalScale.y, originalScale.y * scaleY, timeElapsed / time);

                    transform.localScale = newScale;

                    Func<float> rnd = () => (float)(UnityEngine.Random.value - 1); // shaky
                    var newPos = originalPos + new Vector3(.1f * rnd(), .1f * rnd(), .1f * rnd());

                    transform.localPosition = newPos;
                }

                transform.localPosition = originalPos;
            }
        }
    }
}