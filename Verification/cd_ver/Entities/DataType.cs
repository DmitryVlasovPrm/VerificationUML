namespace Verification.cd_ver.Entities
{
    public class DataType
    {
        public string Id;
        public string Name;
        public bool IsContainer;

        public DataType(string id, string name, bool isContainer)
        {
            Id = id;
            Name = name;
            IsContainer = isContainer;
        }
    }
}