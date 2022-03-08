using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuizDB: MonoBehaviour
{
    [SerializeField] private List<Question> listQuestions = null;
    private List<Question> m_questionBackup = null;

    private void Awake()
    {
        m_questionBackup = listQuestions.ToList();
    }


    public Question GetRandom(bool remove = true)
    {
        if (listQuestions.Count == 0) restoreBackup();

        int index = Random.Range(0, listQuestions.Count);

        if (!remove)
            return listQuestions[index];

        Question q = listQuestions[index];
        listQuestions.RemoveAt(index);

        return q;
    }



    private void restoreBackup()
    {
        listQuestions = m_questionBackup.ToList();
    }
}
