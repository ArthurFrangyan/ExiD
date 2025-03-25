namespace Generator
{
    public interface IAreaGenerator
    {
        public Block[,] Generate(int diameter);
    }
}