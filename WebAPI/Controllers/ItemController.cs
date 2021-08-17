using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ToDoList;
using ToDoList.servicelayer;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{

    [Route("api/[controller]")]
    public class ItemController : Controller
    {

        IOperations itemOperations;

        public ItemController(IOperations itemOperations)
        {
            this.itemOperations = itemOperations;
        }

        // GET: api/item
        [HttpGet]
        public IEnumerable<string> GetTest()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/item/getall
        [HttpGet()]
        [Route("getall")]
        public string GetAll()
        {
            return JsonConvert.SerializeObject(itemOperations.GetAllItems());
        }

        // GET api/item/getsingle
        [HttpGet()]
        [Route("getsingle")]
        public string GetSingle(int itemId)
        {
            return JsonConvert.SerializeObject(itemOperations.GetSingleItem(itemId));
        }

        // POST api/item/create
        [HttpPost]
        [Route("create")]
        public void Create([FromBody] JObject payload)
        {
            itemOperations.AddNewItem(JsonConvert.DeserializeObject<Item>(payload.ToString()));
        }

        // POST api/item/update
        [HttpPost]
        [Route("update")]
        public void Update([FromBody] JObject payload)
        {
            itemOperations.UpdateItem(JsonConvert.DeserializeObject<Item>(payload.ToString()));
        }

        // DELETE api/item/delete
        [HttpDelete]
        [Route("delete")]
        public void Delete([FromBody] JObject payload)
        {
            itemOperations.DeleteItem(JsonConvert.DeserializeObject<Item>(payload.ToString()));
        }

        // DELETE api/item/deletealldone
        [HttpDelete]
        [Route("deletealldone")]
        public void DeleteAllDone()
        {
            itemOperations.DeleteAllDoneItems();
        }
    }
}
