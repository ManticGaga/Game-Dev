using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    public PlayerCam playerCam;
    public Transform orientation;        
    public PlayerModelMovement pmm;
    public PlayerMovementAdvanced pma;    
    public ClimbingDone climbingScriptDone;
    public Animator playerModelAnimator;
    int i;

 

    public int attackNum = 0;
    public float attackResetTime = 1f; // Время до сброса атаки
    public float timeBetweenAttack = 0.5f; // Время для нажатия кнопки между атаками
    float lastAttackTime = 0f; // Время последней атаки
    public string combatstring;
    public string previouscombat;
    public int CombatStyle;

    public string[] AttackNum = { "a0", "a1", "a2", "a3", "a4", "a5"};
    public string atackstring;
    public string previousatack;
    public bool a0;
    public bool a1;
    public bool a2;
    public bool a3;
    public bool a4;
    public bool a5;
    public int aid;

    void Start()
    {
        CombatStyle = 1;
        i = 0;
    }

    // Update is called once per frame
    void Update()
    {
        playerModelAnimator.SetInteger("CombatStyle", CombatStyle);
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerModelAnimator.Play("Boxing");
            CombatStyle = 1;
            Boxing();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerModelAnimator.Play("Taekwondo");
            CombatStyle = 2;
            Taekwando();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            playerModelAnimator.Play("Judo");
            CombatStyle = 3;
            Judo();
        }
        if (Input.GetMouseButtonDown(0))// && Time.time - lastAttackTime > timeBetweenAttack)
        {
            Debug.Log(Time.time);
            Debug.Log(lastAttackTime);
            Debug.Log(attackResetTime);
            attackNum++; // Увеличиваем счетчик атаки            
            if (attackNum > 6) { attackNum = 1; } // Если атак было больше трех, сбрасываем до первой
            playerModelAnimator.SetInteger("AttackingNumber", attackNum);
            Debug.Log("AttackNum: " + attackNum);

            // Проигрываем соответствующую анимацию
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetInteger("AttackNum", attackNum);

                /* Переключаем переменные leftHandAttack и rightHandAttack
                switch (attackNum)
                {
                    case 1:
                        damageDetection.leftHandAttack = false;
                        damageDetection.rightHandAttack = true;
                        Debug.Log("Case 1");
                        break;
                    case 2:
                        damageDetection.leftHandAttack = true;
                        damageDetection.rightHandAttack = false;
                        Debug.Log("Case 2");
                        break;
                    default:
                        damageDetection.leftHandAttack = false;
                        damageDetection.rightHandAttack = false;
                        Debug.Log("Case 3");
                        break;
                }*/
            }

            lastAttackTime = Time.time; // Обновляем время последней атаки

        }      
    }

    private void Boxing()
    {
        

    switch (atackstring)
        {
            case "a1":
                playerModelAnimator.SetBool("a1", true); PreviousAttack();
                break;
            case "a2":
                playerModelAnimator.SetBool("a2", true); PreviousAttack();
                break;
            case "a3":
                playerModelAnimator.SetBool("a3", true); PreviousAttack();
                break;
            case "a4":
                playerModelAnimator.SetBool("a4", true); PreviousAttack();
                break;
            case "a5":
                playerModelAnimator.SetBool("a5", true); PreviousAttack();
                break;
            default:
                playerModelAnimator.SetBool("a0", true); PreviousAttack();
                break;
        }
    }
    private void Taekwando()
    {
        switch (atackstring)
        {
            case "a1":
                playerModelAnimator.SetBool("a1", true); PreviousAttack();
                break;
            case "a2":
                playerModelAnimator.SetBool("a2", true); PreviousAttack();
                break;
            case "a3":
                playerModelAnimator.SetBool("a3", true); PreviousAttack();
                break;
            case "a4":
                playerModelAnimator.SetBool("a4", true); PreviousAttack();
                break;
            case "a5":
                playerModelAnimator.SetBool("a5", true); PreviousAttack();
                break;
            default:
                playerModelAnimator.SetBool("a0", true); PreviousAttack();
                break;
        }
    }
    private void Judo()
    {
        switch (atackstring)
        {
            case "a1":
                playerModelAnimator.SetBool("a1", true); PreviousAttack();
                break;
            case "a2":
                playerModelAnimator.SetBool("a2", true); PreviousAttack();
                break;
            case "a3":
                playerModelAnimator.SetBool("a3", true); PreviousAttack();
                break;
            case "a4":
                playerModelAnimator.SetBool("a4", true); PreviousAttack();
                break;
            case "a5":
                playerModelAnimator.SetBool("a5", true); PreviousAttack();
                break;
            default:
                playerModelAnimator.SetBool("a0", true); PreviousAttack();
                break;
        }
    }

    private void PreviousAttack()
    {
        if (previousatack != atackstring)
        {
            previousatack = atackstring;
            playerModelAnimator.SetBool(previousatack, false);
        }
    }


}
