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
        public tk2dSpriteAnimationClip clip;
        private float time;

        void Update()
        {
            tk2dSpriteAnimator animator = gameObject.GetOrAddComponent<tk2dSpriteAnimator>();
            animator.Play(clip);

            time += Time.deltaTime;
            animator.Sprite.color = new Color(1, 1, 1, (1 - time / AfterimagePlus.Instance.settings.fadeTime));
            if (time > AfterimagePlus.Instance.settings.fadeTime)
            {
                time = 0;
                gameObject.SetActive(false);
                AfterimagePlus.Instance.afterimagePool.ReturnToPool(gameObject);
            }
        }

    }
}
