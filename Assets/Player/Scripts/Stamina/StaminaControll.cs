using UnityEngine;

public class StaminaControll : MonoBehaviour
{
    public enum StaminaSpender
    {
        dash,
        jump,
        downkick,
    }
    [SerializeField] float recoverMultiplier = 1;
    public float CurrentStamina
    {
        get
        {
            return currentStamina;
        }
        private set { currentStamina = value; }
    } // вызвать event
    private float currentStamina;
    private void Start()
    {
        CurrentStamina = 3;

        EventManager.Dash += () => SpendStamina(StaminaSpender.dash);
    }
    private void CalculateStamina(float value)
    {
        CurrentStamina -= value;
        CurrentStamina = Mathf.Clamp(CurrentStamina, 0, 3);
    }
    private void Update()
    {
        if (CurrentStamina < 3)
            RecoverStamina();
    }
    public void SpendStamina(StaminaSpender staminaSpender)
    {
        switch (staminaSpender)
        {
            case StaminaSpender.dash:
                CalculateStamina(1);
                break;
            case StaminaSpender.jump:
                
                break;
            case StaminaSpender.downkick:
                
                break;
        }
    }
    private void RecoverStamina()
    {
        CurrentStamina += recoverMultiplier * Time.deltaTime;
        CurrentStamina = Mathf.Clamp(CurrentStamina, 0, 3);
    }
}
