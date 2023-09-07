using System.Text.Json;

namespace NexusAop.Console.Models
{
    public class FooEntity
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}