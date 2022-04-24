using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic;

namespace PairOfEmployees
{
    public partial class ViewPairOfEmployees : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnViewPairOfEmployees.Click += BtnViewPairOfEmployees_Click;
        }

        private void BtnViewPairOfEmployees_Click(object sender, EventArgs e)
        {
            //Get selected file from input control iSelectFile.
            HttpPostedFile file = Request.Files["iSelectFile"];

            //Check if file was submitted.
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Server.MapPath("~/Document"), fileName);
                file.SaveAs(filePath);

                //Read csv file rows, skipping first (header) row, into generic List.
                List<EmployeeProject> values = File.ReadAllLines(filePath)
                                           .Skip(1)
                                           .Select(v => EmployeesProjectsHelper.ParseFromCsv(v))
                                           .ToList();

                List<PairOfEmployees> lstEmployees = EmployeesProjectsHelper.GetPairOfEmployees(values);

                BindGrid(lstEmployees);

                ShowPairWorkingMost();
            }
            else//Show some error messaage.
            {

            }
        }

        private void ShowPairWorkingMost()
        {
            PairOfEmployees pairOfEmp = EmployeesProjectsHelper.GetPairOfEmployeesWorkingMost();

            lblPairOfEmpWorkingMost.Text = pairOfEmp.EmpID1 + ", " + pairOfEmp.EmpID2 + ", " + pairOfEmp.DaysWorked;
            pnlPairOfEmployees.Visible = true;
        }

        private void BindGrid(List<PairOfEmployees> lstEmployees)
        {
            gvPairs.DataSource = lstEmployees;
            gvPairs.DataBind();
        }
    }
}