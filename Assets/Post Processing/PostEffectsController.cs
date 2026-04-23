using UnityEngine;
using VContainer;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostEffectsController : MonoBehaviour
{
    public static PostEffectsController SingleTon;
    private Animator animator;
    private Volume[] volumes;
    private Volume mainVolume;
    private ChannelMixer channelMixer;

    [Inject] private IPlayerEvents _playerEvents;

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

        _playerEvents.OnParry += () => FlashBang(0.1f);
        _playerEvents.OnPlayerSitDown += () => SetVignette(true);
        _playerEvents.OnPlayerStandUp += () => SetVignette(false);

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
