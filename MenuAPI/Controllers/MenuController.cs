using Common.Models;
using MenuAPI.Repositories;
using MenuAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenuAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMongoRepository<MenuItem> _menuRepository;
        private readonly ISyncService<MenuItem> _menuSyncService;
        public MenuController(IMongoRepository<MenuItem> menuRepository,ISyncService<MenuItem> menuSyncService)
        {
            _menuRepository = menuRepository;
            _menuSyncService = menuSyncService;
        }

        [HttpGet]
        public List<MenuItem> GetAllMenuItems()
        {
            var items = _menuRepository.GetAllMenuItems();

            return items;
        }
        [HttpGet("{Category}")]
        public List<MenuItem> GetMenuItemByCategory(String Category)
        {
            var items = _menuRepository.GetMenuItemByCategory(Category);

            return items;
        }

        [HttpPost]
        public IActionResult Create([FromBody]MenuItem item)
        {
            item.LastChangedAt = DateTime.UtcNow;
            var result = _menuRepository.InsertMenuItem(item);

            _menuSyncService.Upsert(item);


            return Ok(result);
        }

        [HttpPut]
        public IActionResult Upsert([FromBody]MenuItem item)
        {
            if (item.Id == Guid.Empty)
            {
                return BadRequest("Empty Id");
            }
            item.LastChangedAt = DateTime.UtcNow;
            _menuRepository.UpsertItem(item);


            _menuSyncService.Upsert(item);
            return Ok(item);
        }

        [HttpPut("sync")]
        public IActionResult UpsertSync(MenuItem item)
        {
            var existingItem = _menuRepository.GetMenuItemById(item.Id);
            if (existingItem == null || item.LastChangedAt > existingItem.LastChangedAt)
            {
                _menuRepository.UpsertItem(item);

            }
            return Ok();
        }

        [HttpDelete("sync")]
        public IActionResult DeleteSync(MenuItem item)
        {
            var existingItem = _menuRepository.GetMenuItemById(item.Id);
            if (existingItem != null || item.LastChangedAt > existingItem.LastChangedAt)
            {
                _menuRepository.DeleteItem(item.Id);

            }
            return Ok();
        }



        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var item = _menuRepository.GetMenuItemById(id);

            if (item == null)
            {
                return BadRequest("Menu item does not exit");

            }
            _menuRepository.DeleteItem(id);

            item.LastChangedAt = DateTime.UtcNow;
            _menuSyncService.Delete(item);


            return Ok("Menu item deleted" + id);

        }
    }
}
