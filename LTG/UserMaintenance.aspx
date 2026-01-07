<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="UserMaintenance.aspx.cs" Inherits="LTG.UserMaintenance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <main id="main" class="main">

    

    <section class="section dashboard">
      <div class="row">

       

        <!-- Right side columns -->
        <div class="col">

          <!-- Recent Activity -->
          <div class="card">
              <div class="card-title" style="text-align:center;background-color: #4090ce;color:white">User  Maintenance</div>
           <section class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row">
                    <div style="text-align:right;"><asp:ImageButton ID="imgExcel" runat="server" ToolTip="Export To Excel" OnClick="imgExcel_Click" ImageUrl="~/assets/img/excel.png" Width="30" />
                        <asp:ImageButton ID="imgClose" runat="server" ToolTip="Close" OnClick="imgClose_Click" ImageUrl="~/assets/img/cancel.png" Width="30" /></div>
                    <div class="col-12">
                   
                     <asp:TextBox id="txtSearchContainer" placeholder="Enter username" runat="server" ValidationGroup="TimeSlot21" AutoPostBack="true" OnTextChanged="txtSearchContainer_TextChanged"  ClientIDMode="Static" style="padding-top: 14px;" Width="70%"></asp:TextBox>
                        <asp:ImageButton ID="imgSearch" runat="server" OnClick="imgSearch_Click" ValidationGroup="testabc" ImageUrl="~/assets/img/search.png" Width="35" />
                         </div>
                    <div class="col-12">
                      <asp:Gridview ID="grdCustomer" runat="server" OnRowCommand="grdCustomer_RowCommand" HeaderStyle-BackColor="#80808069" Width="100%" GridLines="Both" OnRowDeleting="grdCustomer_RowDeleting" class="table" AutoGenerateColumns="false">
                          <Columns>
                               <asp:TemplateField HeaderText="SNo" HeaderStyle-Width="5%" >
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                              <asp:BoundField DataField="Username" HeaderText="UserName" />
                               <asp:BoundField DataField="FirstName" ItemStyle-Width="60%" HeaderText="Name" />
                              <asp:BoundField DataField="LastName" HeaderText="LastName" />
                               <asp:BoundField DataField="Roles" HeaderText="Roles" />
                              <asp:TemplateField HeaderText="Edit">
            <ItemTemplate>
                <asp:ImageButton ID="btnSelectImage" runat="server" ImageUrl="~/assets/img/editCustomer.png" Width="50" CommandName="SelectImage" CommandArgument='<%# Eval("Username") %>' />
            </ItemTemplate>
        </asp:TemplateField>
                               <asp:TemplateField HeaderText="Assign Roles">
            <ItemTemplate>
                <asp:ImageButton ID="btnSelectImage1" runat="server" ImageUrl="~/assets/img/roles.png" Width="50" CommandName="SelectRole" CommandArgument='<%# Eval("Username") %>' />
            </ItemTemplate>
        </asp:TemplateField>
                               <asp:TemplateField HeaderText="Delete">
            <ItemTemplate>
                <asp:ImageButton ID="btnDeletemage" runat="server" ImageUrl="~/assets/img/cancel.png" Width="50" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this user?');" CommandArgument='<%# Eval("Username") %>' />
            </ItemTemplate>

        </asp:TemplateField>
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
