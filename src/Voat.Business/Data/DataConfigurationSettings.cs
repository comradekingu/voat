using System.Collections.Generic;
using System.Linq;
using Voat.Common;
using Voat.Common.Configuration;

namespace Voat.Data
{
   
    public class DataConfigurationSettings : UpdatableConfigurationSettings<DataConfigurationSettings>
    {
        List<DataConnection> _connections = new List<DataConnection>();

        public DataStoreType StoreType { get; set; }

        public List<DataConnection> Connections {
            get => _connections;
            set
            {
                List<DataConnection> newConnections = new List<DataConnection>();

                if (value != null && value.Any())
                {
                    value.ForEach(c => {
                        var names = c.Name.Split(new[] { ',', ';' }, System.StringSplitOptions.RemoveEmptyEntries);
                        foreach (var name in names)
                        {
                            var safeName = name.TrimSafe();
                            if (!string.IsNullOrEmpty(safeName))
                            {
                                newConnections.Add(new DataConnection() { Name = safeName, Value = c.Value });
                            }
                        }
                    });
                }
                _connections = newConnections;
            }

        }

    }
    public class DataConnection
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
