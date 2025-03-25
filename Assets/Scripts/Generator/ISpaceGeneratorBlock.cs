namespace Generator
{
    public interface ISpaceGeneratorBlock
    {
        public Block[][,] Generate(int diameter, int height);
    }
}