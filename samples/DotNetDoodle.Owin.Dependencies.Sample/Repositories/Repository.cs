using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace DotNetDoodle.Owin.Dependencies.Sample.Repositories
{
    public class Repository : IRepository
    {
        private readonly RequestHandler _requestHandler;

        private static readonly IEnumerable<string> RandomTexts = new List<string> 
        {
            "Yes! This is my favorite sound in the whole world",
            "Excellent. Don't hold back!",
            "Great job! It smells like roses in here now",
            "Sometimes you just have to let 'em rip",
            "Ahhh, that felt right",
            "Ooops, I'm not sure about that one",
            "Wow, someone has been practicing",
            "You might want to be a little careful",
            "What a stinker. The paint is coming off the walls"
        };

        public Repository(RequestHandler requestHandler)
        {
            if (requestHandler == null) throw new ArgumentNullException("requestHandler");
            _requestHandler = requestHandler;
            WriteAndAddInfo("Constructor");
        }

        public string GetRandomText()
        {
            WriteAndAddInfo("Getting the random text");
            return RandomTexts.ElementAt(new Random().Next(RandomTexts.Count()));
        }

        public IEnumerable<string> GetTexts()
        {
            WriteAndAddInfo("Getting all the texts");
            return RandomTexts;
        }

        public void Dispose()
        {
            WriteAndAddInfo("Dispose");
        }

        private void WriteAndAddInfo(string message)
        {
            Console.WriteLine(GetType().ToString() + ": " + message);
            Startup.TypeOperations.AddOrUpdate(GetType(),
                (type) => new ConcurrentBag<string>(new[] { message }),
                (type, bag) => { bag.Add(message); return bag; });
        }
    }
}