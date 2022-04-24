<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewPairOfEmployees.aspx.cs" Inherits="PairOfEmployees.ViewPairOfEmployees" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Projects Teams</title>
    <link rel="stylesheet" href="scripts/styles.css" />
</head>
<body>
    <form id="form1" runat="server">
        <header class="bg-dark py-5">
            <div class="container px-5">
                <div class="row gx-5 justify-content-center">
                    <div class="col-lg-6">
                        <div class="text-center my-5">
                            <h1 class="display-5 fw-bolder text-white mb-2">View Projects Teams</h1>
                            <p class="lead text-white-50 mb-4">An application for loading data from selected csv file and showing in tabular view the result of all employees pairs!</p>
                           
                        </div>
                    </div>
                </div>
            </div>
        </header>
        <section class="py-5 border-bottom">
            <div class="container px-5 my-5">
                <div class="col-lg-12 mb-5 mb-lg-0">
                    <div>
            <input id="iSelectFile" runat="server" type="file" class="btn btn-primary btn-lg px-4 me-sm-3"/>
            <asp:Button ID="btnViewPairOfEmployees" runat="server" Text="View" CssClass="btn btn-primary btn-lg px-4 me-sm-3"/>
        </div>
        <asp:GridView ID="gvPairs" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="EmpID1" HeaderText="Employee ID #1" />
                <asp:BoundField DataField="EmpID2" HeaderText="Employee ID #2" />
                <asp:BoundField DataField="ProjID" HeaderText="Project ID" />                   
               <asp:BoundField DataField="DaysWorked" HeaderText="Days worked" />
            </Columns>
        </asp:GridView>
                </div>
                <div class="col-lg-12 mb-5 mb-lg-0">
                    <asp:Panel ID="pnlPairOfEmployees" runat="server" Visible="false"
>                     <p class="lead mb-4">Pair of employees who have worked
together on common projects for the longest period of time:                     <asp:Label ID="lblPairOfEmpWorkingMost" runat="server"></asp:Label>
</p>                    
                        </asp:Panel>
                </div>
            </div>        
            </section>
    </form>
</body>
</html>
