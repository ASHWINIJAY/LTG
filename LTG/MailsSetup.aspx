<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="MailsSetup.aspx.cs" Inherits="LTG.MailsSetup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <main id="main" class="main">

    

    <section class="section dashboard">
      <div class="row">

       

        <!-- Right side columns -->
        <div class="col">

          <!-- Recent Activity -->
          <div class="card">
              <div id="divTest" runat="server" class="card-title" style="text-align:center;background-color: #4090ce;color:white">Mail Notifications Configuration</div>
           <section class="section error-404 d-flex flex-column align-items-center justify-content-center">
                <div class="row g-3 needs-validation">
                     <div class="col-12">
                      <label for="txtCode" class="form-label">Module</label>
                       <asp:DropDownList ID ="ddlReturnType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlReturnType_SelectedIndexChanged" class="form-select">
                          <asp:ListItem Text="Stock Take Cancel Notification" Value="1"></asp:ListItem>
                           <asp:ListItem Text="Stock Take Confirm Notification" Value="2"></asp:ListItem>
                           <asp:ListItem Text="DeliveryNote Notification" Value="3"></asp:ListItem>
                          <asp:ListItem Text="DeliveryNote - Reminder Notification" Value="4"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-12">
                      <label for="txtCode" class="form-label">E-Mails(For multiple seperate by comma)</label>
                        <asp:TextBox id="txtMails" runat="server" ValidationGroup="TimeSlot" TextMode="MultiLine" ClientIDMode="Static" class="form-control"></asp:TextBox>
                        
                    </div>

                   
                  
                    <div class="col-12">
                        <asp:Button ID="btnCreate" class="btn btn-primary w-100" OnClick="btnCreate_Click" Text="Save" runat="server" />
                    </div>
                   
                  </div>
       
      </section>

          </div><!-- End Recent Activity -->

         
      </div>
          </div>
    </section>

  </main><!-- End #main -->

</asp:Content>
