using Modding.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AfterimagePlus
{
    internal class AfterimageFrameAnimator : MonoBehaviour
    {
        private float time;

        public tk2dSpriteAnimationClip clip;
        public Color color;

        void Update()
        {
            tk2dSpriteAnimator animator = gameObject.GetOrAddComponent<tk2dSpriteAnimator>();
            animator.Play(clip);

            time += Time.deltaTime;
            animator.Sprite.color = new Color(color.r, color.g, color.b, color.a * (1 - time / AfterimagePlus.Instance.settings.decayTime));
            if (time > AfterimagePlus.Instance.settings.decayTime)
            {
                time = 0;
                gameObject.SetActive(false);
                AfterimagePlus.Instance.afterimagePool.ReturnToPool(gameObject);
            }
        }

    }
}
