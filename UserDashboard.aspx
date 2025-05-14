<%@ Page Title="User Dashboard" Language="C#" MasterPageFile="~/Dashboard.master" AutoEventWireup="true" 
    CodeBehind="UserDashboard.aspx.cs" Inherits="FinalSDAProject.UserDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .tab-content {
            padding: 20px;
            border-left: 1px solid #dee2e6;
            border-right: 1px solid #dee2e6;
            border-bottom: 1px solid #dee2e6;
            border-radius: 0 0 5px 5px;
        }
        .nav-tabs .nav-link {
            cursor: pointer;
        }
        .navbar-custom {
            background-color: #f8f9fa;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }
        .airline-logo {
            max-height: 30px;
            margin-right: 10px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <!-- Navigation Bar -->
        <div class="d-flex justify-content-between align-items-center navbar-custom mb-4 px-3 py-2 rounded">
            <div class="nav nav-pills">
                <asp:LinkButton ID="btnSearchTab" runat="server" CssClass="nav-link active" OnClick="btnSearchTab_Click">Search Flights</asp:LinkButton>
                <asp:LinkButton ID="btnBookingsTab" runat="server" CssClass="nav-link" OnClick="btnBookingsTab_Click">My Bookings</asp:LinkButton>
                <asp:LinkButton ID="btnEditProfileTab" runat="server" CssClass="nav-link" OnClick="btnEditProfileTab_Click">Edit Profile</asp:LinkButton>
            </div>
            <div class="d-flex align-items-center">
                <span class="me-3">Welcome, <strong><%= Session["FirstName"] %></strong></span>
                <asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="btn btn-outline-danger" OnClick="btnLogout_Click" />
            </div>
        </div>

        <!-- MultiView Control for tabs -->
        <asp:MultiView ID="mvUserDashboard" runat="server" ActiveViewIndex="0">
            <!-- Search Flights View -->

            <asp:View ID="viewSearch" runat="server">
                <div class="content-panel">
                    <h4>Search Flights</h4>
                    <div class="row g-3">
                        <div class="col-md-4">
                            <label>From</label>
                            <asp:DropDownList ID="ddlSource" runat="server" CssClass="form-control" DataTextField="AirportName" DataValueField="AirportID"></asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <label>To</label>
                            <asp:DropDownList ID="ddlDestination" runat="server" CssClass="form-control" DataTextField="AirportName" DataValueField="AirportID"></asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <label>Departure Date</label>
                            <asp:TextBox ID="txtDepartureDate" runat="server" CssClass="form-control" TextMode="Date" />
                        </div>
                        <div class="col-md-2">
                            <label>&nbsp;</label>
                            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary w-100" OnClick="btnSearch_Click" />
                        </div>
                    </div>
                </div>

                <div class="mt-4">
                    <h4>Available Flights</h4>
                    <asp:GridView ID="gvFlights" runat="server" CssClass="table table-bordered table-hover" 
                        AutoGenerateColumns="false" OnRowCommand="gvFlights_RowCommand" DataKeyNames="FlightID,Price">
                        <Columns>
                            <asp:TemplateField HeaderText="Airline">
                                <ItemTemplate>
                                    <img src='<%# Eval("LogoURL") %>' class="airline-logo" alt='<%# Eval("AirlineName") %>' />
                                    <%# Eval("AirlineName") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FlightNumber" HeaderText="Flight #" />
                            <asp:BoundField DataField="Source" HeaderText="From" />
                            <asp:BoundField DataField="Destination" HeaderText="To" />
                            <asp:BoundField DataField="Departure" HeaderText="Departure" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" />
                            <asp:BoundField DataField="Arrival" HeaderText="Arrival" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" />
                            <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
                            <asp:BoundField DataField="AvailableSeats" HeaderText="Seats" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnBook" runat="server" Text="Book" 
                                        CommandName="BookFlight" 
                                        CommandArgument='<%# Container.DataItemIndex %>'
                                        CssClass="btn btn-success" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="alert alert-info">No flights available. Please try different search criteria.</div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </asp:View>

            <!-- My Bookings View -->
            <asp:View ID="viewBookings" runat="server">
                <div class="content-panel">
                    <h4>My Bookings</h4>
                    <asp:GridView ID="gvBookings" runat="server" CssClass="table table-bordered table-hover" 
                        AutoGenerateColumns="false" OnRowCommand="gvBookings_RowCommand" DataKeyNames="BookingID">
                        <Columns>
                            <asp:TemplateField HeaderText="Airline">
                                <ItemTemplate>
                                    <img src='<%# Eval("LogoURL") %>' class="airline-logo" alt='<%# Eval("AirlineName") %>' />
                                    <%# Eval("AirlineName") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FlightNumber" HeaderText="Flight #" />
                            <asp:BoundField DataField="Source" HeaderText="From" />
                            <asp:BoundField DataField="Destination" HeaderText="To" />
                            <asp:BoundField DataField="Departure" HeaderText="Departure" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" />
                            <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
                            <asp:BoundField DataField="Status" HeaderText="Status" />
                            <asp:BoundField DataField="BookingDate" HeaderText="Booked On" DataFormatString="{0:MM/dd/yyyy}" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" 
                                        CommandName="CancelBooking" 
                                        CommandArgument='<%# Eval("BookingID") %>'
                                        CssClass="btn btn-danger" 
                                        Visible='<%# Eval("Status").ToString() == "Confirmed" %>'
                                        OnClientClick="return confirm('Are you sure you want to cancel this booking?');" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="alert alert-info">You don't have any bookings yet.</div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </asp:View>

            <!-- Edit Profile View -->
            <asp:View ID="viewEditProfile" runat="server">
                <div class="content-panel">
                    <h4>Edit Profile</h4>
                    <div class="row mb-3">
                        <div class="col-md-6">
                            <label>First Name</label>
                            <asp:TextBox ID="txtEditFirstName" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-md-6">
                            <label>Last Name</label>
                            <asp:TextBox ID="txtEditLastName" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                    <div class="row mb-3">
                        <div class="col-md-6">
                            <label>Email</label>
                            <asp:TextBox ID="txtEditEmail" runat="server" CssClass="form-control" TextMode="Email" />
                        </div>
                        <div class="col-md-6">
                            <label>Password (leave blank to keep current)</label>
                            <asp:TextBox ID="txtEditPassword" runat="server" CssClass="form-control" TextMode="Password" />
                        </div>
                    </div>
                    <div class="row mb-3">
                        <div class="col-md-12">
                            <asp:Button ID="btnUpdateProfile" runat="server" Text="Update Profile" 
                                CssClass="btn btn-primary" OnClick="btnUpdateProfile_Click" />
                        </div>
                    </div>
                    <asp:Label ID="lblProfileMessage" runat="server" CssClass="text-success" Visible="false"></asp:Label>
                </div>
            </asp:View>
        </asp:MultiView>
    </div>

    <!-- Toastr notifications -->
    <link href="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/toastr.min.js"></script>
</asp:Content>