using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LabCMS.FixtureDomain.Shared.Events;

namespace LabCMS.FixtureDomain.Shared.Models
{
    public record FixtureEventInDatabase
    {
        public FixtureEventInDatabase(
            int fixtureNo,
            string contentTypeFullName,
            string content)
        {
            FixtureNo = fixtureNo;
            ContentTypeFullName = contentTypeFullName;
            Content = content;
        }

        [Key]
        public int No { get; set; }

        [ForeignKey(nameof(Fixture))]
        public int FixtureNo { get; private set; }
        public Fixture? Fixture { get; set; }
        public string ContentTypeFullName { get; private set; }
        public string Content { get; private set; }
        public FixtureEvent? GetPayload() =>
            JsonSerializer.Deserialize(Content, _assembly.GetType(ContentTypeFullName)!) as FixtureEvent;
        private static readonly Assembly _assembly = typeof(FixtureEvent).Assembly;
    
        public static FixtureEventInDatabase GetEntity(FixtureEvent fixtureEvent)
        {
            string content = JsonSerializer.Serialize(fixtureEvent, fixtureEvent.GetType());
            return new(fixtureEvent.FixtureNo,fixtureEvent.TypeFullName,content);
        }
    }
}
