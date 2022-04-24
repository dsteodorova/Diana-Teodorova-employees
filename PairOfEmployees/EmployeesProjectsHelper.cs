using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PairOfEmployees
{
    public class EmployeesProjectsHelper
    {
        static List<PairOfEmployees> lstPairOfEmployees;
        static List<PairOfEmployees> lstPairOfEmployeesCommon;

        public static EmployeeProject ParseFromCsv(string csvRow)
        {
            string[] values = csvRow.Split(',');

            EmployeeProject empProj = new EmployeeProject();
            empProj.EmpID = values[0];
            empProj.ProjectID = values[1];
            empProj.DateFrom = Convert.ToDateTime(values[2]);            //
            empProj.DateTo = values[3] == "NULL" ? DateTime.Now : ConvertToDateTime(values[3]);

            return empProj;
        }

        private static DateTime ConvertToDateTime(string dateValue)
        {
            DateTime date = DateTime.Now;

            try
            {
                //Convert.ToDateTime should work with most of the dateformats.
                date = Convert.ToDateTime(dateValue);
            }
            catch (FormatException)
            {
                Console.WriteLine("'{0}' is not in the proper format.", dateValue);
            }

            return date;
        }

        /// <summary>
        /// Create List with pairs of employees who were working on common  for every separate project.
        /// </summary>
        /// <param name="lstEmpProj"></param>
        /// <returns></returns>
        public static List<PairOfEmployees> GetPairOfEmployees(List<EmployeeProject> lstEmpProj)
        {
            //First we find all employees who were working toghether on same peojects and store them in lstPairOfEmployees.
            lstPairOfEmployees = new List<PairOfEmployees>();

            //Make a copy of list read from csv - we will use it for skipping already checked records.
            EmployeeProject[] arr = new EmployeeProject[lstEmpProj.Count];

            lstEmpProj.CopyTo(arr);
            List<EmployeeProject> lstEmpProjClone = arr.ToList();

            foreach (EmployeeProject empProj1 in lstEmpProj)
            {
                //Get All employees which had worked on empProj project.
                List<EmployeeProject> lstProjectTeam = lstEmpProjClone.Where(record => record.ProjectID == empProj1.ProjectID && record != empProj1).ToList();

                foreach (EmployeeProject empProj2 in lstProjectTeam)
                {
                    //We assume that if 2 employees has worked on current project in different time, 
                    //they were not working together and we don't add empProj2 item to result list.
                    if (HasWorkedTogether(empProj1, empProj2))
                    {
                        UpdatePairOfEmployees(empProj1, empProj2);
                    }
                    lstEmpProjClone.Remove(empProj2);
                }
                lstEmpProjClone.Remove(empProj1);
            }

            return lstPairOfEmployees;
        }

        private static void UpdatePairOfEmployees(EmployeeProject empProj1, EmployeeProject empProj2)
        {
            int workingDays = CalculateWorkingDays(empProj1, empProj2);

            PairOfEmployees pair = new PairOfEmployees();
            pair.EmpID1 = empProj1.EmpID;
            pair.EmpID2 = empProj2.EmpID;
            pair.ProjID = empProj1.ProjectID;
            pair.DaysWorked = workingDays;

            lstPairOfEmployees.Add(pair);
        }

        /// <summary>
        /// Calculate days that both employees were working together on current project.
        /// </summary>
        /// <param name="empProj1"></param>
        /// <param name="empProj2"></param>
        /// <returns></returns>
        private static int CalculateWorkingDays(EmployeeProject empProj1, EmployeeProject empProj2)
        {
            //check which one is the start date and which one is the end date of working together.
            DateTime startDate = empProj1.DateFrom < empProj2.DateFrom ? empProj2.DateFrom : empProj1.DateFrom;
            DateTime endDate = empProj1.DateTo < empProj2.DateTo ? empProj1.DateTo : empProj2.DateTo;

            return (endDate - startDate).Days;
        }

        private static bool HasWorkedTogether(EmployeeProject empProj1, EmployeeProject empProj2)
        {
            //Check if the period for which both employees were working toghether overlaps.
            if (empProj1.DateFrom.Date <= empProj2.DateTo && empProj1.DateTo >= empProj2.DateFrom)
                return true;

            return false;
        }

        public static PairOfEmployees GetPairOfEmployeesWorkingMost()
        {
            CalculatePairsCommonWorkingDays();

            PairOfEmployees pairofEmp = lstPairOfEmployeesCommon.OrderByDescending(pair => pair.DaysWorked).FirstOrDefault();

            return pairofEmp;
        }

        private static void CalculatePairsCommonWorkingDays()
        {
            lstPairOfEmployeesCommon = new List<PairOfEmployees>();

            //Iterrate through all projects pairs list(which we used for showing in grid).
            foreach (PairOfEmployees pair in lstPairOfEmployees)
            {
                //We will sum all employee pairs working days who had worked together on different projects.                
                UpdatePairOfEmployees(pair);
            }
        }

        private static void UpdatePairOfEmployees(PairOfEmployees pairOfEmp)
        {
            //Find if employee pair already added in list with projects working together.
            PairOfEmployees presentPair = lstPairOfEmployeesCommon.Find(pair => (pair.EmpID1 == pairOfEmp.EmpID1 && pair.EmpID2 == pairOfEmp.EmpID2) ||
            (pair.EmpID1 == pairOfEmp.EmpID2 && pair.EmpID2 == pairOfEmp.EmpID1));

            //Check if pair already exists in lstPairOfEmployeesCommon.
            //If already exist, we add workingDays to already added in lstPairOfEmployeesCommon.
            if (presentPair != null)
            {
                presentPair.DaysWorked += pairOfEmp.DaysWorked;
            }
            //We add new item in lstPairOfEmployeesCommon for this pair of employees.
            else
            {
                lstPairOfEmployeesCommon.Add(pairOfEmp);
            }
        }
    }
}