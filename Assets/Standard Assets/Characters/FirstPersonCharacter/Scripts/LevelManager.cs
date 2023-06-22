using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public int UnlockedLevels;
    public int LoadingLevel;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        UnlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
