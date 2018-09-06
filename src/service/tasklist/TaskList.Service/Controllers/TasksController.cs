using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskList.Common;
using TaskList.Service.Models;

namespace TaskList.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ICrudProvider crud;
        private readonly ILogger logger;

        public TasksController(ICrudProvider crud, ILogger<TasksController> logger)
        {
            this.crud = crud;
            this.logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var task = await crud.GetByIdAsync<TaskItem>(id);
            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync() => Ok((await crud.GetAllAsync<TaskItem>()).OrderByDescending(x => x.DateOpened));

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] TaskItem task)
        {
            if (string.IsNullOrEmpty(task.Name))
            {
                return BadRequest("未指定任务项目的名称。");
            }

            var existingItem = await crud.FindBySpecificationAsync<TaskItem>(item => item.Name == task.Name);
            if (existingItem != null && existingItem.Count() > 0)
            {
                return Conflict($"名称为{task.Name}的任务项目已经存在。");
            }

            await crud.AddAsync(task);

            return Created(Url.Action("GetByIdAsync", new { id = task.Id }), task.Id);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await crud.DeleteByIdAsync<TaskItem>(id);

            return NoContent();
        }
    }
}