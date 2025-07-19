namespace Session
{
    public class TemporalSessionModifier
    {
        public float Duration { get; private set; }
        public float Elapsed { get; private set; }
        public bool Active { get; private set; }

        public TemporalSessionModifier(float duration)
        {
            this.Duration = duration;
        }

        public void Start()
        {
            Elapsed = 0f;
            Active = true;
        }

        public void Update(float timeElapsed)
        {
            if (!Active) return;

            Elapsed += timeElapsed;
            if (Elapsed >= Duration)
            {
                Active = false;
            }
        }

        public void End()
        {
            if (Active)
            {
                Active = false;
            }
        }
    }
}