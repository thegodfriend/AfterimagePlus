using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AfterimagePlus
{
    internal class ImagePool
    {
        private GameObject imageTemplate;
        private List<GameObject> inactiveImages = new List<GameObject>();

        public void SetTemplate(GameObject template)
        {
            imageTemplate = template;
        }

        public GameObject SpawnBlankAfterimage(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            GameObject newBlankImage;

            if (inactiveImages.Count != 0)
            {
                newBlankImage = inactiveImages[0];
                newBlankImage.SetActive(true);
                inactiveImages.RemoveAt(0);
            }
            else
            {
                newBlankImage = UnityEngine.Object.Instantiate(imageTemplate);
                newBlankImage.SetActive(true);
            }

            newBlankImage.transform.position = position;
            newBlankImage.transform.rotation = rotation;
            newBlankImage.transform.localScale = scale;
            newBlankImage.name = "newAfterimage";
            newBlankImage.tag = "Untagged";

            GameObject.DontDestroyOnLoad(newBlankImage);

            return newBlankImage;
        }

        public void ReturnToPool(GameObject image)
        {
            inactiveImages.Add(image);
        }

    }
}
