using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUIController : MonoBehaviour
{

    public EnemyController boss;
    public HeroMovement hero;
    public Slider bossHealth;
    public Slider heroHealth;
    void Start()
    {
        
        
    }

    void Update()
    {
        bossHealth.value = boss.health;
        heroHealth.value = hero.health;
        if(boss == null)
        {
            SceneManager.LoadScene("YouWin");
        }
        if(hero == null)
        {
            SceneManager.LoadScene("YouLose");
        }
        
    }
}
