using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostEffectsController : MonoBehaviour
{
    public static PostEffectsController SingleTon;
    private Animator animator;
    private Volume[] volumes;
    private Volume mainVolume;
    private ChannelMixer channelMixer;
    private void Awake()
    {
        if (SingleTon == null)
            SingleTon = this;
        else Destroy(this);
    }
    private void Start()
    {
        volumes = GetComponents<Volume>();
        animator = GetComponent<Animator>();
        mainVolume = volumes[1];
        mainVolume.profile.TryGet(out channelMixer);
        channelMixer.active = false;

        EventManager.Parry += () => FlashBang(0.1f);
        EventManager.SatDown += () => SetVignette(true);
        EventManager.StandUp += () => SetVignette(false);

    }
    public void FlashBang(float time)
    {
        channelMixer.active = true;
        Invoke(nameof(ResetVolume), time);
    }
    private void ResetVolume()
    {
        channelMixer.active = false;
    }
    private void SetVignette(bool turn)
    {
        if (turn) animator.SetTrigger("isShowVignette");
        else animator.SetTrigger("isHideVignette");
    }
}
