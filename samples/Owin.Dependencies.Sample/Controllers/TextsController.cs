using Owin.Dependencies.Sample.Repositories;
using System.Collections.Generic;
using System.Web.Http;

namespace Owin.Dependencies.Sample.Controllers
{
    public class TextsController : ApiController
    {
        private readonly IRepository _repo;

        public TextsController(IRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<string> Get()
        {
            return _repo.GetTexts();
        }
    }
}