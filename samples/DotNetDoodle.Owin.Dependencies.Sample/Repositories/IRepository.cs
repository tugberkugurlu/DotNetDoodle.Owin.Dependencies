using System;
using System.Collections.Generic;

namespace DotNetDoodle.Owin.Dependencies.Sample.Repositories
{
    public interface IRepository : IDisposable
    {
        string GetRandomText();
        IEnumerable<string> GetTexts();
    }
}
