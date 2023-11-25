using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameObjectExtentions
{
    /// <summary>
    /// changes the layers of an object recursively (cahanges apply to children)
    /// </summary>
    public static void SetLayerRecursively(Transform rootTransform, LayerMask newLayer)
    {
        rootTransform.gameObject.layer = newLayer;

        for (int i = 0; i < rootTransform.childCount; i++)
        {
            Transform child = rootTransform.GetChild(i);
            SetLayerRecursively(child, newLayer);
        }
    }

    public static void SetGameObjectColliders(GameObject gameObject, bool active)
    {
        foreach (Collider collider in gameObject.GetComponents<Collider>()) collider.enabled = active;
    }

    public static void DestroyChildren(GameObject Parant)
    {
        List<GameObject> childrenList = new List<GameObject>();
        for (int i = 0; i < Parant.transform.childCount; i++)
            childrenList.Add(Parant.transform.GetChild(i).gameObject);


        foreach (GameObject child in childrenList)
            MonoBehaviour.DestroyImmediate(child);
    }

    public static Quaternion CalculateRandomYRotation(Transform objectToRotate)
    {
        return new Quaternion(objectToRotate.rotation.x, Random.rotation.y, objectToRotate.rotation.z, objectToRotate.rotation.w);
    }

    public static void SetImageToIcon(Image image, Sprite icon)
    {
        image.sprite = icon;
        image.SetNativeSize();
    }

    public static float NormalizedNumber(float current, float max) => current / max;
}
