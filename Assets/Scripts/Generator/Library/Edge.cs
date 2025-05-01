namespace Generator.Library
{
    public class Edge<T>
    {
        public T A;
        public T B;

        public Edge(T a, T b)
        {
            A = a;
            B = b;
        }
        public void Deconstruct(out T a, out T b)
        {
            a = A;
            b = B;
        }
        // ( a, b)
    }
}