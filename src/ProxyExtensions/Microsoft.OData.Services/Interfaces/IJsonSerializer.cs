using System;
using System.Collections.Generic;

namespace Microsoft.OData.Services.Interfaces
{
    public interface IJsonSerializer
    {
        String Serialize<T>(T objectToSerialize);

        String JsonObjectFromJsonMap(Dictionary<String, String> map);

        T Deserialize<T>(String serializedObject);

        List<T> DeserializeList<T>(String serializedList);
    }
}
