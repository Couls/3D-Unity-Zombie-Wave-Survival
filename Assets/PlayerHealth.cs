using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float hp = 100;
    [SerializeField] private float maxHP = 100;
    [SerializeField] private GameObject HealthUI;

    public void TakeDamage(float damageAmount)
    {
        hp -= damageAmount;

        if (hp < 0)
        {
            hp = 0;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("GameOver");
        }
        HealthUI.GetComponent<TMP_Text>().text = "Health: " + Mathf.Round(hp);
    }

    public void Heal(float healAmount)
    {
        hp += healAmount;

        if (hp > maxHP)
        {
            hp = maxHP;
        }
    }
}