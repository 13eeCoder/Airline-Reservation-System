<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/AdminMaster.master" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="FinalSDAProject.AdminDashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="CSS/AdminDashboardcss.css" rel="stylesheet" type="text/css" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
    <div class="d-flex justify-content-between align-items-center navbar-custom mb-4 px-3 py-2 rounded">
        <div class="nav nav-pills">
            <asp:LinkButton ID="btnDashboard" runat="server" CssClass="nav-link active" OnClick="btnDashboard_Click">Dashboard</asp:LinkButton>
            <asp:LinkButton ID="btnUsers" runat="server" CssClass="nav-link" OnClick="btnUsers_Click">Manage Users</asp:LinkButton>
            <asp:LinkButton ID="btnFlights" runat="server" CssClass="nav-link" OnClick="btnFlights_Click">Manage Flights</asp:LinkButton>
            <asp:LinkButton ID="btnBookings" runat="server" CssClass="nav-link" OnClick="btnBookings_Click">Manage Bookings</asp:LinkButton>
        </div>
        <asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="btn btn-danger" OnClick="btnLogout_Click" />
    </div>

    <asp:MultiView ID="mvDashboard" runat="server" ActiveViewIndex="0">
        <!-- Dashboard View -->
        <asp:View ID="viewHome" runat="server">
            <div class="content-panel">
                <h3>Welcome to the Admin Dashboard</h3>
                <p>Use the navigation above to manage different sections of the system.</p>
            </div>
        </asp:View>

        <!-- Manage Users View -->
        <asp:View ID="viewUsers" runat="server">
            <div class="content-panel">
                <h4>Manage Users</h4>
                <div class="row mb-3">
                    <div class="col-md-3">
                        <label>Username</label>
                        <asp:TextBox ID="txtUsername" runat="server" placeholder="Username" CssClass="form-control" />
                    </div>
                    <div class="col-md-3">
                        <label>Password</label>
                        <asp:TextBox ID="txtPassword" runat="server" placeholder="Password" TextMode="Password" CssClass="form-control" />
                    </div>
                    <div class="col-md-3">
                        <label>Email</label>
                        <asp:TextBox ID="txtEmail" runat="server" placeholder="Email" CssClass="form-control" />
                    </div>
                    <div class="col-md-3">
                        <div class="form-check">
                            <asp:CheckBox ID="chkIsAdmin" runat="server" Text="Admin Status" CssClass="form-check-input border-0 shadow-none" />
                        </div>
                    </div>
                </div>
                <div class="row mb-3">
                    <div class="col-md-3">
                        <label>First Name</label>
                        <asp:TextBox ID="txtFirstName" runat="server" placeholder="First Name" CssClass="form-control" />
                    </div>
                    <div class="col-md-3">
                        <label>Last Name</label>
                        <asp:TextBox ID="txtLastName" runat="server" placeholder="Last Name" CssClass="form-control" />
                    </div>
                </div>
                <div class="row mb-3">
                    <div class="col-md-3">
                        <asp:Button ID="btnAddUser" runat="server" Text="Add User" CssClass="btn btn-primary w-100" OnClick="btnAddUser_Click" />
                    </div>
                    <div class="col-md-3">
                        <asp:Button ID="btnViewUsers" runat="server" Text="View All Users" CssClass="btn btn-secondary w-100" OnClick="btnViewUsers_Click" />
                    </div>
                </div>
                
               <!-- Edit User Section -->
<div class="row mb-3">
    <div class="col-md-3">
        <label>Username</label>
        <asp:TextBox ID="txtEditUsername" runat="server" placeholder="Username" CssClass="form-control" />
    </div>
    <div class="col-md-3">
        <label>New Password</label>
        <asp:TextBox ID="txtEditPassword" runat="server" placeholder="New Password" TextMode="Password" CssClass="form-control" />
    </div>
    <div class="col-md-3">
        <label>New First Name</label>
        <asp:TextBox ID="txtEditFirstName" runat="server" placeholder="New First Name" CssClass="form-control" />
    </div>
</div>
<div class="row mb-3">
    <div class="col-md-3">
        <label>New Last Name</label>
        <asp:TextBox ID="txtEditLastName" runat="server" placeholder="New Last Name" CssClass="form-control" />
    </div>
    <div class="col-md-3">
        <label>New Email</label>
        <asp:TextBox ID="txtEditEmail" runat="server" placeholder="New Email" CssClass="form-control" />
    </div>
   <div class="col-md-3">
    <div class="form-check">
        <input type="checkbox" class="form-check-input" id="chkEditIsAdmin" runat="server" />
        <label class="form-check-label" for="chkEditIsAdmin">Edit Admin Status</label>
    </div>
    <div class="col-md-3">
        <label>&nbsp;</label>
        <asp:Button ID="btnEditUser" runat="server" Text="Update" CssClass="btn btn-warning w-100" OnClick="btnEditUser_Click" />
    </div>
