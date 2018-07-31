namespace No7.Solution
{
    public interface IRepository
    {
        void Save(string connectionString);
        void AddField(string data, int lineCounter);

    }
}
