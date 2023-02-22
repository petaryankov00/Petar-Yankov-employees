using System;
namespace Employees.Models
{
	public class EmployeeProjectsInputModel
	{
		public int UserId { get; set; }

		public int ProjectId { get; set; }

		public DateTime DateFrom { get; set; }

		public DateTime DateTo { get; set; }
	}
}

