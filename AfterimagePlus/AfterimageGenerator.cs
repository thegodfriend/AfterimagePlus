﻿using Modding.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AfterimagePlus
{
    internal class AfterimageGenerator : MonoBehaviour
    {
        private ImagePool pool = AfterimagePlus.Instance.afterimagePool;
        private float time;

        /*public void SetPool(ImagePool setTo)
        {
            pool = setTo;
        }*/

        private void Update()
        {
            time += Time.deltaTime;
            if (time > AfterimagePlus.Instance.settings.interval)
            {
                try
                {
                    time = 0;

                    Vector3 pos = transform.position;
                    pos.z += 1e-3f;
                    GameObject newAfterimageFrame = pool.SpawnBlankAfterimage(pos, transform.rotation, transform.localScale);

                    tk2dSpriteAnimator originalAnimator = gameObject.GetComponent<tk2dSpriteAnimator>();
                    tk2dSpriteAnimator newAnimtator = newAfterimageFrame.GetOrAddComponent<tk2dSpriteAnimator>();
                    newAnimtator.SetSprite(originalAnimator.Sprite.Collection, originalAnimator.Sprite.spriteId);
                    newAnimtator.Library = originalAnimator.Library;

                    tk2dSpriteAnimationClip originalClip = originalAnimator.CurrentClip;
                    tk2dSpriteAnimationClip newClip = new tk2dSpriteAnimationClip();
                    newClip.CopyFrom(originalClip);
                    newClip.frames = new tk2dSpriteAnimationFrame[1];
                    newClip.frames[0] = originalClip.frames[originalAnimator.CurrentFrame];
                    newClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;

                    AfterimageFrameAnimator frameAnimator = newAfterimageFrame.GetOrAddComponent<AfterimageFrameAnimator>();
                    frameAnimator.clip = newClip;

                    newAnimtator.enabled = false;
                }
                catch (Exception ex)
                {
                    AfterimagePlus.Instance.Log(ex);
                }
            }
        }
    }
}