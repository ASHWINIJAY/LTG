<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="OutBoundFee.aspx.cs" Inherits="LTG.OutBoundFee" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <main id="main" class="main">

    

    <section class="section dashboard">
      <div class="row">

       

        <!-- Right side columns -->
        <div class="col">

          <!-- Recent Activity -->
          <div class="card">
           
               <h5 class="card-title" style="text-align:center;background-color: #4090ce;color:white">OutBound Fee</h5>
           <section class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row g-3 needs-validation">
                   <div class="col-12">
                      <label for="ddlBranch" class="form-label">Branch</label>
                        <asp:DropDownList ID ="ddlBranch" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_SelectedIndexChanged" runat="server" class="form-select">
                        </asp:DropDownList>
                        
                    </div>
                     <div class="col-12">
                      <label for="ddlBranch" class="form-label">Customer</label>
                        <asp:DropDownList ID ="ddlCustomer" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged" class="form-select">
                            
                        </asp:DropDownList>
                        
                    </div>
                    <div class="col-12">
                      <label for="txtExistingFee" class="form-label">Existing WeekDays Outbound Fee</label>
                        <asp:TextBox id="txtExistingFee" runat="server" ValidationGroup="TimeSlot5" ReadOnly="true" BackColor="#dfe8f0" Text="0"  ClientIDMode="Static" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="reqName" controltovalidate="txtExistingFee" ForeColor="OrangeRed" />
                      
                    </div>

                    <div class="col-12">
                      <label for="txtNewFee" class="form-label">Enter WeekDays Outbound Fee</label>
                     <asp:TextBox id="txtNewFee" runat="server" ValidationGroup="TimeSlot" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator1" ForeColor="OrangeRed" controltovalidate="txtNewFee" errormessage="Please enter a WeekDays Outbound Fee!" />
                      
                    </div>
                   
                     <div class="col-12">
                      <label for="txtExistingFee" class="form-label">Existing Saturday Outbound Fee</label>
                        <asp:TextBox id="txtExtSaturday" runat="server" ValidationGroup="TimeSlo5t" ReadOnly="true" BackColor="#dfe8f0" Text="0"  ClientIDMode="Static" class="form-control"></asp:TextBox>
                      
                      
                    </div>

                    <div class="col-12">
                      <label for="txtNewFee" class="form-label">Enter Saturday Outbound Fee</label>
                     <asp:TextBox id="txtSaturday" runat="server" ValidationGroup="TimeSlot"  class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator3" ForeColor="OrangeRed" controltovalidate="txtSaturday" errormessage="Please enter a Saturday Outbound Fee!" />
                      
                    </div>
                     <div class="col-12">
                      <label for="txtExistingFee" class="form-label">Existing Sunday Outbound Fee</label>
                        <asp:TextBox id="txtExtSunday" runat="server" ValidationGroup="TimeSlo5t" ReadOnly="true" BackColor="#dfe8f0" Text="0"  ClientIDMode="Static" class="form-control"></asp:TextBox>
                      
                      
                    </div>

                    <div class="col-12">
                      <label for="txtNewFee" class="form-label">Enter Sunday Outbound Fee</label>
                     <asp:TextBox id="txtSunday" runat="server"  class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ValidationGroup="TimeSlot" id="RequiredFieldValidator4" ForeColor="OrangeRed" controltovalidate="txtSunday" errormessage="Please enter a Sunday Outbound Fee!" />
                      
                    </div>
                    <div class="col-12">
                        <asp:Button ID="btnCreate" ValidationGroup="TimeSlot" class="btn btn-primary w-100" OnClick="btnCreate_Click" Text="Save" runat="server" />
                    </div>
                   
                  </div>
       
      </section>

          </div><!-- End Recent Activity -->

         
      </div>
          </div>
    </section>

  </main><!-- End #main -->

</asp:Content>
