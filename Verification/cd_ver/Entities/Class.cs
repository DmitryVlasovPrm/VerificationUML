using System.Collections.Generic;

namespace Verification.cd_ver.Entities
{
    public class Class
    {
        public string Id;
        public string Name;
        public BoundingBox Box;
        public List<Attribute> Attributes;
        public List<Operation> Operations;
        public List<string> GeneralClassesIdxs;
        public bool IsInterface;
        public List<string> InterfacesIdxs;

        public Class(string id, string name, BoundingBox box, List<Attribute> attributes, List<Operation> operations,
            List<string> generalClassesIdxs, bool isInterface, List<string> interfacesIdxs)
        {
            Id = id;
            Name = name;
            Box = box;
            Attributes = new List<Attribute>(attributes);
            Operations = new List<Operation>(operations);
            GeneralClassesIdxs = generalClassesIdxs;
            IsInterface = isInterface;
            InterfacesIdxs = interfacesIdxs;
        }
    }
}