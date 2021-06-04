namespace Verification.cd_ver.Entities
{
    public class DataType
    {
        public string Id;
        public string Name;

        public DataType(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public bool IsContainer(string className)
		{
            var containerNamesCount = ReservedNames.ContainerNames.Length;
            for (var i = 0; i < containerNamesCount; i++)
			{
                var curContainer = ReservedNames.ContainerNames[i];
                var startIdx = Name.ToLower().IndexOf(curContainer);

                if (startIdx != -1)
				{
                    startIdx = startIdx + curContainer.Length;
                    if (Name[startIdx] == '<')
					{
                        startIdx++;
                        var endIdx = Name.IndexOf(">", startIdx);
                        if (endIdx != -1)
						{
                            var typeInsideContainer = Name.Substring(startIdx, endIdx - startIdx);
                            if (typeInsideContainer.Contains(className))
                                return true;
						}
                    }
				}
                continue;
			}
            
            return false;
		}
    }
}