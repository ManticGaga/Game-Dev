using UnityEngine;
using System.Collections;


public class Stamina : MonoBehaviour
{
    public float maxStamina = 100f;
    public float currentStamina;

    public float staminaRechargeDelay = 1f;
    public float staminaRechargeRate = 20f;

    public float dodgeCost = 20f;
    public float dodgeDuration = 0.5f;
    public float dodgeInvincibilityDuration = 0.3f;

    private float lastStaminaUseTime;

    private void Start()
    {
        currentStamina = maxStamina;
        lastStaminaUseTime = Time.time;
    }

    public bool UseStamina(float staminaCost)
    {
        if (Time.time - lastStaminaUseTime < staminaRechargeDelay)
        {
            return false;
        }

        if (currentStamina < staminaCost)
        {
            return false;
        }

        currentStamina -= staminaCost;
        lastStaminaUseTime = Time.time;

        return true;
    }

    public IEnumerator Dodge()
    {
        if (UseStamina(dodgeCost))
        {
            // Make the player character invincible for a short period
            GetComponent<Collider>().enabled = false;
            yield return new WaitForSeconds(dodgeInvincibilityDuration);
            GetComponent<Collider>().enabled = true;

            // Move the player character forward
            Vector3 forward = transform.forward;
            forward.y = 0;
            forward.Normalize();
            transform.position += forward * 3f;

            // Wait for the dodge duration to end before recharging stamina
            yield return new WaitForSeconds(dodgeDuration - dodgeInvincibilityDuration);

            // Start recharging stamina
            while (currentStamina < maxStamina)
            {
                currentStamina += staminaRechargeRate * Time.deltaTime;
                currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
                yield return null;
            }
        }
    }

    private void Update()
    {
        if (currentStamina < maxStamina && Time.time - lastStaminaUseTime > staminaRechargeDelay)
        {
            currentStamina += staminaRechargeRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }
    }
}
