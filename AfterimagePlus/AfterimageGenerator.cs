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

        //private Color afterimageColor = new Color(0.5f,0.5f,0.5f,1);

        /*public void SetAfterimageColor(Color setTo)
        {
            afterimageColor = setTo;
        }*/

        private void Update()
        {

            Settings settings = AfterimagePlus.Instance.settings;

            time += Time.deltaTime;
            if (time > settings.interval)
            {

                Vector3 pos = transform.position;
                pos.z += 1e-3f;
                GameObject newAfterimageFrame = pool.SpawnBlankAfterimage(pos, transform.rotation, transform.localScale);

                try
                {
                    time = 0;

                    tk2dSpriteAnimator originalAnimator = gameObject.GetComponent<tk2dSpriteAnimator>();
                    tk2dSpriteAnimator newAnimator = newAfterimageFrame.GetOrAddComponent<tk2dSpriteAnimator>();
                    newAnimator.SetSprite(originalAnimator.Sprite.Collection, originalAnimator.Sprite.spriteId);
                    newAnimator.Library = originalAnimator.Library;

                    tk2dSpriteAnimationClip originalClip = originalAnimator.CurrentClip;
                    tk2dSpriteAnimationClip newClip = new tk2dSpriteAnimationClip();
                    newClip.CopyFrom(originalClip);
                    newClip.frames = new tk2dSpriteAnimationFrame[1];
                    newClip.frames[0] = originalClip.frames[originalAnimator.CurrentFrame];
                    newClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;

                    AfterimageFrameAnimator frameAnimator = newAfterimageFrame.GetOrAddComponent<AfterimageFrameAnimator>();
                    frameAnimator.clip = newClip;
                    frameAnimator.color = new Color(settings.red, settings.green, settings.blue, settings.fullStrength);//afterimageColor;

                    newAnimator.enabled = false;

                    foreach (Collider2D collider in newAfterimageFrame.GetComponents<Collider2D>())
                    {
                        Destroy(collider);
                    }
                }
                catch (Exception ex)
                {
                    Destroy(newAfterimageFrame);
                    //AfterimagePlus.Instance.Log(ex);
                }
            }
        }
    }
}
