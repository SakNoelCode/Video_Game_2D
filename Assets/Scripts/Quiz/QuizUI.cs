using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class QuizUI : MonoBehaviour
{
    [SerializeField] private Text m_question = null;
    [SerializeField] private List<OptionsButtons> m_buttonList = null;

    public void construct(Question q, Action<OptionsButtons> callback)
    {
        m_question.text = q.text;

        for (int i = 0; i < m_buttonList.Count; i++)
        {
            m_buttonList[i].Construct(q.options[i], callback);
        }
    }

    public void desactivaButtons()
    {
        foreach (OptionsButtons button in m_buttonList)
        {
            button.desactivaButton();
        }
    }

    public void activaButtons()
    {
        foreach (OptionsButtons button in m_buttonList)
        {
            button.activaButton();
        }
    }

}
