using System.Collections.Generic;

namespace Verification.cd_ver.Entities
{
    public class Elements
    {
        public List<Class> Classes;
        public List<Connection> Connections;
        public List<DataType> Types;
        public List<Dependence> Dependences;
        public List<Comment> Comments; // Могут быть в роли ограничений
        public List<Enumeration> Enumerations;
        public List<Package> Packages;

        public Elements()
        {
            Classes = new List<Class>();
            Connections = new List<Connection>();
            Types = new List<DataType>();
            Dependences = new List<Dependence>();
            Comments = new List<Comment>();
            Enumerations = new List<Enumeration>();
            Packages = new List<Package>();
        }
    }
}
