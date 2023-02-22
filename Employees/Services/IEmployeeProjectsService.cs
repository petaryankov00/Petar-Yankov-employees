using System;
using Employees.Models;

namespace Employees.Services
{
	public interface IEmployeeProjectsService
	{
        List<CommonEmployeeProjects> GetCommonEmployeeProjects(List<EmployeeProjectsInputModel> employees);
    }
}

