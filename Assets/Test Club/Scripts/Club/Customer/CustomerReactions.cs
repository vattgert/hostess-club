using Characters;
using UnityEngine;

public class CustomerReactions : MonoBehaviour
{
    private readonly int HEART_FRAME = 0;
    private readonly int BROKEN_HEART_FRAME = 1;

    [SerializeField]
    private ParticleSystem particlesSystem;

    public void Setup(SessionEvents events)
    {

        events.OnCompatibilityReaction += (customer, compatibility) =>
        {
            CompatibilityReaction(compatibility);
        };
    }

    public void CompatibilityReaction(Compatibility compatibility)
    {
        if (particlesSystem == null)
        {
            Debug.LogWarning("CustomerReaction: No ParticleSystem assigned.");
            return;
        }

        var texSheet = particlesSystem.textureSheetAnimation;
        if(compatibility == Compatibility.Full)
        {
            texSheet.frameOverTime = new ParticleSystem.MinMaxCurve(HEART_FRAME);
        } else if (compatibility == Compatibility.Absent)
        {
            texSheet.frameOverTime = new ParticleSystem.MinMaxCurve(BROKEN_HEART_FRAME);
        } else
        {
            Debug.Log($"Compatibility is {compatibility} so nothing really should happen");
        }

        particlesSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        particlesSystem.Play();
    }
}
