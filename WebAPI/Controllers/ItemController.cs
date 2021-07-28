using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{

    [Route("api/[controller]")]
    public class ItemController : Controller
    {

        WebItem webItem = new();

        // GET: api/item
        [HttpGet]
        public IEnumerable<string> GetTest()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/item/get
        [HttpGet()]
        [Route("get")]
        public string Get()
        {
            return webItem.GetAllItems();
        }

        // POST api/item/create
        [HttpPost]
        [Route("create")]
        public void Create([FromBody] JObject payload)
        {
            webItem.AddNewItem(payload);
        }

        // POST api/item/update
        [HttpPost]
        [Route("update")]
        public void Update([FromBody] JObject payload)
        {
            webItem.UpdateItem(payload);
        }

        // DELETE api/item/delete
        [HttpDelete]
        [Route("delete")]
        public void Delete([FromBody] JObject payload)
        {
            webItem.DeleteItem(payload);
        }

        // DELETE api/item/deletealldone
        [HttpDelete]
        [Route("deletealldone")]
        public void DeleteAllDone()
        {
            webItem.DeleteAllDone();
        }
    }
}
