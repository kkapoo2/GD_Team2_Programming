using UnityEngine;

public class CorpseManager : MonoBehaviour
{
    private static CorpseManager instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
    }

    
}
