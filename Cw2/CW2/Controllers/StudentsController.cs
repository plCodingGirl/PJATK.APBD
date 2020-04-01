using System;
using CW2.Models;
using Microsoft.AspNetCore.Mvc;

namespace CW2.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        //httpget, httppost, httpput, httppatch -> załaduj (częściowa aktualizacja), httpdelete

        [HttpGet]
        public IActionResult GetStudents([FromQuery] string orderBy)
        {
            return Ok($"Kowalska, Nowak {orderBy}");
        }

        [HttpPost]
        public IActionResult AddStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }

        [HttpGet("{id}")]
        public IActionResult GetStudentById([FromRoute] int id)
        {
            if (id == 1)
            {
                return Ok("Kowalska");
            }
            else if (id == 2)
            {
                return Ok("Nowak");
            }
            else
            {
                return NotFound($"Nie znaleziono studenta o id {id}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStudent(Student student, [FromRoute] int id)
        {
            return Ok($"Aktualizacja dokończona o id {id}");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent([FromRoute] int id)
        {
           
            return Ok($"Usuwanie ukończone studenta o id {id}");
        }
    }
}