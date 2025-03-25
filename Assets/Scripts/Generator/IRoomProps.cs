namespace Generator
{
    public interface IRoomProps
    {
        int Diameter { get; }
        public IAreaGenerator Generator { get; }
    }
}