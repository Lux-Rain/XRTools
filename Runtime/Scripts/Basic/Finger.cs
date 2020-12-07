
namespace Com.TeoDiaz.VR
{
    public enum FingerType
    {
        None,
        Thumb,
        Index,
        Middle,
        Ring,
        Pinky
    }

    public class Finger
    {
        public FingerType type = FingerType.None;
        public float current = 0;
        public float target;

        public Finger(FingerType type)
        {
            this.type = type;
        }
    }
}
