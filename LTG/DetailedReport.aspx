<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="DetailedReport.aspx.cs" Inherits="LTG.DetailedReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <main id="main" class="main">

     <script type="text/javascript">
         function openNewWindow(url) {
             window.open(url, '_blank');
         }
     </script>
    <section class="section dashboard">
      <div class="row">

       

        <!-- Right side columns -->
        <div class="col">

          <!-- Recent Activity -->
          <div class="card">
              <h5 class="card-title" style="text-align:center;background-color: #4090ce;color:white">HU Tracking Report(Details)-PerMonth</h5>
           <section class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row g-3 needs-validation">
                    <div class="col-12">
                      <label for="ddlBranch" class="form-label">Customer</label>
                        <asp:DropDownList ID ="ddlCustomer" runat="server" class="form-select">
                            
                        </asp:DropDownList>
                        
                    </div>
                    <div class="col-12">
                      <label for="txtFrmDate" class="form-label">Bill From Date</label>
                        <asp:TextBox id="txtFrmDate" runat="server" ValidationGroup="TimeSlot" TextMode="Date"  ClientIDMode="Static" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="reqName" controltovalidate="txtFrmDate" ForeColor="OrangeRed" errormessage="Please enter a fromdate!" />
                      
                    </div>

                    <div class="col-12">
                      <label for="txtToDate" class="form-label">Bill To Date</label>
                     <asp:TextBox id="txtToDate" runat="server" TextMode="Date" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator1" ForeColor="OrangeRed" controltovalidate="txtToDate" errormessage="Please enter a toDate!" />
                      
                    </div>
                  
                    <div class="col-12">
                        <asp:Button ID="btnCreate" class="btn btn-primary w-100" OnClick="btnCreate_Click" Text="Generate Report" runat="server" />
                    </div>
                   
                  </div>
       
      </section>

          </div><!-- End Recent Activity -->

         
      </div>
          </div>
    </section>

  </main><!-- End #main -->

</asp:Content>
