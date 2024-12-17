<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="BinCreate.aspx.cs" Inherits="LTG.BinCreate" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main id="main" class="main">

    

    <section class="section dashboard">
      <div class="row">

       

        <!-- Right side columns -->
        <div class="col">

          <!-- Recent Activity -->
          <div class="card">
              <div id="divTest" runat="server" class="card-title" style="text-align:center;background-color: #4090ce;color:white">Bin Creation</div>
           <section class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row g-3 needs-validation">
                    <div class="col-12">
                      <label for="ddlBranch" class="form-label">Branch</label>
                        <asp:DropDownList ID ="ddlBranch" runat="server" class="form-select">
                            
                        </asp:DropDownList>
                        
                    </div>
                    <div class="col-12">
                      <label for="txtCode" class="form-label">Bin Code</label>
                        <asp:TextBox id="txtCode" runat="server" ValidationGroup="TimeSlot" MaxLength="50" ToolTip="Maximum 50 characters allowed"  ClientIDMode="Static" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="reqName" controltovalidate="txtCode" ForeColor="OrangeRed" errormessage="Please enter a customercode!" />
                      
                    </div>

                    <div class="col-12">
                      <label for="txtSurName" class="form-label">Bin Name</label>
                     <asp:TextBox id="txtSurName" runat="server" MaxLength="50" ToolTip="Maximum 50 characters allowed"  class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" id="RequiredFieldValidator1" ForeColor="OrangeRed" controltovalidate="txtSurName" errormessage="Please enter a customername!" />
                      
                    </div>
                   <div class="col-12">
                     
                     <asp:CheckBox id="chkActive" runat="server" Text="Active"></asp:CheckBox>
                       
                    </div>
                    <div class="col-12">
                        <asp:Button ID="btnCreate" class="btn btn-primary w-100" OnClick="btnCreate_Click" Text="Submit" runat="server" />
                    </div>
                   
                  </div>
       
      </section>

          </div><!-- End Recent Activity -->

         
      </div>
          </div>
    </section>

  </main><!-- End #main -->

</asp:Content>
