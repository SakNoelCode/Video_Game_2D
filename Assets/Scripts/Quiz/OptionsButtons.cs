using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class OptionsButtons : MonoBehaviour
{
    private Button m_button = null;
    private Text m_textoOption = null;
    private Image m_image = null;
    private Color m_colorOriginal = Color.black;

    public Options options { get;  set; }


    private void Awake()
    {
        m_button = GetComponent<Button>();
        m_image = GetComponent<Image>();
        m_textoOption = transform.GetChild(0).GetComponent<Text>();
        m_colorOriginal = m_image.color;
    }


    public void Construct(Options option, Action<OptionsButtons> callback)
    {
        m_textoOption.text = option.text;
        m_button.onClick.RemoveAllListeners();

        options = option;//Asignar esta opción a la variable options
        m_button.enabled = true;
        m_image.color = m_colorOriginal;

        m_button.onClick.AddListener(delegate
        {
            callback(this);
        });
    }

    public void SetColor(Color c)
    {
        m_button.enabled = false;
        m_image.color = c;

    }

}
