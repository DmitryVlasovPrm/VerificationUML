namespace Verification.cd_ver.Entities
{
    public class Attribute
    {
        public string Id;
        public string Name;
        public Visibility Visibility;
        public string DataTypeId;

        public Attribute(string id, string name, Visibility visibility, string dataTypeId)
        {
            Id = id;
            Name = name;
            Visibility = visibility;
            DataTypeId = dataTypeId;
        }
    }
}