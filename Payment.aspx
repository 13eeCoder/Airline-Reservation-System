<%@ Page Title="Payment" Language="C#" MasterPageFile="~/Dashboard.master" AutoEventWireup="true" 
    CodeBehind="Payment.aspx.cs" Inherits="FinalSDAProject.Payment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        body {
            background-color: #e0e0e0 !important;
        }
        .payment-card {
            max-width: 500px;
            margin: 40px auto;
            padding: 20px;
            border-radius: 8px;
            background-color: #fff;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }
        .payment-option {
            margin-bottom: 10px;
        }
        label {
            font-weight: 500;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="payment-card">
            <h3 class="text-center mb-4">Payment Details</h3>
            
            <div class="mb-3">
                <h5>Flight Price: <asp:Label ID="lblPrice" runat="server" CssClass="text-success fw-bold"></asp:Label></h5>
            </div>
            
            <div class="mb-3">
                <h5>Select Payment Method:</h5>
                <div class="payment-option">
                    <asp:RadioButton ID="rbMastercard" runat="server" GroupName="PaymentMethod" Checked="true" />
                    <label for="rbMastercard">Mastercard</label>
                </div>
                <div class="payment-option">
                    <asp:RadioButton ID="rbVisa" runat="server" GroupName="PaymentMethod" />
                    <label for="rbVisa">Visa</label>
                </div>
                <div class="payment-option">
                    <asp:RadioButton ID="rbPaypak" runat="server" GroupName="PaymentMethod" />
                    <label for="rbPaypak">Paypak</label>
                </div>
            </div>

            <div class="mb-3">
                <label>Card Number</label>
                <asp:TextBox ID="txtCardNumber" runat="server" CssClass="form-control" MaxLength="19" placeholder="XXXX-XXXX-XXXX-XXXX" />
                <asp:RegularExpressionValidator ID="revCard" runat="server"
                    ControlToValidate="txtCardNumber"
                    ErrorMessage="Invalid format. Use XXXX-XXXX-XXXX-XXXX"
                    ValidationExpression="^\d{4}-\d{4}-\d{4}-\d{4}$"
                    ForeColor="Red"
                    Display="Dynamic" />
            </div>
            
            <div class="row mb-3">
                <div class="col-md-6">
                    <label>Expiry Date</label>
                    <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="form-control" TextMode="Date" />
                </div>
                <div class="col-md-6">
                    <label>CVV</label>
                    <asp:TextBox ID="txtCVV" runat="server" CssClass="form-control" MaxLength="3" />
                    <asp:RegularExpressionValidator ID="revCVV" runat="server"
                        ControlToValidate="txtCVV"
                        ErrorMessage="Enter 3 digits"
                        ValidationExpression="^\d{3}$"
                        ForeColor="Red"
                        Display="Dynamic" />
                </div>
            </div>
            
            <div class="mb-3">
                <label>Cardholder Name</label>
                <asp:TextBox ID="txtCardName" runat="server" CssClass="form-control" />
            </div>
            
            <div class="text-center">
                <asp:Button ID="btnPay" runat="server" Text="Pay Now" CssClass="btn btn-success btn-lg" OnClick="btnPay_Click" />
            </div>
            
            <asp:Label ID="lblMessage" runat="server" CssClass="text-danger mt-2 d-block text-center" Visible="false"></asp:Label>
        </div>
    </div>
</asp:Content>
