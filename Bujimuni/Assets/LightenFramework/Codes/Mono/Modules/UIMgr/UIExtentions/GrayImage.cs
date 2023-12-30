using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class GrayImage : MonoBehaviour
{
    public Material grayMaterial;

    //指定Image组件
    public Image[] images;

    [HorizontalGroup]
    [Button("变 灰", ButtonSizes.Medium)]
    void EditSetGray()
    {
        SetGray();
    }
    [HorizontalGroup]
    [Button("恢 复", ButtonSizes.Medium)]
    void EditRestore()
    {
        Restore();
    }
    
    //记录图片原始的材质
    protected Dictionary<Image, Material> m_originalMaterials = new Dictionary<Image, Material>();
    
    public void SetGray()
    {
        foreach (var image in images)
        {
            if (image != null)
            {
                SetGrayInterenal(image);
            }
        }
    }

    public void Restore()
    {
        foreach (var image in images)
        {
            if (image != null)
            {
                RestoreInterval(image);
            }
        }
    }

    private const string DEFAULT_UI_MATERIAL_NAME = "Default UI Material";
    protected void SetGrayInterenal(Image image)
    {
        if (grayMaterial == null || image == null || image.material == grayMaterial)
            return;
        if (image.material == null || image.material.name == DEFAULT_UI_MATERIAL_NAME)
        {
            image.material = grayMaterial;
        }
        else
        {
            m_originalMaterials[image] = image.material;
            image.material = grayMaterial;
        }
    }

    protected void RestoreInterval(Image image)
    {
        if (image == null || image.material != grayMaterial)
            return;
        if (m_originalMaterials.ContainsKey(image))
        {
            image.material = m_originalMaterials[image];
            m_originalMaterials.Remove(image);
        }
        else
        {
            image.material = null;
        }
    }
}