</div>
                
                <!-- Delete User Section -->
                <div class="row mb-3">
                    <div class="col-md-4">
                        <label>User ID to Delete</label>
                        <asp:TextBox ID="txtDeleteUserId" runat="server" placeholder="Enter User ID to Delete" CssClass="form-control" />
                    </div>
                    <div class="col-md-2">
                        <label>&nbsp;</label>
                        <asp:Button ID="btnDeleteUser" runat="server" Text="Delete User" CssClass="btn btn-danger w-100" OnClick="btnDeleteUser_Click" />
                    </div>
                </div>
                
                <asp:UpdatePanel ID="upUsers" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblLoadingUsers" runat="server" CssClass="text-info fw-bold" Visible="false" />
                        <asp:Button ID="btnHiddenLoadUsers" runat="server" OnClick="LoadAllUsers" Style="display:none;" />
                        <asp:GridView ID="gvUsers" runat="server" CssClass="table table-bordered table-hover" 
                            AutoGenerateColumns="true" EmptyDataText="No users found" Visible="false">
                        </asp:GridView>
                        <asp:Label ID="lblUserMessage" runat="server" CssClass="text-success fw-bold" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnViewUsers" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnDeleteUser" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnEditUser" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnHiddenLoadUsers" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </asp:View>

        <!-- Manage Flights View -->
        <asp:View ID="viewFlights" runat="server">
            <div class="content-panel">
                <h4>Manage Flights</h4>
                <div class="row mb-3">
                    <div class="col-md-3">
                        <label>Flight Number</label>
                        <asp:TextBox ID="txtFlightNumber" runat="server" placeholder="Flight Number" CssClass="form-control" />
                    </div>
                    <div class="col-md-3">
                        <label>Airline</label>
                        <asp:DropDownList ID="ddlAirlineID" runat="server" CssClass="form-control" DataTextField="AirlineName" DataValueField="AirlineID"></asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <label>Source Airport</label>
                        <asp:DropDownList ID="ddlSourceAirportID" runat="server" CssClass="form-control" DataTextField="AirportName" DataValueField="AirportID"></asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <label>Destination Airport</label>
                        <asp:DropDownList ID="ddlDestinationAirportID" runat="server" CssClass="form-control" DataTextField="AirportName" DataValueField="AirportID"></asp:DropDownList>
                    </div>
                </div>
                <div class="row mb-3">
                    <div class="col-md-3">
                        <label>Departure Time</label>
                        <asp:TextBox ID="txtDepartureTime" runat="server" TextMode="DateTimeLocal" CssClass="form-control" />
                    </div>
                    <div class="col-md-3">
                        <label>Arrival Time</label>
                        <asp:TextBox ID="txtArrivalTime" runat="server" TextMode="DateTimeLocal" CssClass="form-control" />
                    </div>
                    <div class="col-md-2">
                        <label>Price</label>
                        <asp:TextBox ID="txtPrice" runat="server" placeholder="Price" CssClass="form-control" TextMode="Number" step="0.01" />
                    </div>
                    <div class="col-md-2">
                        <label>Total Seats</label>
                        <asp:TextBox ID="txtTotalSeats" runat="server" placeholder="Total Seats" CssClass="form-control" TextMode="Number" />
                    </div>
                    <div class="col-md-2">
                        <label>Available Seats</label>
                        <asp:TextBox ID="txtAvailableSeats" runat="server" placeholder="Available Seats" CssClass="form-control" TextMode="Number" />
                    </div>
                </div>
                <div class="row mb-3">
                    <div class="col-md-3">
                        <label>Duration (minutes)</label>
                        <asp:TextBox ID="txtDuration" runat="server" placeholder="Duration" CssClass="form-control" TextMode="Number" />
                    </div>
                    <div class="col-md-3">
                        <label>&nbsp;</label>
                        <asp:Button ID="btnAddFlight" runat="server" Text="Add Flight" CssClass="btn btn-success w-100" OnClick="btnAddFlight_Click" />
                    </div>
                    <div class="col-md-3">
                        <label>&nbsp;</label>
                        <asp:Button ID="btnViewFlights" runat="server" Text="View All Flights" CssClass="btn btn-secondary w-100" OnClick="btnViewFlights_Click" />
                    </div>
                </div>
                
                <!-- Delete Flight Section -->
                <div class="row mb-3">
                    <div class="col-md-4">
                        <label>Flight ID to Delete</label>
                        <asp:TextBox ID="txtDeleteFlightId" runat="server" placeholder="Enter Flight ID to Delete" CssClass="form-control" />
                    </div>
                    <div class="col-md-2">
                        <label>&nbsp;</label>
                        <asp:Button ID="btnDeleteFlight" runat="server" Text="Delete Flight" CssClass="btn btn-danger w-100" OnClick="btnDeleteFlight_Click" />
                    </div>
                </div>
                
                <asp:UpdatePanel ID="upFlights" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblLoadingFlights" runat="server" CssClass="text-info fw-bold" Visible="false" />
                        <asp:Button ID="btnHiddenLoadFlights" runat="server" OnClick="LoadAllFlights" Style="display:none;" />
                        <asp:GridView ID="gvFlights" runat="server" CssClass="table table-bordered table-hover" 
                            AutoGenerateColumns="true" EmptyDataText="No flights found" Visible="false">
                        </asp:GridView>
                        <asp:Label ID="lblFlightMessage" runat="server" CssClass="text-success fw-bold" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnViewFlights" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnDeleteFlight" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnHiddenLoadFlights" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </asp:View>

        <!-- Manage Bookings View -->
        <asp:View ID="viewBookings" runat="server">
            <div class="content-panel">
                <h4>Manage Bookings</h4>
                <div class="row mb-3">
                    <div class="col-md-3">
                        <label>User</label>
                        <asp:DropDownList ID="ddlUserID" runat="server" CssClass="form-control" DataTextField="Username" DataValueField="UserID"></asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <label>Flight</label>
                        <asp:DropDownList ID="ddlFlightID" runat="server" CssClass="form-control" DataTextField="FlightNumber" DataValueField="FlightID"></asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <label>Booking Date</label>
                        <asp:TextBox ID="txtBookingDate" runat="server" TextMode="DateTimeLocal" CssClass="form-control" />
                    </div>
                </div>
                <div class="row mb-3">
                    <div class="col-md-2">
                        <label>Passengers</label>
                        <asp:TextBox ID="txtPassengers" runat="server" placeholder="Passengers" CssClass="form-control" TextMode="Number" />
                    </div>
                    <div class="col-md-2">
                        <label>Total Price</label>
                        <asp:TextBox ID="txtTotalPrice" runat="server" placeholder="Total Price" CssClass="form-control" TextMode="Number" step="0.01" />
                    </div>
                    <div class="col-md-3">
                        <label>Status</label>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Confirmed" Value="Confirmed" />
                            <asp:ListItem Text="Cancelled" Value="Cancelled" />
                            <asp:ListItem Text="Pending" Value="Pending" />
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <label>&nbsp;</label>
                        <asp:Button ID="btnAddBooking" runat="server" Text="Add Booking" CssClass="btn btn-success w-100" OnClick="btnAddBooking_Click" />
                    </div>
                </div>
                <div class="row mb-3">
                    <div class="col-md-4">
                        <label>Booking ID</label>
                        <asp:TextBox ID="txtBookingID" runat="server" placeholder="Booking ID" CssClass="form-control" />
                    </div>
                    <div class="col-md-3">
                        <label>&nbsp;</label>
                        <asp:Button ID="btnSearchBooking" runat="server" Text="Search Booking" CssClass="btn btn-info w-100" OnClick="btnSearchBooking_Click" />
                    </div>
                    <div class="col-md-3">
                        <label>&nbsp;</label>
                        <asp:Button ID="btnViewBookings" runat="server" Text="View All Bookings" CssClass="btn btn-secondary w-100" OnClick="btnViewBookings_Click" />
                    </div>
                </div>
                
                <!-- Delete Booking Section -->
                <div class="row mb-3">
                    <div class="col-md-4">
                        <label>Booking ID to Delete</label>
                        <asp:TextBox ID="txtDeleteBookingId" runat="server" placeholder="Enter Booking ID to Delete" CssClass="form-control" />
                    </div>
                    <div class="col-md-2">
                        <label>&nbsp;</label>
                        <asp:Button ID="btnDeleteBooking" runat="server" Text="Delete Booking" CssClass="btn btn-danger w-100" OnClick="btnDeleteBooking_Click" />
                    </div>
                </div>
                
                <asp:UpdatePanel ID="upBookings" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Label ID="lblLoadingBookings" runat="server" CssClass="text-info fw-bold" Visible="false" />
                        <asp:Button ID="btnHiddenLoadBookings" runat="server" OnClick="LoadAllBookings" Style="display:none;" />
                        <asp:GridView ID="gvBookings" runat="server" CssClass="table table-bordered table-hover" 
                            AutoGenerateColumns="true" EmptyDataText="No bookings found" Visible="false">
                        </asp:GridView>
                        <asp:Label ID="lblBookingMessage" runat="server" CssClass="text-info fw-bold" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnViewBookings" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnDeleteBooking" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnAddBooking" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnHiddenLoadBookings" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>