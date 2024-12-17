<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AuditLogs.aspx.cs" Inherits="LTG.AuditLogs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <main id="main" class="main">

    
         <style type="text/css">
             .tbody, td, tfoot, th, thead, tr{
                 border-width:1px !important;
             }
         </style>
    <section class="section dashboard">
      <div class="row">

       

        <!-- Right side columns -->
        <div class="col">

          <!-- Recent Activity -->
          <div class="card">
              <h5 class="card-title" style="text-align:center;background-color: #4090ce;color:white">Audit Logs</h5>
           <section class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row g-3 needs-validation">
                     <div style="text-align:right;"><asp:ImageButton ID="imgExcel" runat="server" ToolTip="Export To Excel" OnClick="imgExcel_Click" ImageUrl="~/assets/img/excel.png" Width="30" /></div>
                   <div class="col-12" style="margin-top: -9px;">
                      <label for="txtFrmDate" class="form-label">From Date</label>
                        <asp:TextBox id="txtFrmDate" runat="server" ValidationGroup="TimeSlot" TextMode="Date"  ClientIDMode="Static" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="reqName" controltovalidate="txtFrmDate" ForeColor="OrangeRed" errormessage="Please enter a fromdate!" />
                      
                    </div>

                    <div class="col-12" style="margin-top: -9px;">
                      <label for="txtToDate" class="form-label">To Date</label>
                     <asp:TextBox id="txtToDate" runat="server" TextMode="Date" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator1" ForeColor="OrangeRed" controltovalidate="txtToDate" errormessage="Please enter a toDate!" />
                      
                    </div>
                  
                    <div class="col-12">
                        <asp:Button ID="btnCreate" class="btn btn-primary w-100" OnClick="btnCreate_Click" Text="Search" runat="server" />
                    </div>
                      
                    <div class="col-12">
                      <asp:Gridview ID="grdCustomer" runat="server" HeaderStyle-BackColor="#4090ce" Width="100%" GridLines="Both" AutoGenerateColumns="false">
                          <Columns>
                              <asp:TemplateField HeaderText="SNo" HeaderStyle-Width="10%" >
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                               <asp:BoundField DataField="Table" HeaderText="Table" />
                              <asp:BoundField DataField="Field" HeaderText="Field" />
                              <asp:BoundField DataField="Custom" HeaderText="Event" />
                              <asp:BoundField DataField="ExistingValue" HeaderText="ExistingValue" />
                             <asp:BoundField DataField="NewValue" HeaderText="NewValue" />
                               <asp:BoundField DataField="ModifiedByUserName" HeaderText="UpdatedBy" />
                             <asp:BoundField DataField="ModifiedByDate" DataFormatString="{0:dd/MMM/yyyy hh:mm tt}" HtmlEncode="False" HeaderText="UpdatedDate" />
                          </Columns>
                      </asp:Gridview>
                    </div>
                   
                   
                  </div>
       
      </section>

          </div><!-- End Recent Activity -->

         
      </div>
          </div>
    </section>

  </main><!-- End #main -->
</asp:Content>
