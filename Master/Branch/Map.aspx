<%@ Page Title="" Language="C#" MasterPageFile="~/PreParking.master" AutoEventWireup="true" CodeFile="Map.aspx.cs" Inherits="Master_Branch_Map" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MasterPage" runat="Server">
    <div class="row">
        <div class="col-12 col-sm-12 col-md-4 col-lg-4 col-xl-4">
            <asp:Label ID="lblLatitude" runat="server" Text="Latitude" CssClass="form-check-label"></asp:Label>
            <span class="spanStar">*</span>
            <asp:TextBox ID="txtLatitude" runat="server" CssClass="form-control" AutoComplete="off" onkeypress="return isNumber(event);"
                TabIndex="27"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="BranchMasterSub" ControlToValidate="txtLatitude" runat="server" CssClass="rfvClr"
                ErrorMessage="Enter Latitude"> </asp:RequiredFieldValidator>
        </div>
        <div class="col-12 col-sm-12 col-md-4 col-lg-4 col-xl-4">
            <asp:Label ID="lblLongitude" runat="server" Text="Longitude" CssClass="form-check-label"></asp:Label>
            <span class="spanStar">*</span>
            <asp:TextBox ID="txtLongitude" runat="server" CssClass="form-control" AutoComplete="off" onkeypress="return isNumber(event);"
                TabIndex="28"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="BranchMasterSub" ControlToValidate="txtLongitude" runat="server" CssClass="rfvClr"
                ErrorMessage="Enter Longitude"> </asp:RequiredFieldValidator>

        </div>
    </div>
    <div class="col-xs-12 col-sm-12" id="dvMapget" style="height: 500px;">
    </div>
    <asp:HiddenField ID="hflongitude" EnableViewState="true" runat="server" />
    <asp:HiddenField ID="hflatitude" EnableViewState="true" runat="server" />
    <%--    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyB56Km4bH3DEKxXLRZBltsTIm3eYgPqt0k"></script>--%>

    <script async defer src="https://maps.googleapis.com/maps/api/js?key=AIzaSyB56Km4bH3DEKxXLRZBltsTIm3eYgPqt0k" type="text/javascript"></script>
    <%-- Script for the Get the Lat and Log--%>
    <script type="text/javascript">
        window.onload = function () {
            const options = {
                enableHighAccuracy: true,
                maximumAge: 30000
            };

            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(success, fail, options);
            }
            else {
                alert("Sorry, your browser does not support geolocation services.");
            }
            var latt, longg, map;

            function success(position) {
                document.getElementById('<%=hflongitude.ClientID %>').value = position.coords.longitude;
                document.getElementById('<%=hflatitude.ClientID %>').value = position.coords.latitude;

                latt = position.coords.latitude;
                longg = position.coords.longitude;
                setlatlong(latt, longg);
            }

            function setlatlong(latt, longg) {
                document.getElementById('<%=txtLatitude.ClientID %>').value = latt;
                document.getElementById('<%=txtLongitude.ClientID %>').value = longg;

                var mapOptions = {
                    center: new google.maps.LatLng(latt, longg),
                    zoom: 10,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                };

                map = new google.maps.Map(document.getElementById("dvMapget"), mapOptions);

                var marker = new google.maps.Marker({
                    position: new google.maps.LatLng(latt, longg),
                });
                marker.setMap(map);

                google.maps.event.addListener(map, "click", function (e) {
                    var latLng = e.latLng;
                    var lat = e.latLng.lat();
                    var long = e.latLng.lng();
                    document.getElementById('<%=txtLatitude.ClientID %>').value = lat;
                    document.getElementById('<%=txtLongitude.ClientID %>').value = long;
               
                    marker.setMap(null);//remove other markers

                    marker = new google.maps.Marker({
                        position: new google.maps.LatLng(lat, long)
                    });
                    marker.setMap(map);//Set new marker

                    //google.maps.event.addDomListener(window, 'load', initialize);
                });

            }

            function fail(error) {
                if (alert(error.message) == "") {
                    alert("Sorry, Failed to get Your Location.");
                }
                else {
                    alert(error.message);
                }
            }
        }

    </script>

</asp:Content>

