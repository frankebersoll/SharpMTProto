using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.Storage;
using SharpMTProto.Client;

namespace SharpMTProto.Extra.WinRT
{
    public class RTSettingsPersistance : IPersistance
    {
        private readonly DataContractSerializer _serializer = new DataContractSerializer(typeof(PersistanceInfo));
        private const string Key = "SharpMTProto";

        public Task Save(PersistanceInfo persistanceInfo)
        {
            var builder = new StringBuilder();
            using (var writer = XmlWriter.Create(builder))
            _serializer.WriteObject(writer, persistanceInfo);
            string xml = builder.ToString();

            ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
            settings.Values[Key] = xml;
            return Task.FromResult(true);
        }

        public Task<PersistanceInfo> Load()
        {
            ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
            var xml = (string)settings.Values[Key];
            if (xml == null) return Task.FromResult<PersistanceInfo>(null);

            try
            {
                using (var reader = XmlReader.Create(new StringReader(xml)))
                    return Task.FromResult((PersistanceInfo)this._serializer.ReadObject(reader));
            }
            catch (XmlException)
            {
                return Task.FromResult<PersistanceInfo>(null);
            }
        }

        public Task Clear()
        {
            ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
            settings.Values.Remove(Key);

            return Task.FromResult(true);
        }
    }
}