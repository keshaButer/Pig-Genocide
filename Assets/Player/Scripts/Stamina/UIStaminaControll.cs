using UnityEngine;
using UnityEngine.UI;

public class UIStaminaControll : MonoBehaviour
{
    // если я вычитаю из одной колбочки (она равна 1), то если получается значение
    // меньше нуля, то приравниваю эту колбочку к нулю, а потом из следующей вычитаю остаток (добавляю это отрицательное значение)
    [SerializeField] private Slider bar1;
    [SerializeField] private Slider bar2;
    [SerializeField] private Slider bar3;
    private StaminaControll staminaControll;

    private void OnEnable() => MovementPlayer.OnPlayerSpawned += Initialize;
    private void OnDisable() => MovementPlayer.OnPlayerSpawned -= Initialize;

    public void Initialize()
    {
        staminaControll = GameObject.FindGameObjectWithTag("Player").GetComponent<StaminaControll>();
    }
    private void SynchronizeStamina()
    {
        if (staminaControll.CurrentStamina <= 1)
        {
            bar3.value = 0;
            bar2.value = 0;
            bar1.value = staminaControll.CurrentStamina;
        }
        if (staminaControll.CurrentStamina >= 1 && staminaControll.CurrentStamina <= 2)
        {
            bar3.value = 0;
            bar2.value = staminaControll.CurrentStamina - 1;
            bar1.value = 1;
        }
        if (staminaControll.CurrentStamina >= 2)
        {
            bar3.value = staminaControll.CurrentStamina - 2;
            bar2.value = 1;
            bar1.value = 1;
        }
    }
    private void Update()
    {
        SynchronizeStamina();
    }
}
