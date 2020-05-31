using System;
using System.Linq;
using CW2.Models;
using CW2.ModelsEf;
using CW2.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student = CW2.ModelsEf.Student;

namespace CW2.Controllers
{
    [Route("api/enrollments")]
    [Authorize(Roles = Roles.Employee)]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly StudentsDbContext _studentsDbContext;

        public EnrollmentsController(StudentsDbContext studentsDbContext)
        {
            _studentsDbContext = studentsDbContext;
        }

        [HttpPost]
        public IActionResult CreateStudent(CreateStudentDTO createStudentDto)
        {
            var studies = _studentsDbContext.Studies
                .Include(x => x.Enrollments)
                .FirstOrDefault(x => x.Name == createStudentDto.Studies);

            if (studies == null)
            {
                return BadRequest($"Nie znaleziono kierunku studiow {createStudentDto.Studies}");
            }

            if (_studentsDbContext.Student.Any(x => x.IndexNumber == createStudentDto.IndexNumber))
            {
                return BadRequest($"Istnieje juz student z numerem indeksu {createStudentDto.IndexNumber}");
            }

            var enrollment = studies.Enrollments.OrderByDescending(x => x.StartDate)
                .FirstOrDefault(x => x.Semester == 1);
            if (enrollment == null)
            {
                enrollment = new ModelsEf.Enrollment()
                {
                    IdEnrollment = _studentsDbContext.Enrollment.Max(x => x.IdEnrollment) + 1,
                    Studies = studies,
                    StartDate = DateTime.Now,
                    Semester = 1
                };

                _studentsDbContext.Enrollment.Add(enrollment);
            }

            var password = PasswordHelper.HashPassword(createStudentDto.Password);
            var student = new Student()
            {
                IndexNumber = createStudentDto.IndexNumber,
                Password = password.PasswordHash,
                PasswordSalt = password.Salt,
                LastName = createStudentDto.LastName,
                FirstName = createStudentDto.FirstName,
                BirthDate = createStudentDto.BirthDate,
                Enrollment = enrollment
            };

            _studentsDbContext.Student.Add(student);
            _studentsDbContext.SaveChanges();

            return Created("", enrollment);
        }

        [HttpPost("promotions")]
        public IActionResult PromoteStudents(PromoteStudentsDTO promoteStudents)
        {
            var studies = _studentsDbContext.Studies
                .Include(x => x.Enrollments)
                .FirstOrDefault(x => x.Name == promoteStudents.Studies);

            if (studies == null)
            {
                return NotFound($"Nie znaleziono kierunku studiow {promoteStudents.Studies}");
            }

            if (studies.Enrollments.All(x => x.Semester != promoteStudents.Semester)) 
            {
                return NotFound($"Nie znaleziono semestru {promoteStudents.Semester}");
            }

            // sprawdź czy istnieje enrollment dla następnego semestru
            var nextSemesterEnrollment = studies.Enrollments
                .OrderByDescending(x => x.StartDate)
                .FirstOrDefault(x => x.Semester == promoteStudents.Semester + 1);
            if (nextSemesterEnrollment == null)
            {
                // tworzymy nowy enrollment z semestrem + 1
                nextSemesterEnrollment = new ModelsEf.Enrollment()
                {
                    IdEnrollment = _studentsDbContext.Enrollment.Max(x => x.IdEnrollment) + 1,
                    Studies = studies,
                    StartDate = DateTime.Now,
                    Semester = promoteStudents.Semester + 1
                };

                _studentsDbContext.Enrollment.Add(nextSemesterEnrollment);
            }

            // promujemy studentow
           var studentsToPromote = _studentsDbContext.Student
                .Where(x => x.Enrollment.Studies == studies && x.Enrollment.Semester == promoteStudents.Semester)
                .ToList();

            foreach (var student in studentsToPromote)
            {
                student.Enrollment = nextSemesterEnrollment;
            }

            _studentsDbContext.SaveChanges();

            return Created("", nextSemesterEnrollment);
        }
    }
}