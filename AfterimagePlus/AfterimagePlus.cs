using Modding;
using Modding.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AfterimagePlus
{
    public class AfterimagePlus : Mod
    {
        public static AfterimagePlus Instance;

        public Settings settings = new();
        internal ImagePool afterimagePool = new();

        //public override List<ValueTuple<string, string>> GetPreloadNames()
        //{
        //    return new List<ValueTuple<string, string>>
        //    {
        //        new ValueTuple<string, string>("White_Palace_18", "White Palace Fly")
        //    };
        //}

        public AfterimagePlus() : base("Afterimage †") { }
        public override string GetVersion() => "0.0.2-0";

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Log("Initializing");

            Instance = this;

            ModHooks.HeroUpdateHook += HeroUpdateHook;
            ModHooks.OnEnableEnemyHook += ModHooks_OnEnableEnemyHook;

            GameObject afterimageTemplate = new();
            afterimageTemplate.AddComponent<tk2dSprite>();
            afterimageTemplate.AddComponent<tk2dSpriteAnimator>();
            afterimageTemplate.name = "afterimageTemplate";
            UnityEngine.Object.DontDestroyOnLoad(afterimageTemplate);
            afterimageTemplate.SetActive(false);
            afterimagePool.SetTemplate(afterimageTemplate);

            Log("Initialized");
        }

        private bool ModHooks_OnEnableEnemyHook(GameObject enemy, bool isAlreadyDead)
        {
            enemy.GetOrAddComponent<AfterimageGenerator>();
            return isAlreadyDead;
        }

        private void HeroUpdateHook()
        {
            HeroController.instance.gameObject.GetOrAddComponent<AfterimageGenerator>();
        }

    }
}