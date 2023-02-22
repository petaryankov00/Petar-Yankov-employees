using System;
using Employees.Models;

namespace Employees.Services
{
    public class EmployeeProjectsService : IEmployeeProjectsService
    {
        public List<CommonEmployeeProjects> GetCommonEmployeeProjects(List<EmployeeProjectsInputModel> employees)
        {
            var pairOfEmployees = new List<CommonEmployeeProjects>();

            for (int i = 0; i < employees.Count() - 1; i++)
            {
                for (int j = i + 1; j < employees.Count(); j++)
                {
                    if (employees[i].UserId != employees[j].UserId &&
                        employees[i].ProjectId == employees[j].ProjectId &&
                        employees[i].DateFrom < employees[j].DateTo &&
                        employees[j].DateFrom < employees[i].DateTo)
                    {
                        var workedDays = CalculateDaysWorkedTogether(employees[i].DateFrom, employees[i].DateTo, employees[j].DateFrom, employees[j].DateTo);
                        var pair = pairOfEmployees.FirstOrDefault(x => (x.WorkerId == employees[i].UserId && x.CoWorkerId == employees[j].UserId) ||
                                                                       (x.WorkerId == employees[j].UserId && x.CoWorkerId == employees[i].UserId));

                        if (pair is null)
                        {
                            //Use Math to ensure we don't have duplicate pairs
                            pair = new CommonEmployeeProjects
                            {
                                WorkerId = Math.Min(employees[i].UserId, employees[j].UserId),
                                CoWorkerId = Math.Max(employees[i].UserId, employees[j].UserId),
                                ProjectId = employees[i].ProjectId,
                            };

                            pairOfEmployees.Add(pair);
                        }

                        pair.WorkedDays += workedDays;
                    }
                }
            }

            return pairOfEmployees;
        }

        private int CalculateDaysWorkedTogether(DateTime dateFrom1, DateTime dateTo1, DateTime dateFrom2, DateTime dateTo2)
        {
            var overlapStart = dateFrom1 > dateFrom2 ? dateFrom1 : dateFrom2;
            var overlapEnd = dateTo1 < dateTo2 ? dateTo1 : dateTo2;

            return (overlapEnd - overlapStart).Days;
        }
    }
}

