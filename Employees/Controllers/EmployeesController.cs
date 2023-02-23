using System;
using Employees.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using Employees.Services;
using System.Globalization;

namespace Employees.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class EmployeesController : ControllerBase
	{
		private readonly IEmployeeProjectsService _employeeProjectsService;

        public EmployeesController(IEmployeeProjectsService employeeProjectsService)
        {
            _employeeProjectsService = employeeProjectsService;
        }

        [HttpPost]
		[Route("get-longest-pair")]
		public IActionResult GetEmployeesForLongestPeriodOfTime([FromForm] IFormFile document)
		{
			if (document.ContentType != "text/csv")
			{
				return this.BadRequest("Please select csv file.");
			}
			try
			{
                var employeeProjects = this.ConvertFileToObject(document);

                var pair = this._employeeProjectsService.GetCommonEmployeeProjects(employeeProjects);

                return this.Ok(pair.OrderByDescending(x => x.WorkedDays).FirstOrDefault());
            }
			catch
			{
				return this.BadRequest("Exception occured");
			}
		}

		[HttpPost]
		[Route("get-employeeProjects")]
		public IActionResult GetEmployeeProjects([FromForm] IFormFile document)
		{
						if (document.ContentType != "text/csv")
			{
				return this.BadRequest("Please select csv file.");
			}
			try
			{
                var employeeProjects = this.ConvertFileToObject(document);

                return this.Ok(this._employeeProjectsService.GetCommonEmployeeProjects(employeeProjects));
            }
			catch
			{
				return this.BadRequest("Exception occured");
			}
        }

		private List<EmployeeProjectsInputModel> ConvertFileToObject(IFormFile document)
		{
            var result = this.ReadFile(document);
            return result.Select(this.ConvertToEmloyeeProject).ToList();
        }

		private List<string> ReadFile(IFormFile document)
		{
            var result = new List<string>();
            using (var stream = new StreamReader(document.OpenReadStream()))
            {
                while (stream.Peek() >= 0)
                {
                    result.Add(stream.ReadLine());
                }
            }

			//If there is header in the file.
			//return result.Skip(1),.ToList();
			return result.ToList();
        }

		private EmployeeProjectsInputModel ConvertToEmloyeeProject(string line)
		{
			string[] values = line.Split(", ", StringSplitOptions.RemoveEmptyEntries);

			var userId = int.Parse(values[0]);
			var projectId = int.Parse(values[1]);
			DateTime dateFrom;
			DateTime.TryParse(values[2], CultureInfo.InvariantCulture, out dateFrom);
			DateTime dateTo;
            dateTo = DateTime.TryParse(values[3], CultureInfo.InvariantCulture, out var parsedDateTo) ? parsedDateTo : DateTime.Now;
			return new EmployeeProjectsInputModel
			{
				UserId = userId,
				ProjectId = projectId,
				DateFrom = dateFrom,
				DateTo = dateTo
			};
		}
	}
}

