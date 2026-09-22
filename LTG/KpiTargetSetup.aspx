<%@ Page Title="KPI Target Setup" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true"
    CodeBehind="KpiTargetSetup.aspx.cs" Inherits="LTG.KpiTargetSetup" %>
    <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <main id="main" class="main">
            <section class="section dashboard">
                <div class="row">
                    <div class="col-lg-6">
                        <div class="card">
                            <div class="card-title" style="text-align:center;background-color:#4090ce;color:white;">KPI
                                Target Setup</div>
                            <div class="card-body">
                                <p>Configure the scans-per-hour thresholds used by the KPI Dashboard.</p>
                                <div class="mb-3">
                                    <label for="txtOnTrackMinimum" class="form-label">On Track minimum scans per
                                        hour</label>
                                    <asp:TextBox ID="txtOnTrackMinimum" runat="server" CssClass="form-control"
                                        TextMode="Number" step="0.01" />
                                </div>
                                <div class="mb-3">
                                    <label for="txtAboveTargetMinimum" class="form-label">Above Target minimum scans per
                                        hour</label>
                                    <asp:TextBox ID="txtAboveTargetMinimum" runat="server" CssClass="form-control"
                                        TextMode="Number" step="0.01" />
                                </div>
                                <asp:Label ID="lblMessage" runat="server" CssClass="d-block mb-3" />
                                <asp:Button ID="btnSave" runat="server" Text="Save KPI Targets"
                                    CssClass="btn btn-primary w-100" OnClick="btnSave_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </main>
    </asp:Content>