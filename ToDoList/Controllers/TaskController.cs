using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class TaskController : Controller
    {
        [HttpGet("/items/{id}/update")]
        public ActionResult UpdateForm(int id)
        {
            ListMaker thisItem = ListMaker.Find(id);
            return View(thisItem);
        }

        [HttpPost("/items/{id}/update")]
        public ActionResult Update(int id)
        {
            ListMaker thisItem = ListMaker.Find(id);
            thisItem.Edit(Request.Form["newname"]);
            return RedirectToAction("Tasks");
        }

        [HttpGet("/items/{id}/delete")]
        public ActionResult Delete(int id)
        {
            ListMaker thisItem = ListMaker.Find(id);
            thisItem.Delete();
            return RedirectToAction("Tasks");
        }

        [HttpGet("/form")]
        public ActionResult Form()
        {
            return View();
        }

        [HttpGet("/tasks")]
        public ActionResult Tasks()
        {
            List<ListMaker> taskList = ListMaker.GetAll();

            return View(taskList);
        }

        [HttpPost("/tasks")]
        public ActionResult Create(string description)
        {
            ListMaker newTask = new ListMaker(description);
            newTask.Save();

            return RedirectToAction("Tasks");
        }
    }
}
