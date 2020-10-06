using System.Collections;
using TMPro;
using UnityEngine;

[AddComponentMenu("Health")]
[RequireComponent(typeof(Health))]
public class Dummy : MonoBehaviour
{
    private Health healthRef;
    private const float baseHP = 100f;
    private float currentHP;

    private bool isAttackStopped = true;
    private bool isGettingHealed = false;

    [SerializeField]
    private TextMeshProUGUI logText;

    private float delayTime = 1f;

    private void Awake()
    {
        healthRef = GetComponent<Health>();
        ResetComponents();
    }
    private void Start()
    {
        logText.text = healthRef.SetTotalHealth(baseHP).ToString();
    }

    private void Update()
    {
        //Just For testing will change once dmg system is implemented
        if (Input.GetKeyDown(KeyCode.Space))
        {
            delayTime = 1f;
            if (isGettingHealed) //Means there is possibilty that Coroutine is running, Also needs to reset HP
            {
                StopCoroutine(StartHealthRecovery());
                ResetComponents();
            }
            isAttackStopped = false;
            currentHP = healthRef.DamageHealth(baseHP * 0.15f);
            logText.text = currentHP.ToString();
        }

        if (currentHP < baseHP && !isAttackStopped)
        {
            if (delayTime > 0)
            {
                delayTime -= Time.deltaTime;
            }
            else
            {
                isAttackStopped = true;
                StopCoroutine(StartHealthRecovery()); //Safety Purpose
                StartCoroutine(StartHealthRecovery());
            }
        }
    }

    public IEnumerator StartHealthRecovery()
    {
        if (!isAttackStopped)
            yield return null;

        isGettingHealed = true;

        while (currentHP < baseHP && isGettingHealed)
        {
            if (isAttackStopped && isGettingHealed)
            {
                currentHP++;
                logText.text = currentHP.ToString();
                yield return new WaitForEndOfFrame();
            }
            else if (!isAttackStopped)
            {
                StopCoroutine(StartHealthRecovery());
                break;
            }
        }
        if (isGettingHealed)
        {
            ResetComponents();
        }
    }

    private void ResetComponents()
    {
        currentHP = baseHP;
        healthRef.SetTotalHealth(baseHP);
        logText.text = currentHP.ToString();
        isGettingHealed = false;
    }
}
