<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="UOPMaintain.aspx.cs" Inherits="LTG.UOPMaintain" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <main id="main" class="main">

    

    <section class="section dashboard">
      <div class="row">

       

        <!-- Right side columns -->
        <div class="col">

          <!-- Recent Activity -->
          <div class="card">
              <h5 class="card-title" style="text-align:center;background-color: #4090ce;color:white">Unit Of Package(UOP) Maintenance</h5>
           <section class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row g-3 needs-validation">
                     <div style="text-align:right;"><asp:ImageButton ID="imgExcel" runat="server" ToolTip="Export To Excel" OnClick="imgExcel_Click" ImageUrl="~/assets/img/excel.png" Width="30" /></div>
                   
                      <div class="col-12">
                   
                     <asp:TextBox id="txtSearchContainer" runat="server" ValidationGroup="TimeSlot21" placeholder="Enter UOP" AutoPostBack="true" OnTextChanged="txtSearchContainer_TextChanged"  ClientIDMode="Static" style="padding-top: 14px;" Width="70%"></asp:TextBox>
                        <asp:ImageButton ID="imgSearch" runat="server" OnClick="imgSearch_Click" ValidationGroup="testabc" ImageUrl="~/assets/img/search.png" Width="35" />
                         </div>
                    <div class="col-12">
                      <asp:Gridview ID="grdCustomer" runat="server" OnRowCommand="grdCustomer_RowCommand" HeaderStyle-BackColor="#80808069" Width="100%" GridLines="Both" class="table" OnRowDeleting="grdCustomer_RowDeleting" AutoGenerateColumns="false">
                          <Columns>
                              <asp:TemplateField HeaderText="SNo" HeaderStyle-Width="10%" >
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                              <asp:BoundField DataField="UOP" ItemStyle-Width="35%" HeaderText="Unit Of Package(UOP)" />
                             
                              <asp:TemplateField HeaderText="Edit">
            <ItemTemplate>
                <asp:ImageButton ID="btnSelectImage" runat="server" ImageUrl="~/assets/img/edit.png" Width="50" CommandName="SelectImage" CommandArgument='<%# Eval("UOP") %>' />
            </ItemTemplate>
        </asp:TemplateField>
                               <asp:TemplateField HeaderText="Delete">
            <ItemTemplate>
                <asp:ImageButton ID="btnDeletemage" runat="server" ImageUrl="~/assets/img/cancel.png" Width="50" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this UOP?');" CommandArgument='<%# Eval("UOP") %>' />
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
