using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Random = System.Random;


public static class GameObjectUtils
{
    public static void ClearAllChild(this Transform parent)
    {
        foreach (Transform child in parent)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public static T CreateChild<T>(this Transform parent, T prefab) where T : MonoBehaviour
    {
        var obj = GameObject.Instantiate<T>(prefab, parent);
        return obj;
    }

    private static Random rng = new Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
    public static void MoveHexTypeToEnd<T>(this IList<T> list, T hexType)
    {
        int n = list.Count;
        for (int i = n - 1; i >= 0; i--)
        {
            if (list[i].Equals(hexType))
            {
                T value = list[i];
                list.RemoveAt(i);
                list.Add(value);
            }
        }
    }
    
    public static int GetSceneIndexByName(string sceneName)
    {
        // Duyệt qua các scene đã được thêm vào Build Settings
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneFileName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            if (sceneFileName == sceneName)
            {
                return i; // Trả về index của scene nếu tên khớp
            }
        }

        Debug.LogWarning($"Scene with name {sceneName} not found in Build Settings.");
        return -1; // Trả về -1 nếu không tìm thấy scene
    }
}

public static class TransformUtils
{
    /// <summary>
    /// Sets the X component of the given transform's position.
    /// </summary>
    /// <param name="transform">Transform to modify</param>
    /// <param name="value">New X</param>
    /// <param name="local">Specify true to apply the value in local space</param>
    /// <returns>Given transform to allow for chaining</returns>
    public static Transform SetX(this Transform transform, float value, bool local = false)
    {
        if (transform == null)
        {
            throw new ArgumentException("transform is null", "transform");
        }

        if (local)
        {
            transform.localPosition = new Vector3(value, transform.localPosition.y, transform.localPosition.z);
        }
        else
        {
            transform.position = new Vector3(value, transform.position.y, transform.position.z);
        }

        return transform;
    }

    /// <summary>
    /// Sets the Y component of the given transform's position.
    /// </summary>
    /// <param name="transform">Transform to modify</param>
    /// <param name="value">New Y</param>
    /// <param name="local">Specify true to apply the value in local space</param>
    /// <returns>Given transform to allow for chaining</returns>
    public static Transform SetY(this Transform transform, float value, bool local = false)
    {
        if (transform == null)
        {
            throw new ArgumentException("transform is null", "transform");
        }

        if (local)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, value, transform.localPosition.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, value, transform.position.z);
        }

        return transform;
    }

    /// <summary>
    /// Sets the Z component of the given transform's position.
    /// </summary>
    /// <param name="transform">Transform to modify</param>
    /// <param name="value">New Z</param>
    /// <param name="local">Specify true to apply the value in local space</param>
    /// <returns>Given transform to allow for chaining</returns>
    public static Transform SetZ(this Transform transform, float value, bool local = false)
    {
        if (transform == null)
        {
            throw new ArgumentException("transform is null", "transform");
        }

        if (local)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, value);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, value);
        }

        return transform;
    }

    /// <summary>
    /// Sets the X component of the given transform's local scale.
    /// </summary>
    /// <param name="transform">Transform to modify</param>
    /// <param name="value">New X</param>
    /// <returns>Given transform to allow for chaining</returns>
    public static Transform ScaleX(this Transform transform, float value)
    {
        if (transform == null)
        {
            throw new ArgumentException("transform is null", "transform");
        }

        transform.localScale = new Vector3(value, transform.localScale.y, transform.localScale.z);
        return transform;
    }

    /// <summary>
    /// Sets the Y component of the given transform's local scale.
    /// </summary>
    /// <param name="transform">Transform to modify</param>
    /// <param name="value">New Y</param>
    /// <returns>Given transform to allow for chaining</returns>
    public static Transform ScaleY(this Transform transform, float value)
    {
        if (transform == null)
        {
            throw new ArgumentException("transform is null", "transform");
        }

        transform.localScale = new Vector3(transform.localScale.x, value, transform.localScale.z);
        return transform;
    }

    /// <summary>
    /// Sets the Z component of the given transform's local scale.
    /// </summary>
    /// <param name="transform">Transform to modify</param>
    /// <param name="value">New Z</param>
    /// <returns>Given transform to allow for chaining</returns>
    public static Transform ScaleZ(this Transform transform, float value)
    {
        if (transform == null)
        {
            throw new ArgumentException("transform is null", "transform");
        }

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, value);
        return transform;
    }

    /// <summary>
    /// Sets the X component of the given transform's rotation.
    /// </summary>
    /// <param name="transform">Transform to modify</param>
    /// <param name="value">New X</param>
    /// <param name="local">Specify true to apply the value in local space</param>
    /// <returns>Given transform to allow for chaining</returns>
    public static Transform RotateX(this Transform transform, float value, bool local = false)
    {
        if (transform == null)
        {
            throw new ArgumentException("transform is null", "transform");
        }

        var euler = (local ? transform.localRotation : transform.rotation).eulerAngles;
        euler.x = value;
        if (local)
        {
            transform.localRotation = Quaternion.Euler(euler);
        }
        else
        {
            transform.rotation = Quaternion.Euler(euler);
        }

        return transform;
    }

    /// <summary>
    /// Sets the Y component of the given transform's rotation.
    /// </summary>
    /// <param name="transform">Transform to modify</param>
    /// <param name="value">New Y</param>
    /// <param name="local">Specify true to apply the value in local space</param>
    /// <returns>Given transform to allow for chaining</returns>
    public static Transform RotateY(this Transform transform, float value, bool local = false)
    {
        if (transform == null)
        {
            throw new ArgumentException("transform is null", "transform");
        }

        var euler = (local ? transform.localRotation : transform.rotation).eulerAngles;
        euler.y = value;
        if (local)
        {
            transform.localRotation = Quaternion.Euler(euler);
        }
        else
        {
            transform.rotation = Quaternion.Euler(euler);
        }

        return transform;
    }

    /// <summary>
    /// Sets the Z component of the given transform's rotation.
    /// </summary>
    /// <param name="transform">Transform to modify</param>
    /// <param name="value">New Z</param>
    /// <param name="local">Specify true to apply the value in local space</param>
    /// <returns>Given transform to allow for chaining</returns>
    public static Transform RotateZ(this Transform transform, float value, bool local = false)
    {
        if (transform == null)
        {
            throw new ArgumentException("transform is null", "transform");
        }

        var euler = (local ? transform.localRotation : transform.rotation).eulerAngles;
        euler.z = value;
        if (local)
        {
            transform.localRotation = Quaternion.Euler(euler);
        }
        else
        {
            transform.rotation = Quaternion.Euler(euler);
        }

        return transform;
    }
   
}
public static class MaterialExtensions
{
    public static void ToOpaqueMode(this Material material)
    {
        material.SetOverrideTag("RenderType", "");
        material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.Zero);
        material.SetInt("_ZWrite", 1);
        material.DisableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = -1;
    }
  
    public static void ToFadeMode(this Material material)
    {
        material.SetOverrideTag("RenderType", "Transparent");
        material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = (int) UnityEngine.Rendering.RenderQueue.Transparent;
    }
}