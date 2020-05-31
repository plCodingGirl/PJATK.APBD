using System;
using System.Linq;
using CW2.DAL;
using CW2.Models;
using CW2.ModelsEf;
using Microsoft.AspNetCore.Mvc;
using Student = CW2.Models.Student;

namespace CW2.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IDbService _dbService;
        private readonly StudentsDbContext _studentsDbContext;

        public StudentsController(IDbService dbService, StudentsDbContext studentsDbContext)
        {
            _studentsDbContext = studentsDbContext;
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            var students =
                _studentsDbContext.Student.ToList();
            return Ok(students);
        }

        [HttpPost]
        public IActionResult AddStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }

        [HttpGet("{id}")]
        public IActionResult GetStudentById([FromRoute] string id)
        {
            var student = _dbService.GetStudentByIndexNumber(id);
            if (student != null)
            {
                return Ok(student);
            }

            return NotFound($"Nie znaleziono studenta o id {id}");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStudent(UpdateStudentDTO updateStudentDto, [FromRoute] string id)
        {
            var student = 
                _studentsDbContext.Student.FirstOrDefault(x => x.IndexNumber == id);
            if (student == null)
            {
                return NotFound($"Nie znaleziono studenta o id {id}");
            }

            student.FirstName = updateStudentDto.FirstName;
            student.LastName = updateStudentDto.LastName;
            student.BirthDate = updateStudentDto.BirthDate;

            _studentsDbContext.SaveChanges();

            return Ok(student);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent([FromRoute] string id)
        {
            var student =
                _studentsDbContext.Student.FirstOrDefault(x => x.IndexNumber == id);
            if (student == null)
            {
                return NotFound($"Nie znaleziono studenta o id {id}");
            }

            _studentsDbContext.Student.Remove(student);

            _studentsDbContext.SaveChanges();

            return Ok();
        }
    }
}